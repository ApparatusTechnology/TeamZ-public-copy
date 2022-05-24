using System;
using System.Collections.Generic;
using System.Threading;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.AI.Core
{
    public abstract class Mover : MonoBehaviour
    {
        public float Speed = 5;

        public abstract UniTask MoveAsync(IEnumerable<Vector3> path, Func<bool> cancel = null);
    }
}