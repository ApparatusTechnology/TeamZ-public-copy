using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamZ.Code.Game;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Enemies.Core
{
    public abstract class Gun : MonoBehaviour
    {
        public LayerMask LayerMask;
        public abstract IObservable<IDamageable> Hits { get; }

        public abstract void Shoot(Vector3 target);
    }
}
