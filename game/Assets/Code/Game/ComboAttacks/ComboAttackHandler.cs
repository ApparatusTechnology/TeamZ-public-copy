using System.Diagnostics;
using System.Linq;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Helpers.Extentions;
using UniRx;
using Debug = UnityEngine.Debug;

namespace TeamZ.Code.Game.ComboAttacks
{
    public class ComboAttackHandler
    {
        public void Init(CharacterControllerScript characterController)
        {
            var testSuperAttack = new[] { FightMode.Kick, FightMode.Kick, FightMode.Punch };

            characterController.Attacks
                .Where(o => o != FightMode.None)
                .Window(3)
                .Subscribe(attackSequence =>
                {
                    if (attackSequence.SequenceEqual(testSuperAttack))
                    {
                        Debug.Log("supper attack activated");
                    }
                })
                .AddTo(characterController);

        }
    }
}