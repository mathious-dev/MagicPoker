using MagicPoker.Interface;
using MagicPoker.Models;
namespace MagicPoker;

public class GamePlay
{
    private readonly IGameDisplay _display;
    private readonly IInputPlayer _input;
    public GamePlay(IGameDisplay display,IInputPlayer input)
    {
        _display=display;
        _input=input;
        Menu();
    }
    public void Menu()
    {
        string[] options;
        int choice=1;
        int countGame=1;
        while(choice!=4)
        {
            int i=1;
            if(countGame>1)
                options=["Rejouer","Voir les combinaisons possibles","Configuration de jeu","Quitter"];
            else
                options=["Jouer","Voir les combinaisons possibles","Configuration de jeu","Quitter"];
            _display.ShowMenuOption(options);
            choice=_input.IntEnter();
            switch(choice)
            {
                case 1: StartGame();countGame++;break;
                case 2: Combinations();break;
                case 3 :EditSettings();break;
                case 4: _display.ExitGame();break;
            }
        }
    }
    public void StartGame()
    {
        
    }
    public void Combinations()
    {
        
    }
    public void EditSettings()
    {
        
    }
}
