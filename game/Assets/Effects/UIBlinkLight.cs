using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.Effects
{
    public class UIBlinkLight : MonoBehaviour
    {
        private Image sourceImage;

        private void OnEnable()
        {
            this.sourceImage = this.GetComponent<Image>();
            this.StartCoroutine(this.Blink());
        }

        private IEnumerator Blink()
        {
            while (true)
            {
                while (this.sourceImage.color.a > 0.0f)
                {
                    var tmpColor = this.sourceImage.color;
                    tmpColor.a -= 0.1f;
                    this.sourceImage.color = tmpColor;
                    yield return null;
                }

                while (this.sourceImage.color.a < 1.0f)
                {
                    var tmpColor = this.sourceImage.color;
                    tmpColor.a += 0.1f;
                    this.sourceImage.color = tmpColor;
                    yield return null;
                }
            }
        }

        private void OnDisable()
        {
            this.StopAllCoroutines();
        }
    }
}
