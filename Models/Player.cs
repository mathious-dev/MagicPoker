namespace MagicPoker.Models;
using System;
public class Player
{
    public event Action<int> OnMoneyBet;
    public string Name{get;set;}
    public int Coin{get;set;}=GameConfig.StartCoins;
    public int BetOfTheRound{get;set;}=0;
    public int BetOfTheTour{get;set;}=0;
    public Card[]? Deck{get;set;}=new Card[2];
    public bool AllIn{get;set;}=false;
    public bool HasFolded{get;set;}=false;

    public void Bet(int AmountBet)
    {
        BetOfTheTour += AmountBet;  
        BetOfTheRound += AmountBet;
        Console.WriteLine($"\t\t\t\tLe joueur {this.Name} a misé {AmountBet}");
        if(AmountBet>=Coin)
            this.AllIn=true;
        Coin-=AmountBet;
        //événement
        OnMoneyBet?.Invoke(AmountBet);
    }
    public void PlayerSleep()
    {
        this.HasFolded=true;
        Console.WriteLine($"\nLe joueur {this.Name} s'est couché");
    }
    public void CheckCard()
    {
        Console.WriteLine($"\nles cartes du joueur {this.Name} sont ");
        foreach(Card card in this.Deck)
        {
            if(card.Number>10)
                Console.WriteLine($"\n{(FaceCard)card.Number} de {card.Type}");
            else
                Console.WriteLine($"\n{card.Number} de {card.Type}");
        }
    }
    public void DefaultFields()
    {
        this.BetOfTheRound=0;
        this.AllIn=false;
        this.HasFolded=false;
    }
    public void CheckBet()
    {
        Console.WriteLine($"\nVotre mise : {this.BetOfTheRound}");
    }
    
    public virtual void PlayerAllIn()
    {
        Bet(this.Coin);
        Console.WriteLine("\nVous avez tout miser !" );
        this.AllIn=true;
    }
    public void AddCoins(int amount)
    {
        this.Coin += amount;
    }
    /*
    *méthode si le joueur suit*
    *On prend en compte le fait qu'il faut soustraire ce qu'il doit miser sinon il va miser en trop*
    */
    public int FollowBet(int minBet)
    {
        int amountToCall=minBet-this.BetOfTheRound;
        if(amountToCall>=this.Coin)
            this.PlayerAllIn();
        else
        {
            Console.WriteLine($"\nLe joueur {this.Name} suit");
            Bet(amountToCall);
        }
        return amountToCall;
    }
    /*
    *Si le joueur décide de suivre en n'augmentant pas la mise*
    *Il faut aussi prendre en compte que le minimum à miser peut-être supérieur à tout ce que le joueur est capable de miser si jamais un autre joueur a beaucoup plus de jetons*
    */
    public static bool IsPlayerFollowingWithNoMoreBet(Player player,int minBet)
    {
        if(player.BetOfTheRound==minBet||player.AllIn==true)
        {
            return true;
        }
        return false;
    }
    
}
