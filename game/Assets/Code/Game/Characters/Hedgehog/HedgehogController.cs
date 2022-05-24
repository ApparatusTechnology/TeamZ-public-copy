using UnityEngine;

namespace TeamZ.Code.Game.Characters.Hedgehog
{
    public class HedgehogController : CharacterControllerScript
    {
        // TODO: add specific Hedgehog properties and behavior

        protected override void Start()
        {
            this.Character = this.GetComponent<Hedgehog>();

            base.Start();
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
        }
    }
}
