namespace pysharp_good;

public enum TokenType : byte
{
    // KEYWORDS
    Let,
    IntegerType,
    BoolType,
    FloatType,
    StringType,
    FunctionDeclaration,
    If,
    ElseIf,
    ForLoop,
    WhileLoop,
    ClassDeclaration,
    Inheritance,
    Identifier,
    Return,
    
    // MATHEMATICAL
    DeclarativeEquals,
    Plus,
    Subtract,
    Multiply,
    Divide,
    Modulo,
    Power,
    PlusEquals,
    SubtractEquals,
    MultiplyEquals,
    DivideEquals,
    ModuloEquals,
    PowerEquals,
    
    // BOOLEAN
    ComparativeEquals,
    NotEqual,
    GreaterThan,
    LessThan,
    GreaterThanEqualTo,
    LessThanEqualTo,
    
    // OTHER
    String,
    Numeric,
    BoolTrue,
    BoolFalse,
    OpenBracket,
    CloseBracket,
    OpenBrace,
    CloseBrace,
    Comma,
    Accessor,
    EOL,
    EOF,
    Stupid
}

public struct Token(TokenType type, string value, int columnNumber, int rowNumber)
{
    public readonly TokenType Type = type;
    public readonly string Value = value;

    public readonly int ColumnNumber = columnNumber;
    public readonly int RowNumber = rowNumber;

    public override string ToString() => $"[Type: TokenType.{Type}, Value: {(Value == "\n" ? "NEWLINE EOL" : Value)}, Col: {ColumnNumber}, Row: {RowNumber}]";
}