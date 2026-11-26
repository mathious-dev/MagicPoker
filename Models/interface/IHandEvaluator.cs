namespace MagicPoker.Models.interface;

public class IHandEvaluator
{
    public (Player,List<Player>) WhoWin(List<Player> listPlayers,List<Card> cardPlace);
}
