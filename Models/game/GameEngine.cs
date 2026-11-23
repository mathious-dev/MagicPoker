namespace MagicPoker.Models.game;

public class GameEngine
{
    private readonly IGameEngineDisplay _display;
    public GameEngine(IGameEngineDisplay display)
    {
        _display=display;
    }
    public static void MultiplePLayersWin(int mainPot,List<Player>othersWinners,Player winner,string? combination,List<Card>mainCards)
    {
        var allWinners=new List<Player>();
        allWinners.AddRange(othersWinners);
        allWinners.Add(winner);
        int countPlayer=allWinners.Count();
        int amount=mainPot/countPlayer;
        int lastCoins=mainPot % countPlayer;
        foreach(Player player in allWinners )
        {
            player.PlayerWin(amount,combination,mainCards);
            _display.MultiplePLayersWinConsole(countPlayer);
        }
        if(lastCoins>0)
        {
            winner.Coin+=lastCoins;
            Console.WriteLine($"\n le gagnant principal {winner} reçoit les jetons restants qui sont de {lastCoins} jetons");
        }
            
    }
    public static List<Player> RemoveLoserPlayer(List<Player> players)
    {
        var newListPlayer=new List<Player>();
        foreach(Player player in players)
        {
            if(player.Coin>0)
                newListPlayer.Add(player);
            else
                _display.PlayerEliminate(player);
        }
        return newListPlayer;
    }
}
