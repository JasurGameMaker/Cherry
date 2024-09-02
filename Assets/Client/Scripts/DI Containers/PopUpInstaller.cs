using Client;
using UnityEngine;
using Zenject;

public class PopUpInstaller : MonoInstaller
{
    [SerializeField] private SubjectPopUpView subjectPopUpView;
    
    public override void InstallBindings()
    {
        Container.Bind<IPopUp>()
            .FromInstance(subjectPopUpView)
            .AsSingle();
    }
}