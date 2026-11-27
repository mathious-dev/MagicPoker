using MagicPoker.Models;
using MagicPoker.Models.card;
using MagicPoker.Interface;
namespace MagicPoker.Game;

//On va définir cette classe mère en classe abstraite afin de pouvoir faire hériter la méthode WichCombination et obliger les classes enfants à compléter la méthode WhoWin
//On fait cette classe mère afin d'éviter de répéter le code et surtout on ne va pas obliger à toutes les classes enfants de devoir utiliser WichCombination car plus tard il n'y aura pas que le poker
public abstract class BasePokerHandEvaluator : IHandEvaluator
{
    //On nomme le type de retour afin d'avoir plus facile pour afficher et aussi pouvoir stocker dans une seule variable
    public (Rule.RuleEnum combination,List<int> listCards) WichCombination(List<Card> playerCards,List<Card> cardPlace)
    {
        return Rule.RuleFollowRoyalFlush(playerCards, cardPlace)
        ?? Rule.RuleFollowFlush(playerCards, cardPlace)
        ?? Rule.RuleSquare(playerCards, cardPlace)
        ?? Rule.RuleFull(playerCards, cardPlace)
        ?? Rule.RuleSameCardColor(playerCards, cardPlace)
        ?? Rule.RuleFollow(playerCards, cardPlace)
        ?? Rule.RuleThreeSameKind(playerCards, cardPlace)
        ?? Rule.RuleDoublePair(playerCards, cardPlace)
        ?? Rule.RulePair(playerCards, cardPlace)
        ?? Rule.RuleHighCard(playerCards, cardPlace);
    }
    public abstract (Player,List<Player>,string?) WhoWin(List<Player> listPlayers,List<Card> cardPlace);
}
