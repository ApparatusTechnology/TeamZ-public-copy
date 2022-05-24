using TeamZ.Code.Game.Characters;
using UnityEngine;

namespace TeamZ.Code.Game.Boosters
{
    public class ArmorKit : MonoBehaviour
    {
        public int ArmorKitCapacity;

        void OnTriggerEnter2D(Collider2D col)
        {
            var character = col.gameObject.GetComponentInParent<ICharacter>();

            if (character == null)
            {
                return;
            }

            character.TakeArmor(this.ArmorKitCapacity);
        }
    }
}
