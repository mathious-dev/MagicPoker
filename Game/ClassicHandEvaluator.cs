using System;
using System.Collections.Generic;
using System.Linq;
using MagicPoker.Models;
using MagicPoker.Models.card;
namespace MagicPoker.Game;

public class classicHandEvaluator : BasePokerHandEvaluator
{
    
    public override (Player,List<Player>,string?) WhoWin(List<Player> listPlayers,List<Card> cardPlace)
    {
        Rule.RuleEnum bestCombination=0;
        var listBestCards=new List<int>();
        var othersWinners=new List<Player>();
        Player winner=null;
        
        foreach(Player player in listPlayers)
        {
            var result=WichCombination(player.Deck.ToList(),cardPlace);
            if(result.combination>bestCombination)
            {
                bestCombination=result.combination;
                winner=player;
                listBestCards=result.listCards;
                othersWinners.Clear();
            }
            else if(result.combination==bestCombination)
            {
                //Zip permet de comparer 2 listes
                var winnerListCards= result.listCards.Zip(listBestCards,(a,b)=>a.CompareTo(b))
                                                    .FirstOrDefault(x=>x!=0);
                if(winnerListCards>0)
                {
                    listBestCards=result.listCards;
                    winner=player;
                    othersWinners.Clear();
                }
                else if(winnerListCards==0)
                    othersWinners.Add(player);
            }
        }
        return (winner,othersWinners,bestCombination.ToString());
    }
}
