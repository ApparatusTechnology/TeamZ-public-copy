using UnityEngine;

namespace TeamZ.Code.Game.Activation.Core
{
    public class DebugActivation : MonoBehaviour, IActivable
    {
        public void Activate()
        {
            Debug.Log("yo");
        }
    }
}
