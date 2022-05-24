using UnityEngine;

namespace TeamZ.Effects
{
    public class Pendulum : MonoBehaviour
    {
        public Transform Pivot;
        public float Speed;
        public float Amplitude;

        private Vector3 pivotPosition;

        private void Start()
        {
            this.pivotPosition = this.Pivot.transform.localPosition;
        }

        void FixedUpdate()
        {
            this.transform.position += this.transform.rotation * this.pivotPosition;
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Sin(Time.time * this.Speed) * this.Amplitude));
            this.transform.position -= this.transform.rotation * this.pivotPosition;
        }
    }
}