namespace MagicPoker;

public class ConsoleDisplay : IGameEngineDisplay
{
    //Ajouter tous les affichages dans cette classe
    // public static void EveryPlayerInGameShowCard(List<Player> players)
    // {
    //     foreach(Player player in players)
    //     {
    //         player.CheckCard();
    //     }
    // }


    /*
    *Méthode pour qu'un joueur regarde une nouvelle fois ces cartes*
    */
    public static void PlayerCheckCard(Player player)
    {
        Console.WriteLine($"\nVos cartes sont ");
        foreach(Card card in player.Deck)
        {
            if(card.Number>10)
                Console.WriteLine($"\n{(FaceCard)card.Number} de {card.Type}");
            else
                Console.WriteLine($"\n{card.Number} de {card.Type}");
        }
    }
    public static void PlayerSleepConsole(Player player)
    {
        Console.WriteLine($"\nLe joueur {player.Name} s'est couché");
    }
    public static void CheckBet(Player player)
    {
        Console.WriteLine($"\nVotre mise : {player.BetOfTheRound}");
    }
    public static void PlayerAllInConsole()
    {
        Console.WriteLine("\nVous avez tout miser !" );
    }
    public static void PlayerBet(Player player,int amountBet)
    {
        Console.WriteLine($"\t\t\t\tLe joueur {player.Name} a misé {amountBet}");
    }
    public static void PlayerFollowBet(Player player)
    {
        Console.WriteLine($"\nLe joueur {player.Name} suit");
    }


    public static void ShowCardsPlaceOrPlayerDeck(IEnumerable<Card> cards)
    {
        if(cards.Any())//si c'est une liste donc les cartes sur la table
        {
            Console.WriteLine($"\nVoici les cartes sur la table\n");
            foreach(Card card in cards)
            {
                if(card.Number>10)
                    Console.WriteLine($"{(FaceCard)card.Number} de {card.Type} ");
                else
                    Console.WriteLine($" {card.Number} de {card.Type}");
            }
        }
        else if(cards!=null)//si c'est un tableau donc le Deck du joueur
        {
            Console.WriteLine($"\nVoici les cartes du joueur\n");
            foreach(Card card in cards)
            {
                if(card.Number>10)
                    Console.WriteLine($"{(FaceCard)card.Number} de {card.Type} ");
                else
                    Console.WriteLine($" {card.Number} de {card.Type}");
            }
        }
    }
    public void MultiplePLayersWinConsole(int countPlayer)
    {
        Console.WriteLine($"\n{countPlayer} joueurs ont gagné, le pot est donc partagé");
    }
    public void PlayerEliminate(Player player)
    {
        Console.WriteLine($"\nLe joueur {player.Name} est éliminé");
    }
}
