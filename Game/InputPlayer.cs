using MagicPoker.Interface;
namespace MagicPoker;

public class InputPlayer : IInputPlayer
{
    public int IntEnter()
    {
        string? input=Console.ReadLine();
        int.TryParse(input,out int intInput);
        return intInput;
    }
    public double DoubleEnter()
    {
        string? input = Console.ReadLine();
        double.TryParse(input, out double doubleInput);
        return doubleInput;
    }
    public string StringEnter()
    {
        return Console.ReadLine() ?? string.Empty;
    }
}
