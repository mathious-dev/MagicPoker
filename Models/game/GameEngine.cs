namespace MagicPoker.Models.game;

public class GameEngine
{
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
            Console.WriteLine($"\n{countPlayer} joueurs ont gagné, le pot est donc partagé");
            player.PlayerWin(amount,combination,mainCards);
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
                Console.WriteLine($"\nLe joueur {player.Name} est éliminé"); 
        }
        return newListPlayer;
    }
}
