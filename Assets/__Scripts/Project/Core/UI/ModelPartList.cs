using __Scripts.Project.Core.Model;
using UnityEngine;
using Zenject;

namespace __Scripts.Project.Core.UI
{
    public class ModelPartList : MonoBehaviour
    {
        [SerializeField] private ModelListEntry modelListEntryPrefab;
        [SerializeField] private RectTransform contaiter;
        
        private CourseModelInitializer _courseModelInitializer;
        private AtlasCameraManager _cameraManager;
        private CoreState _coreState;

        [Inject]
        private void Construct(CourseModelInitializer courseModelInitializer, AtlasCameraManager cameraManager, CoreState coreState)
        {
            _courseModelInitializer = courseModelInitializer;
            _cameraManager = cameraManager;
            _coreState = coreState;
        }
        
        public void Init()
        {
            foreach (CourseMesh mesh in _courseModelInitializer.CourseModel.CourseMeshes)
            {
                if (string.IsNullOrEmpty(mesh.MeshData.titleKey))
                    continue;
                
                ModelListEntry entry = Instantiate(modelListEntryPrefab, contaiter);
                entry
                    .SetCamera(_cameraManager)
                    .SetText(mesh.MeshData.tableReference, mesh.MeshData.titleKey)
                    .SetTargetMesh(mesh)
                    .SetState(_coreState);
            }
        }
    }
}
