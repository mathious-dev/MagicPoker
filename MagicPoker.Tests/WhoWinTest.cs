namespace MagicPoker.Tests;

public class WhoWinTest
{
    [Fact]
    public void WinnerTest()
    {
        //Arrange
        var player1=new Player
        {
            Name="Math",
            Deck =
            [
                new(5){Type=TypeCard.Carreau},
                new(7){Type=TypeCard.Carreau}
            ]
        };
        var player2=new Player
        {
            Name="Benoit",
            Deck =
            [
                new(6){Type=TypeCard.Pique},
                new(14){Type=TypeCard.Carreau}
            ]
        };
        var listPlayers=new List<Player>();
        listPlayers.Add(player1);
        listPlayers.Add(player2);
        var listCards=new List<Card>()
        {
            new Card(14,TypeCard.Pique),
            new Card(3,TypeCard.Trefle) ,
            new Card(4,TypeCard.Coeur ) ,
            new Card(8,TypeCard.Coeur),
            new Card(9,TypeCard.Coeur) 
        };
        //Act
        var handEvaluator= new classicHandEvaluator();
        var (winner,othersWinners,combination)=handEvaluator.WhoWin(listPlayers,listCards);
        //Assert
        Assert.Equal(player1,winner);
        Assert.Equal(Rule.RuleEnum.Follow.ToString(),combination);
        Assert.Empty(othersWinners);
    }
}