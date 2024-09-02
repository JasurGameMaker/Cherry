using __Scripts.Project.Core.Model;
using __Scripts.Project.Menu.UI.Utils;
using Zenject;

namespace __Scripts.Project.Core.Toggles
{
    public class ManualAssembleToggle : DoozyToggleListener
    {
        private CourseModelInitializer _courseModelInitializer;

        [Inject]
        private void Construct(CourseModelInitializer courseModelInitializer)
        {
            _courseModelInitializer = courseModelInitializer;
        }
        
        protected override void OnToggle(bool state)
        {
            foreach (var socketController in _courseModelInitializer.CourseModel.SocketControllers)
                socketController.SwitchManipulationMode(state ? ManipulationMode.Drag : ManipulationMode.Disabled);
        }
    }
}
