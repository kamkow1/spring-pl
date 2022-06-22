namespace spli.Interpreter;

public class CallStack
{
    private Stack<ActivationRecord> _activationRecords = new();

    public void Push(ActivationRecord activationRecord)
    {
        _activationRecords.Push(activationRecord);
    }

    public ActivationRecord Pop()
    {
        return _activationRecords.Pop();
    }

    public ActivationRecord Peek()
    {
        return _activationRecords.Peek();
    }
}