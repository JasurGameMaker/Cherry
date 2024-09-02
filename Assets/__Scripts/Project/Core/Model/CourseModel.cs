using System.Collections.Generic;
using System.Linq;
using __Scripts.Project.Core.Model.Socket;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

namespace __Scripts.Project.Core.Model
{
    public class CourseModel : MonoBehaviour
    {
        [SerializeField] private CourseMesh[] courseMeshes;

        public PlayableDirector PlayableDirector => _playableDirector;
        public CourseMesh[] CourseMeshes => courseMeshes;
        public IEnumerable<SocketController> SocketControllers => courseMeshes.Select(m => m.SocketController);

        private PlayableDirector _playableDirector;
        private void Awake()
        {
            if (TryGetComponent(out PlayableDirector director))
                _playableDirector = director;
        }

        public Tween Assemble()
        {
            _playableDirector?.Stop();

            var sequence = DOTween.Sequence();
            
            foreach (var courseMesh in courseMeshes)
                sequence.Join(courseMesh.ResetPosition().OnComplete(() => courseMesh.SocketController.SetIsAttachedState(true)));
            
            return sequence;
        }

        public void Initialize(CoreState coreState, CameraManipulationsHandler cameraManipulationsHandler)
        {
            foreach (CourseMesh courseMesh in courseMeshes)
                courseMesh.Initialize(coreState, cameraManipulationsHandler);
        }

        public async UniTask LoadClips()
        {
            UniTask[] tasks = new UniTask[courseMeshes.Length];

            for (int index = 0; index < courseMeshes.Length; index++)
            {
                CourseMesh courseMesh = courseMeshes[index];
                tasks[index] = courseMesh.LoadClip();
            }

            await UniTask.WhenAll(tasks);
        }
    }
}
