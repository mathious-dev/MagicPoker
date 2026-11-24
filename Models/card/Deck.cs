namespace MagicPoker.Models.card;

public class Deck
{
    public static List<Card> GeneralDeckCard()
    {
        var listCards=new List<Card>();
        for(int i=2;i<=14;i++)//car l'As va valoir à la fois 1 et 14
        {
            for(int j=1;j<5;j++)
            {
                listCards.Add(new Card(i,(TypeCard)j));
            }
        }
        listCards=listCards.OrderBy(c=>Random.Shared.Next()).ToList();
        return listCards;
    }
}
