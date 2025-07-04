namespace pysharp_good;

public abstract class Expression
{
    public override string ToString() => "generic";
}

public class fuck : Expression
{
    public override string ToString() => "fuck";
}

public class EOFExpression : Expression
{
    public override string ToString() => "EOF";
}

public class IntegerExpression(int val) : Expression
{
    public int Value = val;

    public override string ToString() => $"INT: {Value}";
}

public class FloatExpression(float val) : Expression
{
    public float Value = val;
}

public class StringExpression(string val) : Expression
{
    public string Value = val;
}

public class BooleanExpression(bool val) : Expression
{
    public bool Value = val;
}

public enum VariableBinding : byte
{
    Final,
    Mutable
}

public abstract class VariableExpression(VariableBinding binding) : Expression
{
    public VariableBinding Binding = binding;

    public bool isMutable => Binding == VariableBinding.Mutable;
    public bool isFinal => Binding == VariableBinding.Final;
}

public class IdentifierExpression(string identifier) : Expression
{
    public string Identifier = identifier;
}

public class LetExpression(VariableBinding binding, IdentifierExpression identifier, Expression value) : VariableExpression(binding)
{
    public VariableBinding Binding = binding;
    public IdentifierExpression Identifier = identifier;
    public Expression Value = value;
}