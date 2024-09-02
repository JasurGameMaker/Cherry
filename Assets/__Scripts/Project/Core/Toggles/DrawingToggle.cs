using System;
using System.Collections.Generic;
using System.IO;
using __Scripts.Project.Core.Model;
using __Scripts.Project.Menu.UI.Utils;
using __Scripts.Project.Utils;
using Doozy.Runtime.UIManager.Components;
using FreeDraw;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;
using Zenject;

namespace __Scripts.Project.Core.Toggles
{
    public class DrawingToggle : DoozyToggleListener
    {
        [SerializeField] private Drawable drawable;
        [SerializeField] private ScreenCapturer screenCapturer;
        [SerializeField] private CanvasGroup[] ignoredCanvasGroups;
        [SerializeField] private List<RectTransform> objsToHide;
        [SerializeField] private List<UIToggle> togglesToSwitch;
        [SerializeField] private RectTransform drawingPanel;
        
        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;
        private CourseModelInitializer _modelInitializer;

        [Inject]
        private void Construct(CourseModelInitializer modelInitializer)
        {
            _modelInitializer = modelInitializer;
        }
        
        protected override void OnToggle(bool state)
        {
            if (state)
                Setup();
            else
                Cancel();
        }

        private void Setup()
        {
            screenCapturer.Capture(t =>
            {
                var cam = Camera.main;
                drawingPanel.gameObject.SetActive(true);
                _modelInitializer.CourseModel.gameObject.SetActive(false);
                togglesToSwitch.ForEach(x => x.SetIsOn(false, false, true));
                objsToHide.ForEach(x => x.gameObject.SetActive(false));
                drawable.Init(t);
                cam.orthographic = true;
                cam.orthographicSize = drawable.GetSpriteHeight() * 0.5f;
                _cameraPosition = cam.transform.position;
                _cameraRotation = cam.transform.rotation;
                cam.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                drawable.gameObject.SetActive(true);
            }, ignoredCanvasGroups);
        }

        private void Cancel()
        {
            var cam = Camera.main;
            drawingPanel.gameObject.SetActive(false);
            drawable.gameObject.SetActive(false);
            cam.orthographic = false;
            cam.transform.SetPositionAndRotation(_cameraPosition, _cameraRotation);
            _modelInitializer.CourseModel.gameObject.SetActive(true);
            
            foreach (RectTransform objToHide in objsToHide)
            {
                if (objToHide.TryGetComponent(out ARToggle _) && ARSession.state != ARSessionState.Ready)
                    continue;
                
                objToHide.gameObject.SetActive(true);
            }
        }

        public void SaveToDevice()
        {
            screenCapturer.Capture(t =>
            {
                var drawingName = "Drawing-" + Guid.NewGuid() + ".png";
                if (Application.isMobilePlatform)
                {
                    NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(t, "Drawings", drawingName,
                        (success, path) => Debug.Log("Media save result: " + success + " " + path));
                    Debug.Log("Permission result: " + permission);
                }
                else
                {
                    byte[] bytes = t.EncodeToPNG();
                    string dirPath = Application.persistentDataPath + "/Drawings/";
                    
                    print(dirPath);
                    
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);
                    
                    File.WriteAllBytes(dirPath + drawingName, bytes);
                }
                Destroy(t);
            }, ignoredCanvasGroups);
        }
    }
}
