using System;
using System.Threading.Tasks;
using TeamZ.AI.Core;
using TeamZ.Assets.Code.Game.Enemies.Core;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Game.Levels;
using TeamZ.Code.Helpers;
using TeamZ.Effects;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Enemies.Turel
{
    public class Turel : MonoBehaviour
    {
        public Sensor Sensor;
        public SpriteRenderer GunRoot;
        public AudioSource Alarm;
        public ParticleSystem Smoke;
        public ParticleSystem Sparks;
        public AudioSource SparksSound;
        public Light SparksLight;
        public SpriteRenderer Cracks;

        public int Damage = 25;
        public double Speed = 1.0;

        private ReactiveProperty<GameObject> target
            = new ReactiveProperty<GameObject>();
        private IDisposable shooting;
        private Gun gun;
        private LoopedMover[] recoilAnimators;
        private LevelObject levelObject;

        public UnityDependency<Terminal> Terminal { get; set; }

        private void Start()
        {
            this.Sparks.Stop();
            this.SparksSound.Stop();
            this.SparksLight.intensity = 0.0f;
            this.Cracks.enabled = false;

            this.gun = this.GetComponentInChildren<Gun>();
            this.gun.Hits.Subscribe(o => o.TakeDamage(this.Damage, 0)).AddTo(this);

            var particleSystem = this.GetComponentInChildren<SparksBlinker>();

            this.recoilAnimators = this.GetComponentsInChildren<LoopedMover>();
            this.levelObject = this.GetComponent<LevelObject>();
            this.levelObject.Strength
                .Subscribe(o =>
                {
                    if (o < 30 && o > 9)
                    {
                        this.Smoke.Play();
                        this.Cracks.enabled = true;
                    }
                    else if (o < 9)
                    {
                        this.Sparks.Play();
                        this.SparksSound.Play();
                        this.SparksLight.intensity = 8.0f;
                    }
                })
                .AddTo(this);

            this.levelObject.Strength
                .First(o => o <= 0)
                .Subscribe(o =>
                {
                    this.GunRoot.transform.rotation = Quaternion.Euler(0, 0, 15);
                    this.GetComponentsInChildren<SpriteRenderer>().ForEach(oo => oo.color = Color.gray);
                    this.GetComponentInChildren<Lazer>().DestroyGameObject();
                    this.Sensor.Destroy();
                    this.Destroy();
                })
                .AddTo(this);

            if (this.levelObject.Strength.Value < 1)
            {
                return;
            }

            this.Sensor.DetectedObjects.ObserveAdd().Subscribe(o =>
            {
                if (!this.target.Value)
                {
                    this.target.Value = o.Value;
                    this.Terminal.Value.PrintAndHideAsync("Enemy detected... Exterminate!", 500, 2000, true).Forget();
                }
            }).AddTo(this);

            this.Sensor.DetectedObjects.ObserveRemove().Subscribe(o =>
            {
                if (this.target.Value == o.Value)
                {
                    this.target.Value = null;
                }
            }).AddTo(this);


            this.target.Subscribe(value =>
            {
                if (value)
                {
                    this.Alarm.Play();
                    this.Fire(value).Forget();
                    this.shooting = Observable.Interval(TimeSpan.FromSeconds(this.Speed)).
                        Subscribe(o => this.Fire(value).Forget());
                }

                if (!value)
                {
                    this.shooting?.Dispose();
                }
            }).AddTo(this);
        }

        private async UniTask Fire(GameObject value)
        {
            this.GunRoot.transform.LookAt2D(value.transform.position);
            await Task.Delay(300);

            foreach (var animator in this.recoilAnimators)
            {
                animator.Move();
            }

            this.gun.Shoot(Vector3.zero);
        }

        private void OnDestroy()
        {
            this.shooting?.Dispose();
        }
    }
}
