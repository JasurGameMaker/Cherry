using System.Collections.Generic;
using __Scripts.Project.Menu.UI.Subjects;

namespace __Scripts.Project.Menu.UI.New
{
    public interface ILessonsProvider
    {
        public IReadOnlyList<LessonView> LessonViews { get; }
    }
}
