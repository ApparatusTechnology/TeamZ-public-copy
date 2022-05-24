using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TeamZ.Assets.Code.Helpers
{
    public class Follower : MonoBehaviour
    {
        public Transform Transform;
        public Transform TargetToFollow;
        public float dampTime = 0.3f;
        
        private Vector3 velocity = Vector3.zero;

        public float Speed = 5;

        private void Update()
        {
            if (this.Transform && this.TargetToFollow)
            {
                this.Transform.position = Vector3.SmoothDamp(this.Transform.position, this.TargetToFollow.transform.position, ref this.velocity, this.dampTime);
            }
            else
            {
                this.gameObject.Destroy();
            }
        }
    }
}
