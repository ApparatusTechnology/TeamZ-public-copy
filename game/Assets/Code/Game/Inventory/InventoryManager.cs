using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TeamZ.Assets.UI.Terminal;
using TeamZ.Code.Helpers;
using UniRx.Async;

namespace TeamZ.Code.Game.Inventory
{
    public static class InventoryManager
    {
        private static List<IInventoryItem> inventoryItems = new List<IInventoryItem>();

        private static UnityDependency<Terminal> Terminal;

        public static void AddInventory(IInventoryItem item)
        {
            Terminal.Value.PrintAndHideAsync($"You received {item.InventoryItemName} in your inventory.", 500, 2000, true).Forget();

            inventoryItems.Add(item);
        }

        public static void UseInventory(int index)
        {
            inventoryItems[index].Use();
        }

        public static void RemoveInventory(int index)
        {
            inventoryItems.RemoveAt(index);
        }

        public static void UseAndRemove(int index)
        {
            UseInventory(index);
            RemoveInventory(index);
        }

        public static bool HasItem<T>()
        {
            return inventoryItems.Find(item => item is T) != null;
        }

        public static int FindIndexOfItem<T>()
        {
            return inventoryItems.FindIndex(item => item is T);
        }
    }
}