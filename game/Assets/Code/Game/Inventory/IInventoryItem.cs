using UnityEngine;

namespace TeamZ.Code.Game.Inventory
{
    public interface IInventoryItem
    {
        string InventoryItemName { get; }

        void Use();
    }
}