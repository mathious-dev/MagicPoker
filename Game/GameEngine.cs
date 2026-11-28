using MagicPoker.Models;
using MagicPoker.Models.card;
using MagicPoker.Models.bot;
using MagicPoker.Interface;
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
    // public void MultiplePLayersWin(int mainPot,List<Player>othersWinners,Player winner,string? combination,List<Card>mainCards)
    // {
    //     var allWinners=new List<Player>();
    //     allWinners.AddRange(othersWinners);
    //     allWinners.Add(winner);
    //     int countPlayer=allWinners.Count();
    //     int amount=mainPot/countPlayer;
    //     int lastCoins=mainPot % countPlayer;
    //     foreach(Player player in allWinners )
    //     {
    //         player.PlayerWin(amount,combination,mainCards);
    //         _display.MultiplePLayersWinConsole(countPlayer);
    //     }
    //     if(lastCoins>0)
    //     {
    //         winner.Coin+=lastCoins;
    //         Console.WriteLine($"\n le gagnant principal {winner} reçoit les jetons restants qui sont de {lastCoins} jetons");
    //     }
    // }
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
    public void WhoWinGame(List<Player> allPlayers,List<Card> mainCards)
    {
        Player winner = new Player();
        var otherWinners=new List<Player>();
        string? combination;
        (winner,otherWinners,combination)=_handEvaluator.WhoWin(allPlayers,mainCards);
        _display.Winner(winner,combination);
        if(!otherWinners.Any())
            _display.OtherWinners(otherWinners);
    }
    public static void Init(List<Bot>bots)
    {
        Random randomLevel=new Random();
        for(int i=1;i<=GameConfig.NumberBots;i++)
        {
            int randomNumber=Random.Shared.Next(1,4);
            Bot bot=randomNumber switch
            {
                1=>new EasyBot{Name="bot"+i},
                2=>new MiddleBot{Name="bot"+i},
                _=>new HardBot{Name="bot"+i}
            };
            
            bots.Add(bot);
        }
        Console.ForegroundColor=ConsoleColor.Green;
        Console.WriteLine("\nCommencement de la partie.");
        foreach(Bot bot in bots)
        {
            Console.WriteLine($"\nLe bot {bot.Name} a rejoint la partie");
            bot.OnMoneyBet+=AddBetOnMainPot;
        }
        Console.ResetColor();
    }
    //peut etre changer de classe
    public void AddBetOnMainPot(int amountBet)
    {
        MainPot+=amountBet;
    }
}
