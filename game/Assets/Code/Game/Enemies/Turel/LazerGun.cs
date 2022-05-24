using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamZ.Assets.Code.Game.Enemies.Core;
using TeamZ.Code.Game;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Enemies.Turel
{
    public class LazerGun : Gun
    {
        public float FadeOutTime = 1;
        private Subject<IDamageable> hits;
        private LineRenderer lineRender;

        public override IObservable<IDamageable> Hits 
            => this.hits;

        private void Start()
        {
            this.hits = new Subject<IDamageable>().AddTo(this);
            this.lineRender = this.GetComponent<LineRenderer>();
            this.lineRender.positionCount = 2;
        }

        public override void Shoot(Vector3 target)
        {
            var position = this.transform.position;
            var direction = (target - position).normalized;

            var result = Physics2D.Raycast(position, direction, 100, this.LayerMask.value);
            var hit = result ? result.transform.position : target * 100;

            this.lineRender.SetPosition(1, this.transform.InverseTransformPoint(hit));
            var material = this.lineRender.material;
            material.SetFloat("_CurrentTime", Time.time + this.FadeOutTime);

            if(result.transform.GetComponent<IDamageable>() is IDamageable damageable)
            {
                this.hits.OnNext(damageable);
            }
        }
    }
}
