namespace MagicPoker.Interface;

//Cette interface est importante car on doit pouvoir mettre dans les paramètres de GameEngine la bonne classe de règle à appliquer en cas de changement de règle avec les spells
public interface IHandEvaluator
{
    (Player,List<Player>,string?) WhoWin(List<Player> listPlayers,List<Card> cardPlace);
}
