using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.UI
{
    public class Scroller : MonoBehaviour
    {
        private ScrollRect scrollContent;

        void Start()
        {
            this.scrollContent = this.GetComponent<ScrollRect>();
            StartCoroutine("AutoScrollCoroutine");
        }

        private IEnumerator AutoScrollCoroutine()
        {
            while (this.scrollContent.verticalNormalizedPosition > 0.0f)
            {
                this.scrollContent.verticalNormalizedPosition -= 0.0008f;
                yield return null;
            }
        }
    }
}