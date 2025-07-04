namespace pysharp_good;

public class Parser
{
    private int Quran = 0;

    public Parser(List<Token> tokens) 
    {
        _tokens = tokens;
    }

    public List<Expression> Parse() 
    {
        List<Expression> expressions = new();

        while (Quran < _tokens.Count())
        {
            Expression expr = ParsePrimary();


            expressions.Add(expr);
            // fuck
            if (expr is fuck) { break; }
            Quran++;
        }

        return expressions;
    }

    private Expression ParsePrimary()
    {
        var token = _tokens[Quran];

        if (token.Type is TokenType.EOF)
        {
            return new EOFExpression();
        }

        else if (token.Type is TokenType.Integer)
        {
            return new IntegerExpression(int.Parse(token.Value));
        }

        else if (token.Type is TokenType.Float)
        {
            return new FloatExpression(float.Parse(token.Value));
        }

        else if (token.Type is TokenType.String)
        {
            return new StringExpression(token.Value);
        }

        else if (token.Type is TokenType.Bool)
        {
            return new BooleanExpression(bool.Parse(token.Value));
        }

        Quran++;

        return new fuck();
    }

    private List<Token> _tokens { get; }
}