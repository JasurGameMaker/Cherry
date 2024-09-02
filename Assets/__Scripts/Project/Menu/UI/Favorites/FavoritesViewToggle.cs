using System.Collections.Generic;
using System.Linq;
using __Scripts.Project.Data;
using __Scripts.Project.Menu.UI.Utils;
using Zenject;

namespace __Scripts.Project.Menu.UI.Favorites
{
    public class FavoritesViewToggle : DoozyToggleListener
    {
        private MenuState _menuState;
        private Catalog _catalog;

        [Inject]
        private void Construct(Catalog catalog, MenuState menuState)
        {
            _catalog = catalog;
            _menuState = menuState;
        }
        
        protected override void OnToggle(bool state)
        {
            if (state)
            {
                IEnumerable<Catalog.Subject.Lesson> allLessons = _catalog.subjects.SelectMany(s => s.lessons);
                IEnumerable<Catalog.Subject.Lesson> filteredLessons = allLessons
                    .Where(x => _menuState.FavoriteLessonNames.Any(y => y == x.Name));
                
                _menuState.SelectedSubject.Value = new Catalog.Subject("Favorites", "Избранное", "Избранное")
                {
                    lessons = filteredLessons.ToArray()
                };
            }
        }
    }
}
