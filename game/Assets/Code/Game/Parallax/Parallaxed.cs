using System;
using TeamZ.Code.Helpers;
using UnityEngine;

namespace TeamZ.Code.Game.Parallax
{
    public class Parallaxed : UnityEngine.MonoBehaviour
    {
        public UnityDependency<MainCamera> MainCamera;
        public float Amplitude = 10;
        public float Scale = 0.1f;

        private Vector3 initialPosition;
        private Vector3 initialLocalPosition;

        private void Start()
        {
            this.initialPosition = this.transform.position;
            this.initialLocalPosition = this.transform.localPosition;
        }

        public void Update()
        {
            if (!this.MainCamera)
            {
                return;
            }
            
            var delta = Mathf.Clamp(
                this.initialPosition.x - this.MainCamera.Value.transform.position.x,
                -this.Amplitude, this.Amplitude);
            this.transform.localPosition = new Vector3(this.initialLocalPosition.x + delta * this.Scale, this.initialLocalPosition.y, this.initialLocalPosition.z);
        }
    }
}