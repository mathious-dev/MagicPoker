namespace MagicPoker.Game;

public class GameEngine
{
    //Variable de stockage du contrat (interface)
    private readonly IGameDisplay _display;
    private  IHandEvaluator _handEvaluator;
    //On stocke ensuite dans les variable du constructeur pour des raisons de sécurité et surtout éviter des bugs
    public GameEngine(IGameDisplay display,IHandEvaluator handEvaluator)
    {
        _display=display;
        _handEvaluator=handEvaluator;
    }
    public void SetHandEvaluator(IHandEvaluator newEvaluator)
    {
        _handEvaluator = newEvaluator;
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
    //à modifier
    public void WhoWinGame(List<Player> allPlayers,List<Card> mainCards)
    {
        Player winner = new Player();
        var otherWinners=new List<Player>();

        foreach(Player player in allPlayers)
        {
            (winner,otherWinners)=handEvaluator.WhoWin(allPlayers,mainCards);
        }
    }
}
