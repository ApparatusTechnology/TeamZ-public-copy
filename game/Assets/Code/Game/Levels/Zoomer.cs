using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Players;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Levels
{
    class Zoomer : MonoBehaviour
    {
        public float ScaleFactor;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponent<CharacterControllerScript>();

            if (player)
            {
                var newScale = player.transform.localScale;

                newScale.x *= this.ScaleFactor;
                newScale.y *= this.ScaleFactor;

                player.transform.localScale = newScale;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var player = collision.GetComponent<CharacterControllerScript>();

            if (player)
            {
                var newScale = player.transform.localScale;

                newScale.x /= this.ScaleFactor;
                newScale.y /= this.ScaleFactor;

                player.transform.localScale = newScale;
            }
        }
    }
}
