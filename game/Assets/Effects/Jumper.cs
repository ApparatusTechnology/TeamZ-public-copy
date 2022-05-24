using UnityEngine;

namespace TeamZ.Effects
{
    public class Jumper : MonoBehaviour
    {
        private Vector3 startLocationPosition;
        private float randomShift;
        public float InitialOffset;
        public float JumpHeight;
        public float Speed;

        private void Start()
        {
            this.startLocationPosition = this.transform.localPosition;
            this.randomShift = Random.value;
        }

        private void Update()
        {
            var localPosition = this.startLocationPosition;
            localPosition.y = localPosition.y + this.InitialOffset + Mathf.Cos((Time.realtimeSinceStartup + this.randomShift) * this.Speed) * this.JumpHeight;

            this.transform.localPosition = localPosition;
        }
    }
}
