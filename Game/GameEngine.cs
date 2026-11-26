namespace MagicPoker.Game;

public class GameEngine
{
    //Variables de stockage du contrat (interface)
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
    public void MultiplePLayersWin(int mainPot,List<Player>othersWinners,Player winner,string? combination,List<Card>mainCards)
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
    public List<Player> RemoveLoserPlayer(List<Player> players)
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
    public static void WhoWinGame(List<Player> allPlayers,List<Card> mainCards)
    {
        Player winner = new Player();
        var otherWinners=new List<Player>();
        var combination;
        (winner,otherWinners,combination)=_handEvaluator.WhoWin(allPlayers,mainCards);
        _display.Winner(winner,combination);
        if(!otherWinners.Any())
            _display.OtherWinners(othersWinners);
    }
}
