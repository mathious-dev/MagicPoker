namespace MagicPoker.Models;

using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using MagicPoker.Models.card;
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
    public static (Rule.RuleEnum,List<int>) RuleHighCard(List<Card>playerCards,List<Card> cardPlace)
    {
        var finalListCards=playerCards.Concat(cardPlace)
                                    .Select(c => c.Number)
                                    .OrderByDescending(c=>c)
                                    .Take(5)
                                    .ToList();
        return (Rule.RuleEnum.HighCard,finalListCards);
    }
    public static (Rule.RuleEnum,List<int>)? RulePair(List<Card>playerCards,List<Card> cardPlace)
    {
        var finalListCards=GroupRuleSameNumber(playerCards,cardPlace,2);
        if(finalListCards!=null)
            return (Rule.RuleEnum.Pair,finalListCards);
        return null;
    }
    //à modifier
    public static (Rule.RuleEnum,List<int>)? RuleDoublePair(List<Card>playerCards,List<Card> cardPlace)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,2);
        if(listSameKind.Count()>=2)
        {
            var finalListCards=higherOtherCards(allCards,listSameKind.Take(2).ToList(),2);
            return (Rule.RuleEnum.DoublePair,finalListCards);
        }
        return null;
    }
    public static (Rule.RuleEnum,List<int>)? RuleThreeSameKind(List<Card>playerCards,List<Card> cardPlace)
    {
        var finalListCards=GroupRuleSameNumber(playerCards,cardPlace,3);
        if(finalListCards!=null)
            return (Rule.RuleEnum.ThreeSameKind,finalListCards);
        return null;
    }
    public static (Rule.RuleEnum,List<int>)? RuleSquare(List<Card>playerCards,List<Card> cardPlace)
    {
        var finalListCards=GroupRuleSameNumber(playerCards,cardPlace,4);
        if(finalListCards!=null)
            return (Rule.RuleEnum.Square,finalListCards);
        return null;
    }
    public static (Rule.RuleEnum,List<int>)? RuleSameCardColor(List<Card>playerCards,List<Card> cardPlace)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listSameColor=GroupCardByColor(allCards);
        if(listSameColor.Any())
        {
            listSameColor=listSameColor.Take(5).ToList();
            return (Rule.RuleEnum.Color,listSameColor);
        }
        return null;
    }
    public static (Rule.RuleEnum,List<int>)? RuleFollow(List<Card>playerCards,List<Card> cardPlace)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listFollow=GroupCardByFollow(allCards);
        if(listFollow!=null)
            return (Rule.RuleEnum.Follow,listFollow);
        return null;
    }
    public static (Rule.RuleEnum,List<int>)? RuleFull(List<Card>playerCards,List<Card> cardPlace)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listFullTriple=SearchPairOrThreeSameOrFourSame(allCards,3);
        if (!listFullTriple.Any())
            return null;
        int bestTriple = listFullTriple.First();
        var listFullPair=SearchPairOrThreeSameOrFourSame(allCards,2)
                        .Where(n=>n!=bestTriple)
                        .ToList();
        if(listFullPair.Any())
        {
            int bestPair=listFullPair.First();
            var finalListCards=Enumerable.Repeat(bestTriple,3).Concat(Enumerable.Repeat(bestPair,2)).ToList();
            return (Rule.RuleEnum.Full,finalListCards);
        }
        return null;
    }
    public static (Rule.RuleEnum,List<int>)? RuleFollowFlush(List<Card>playerCards,List<Card> cardPlace)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listFollowFlush=GroupCardByFollowFlush(allCards);
        if(listFollowFlush!=null)
            return (Rule.RuleEnum.FollowFlush,listFollowFlush);
        return null;
    }
    public static (Rule.RuleEnum,List<int>)? RuleFollowRoyalFlush(List<Card>playerCards,List<Card> cardPlace)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listFollowRoyalFlush=GroupCardByFollowRoyalFlush(allCards);
        if(listFollowRoyalFlush!=null)
            return (Rule.RuleEnum.RoyalFlush,listFollowRoyalFlush);
        return null;
    }

    public static List<int>? GroupRuleSameNumber(List<Card>playerCards,List<Card> cardPlace,int amountSameKindNumber)
    {
        var allCards=playerCards.Concat(cardPlace).ToList();
        var listSameKind=SearchPairOrThreeSameOrFourSame(allCards,amountSameKindNumber);
        if(listSameKind.Any())
        {
            var finalListCards=higherOtherCards(allCards,listSameKind,amountSameKindNumber);
            return finalListCards;
        }
        return null;  
    }
    public static List<int> higherOtherCards(List<Card> allCards,List<int> listSameKind,int amountSameKindNumber)
    {
        List<int> sameKindNumbers = new List<int>();
        foreach(int sameKind in listSameKind)
        {
            sameKindNumbers.AddRange(Enumerable.Repeat(sameKind,amountSameKindNumber));
        }
        int amountOtherCard=5-sameKindNumbers.Count();
        List<int> otherCard= allCards.Where(c => !listSameKind.Contains(c.Number))
                                .Select(c=>c.Number)
                                .OrderByDescending(c=>c)
                                .Take(amountOtherCard)
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
            return listCardFollowFlush=IsFollow(listCardFollowFlush);
        return null;
    }
    public static List<int>? GroupCardByFollowRoyalFlush(List<Card> cards)
    {
        var listRoyal=GroupCardByFollowFlush(cards);
        if(listRoyal != null &&listRoyal.Any()&&listRoyal.First()==14)
            return listRoyal;
        return null;
    }
    //A supprimer probablement ou déplacer
    // public static void CompareLastCards(List<Player>othersWinners,List<Card>cardPlace,int highestValue,ref Player winner)
    // {
    //     var allWinners= new List<Player>();
    //     List<int> bestLastCards = null;//stocker les meilleurs cartes d'un joueur à battre
    //     allWinners.AddRange(othersWinners.ToList());
    //     allWinners.Add(winner);
    //     othersWinners.Clear();
    //     foreach(Player player in allWinners)
    //     {
    //         var allCards=GroupCards(player.Deck,cardPlace);
    //         var allCardsWithoutSameNumber=allCards.Select(c=>c.Number)
    //                 .Where(c=>c!=highestValue)
    //                 .OrderByDescending(n => n)  
    //                 .SkipLast(2)
    //                 .ToList();
    //         if(bestLastCards==null)
    //         {
    //             bestLastCards=allCardsWithoutSameNumber;
    //             winner=player;
    //         }
    //         else
    //         {
    //             bool isBetter = false;
    //             bool isEqual = true;
    //             for(int i=0;i<allCardsWithoutSameNumber.Count();i++)
    //             {
    //                 if(allCardsWithoutSameNumber[i]>bestLastCards[i])
    //                 {
    //                     winner=player;
    //                     bestLastCards=allCardsWithoutSameNumber;
    //                     othersWinners.Clear();
    //                     isBetter = true;
    //                     isEqual = false;
    //                     break;
    //                 }
    //                 else if(allCardsWithoutSameNumber[i]<bestLastCards[i])
    //                 {
    //                     isEqual=false;
    //                     break;
    //                 }
    //             }
    //             if(isEqual)
    //                 othersWinners.Add(player); 
    //         }
    //     }
    // }
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

