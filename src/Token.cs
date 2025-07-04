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
    Else,
    ForLoop,
    WhileLoop,
    ClassDeclaration,
    Inheritance,
    Identifier,
    Return,
    
    // MATHEMATICAL
    IncrementOne,
    DecrementOne,
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
    StringInterpolation,
    Info,
    String,
    Float,
    Integer,
    Bool,
    OpenBracket,
    CloseBracket,
    OpenSquareBracket,
    CloseSquareBracket,
    OpenBrace,
    CloseBrace,
    Comma,
    Accessor,
    Semicolon,
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