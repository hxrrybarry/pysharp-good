using System.Runtime.CompilerServices;

namespace pysharp_good;

public enum DataType : byte
{
    Integer,
    Float,
    Bool,
    String,
    CustomClass,
    Stupid
}

public class Variable<T>(T value, string identifier)
{
    public T Value = value;
    public readonly DataType AbstractType = GetDataType(typeof(T));
    public readonly string Identifier = identifier;

    private static DataType GetDataType(Type t) => t switch
    {
        not null when t == typeof(int) => DataType.Integer,
        not null when t == typeof(float) => DataType.Float,
        not null when t == typeof(double) => DataType.Float,
        not null when t == typeof(decimal) => DataType.Float,
        
        not null when t == typeof(bool) => DataType.Bool,
        
        not null when t == typeof(string) => DataType.String,
        
        // unsure how I will handle CustomClasses, really default should be DataType.Stupid
        _ => DataType.CustomClass
    };
}