using Client;
using UnityEngine;
using UnityEngine.UI;

public class SubjectItem : MonoBehaviour
{
    [SerializeField] private Button subjectButton;
    [SerializeField] private SubjectPopUpView subjectPopUpViewPrefab;

    public SubjectPopUpView subjectPopUpView;
    
    private void OnEnable()
    {
        subjectButton.onClick.AddListener(OnSubjectButtonClicked);
    }

    private void OnDisable()
    {
        subjectButton.onClick.RemoveListener(OnSubjectButtonClicked);
    }

    private void OnSubjectButtonClicked()
    {
        subjectPopUpView.Open();
    }
}