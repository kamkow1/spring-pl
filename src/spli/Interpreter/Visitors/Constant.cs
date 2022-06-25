using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitConstant([NotNull] SpringParser.ConstantContext context)
    {
        if (context.INTEGER_VALUE() is {} intValue)
            return int.Parse(intValue.GetText());

        if (context.FLOAT_VALUE() is {} floatValue)
            return float.Parse(floatValue.GetText());

        if (context.STRING_VALUE() is {} stringValue)
            return stringValue.GetText()
                .Replace(Char.ToString(stringValue.GetText()[stringValue.GetText().Length - 1]), "")
                .Replace(Char.ToString(stringValue.GetText()[stringValue.GetText().Length - 1]), "")
                .Replace(Char.ToString(stringValue.GetText()[0]), "")
                .Replace(Char.ToString(stringValue.GetText()[0]), "");

        if (context.BOOL_VALUE() is {} boolValue)
            return boolValue.GetText() == "True" ? true : false;

        if (context.NULL() is {} nullValue)
            return null;

        if (context.array() is {} array)
            return Visit(array);

        throw new Exception($"unknown type {context.GetText()}");
    }
}