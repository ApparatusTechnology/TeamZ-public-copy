using UnityEngine;

namespace TeamZ.Code.Game.Inventory
{
    public class AccessCard : MonoBehaviour, IInventoryItem
    {
        // TODO: remove this and MonoBehaviour if not needed
        void OnTriggerEnter2D(Collider2D col)
        {

        }

        public string InventoryItemName 
        { 
            get { return "Access Card"; }
        }

        public virtual void Use()
        {

        }
    }
}