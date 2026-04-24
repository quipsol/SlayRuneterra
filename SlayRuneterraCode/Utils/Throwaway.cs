using System.Diagnostics.CodeAnalysis;

namespace SlayRuneterra.Utils;

public class Throwaway
{
    
    public bool TryCleanDivideTwo(int number, [NotNullWhen(true)] out int? result)
    {
        if (number % 2 == 0)
            result = number / 2;
        else
            result = null;
        return result != null;
    }
}