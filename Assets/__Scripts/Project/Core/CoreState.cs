using __Scripts.Project.Core.Model;
using __Scripts.Project.Utils;

namespace __Scripts.Project.Core
{
    public class CoreState
    {
        public ReactiveProperty<CourseMesh> SelectedMesh { get; private set; } = new ();
    }
}
