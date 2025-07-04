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
    
    public static void Main(string[] args)
    {
        string source = File.ReadAllText(args[0]);
        Console.WriteLine(source + '\n');

        Lexer lexer = new(source);
        List<Token> tokens = lexer.ScanTokens();

        Parser parser = new(tokens);
        List<Expression> expressions = parser.Parse();

        string tokenString = GetPrettyTokenString(tokens);
        
        Console.WriteLine(tokenString);
        Console.WriteLine();
        foreach (var expr in expressions)
        {
            Console.WriteLine(expr.ToString());
        }
    }
}