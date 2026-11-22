namespace MagicPoker.Models.card;
public enum TypeCard{
        Carreau=1,
        Pique,
        Coeur,
        Trefle
    }
public enum FaceCard
{
    Valet = 11,
    Dame = 12,
    Roi = 13,
    As = 14
}   
public class Card
{
    
    public TypeCard Type{get;set;}
    private int _number;
    public int Number
    {
        get{return _number;}
        set
        {
            if(value<15&&value>0)
                _number=value;
            else
                throw new ArgumentOutOfRangeException("Le nombre doit être entre 1 et 14 compris");
        }
    }
    public Card(int number)
    {
        this.Number=number;
    }
    public static void ShowCards(IEnumerable<Card> cards)
    {
        if(cards.Any())//si c'est une liste donc les cartes sur la table
        {
            Console.WriteLine($"\nVoici les cartes sur la table\n");
            foreach(Card card in cards)
            {
                if(card.Number>10)
                    Console.WriteLine($"{(FaceCard)card.Number} de {card.Type} ");
                else
                    Console.WriteLine($" {card.Number} de {card.Type}");
            }
        }
        else if(cards!=null)//si c'est un tableau donc le Deck du joueur
        {
            Console.WriteLine($"\nVoici les cartes du joueur\n");
            foreach(Card card in cards)
            {
                if(card.Number>10)
                    Console.WriteLine($"{(FaceCard)card.Number} de {card.Type} ");
                else
                    Console.WriteLine($" {card.Number} de {card.Type}");
            }
        }
        
    }
}

