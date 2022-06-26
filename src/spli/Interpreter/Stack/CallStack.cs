namespace spli.Interpreter.Stack;

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

    public ActivationRecord GetPreviousArOrCurrent()
    {
        if (_activationRecords.Count == 0)
            return _activationRecords.ElementAt(0);

        if (_activationRecords.Count != 0)
            return _activationRecords.ElementAt(1);
        return Peek();
    }

    public int GetLength()
    {
        return _activationRecords.Count;
    }
}