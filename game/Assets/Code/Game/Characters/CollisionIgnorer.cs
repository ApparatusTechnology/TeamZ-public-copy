using System.Collections.Generic;
using TeamZ.Code.Helpers.Extentions;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Characters
{
    public class CollisionIgnorer : MonoBehaviour
    {
        public BoolReactiveProperty Ignore = new BoolReactiveProperty(false);
        public LayerMask Mask;

        private Collider2D currentCollider;
        private HashSet<Collider2D> collidersToIgnore;

        private void Awake()
        {
            this.currentCollider = this.GetComponent<Collider2D>();
            this.collidersToIgnore = new HashSet<Collider2D>();
            this.Ignore
                .False()
                .Subscribe(_ =>
                {
                    this.collidersToIgnore
                        .ForEach(o => Physics2D.IgnoreCollision(this.currentCollider, o, false));
                    this.collidersToIgnore.Clear();
                })
                .AddTo(this);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (this.Ignore.Value && 
                this.Mask.Contains(other.gameObject.layer) && 
                !this.collidersToIgnore.Contains(other.collider))
            {
                Physics2D.IgnoreCollision(this.currentCollider, other.collider);
                this.collidersToIgnore.Add(other.collider);
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (this.Ignore.Value && 
                this.Mask.Contains(other.gameObject.layer) && 
                !this.collidersToIgnore.Contains(other.collider))
            {
                Physics2D.IgnoreCollision(this.currentCollider, other.collider);
                this.collidersToIgnore.Add(other.collider);
            }
        }
    }
}