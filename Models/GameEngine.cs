namespace MagicPoker;

public class GameEngine
{
    //On nomme le type de retour afin d'avoir plus facile pour afficher et aussi pouvoir stocker dans une seule variable
    public static (Rule.RuleEnum combination,List<int> listCards) WichCombination(List<Card> playerCards,List<Card> cardPlace)
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
}
