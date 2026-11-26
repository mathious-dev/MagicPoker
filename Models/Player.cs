using System;

namespace MagicPoker.Models;

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
        if(AmountBet>=Coin)
            this.AllIn=true;
        Coin-=AmountBet;
        //événement
        OnMoneyBet?.Invoke(AmountBet);
    }
    public void PlayerSleep()
    {
        this.HasFolded=true;
    }
    public void DefaultFields()
    {
        this.BetOfTheRound=0;
        this.AllIn=false;
        this.HasFolded=false;
    }
    public virtual void PlayerAllIn()
    {
        Bet(this.Coin);
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
