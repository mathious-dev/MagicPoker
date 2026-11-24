namespace MagicPoker.Models.game;

public class GameEngine
{
    private readonly IGameDisplay _display;
    public GameEngine(IGameDisplay display)
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
    //à modifier
    public void WhoWin(List<Player> allPlayers,List<Card> mainCards)
    {
        string combination;
        Player winner = null;
        var winners=new List<Player>();
        foreach(Player player in allPlayers)
        {
            (combination,listCardsPlayer)=Rule.RuleFollowRoyalFlush(player.Deck,mainCards);
            if(CheckWinner(winners,winner,combination,mainCards))return;
        }
        
    
        (winners,winner,combination)=Rule.RuleFollowFlush(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RuleSquare(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RuleFull(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RuleSameCardColor(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RuleFollow(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RuleThreeSameKind(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RuleDoublePair(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RulePair(allPlayers,mainCards);
        if(CheckWinner(winners,winner,combination,mainCards))return;

        (winners,winner,combination)=Rule.RuleHightCard(allPlayers);
        if(CheckWinner(winners,winner,combination,mainCards))return;
    }
    //à vérifier
    public bool CheckWinner(List<Player>winners,Player winner,string combination,List<Card>cardsOnTable)
    {
        if(winner!=null)
        {
            OneWinnerOrMore(winners,winner,combination,cardsOnTable);
            return true;
        }
        return false;
    }
    public void OneWinnerOrMore(List<Player>winners,Player winner,string combination,List<Card>cardsOnTable)
    {
        if(winners.Any())
            Player.MultiplePLayersWin(MainPot,winners,winner,combination,cardsOnTable);
        else
            winner.PlayerWin(MainPot,combination,cardsOnTable);
    }
}
