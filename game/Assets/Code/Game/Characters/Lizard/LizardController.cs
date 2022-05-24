using UnityEngine;

namespace TeamZ.Code.Game.Characters.Lizard
{
    public class LizardController : CharacterControllerScript
    {
        // TODO: add specific Lizard properties and behavior

        protected override void Start()
        {
            this.Character = this.GetComponent<Lizard>();

            base.Start();
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
        }
    }
}