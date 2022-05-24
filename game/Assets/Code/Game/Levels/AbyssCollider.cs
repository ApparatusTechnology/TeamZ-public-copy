using TeamZ.Code.Game.Characters;
using UnityEngine;

namespace TeamZ.Code.Game.Levels
{
    public class AbyssCollider : MonoBehaviour
    {
        public int AbyssDepth;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            var character = col.gameObject.GetComponentInParent<ICharacter>();

            if (character == null)
            {
                return;
            }

            character.TakeDamage(this.AbyssDepth, 0);

            Debug.Log("Damage is taken! -500 damage. now you have " + character.Armor + " armor and " + character.Health + " health");
        }
    }
}
