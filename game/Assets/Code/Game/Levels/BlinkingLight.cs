using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace TeamZ.Code.Game.Levels
{
	public class BlinkingLight : MonoBehaviour
	{
		public float Speed = 1;
		public float MaximumIntensityDelta = 0.05f;
		public bool Synchonized = true;
		public float Shift = 0;
		private Light2D currentLight;
		private float initialIntensity;

		private void Start()
		{
			this.currentLight = this.GetComponentInChildren<Light2D>();
			this.initialIntensity = this.currentLight.intensity;

			if (this.Synchonized)
			{
				return;
			}

			this.Shift = Random.Range(0, 3.14f);
		}

		private void FixedUpdate()
		{
			var scaleByTime = Mathf.Sin(this.Speed * (this.Shift + Time.time));
			var change = this.initialIntensity * scaleByTime * this.MaximumIntensityDelta;
			this.currentLight.intensity = this.initialIntensity + change;
		}
	}
}
