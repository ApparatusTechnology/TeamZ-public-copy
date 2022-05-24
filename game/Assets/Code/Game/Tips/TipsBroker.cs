using TeamZ.Code.Game.Tips.Core;
using UnityEngine;

namespace TeamZ.Code.Game.Tips
{
    public class TipsBroker : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Tip>() is Tip tip)
            {
                tip.Activate();
            }    
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Tip>() is Tip tip)
            {
                tip.Deactivate();
            }
        }
    }
}
