using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
     public override object? VisitEmphasizedExpression([NotNull] SpringParser.EmphasizedExpressionContext context)
    {
        return Visit(context.expression());
    }
}