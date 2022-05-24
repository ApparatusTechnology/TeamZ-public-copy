using System.Linq;
using System.Threading.Tasks;
using TeamZ.Assets.Code.Helpers;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters.Hedgehog;
using TeamZ.Code.Game.Characters.Lizard;
using TeamZ.GameSaving;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Helpers
{
    public class ShakeMainCamera
    {
        public static void Emit(float time, float amplitude)
        {
            MessageBroker.Default.Publish(new ShakeMainCamera { Time = time, Amplitude = amplitude });
        }

        public float Time { get; set; }
        public float Amplitude { get; set; }
    }

    public class MainCamera : MonoBehaviour
    {
        public float dampTime = 0.3f;

        private Vector3 velocity = Vector3.zero;
        private Dependency<GameController> gameController;
        private Dependency<EntitiesStorage> entitiesStorage;
        private Transform[] targets;
        private Camera mainCamera;
        private ShakeableTransform shakeableTransform;

        private void Start()
        {
            MessageBroker.Default
                .Receive<ShakeMainCamera>()
                .Subscribe(o => this.shakeableTransform.Shake(o.Time, o.Amplitude))
                .AddTo(this);

            this.gameController.Value.Loaded.Subscribe(async _ => await this.SearchForPlayers());
            this.mainCamera = this.GetComponent<Camera>();
            this.shakeableTransform = this.GetComponent<ShakeableTransform>();
        }

        public async UniTask SearchForPlayers()
        {
            await UniTask.DelayFrame(5);
            this.targets = this.entitiesStorage.Value.Entities.Values
                .Where(o => o.GetComponent<Lizard>() || o.GetComponent<Hedgehog>())
                .Select(o => o.transform)
                .ToArray();

            if (!targets.Any())
            {
                return;
            }

            var approximateDestination = this.CalculateDestination();
            this.shakeableTransform.position = approximateDestination;

        }

        private void Update()
        {
            if (this.targets?.Any() ?? false)
            {
                var approximateDestination = this.CalculateDestination();
                var newPosition = Vector3.SmoothDamp(this.shakeableTransform.position, approximateDestination, ref this.velocity, this.dampTime);

                if (Vector3.Distance(this.shakeableTransform.position, newPosition) > 10)
                {
                    this.shakeableTransform.position = approximateDestination;
                    return;
                }

                this.shakeableTransform.position = newPosition;
            }
        }

        private Vector3 CalculateDestination()
        {
            var sum = Vector3.zero;

            for (int i = 0; i < this.targets.Length; i++)
            {
                var target = this.targets[i];

                // it can be broken during level switch
                if (!target)
                {
                    continue;
                }

                var targetPosition = target.position;
                var point = this.mainCamera.WorldToViewportPoint(new Vector3(targetPosition.x, targetPosition.y + 5.5f, targetPosition.z));
                var delta = new Vector3(targetPosition.x, targetPosition.y + 5.5f, targetPosition.z) - this.mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                var destination = this.shakeableTransform.position + delta;

                sum += destination;
            }

            var approximateDestination = sum / this.targets.Length;
            approximateDestination.z = -15;
            return approximateDestination;
        }
    }
}