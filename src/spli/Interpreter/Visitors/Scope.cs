using Antlr4.Runtime.Misc;

namespace spli.Interpreter.Visitors;

public partial class Visitor
{
    public override object? VisitScope([NotNull] SpringParser.ScopeContext context)
    {
        var activationRecord = new ActivationRecord();

        _stack.Push(activationRecord);

        foreach (var statement in context.statement())
            Visit(statement);
            
        return _stack.Pop();
    }
}