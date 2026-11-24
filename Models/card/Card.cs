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
    public Card(int number,TypeCard type)
    {
        this.Number=number;
        this.Type=type;
    }
}

