using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamZ.AI.Core;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class FlyMover : Mover
    {
        public Rigidbody2D Rigidbody2D { get; private set; }

        private void Start()
        {
            this.Rigidbody2D = this.GetComponentInChildren<Rigidbody2D>();
        }

        public float MaxFlyHeight = 2;
        public float MinFlyHeight = 1;

        public async override UniTask MoveAsync(IEnumerable<Vector3> path, Func<bool> cancel = null)
        {
            if (!this || (cancel?.Invoke() ?? false))
            {
                return;
            }

            IEnumerator MoveCoroutine(Vector3 targetPosition)
            {

                var currentPosition = this.transform.position;
                while (Vector3.Distance(currentPosition, targetPosition) > 0.5)
                {
                    if (!this.Rigidbody2D || (cancel?.Invoke() ?? false))
                    {
                        yield break;
                    }

                    currentPosition = this.transform.position;
                    var direction = (targetPosition - currentPosition).normalized;

                    this.Rigidbody2D.velocity = this.Speed * direction;
                    yield return null;
                }
            }

            foreach (var point in path)
            {
                var adjustedHeight = Random.Range(this.MinFlyHeight, this.MaxFlyHeight);
                var adjustedPosition = point + new Vector3(0, adjustedHeight, 0);
                await Observable.FromCoroutine(o => MoveCoroutine(adjustedPosition)).First();

                if (cancel?.Invoke() ?? false)
                {
                    if (this.Rigidbody2D)
                    {
                        this.Rigidbody2D.velocity = Vector3.zero;
                    }

                    return;
                }
            }

            this.Rigidbody2D.velocity = Vector2.zero;
        }
    }
}