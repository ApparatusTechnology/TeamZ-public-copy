using System.Threading.Tasks;

namespace TeamZ.GameSaving
{
    public interface IGameController
    {
        Task LoadSavedGameAsync(string slotName);
        Task SaveAsync(string slotName);
    }
}