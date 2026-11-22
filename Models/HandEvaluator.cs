namespace MagicPoker;

public class HandEvaluator
{
    //On nomme le type de retour afin d'avoir plus facile pour afficher et aussi pouvoir stocker dans une seule variable
    public (Rule.RuleEnum combination,List<int> listCards) WichCombination(List<Card> playerCards,List<Card> cardPlace)
    {
        return RuleRoyalFlush(playerCards, cardPlace)
        ?? RuleStraightFlush(playerCards, cardPlace)
        ?? RuleFourSameKind(playerCards, cardPlace)
        ?? RuleFull(playerCards, cardPlace)
        ?? RuleSameCardColor(playerCards, cardPlace)
        ?? RuleStraight(playerCards, cardPlace)
        ?? RuleThreeSameKind(playerCards, cardPlace)
        ?? RuleDoublePair(playerCards, cardPlace)
        ?? RulePair(playerCards, cardPlace)
        ?? RuleHighCard(playerCards, cardPlace);
    }
    public (Player,List<Player>) WhoWin(List<Player> listPlayers,List<Card> cardPlace)
    {
        Rule.RuleEnum bestCombination=0;
        var listBestCards=new List<int>();
        var othersWinners=new List<Player>();
        Player winner=null;
        
        foreach(Player player in listPlayers)
        {
            var result=WichCombination(player.Deck.ToList(),cardPlace)
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
        return (winner,othersWinners)
    }
}
