using System.Collections;
using UnityEngine;

namespace TeamZ.Effects
{
    public class SparksBlinker : MonoBehaviour
    {
        public float BlinkPower;

        private ParticleSystem sparks;
        private Light sparksLight;
        private AudioSource audioSource;

        private static float[] audioData;

        private float MaxIntensity;
        private float MaxSparkSpeed;

        // Start is called before the first frame update
        void Start()
        {
            this.sparks = GetComponentInChildren<ParticleSystem>();
            this.sparksLight = GetComponentInChildren<Light>();
            this.audioSource = GetComponent<AudioSource>();

            this.MaxIntensity = this.sparksLight.intensity;
            this.MaxSparkSpeed = this.sparks.main.startSpeed.constant;

            int samplesCount = this.audioSource.clip.samples;
            audioData = new float[samplesCount];
            this.audioSource.clip.GetData(audioData, 0);

            this.StartCoroutine(this.FlashBySound());
        }

        private IEnumerator FlashBySound()
        {
            while (true)
            {
                if (audioData[audioSource.timeSamples] > 0)
                {
                    this.sparksLight.intensity += Time.deltaTime * this.BlinkPower;
                }
                else
                {
                    this.sparksLight.intensity -= Time.deltaTime * this.BlinkPower;
                }

                yield return null;
            }
        }
    }
}