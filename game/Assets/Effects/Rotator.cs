using System;
using UnityEngine;

namespace TeamZ.Effects
{
	public class Rotator : MonoBehaviour
	{
		public float rotationSpeed = 1;
		public bool useRandomBoost = true;
		public float randomBoostValue = 1;
		public int randomBoostSeed = 0;
		public Vector3 axis = new Vector3(1, 0, 0);

        private Transform currentTransform;
		private System.Random random;
		private Action update;

		private void Start()
		{
			this.currentTransform = this.transform;

			if (this.useRandomBoost)
			{
				this.update = () => this.currentTransform.Rotate(this.axis, Math.Max(this.rotationSpeed, this.rotationSpeed + Mathf.Sin(Time.time) * this.randomBoostValue) * Time.deltaTime);
			}
			else
			{
				this.update = () => this.currentTransform.Rotate(this.axis, this.rotationSpeed * Time.deltaTime);
			}

			this.random = new System.Random(this.randomBoostSeed);
		}

		private void Update()
		{
			this.update();
		}
	}
}
