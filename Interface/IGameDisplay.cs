using MagicPoker.Models;
using MagicPoker.Models.card;
namespace MagicPoker.Interface;

public interface IGameDisplay
{
    void PlayerEliminate(Player player);
    void Winner(Player winner,string? combination);
    void OtherWinners();
    // void MultiplePLayersWinConsole(Player player);
}
