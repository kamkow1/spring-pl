using Antlr4.Runtime.Misc;
using spli.Interpreter.Stack;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitScope([NotNull] SpringParser.ScopeContext context)
    {
        var activationRecord = new ActivationRecord();

        RuntimeStack.Push(activationRecord);

        foreach (var statement in context.statement())
            Visit(statement);
            
        return RuntimeStack.Pop();
    }
}