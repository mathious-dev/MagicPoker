namespace MagicPoker.Models;

using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
public class Rule
{
    public enum RuleEnum
    {
        HighCard=1,//fait
        Pair,//fait
        DoublePair,//fait
        ThreeSameKind,//fait
        Follow,//fait
        Color,//fait
        Full,//fait
        Square,//fait
        FollowFlush,//fait
        RoyalFlush//fait
    }
    public static (string,List<int>) RuleHightCard(List<Card> playerCards)
    {
        int numCombination=1;
        var finalListCards=playerCards.Select(c => c.Number)
                                    .OrderByDescending(c=>c.Number)
                                    .Take(5)
                                    .ToList();
        return (((Rule.RuleEnum)numCombination).ToString(),finalListCards);
    }
    public static (string,List<int>)? RulePair(List<Card>playerCards,List<Card> cardPlace)
    {
        int numCombination=2;
        var finalListCards=GroupRuleSameNumber(playerCards,cardPlace,2);
        if(finalListCards!=null)
            return (((Rule.RuleEnum)numCombination).ToString(),finalListCards);
        return null;
    }
    public static (List<Player>,Player,string) RuleDoublePair(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=3;
        var othersWinners=new List<Player>();
        Player winner = null;
        int highestValueFirstPair=0;
        int highestValueSecondPair=0;
        int highestValueLastCardForWinner=0;
        foreach(var player in players)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,2);
            if(listSameKind.Any())
            {
                if(listSameKind.Count()==2)
                {
                    if(listSameKind.First()>highestValueFirstPair)
                    {
                        highestValueFirstPair=listSameKind.First();
                        highestValueSecondPair=listSameKind[1];
                        winner=player;
                        othersWinners.Clear();
                    }
                    else if(listSameKind.First()==highestValueFirstPair)
                    {
                        if(listSameKind.Last()>highestValueSecondPair)
                        {
                            highestValueSecondPair=listSameKind.Last();
                            winner=player;
                            othersWinners.Clear();
                        }
                        else if(listSameKind.Last()==highestValueSecondPair)
                        {
                            othersWinners.Add(player);
                        }
                    }      
                }     
            }
        }
        if(othersWinners.Any())
        {
            var allWinners=new List<Player>();
            allWinners.Add(winner);
            allWinners.AddRange(othersWinners);
            othersWinners.Clear();
            foreach(Player player in allWinners)
            {
                var allCardsForWinners=GroupCards(player.Deck,cardPlace);
                var cardWithoutSameNumber=allCardsForWinners.Select(c=>c.Number)
                        .Where(c=>c!=highestValueFirstPair&&c!=highestValueSecondPair)
                        .OrderByDescending(n => n)  
                        .SkipLast(2)
                        .First();
                if(highestValueLastCardForWinner==0)
                    highestValueLastCardForWinner=cardWithoutSameNumber;
                if(cardWithoutSameNumber>highestValueLastCardForWinner)
                {
                    highestValueLastCardForWinner=cardWithoutSameNumber;
                    othersWinners.Clear();
                    winner=player;
                }
                else if(cardWithoutSameNumber==highestValueLastCardForWinner)
                {
                    othersWinners.Add(player);
                }
                    
            }
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (string,List<int>)? RuleThreeSameKind(List<Card>playerCards,List<Card> cardPlace)
    {
        int numCombination=4;
        var finalListCards=GroupRuleSameNumber(playerCards,cardPlace,3);
        if(finalListCards!=null)
            return (((Rule.RuleEnum)numCombination).ToString(),finalListCards);
        return null;
    }
    public static (string,List<int>)? RuleSquare(List<Card>playerCards,List<Card> cardPlace)
    {
        int numCombination=8;
        var finalListCards=GroupRuleSameNumber(playerCards,cardPlace,4);
        if(finalListCards!=null)
            return (((Rule.RuleEnum)numCombination).ToString(),finalListCards);
        return null;
    }
    public static (string,List<int>)? RuleSameCardColor(List<Card>playerCards,List<Card> cardPlace)
    {
        int numCombination=6;
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listSameColor=GroupCardByColor(allCards);
        if(listSameColor!=null)
        {
            listSameColor=listSameColor.Take(5).ToList();
            return (((Rule.RuleEnum)numCombination).ToString(),listSameColor);
        }
        return null;
    }
    public static (string,List<int>)? RuleFollow(List<Card>playerCards,List<Card> cardPlace)
    {
        int numCombination=5;
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listFollow=GroupCardByFollow(allCards);
        if(listFollow!=null)
            return (((Rule.RuleEnum)numCombination).ToString(),listFollow);
        return null
    }
    public static (List<Player>,Player,string) RuleFull(List<Player>players,List<Card> cardPlace)
    {
        int numCombination=7;
        var othersWinners=new List<Player>();
        Player winner = null;
        int highestValueTriple=0;
        int highestValuePair=0;
        foreach(var player in players)
        {
            var allCards=playerCards.Concat(cardPlace).ToList();
            var allCards=GroupCards(player.Deck,cardPlace);
            var listFullPair=SearchPairOrThreeSameOrFourSame(allCards,2);
            var listFullTriple=SearchPairOrThreeSameOrFourSame(allCards,3);
            if(listFullPair.Any()&&listFullTriple.Any())
            {
                if(listFullTriple.First()>highestValueTriple)
                {
                    highestValueTriple=listFullTriple.First();
                    highestValuePair=listFullPair.First();
                    winner=player;
                    othersWinners.Clear();
                }
                else if(listFullTriple.First()==highestValueTriple)
                {
                    if(listFullPair.First()>highestValuePair)
                    {
                        highestValuePair=listFullPair.First();
                        winner=player;
                        othersWinners.Clear();
                    }
                    else if(listFullPair.First()==highestValuePair)
                        othersWinners.Add(player);
                }
            }
                
        }
        return (othersWinners,winner,((Rule.RuleEnum)numCombination).ToString());
    }
    public static (string,List<int>)? RuleFollowFlush(List<Card>playerCards,List<Card> cardPlace)
    {
        int numCombination=9;
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listFollowFlush=GroupCardByFollowFlush(allCards);
        if(listFollowFlush!=null)
            return (((Rule.RuleEnum)numCombination).ToString(),listFollowFlush);
        return null;
    }
    public static (List<Player>,Player,string) RuleFollowRoyalFlush(List<Card>playerCards,List<Card> cardPlace)
    {
        int numCombination=10;
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listFollowRoyalFlush=GroupCardByFollowRoyalFlush(allCards);
        if(listFollowRoyalFlush!=null)
            return (((Rule.RuleEnum)numCombination).ToString(),listFollowRoyalFlush);
        return null;
    }

    public static int GroupRuleSameNumber(List<Card>playerCards,List<Card> cardPlace,int amountSameKindNumber)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,amountSameKindNumber);
        if(listSameKind)
        {
            var finalListCards=higherOtherCards(allCards,listSameKind);
            return finalListCards;
        }
        return null;  
    }
    public static (List<int>,List<int>) higherOtherCards(List<Card> allCards,List<Card> listSameKind)
    {
        int amountOtherCard=5;
        amountOtherCard-=listSameKind.Count();
        int numberSameKind=listSameKind.First().Number;
        List<int> otherCard= allCards.Where(c=>c.Number!=numberSameKind) 
                                .Select(c=>c.Number)
                                .OrderByDescending(c=>c)
                                .Take(amountOtherCard)
                                .ToList();
        List<int> sameKindNumbers=listSameKind.Select(c=>c.Number)
                                            .ToList();
        return sameKindNumbers.Concat(otherCard).ToList();
    }
    public static List<int> GroupCardByColor(List<Card> cards)
    {
        //on va prendre toutes les cartes afin de pouvoir utiliser la méthode pour la suite flush
        var listCardSameColor=cards.GroupBy(g=>g.Type)
                                .Where(c=>c.Count()>=5)
                                .SelectMany(c=>c)//va ouvrir la boite "coeur" par exemple avec toutes les cartes
                                .Select(c=>c.Number)
                                .OrderByDescending(c=>c)
                                .ToList();
        return listCardSameColor;
    }
    public static List<int>? GroupCardByFollow(List<Card> cards)
    {

        var listCardFollow=cards.Select(c=>c.Number)
                                .Distinct()
                                .OrderByDescending(c=>c)
                                .ToList();
        listCardFollow=IsFollow(listCardFollow);
        return listCardFollow;
    }
    public static List<int> GroupCardByFollowFlush(List<Card> cards)
    {

        var listCardFollowFlush=GroupCardByColor(cards);
        if(listCardFollowFlush.Any())
            listCardFollowFlush=IsFollow(listCardFollowFlush);
        return listCardFollowFlush;
    }
    public static List<int>? GroupCardByFollowRoyalFlush(List<Card> cards)
    {
        var listRoyal=GroupCardByFollowFlush(cards);
        if(listRoyal.Any()&&listRoyal.First()==14)
            return listRoyal;
        return null;
    }
    
    public static void CompareLastCards(List<Player>othersWinners,List<Card>cardPlace,int highestValue,ref Player winner)
    {
        var allWinners= new List<Player>();
        List<int> bestLastCards = null;//stocker les meilleurs cartes d'un joueur à battre
        allWinners.AddRange(othersWinners.ToList());
        allWinners.Add(winner);
        othersWinners.Clear();
        foreach(Player player in allWinners)
        {
            var allCards=GroupCards(player.Deck,cardPlace);
            var allCardsWithoutSameNumber=allCards.Select(c=>c.Number)
                    .Where(c=>c!=highestValue)
                    .OrderByDescending(n => n)  
                    .SkipLast(2)
                    .ToList();
            if(bestLastCards==null)
            {
                bestLastCards=allCardsWithoutSameNumber;
                winner=player;
            }
            else
            {
                bool isBetter = false;
                bool isEqual = true;
                for(int i=0;i<allCardsWithoutSameNumber.Count();i++)
                {
                    if(allCardsWithoutSameNumber[i]>bestLastCards[i])
                    {
                        winner=player;
                        bestLastCards=allCardsWithoutSameNumber;
                        othersWinners.Clear();
                        isBetter = true;
                        isEqual = false;
                        break;
                    }
                    else if(allCardsWithoutSameNumber[i]<bestLastCards[i])
                    {
                        isEqual=false;
                        break;
                    }
                }
                if(isEqual)
                    othersWinners.Add(player); 
            }
        }
    }
    public static List<int>? IsFollow(List<int> listCardFollow)
    {
        var count=1;
        var countSinceStart=2;//on le commence à 2 car si count est à 5 il y a un break, on ne va donc jamais atteindre le countSinceStart++ et la soustraction de skip sera faussée
        //on commence à 2 aussi car on va comparer 2 cartes au début
        var previousCard=0;
        if(listCardFollow.Contains(14))
            listCardFollow.Add(1);// l'As peut valoir 1 comme il peut valoir 14 donc au dessus du roi
        for(int i=0;i<listCardFollow.Count()-1;i++)//-1 car on va toujours comparer avec le chiffre d'après
        {
            previousCard=listCardFollow[i];
            if(listCardFollow[i+1]==previousCard-1)
            {
                count++;
                
                if(count==5)
                {
                    var skip=countSinceStart-count;
                    return listCardFollow.Skip(skip)//il faut modifier la liste
                                .Take(5)
                                .ToList();
                }
            }
            else
                count=1;
            countSinceStart++;
        }
        return null;
    }
    
    public static List<int> SearchPairOrThreeSameOrFourSame(List<Card> allCards,int whereNumber)
    {
        var SameKind=allCards.GroupBy(c=>c.Number)
                        .Where(g=>g.Count()>=whereNumber)
                        .Select(g => g.Key)
                        .OrderByDescending(n=>n)
                        .ToList();
        return SameKind;
    }
    
}

