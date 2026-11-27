using MagicPoker.Models;
using MagicPoker.Models.card;
namespace MagicPoker.Interface;

public interface IGameDisplay
{
    void PlayerEliminate(Player player);
    void Winner(Player winner,string? combination);
    void OtherWinners(List<Player> othersWinners);
    void ShowMenuOption(string[]options);
    void ExitGame();
    // void MultiplePLayersWinConsole(Player player);
}
