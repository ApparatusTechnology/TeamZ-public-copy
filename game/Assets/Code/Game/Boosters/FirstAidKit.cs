using TeamZ.Code.Game.Characters;
using UnityEngine;

namespace TeamZ.Code.Game.Boosters
{
    public class FirstAidKit : MonoBehaviour
    {
        public int FirstAidKitCapacity;

        void OnTriggerEnter2D(Collider2D col)
        {
            var character = col.gameObject.GetComponentInParent<ICharacter>();

            if (character == null)
            {
                return;
            }

            character.TakeHealth(this.FirstAidKitCapacity);
        }
    }
}
