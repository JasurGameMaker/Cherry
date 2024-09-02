using __Scripts.Project.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace __Scripts.Project.Services
{
    public interface IRemoteHelper
    {
        public UniTask<Catalog> LoadCatalog();
        public UniTask<Texture2D> LoadPreviewImage(Catalog.Subject.Lesson lesson);
        public void LoadVideo(VideoPlayer videoPlayer, Catalog.Subject subject);
    }
}
