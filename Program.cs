using System;

namespace pysharp_good;

internal static class Program
{
    private static string GetPrettyTokenString(List<Token> tokens)
    {
        string tokenString = string.Empty;

        int currentRow = 1;
        foreach (Token t in tokens)
        {
            if (t.RowNumber > currentRow)
            {
                tokenString += '\n';
            }

            currentRow = t.RowNumber;
            tokenString += t.ToString() + ' ';
        }

        return tokenString;
    }
    
    public static void Main()
    {
        string source = File.ReadAllText("C:\\Users\\HarrisonO’Leary\\RiderProjects\\pysharp-good\\pysharp-good\\test.pys");
        Console.WriteLine(source + '\n');

        Lexer lexer = new(source);
        List<Token> tokens = lexer.ScanTokens();

        string tokenString = GetPrettyTokenString(tokens);
        Console.WriteLine(tokenString);
    }
}