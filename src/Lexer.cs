namespace pysharp_good;

public class SyntaxErrorException(string message) : Exception(message);

public class Lexer(string source)
{
    private readonly string _source = source;
    private int _index = 0;
    private int _col = 1;
    private int _row = 1;

    private readonly List<Token> _tokens = new();

    private const char EOL_CHAR = '\n';

    public List<Token> ScanTokens()
    {
        while (!IsEOF())
        {
            char c = PeekCurrent();
            
            // the skipable
            if (char.IsWhiteSpace(c) && c != '\n') // '\n' categorized as whitespace
            { 
                ConsumeWhiteSpace();
            }

            else switch (c)
            {
                case '/' when PeekNext() == '/':
                    ConsumeLineComment();
                    break;
                case '"':
                    _tokens.Add(HandleStringToken(_col, _row));
                    break;
                default:
                {
                    if (char.IsDigit(c))
                    {
                        _tokens.Add(HandleNumericToken(_col, _row));
                    }

                    else if (char.IsLetter(c) || c == '_')
                    {
                        _tokens.Add(HandleKeywordOrIdentifier(_col, _row));
                    }

                    else
                    {
                        _tokens.Add(HandleOperatorOrPunctuator(_col, _row));
                    }

                    break;
                }
            }
        }

        _tokens.Add(new Token(TokenType.EOF, "EOF", _col, _row));
        return _tokens;
    }

    private Token HandleStringToken(int startCol, int startRow)
    {
        string stringValue = string.Empty;
        
        Advance();
        while (!IsEOF() && PeekCurrent() != '"')
        {
            if (PeekCurrent() == '\\')
            {
                Advance();
                char c = Advance();
                stringValue += c switch
                {
                    'n' => '\n',
                    't' => '\t',
                    _   => c
                };
            }
            else
            {
                stringValue += Advance();
            }
            
        }
        
        Expect('"', $"Unterminated string at {startCol}:{startRow}.");
        Advance();
        return new Token(TokenType.String, stringValue, startCol, startRow);
    }
    
    private Token HandleOperatorOrPunctuator(int startCol, int startRow)
    {
        char c = Advance();
        switch (c)
        {
            case '+': return Match('=') ? GetNewToken(TokenType.PlusEquals, "+=")      : Match('+') ? GetNewToken(TokenType.IncrementOne, "++") : GetNewToken(TokenType.Plus, "+");
            case '-': return Match('=') ? GetNewToken(TokenType.SubtractEquals, "-=")  : Match('-') ? GetNewToken(TokenType.DecrementOne, "--") : GetNewToken(TokenType.Subtract, "-");
            case '*': return Match('=') ? GetNewToken(TokenType.MultiplyEquals, "*=")  : GetNewToken(TokenType.Multiply, "*");
            case '/': return Match('=') ? GetNewToken(TokenType.DivideEquals, "/=")    : GetNewToken(TokenType.Divide, "/");
            case '%': return Match('=') ? GetNewToken(TokenType.ModuloEquals, "%=")    : GetNewToken(TokenType.Modulo, "%");
            case '^': return Match('=') ? GetNewToken(TokenType.PowerEquals, "^=")     : GetNewToken(TokenType.Power, "^");

            case '=': return Match('=') ? GetNewToken(TokenType.ComparativeEquals, "==") : GetNewToken(TokenType.DeclarativeEquals, "=");
            case '!': if (Match('='))   return GetNewToken(TokenType.NotEqual, "!=");    break;
            case '>': return Match('=') ? GetNewToken(TokenType.GreaterThanEqualTo, ">=") : GetNewToken(TokenType.GreaterThan, ">");
            case '<': return Match('=') ? GetNewToken(TokenType.LessThanEqualTo, "<=")   : GetNewToken(TokenType.LessThan, "<");

            case '(': return GetNewToken(TokenType.OpenBracket, "(");
            case ')': return GetNewToken(TokenType.CloseBracket, ")");
            case '[': return GetNewToken(TokenType.OpenSquareBracket, "[");
            case ']': return GetNewToken(TokenType.CloseSquareBracket, "]");
            case '{': return GetNewToken(TokenType.OpenBrace, "{");
            case '}': return GetNewToken(TokenType.CloseBrace, "}");
            case ',': return GetNewToken(TokenType.Comma, ",");
            case ';': return GetNewToken(TokenType.Semicolon, ";");
            case '.': return GetNewToken(TokenType.Accessor, ".");
            
            case '$': return GetNewToken(TokenType.StringInterpolation, "$");
            case '@': return GetNewToken(TokenType.Info, "@");
            
            case EOL_CHAR: return GetNewToken(TokenType.EOL, EOL_CHAR.ToString());
        }

        // uh oh
        throw new SyntaxErrorException($"Lexing failure: syntax error at {_col}:{_row} unresolved token '{c}'");
        Token GetNewToken(TokenType t, string v) => new Token(t, v, startCol, startRow);
    }
    
    private Token HandleKeywordOrIdentifier(int startCol, int startRow)
    {
        string keyword = string.Empty;

        while (char.IsLetter(PeekCurrent()) || PeekCurrent() == '_')
        {
             keyword += Advance();
        }

        TokenType tokenType = Keywords.GetValueOrDefault(keyword, TokenType.Identifier);
        return new Token(tokenType, keyword, startCol, startRow);
    }
    
    private Token HandleNumericToken(int startCol, int startRow)
    {
        string numericValue = string.Empty;
        bool hasFoundDecimalPoint = false;
        
        while (char.IsDigit(PeekCurrent()) || (PeekCurrent() == '.' && !hasFoundDecimalPoint))
        {
            char c = Advance();
            numericValue += c;
            if (c == '.')
            {
                hasFoundDecimalPoint = true;
            }
        }
        
        return new Token(hasFoundDecimalPoint ? TokenType.Float : TokenType.Integer, numericValue, startCol, startRow);
    }

    private void ConsumeWhiteSpace()
    {
        while (char.IsWhiteSpace(PeekCurrent()) && PeekCurrent() != '\n')
        {
            Advance();
        }
    }

    private void ConsumeLineComment()
    {
        while (PeekCurrent() != '\n')
        {
            Advance();
        }
    }

    private bool IsEOF() => _index >= source.Length;
    private char PeekCurrent() => IsEOF() ? '\0' : source[_index];
    private char PeekNext() => _index + 1 >= source.Length ? '\0' : source[_index + 1];

    private char Advance()
    {
        char c = source[_index++];

        if (c == EOL_CHAR)
        {
            _row++;
            _col = 1;
        }
        else
        {
            _col++;
        }

        return c;
    }

    private bool Match(char expected)
    {
        if (IsEOF() || _source[_index] != expected) { return false; }
        Advance();
        return true;
    }
    
    private void Expect(char expected, string errorMessage)
    {
        if (IsEOF() || PeekCurrent() != expected)
        {
            // absolutely no idea why here it has to be _source[_index - 1] but it does
            throw new SyntaxErrorException($"Lexing failure: syntax error at {_col}:{_row} expected: '{expected}' got: '{_source[_index - 1]}'\n{errorMessage}");
        }
    }
    
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        ["let"] = TokenType.Let,
        ["int"] = TokenType.IntegerType,
        ["bool"] = TokenType.BoolType,
        ["true"] = TokenType.Bool,
        ["false"] = TokenType.Bool,
        ["float"] = TokenType.FloatType,
        ["string"] = TokenType.StringType,
        ["fn"] = TokenType.FunctionDeclaration,
        ["if"] = TokenType.If,
        ["elif"] = TokenType.ElseIf,
        ["else"] = TokenType.Else,
        ["for"] = TokenType.ForLoop,
        ["while"] = TokenType.WhileLoop,
        ["class"] = TokenType.ClassDeclaration,
        ["extends"] = TokenType.Inheritance,
        ["return"] = TokenType.Return,
    };
}