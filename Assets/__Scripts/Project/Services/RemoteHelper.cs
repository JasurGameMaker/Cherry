using System;
using __Scripts.Project.Data;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace __Scripts.Project.Services
{
    public class RemoteHelper : IRemoteHelper
    {
        private readonly string _catalogUrl;
        
        public RemoteHelper(string catalogUrl)
        {
            _catalogUrl = catalogUrl;
        }
        
        public async UniTask<Catalog> LoadCatalog()
        {
            if (Application.isEditor)
            {
                TextAsset textAsset = await Addressables.LoadAssetAsync<TextAsset>("catalog");
                return JsonConvert.DeserializeObject<Catalog>(textAsset.text);
            }
            
            UnityWebRequest request = UnityWebRequest.Get(_catalogUrl);
            await request.SendWebRequest();
            
            if (request.error != null)
                throw new Exception(request.error);
            
            return JsonConvert.DeserializeObject<Catalog>(request.downloadHandler.text);
        }

        public async UniTask<Texture2D> LoadPreviewImage(Catalog.Subject.Lesson lesson)
        {
            UnityWebRequest request = await UnityWebRequest.Get(lesson.previewImageKey).SendWebRequest();

            if (request.error != null)
                throw new Exception(request.error);
            
            return DownloadHandlerTexture.GetContent(request);;
        }

        public void LoadVideo(VideoPlayer videoPlayer, Catalog.Subject subject)
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = subject.VideoLink;
        }
    }
}
