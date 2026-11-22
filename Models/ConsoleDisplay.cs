namespace MagicPoker;

public class ConsoleDisplay
{
    public static void EveryPlayerInGameShowCard(List<Player> players)
    {
        foreach(Player player in players)
        {
            player.CheckCard();
        }
    }
}
