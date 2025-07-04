using System.Reflection;
using System.Xml.Linq;

namespace pysharp_good;

public class Program
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

    public static void PrintExpr(Expression expr, int indent = 0)
    {
        var pad = new string(' ', indent * 2);
        var type = expr.GetType();

        Console.WriteLine($"{pad}Expr: {type.Name}");

        var fields = type.GetFields();

        foreach (var f in fields)
        {
            var value = f.GetValue(expr);
            Console.WriteLine($"{pad}  {f.Name}: {value}");

            if (value is Expression child)
            {
                PrintExpr(child, indent + 1);
            }  
        }
    }

    public static void Main(string[] args)
    {
        /*string source = File.ReadAllText(args[0]);
        Console.WriteLine(source + '\n');*/

        bool isRepl = true;
        foreach (var arg in args) {
            if (arg == "--repl") isRepl = true;
        }

        if (isRepl) {
            while (true) {
                Console.Write("\nPy#> ");
                var input = Console.ReadLine();

                Lexer lexer = new(input);
                List<Token> tokens = lexer.ScanTokens();

                Parser parser = new(tokens);
                List<Expression> expressions = parser.Parse();

                string tokenString = GetPrettyTokenString(tokens);

                foreach (var expr in expressions)
                {
                    PrintExpr(expr);
                }
                Console.WriteLine();
            }
        }
    }
}