using Sirenix.Utilities;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject cameraStandard;
    [SerializeField] private GameObject cameraAr;
    [SerializeField] private GameObject[] objsToHideInAR;
    
    private void Awake()
    {
        cameraStandard.SetActive(true);
        cameraAr.SetActive(false);
    }
    
    public void ChangeCameraMode(bool state)
    {
        if (state)
        {
            cameraStandard.SetActive(false);
            cameraAr.SetActive(true);
            objsToHideInAR.ForEach(o => o.SetActive(false));
        }
        else
        {
            cameraStandard.SetActive(true);
            cameraAr.SetActive(false);
            objsToHideInAR.ForEach(o => o.SetActive(true));
        }
    }
}
