namespace Find_Register.Models;

public class LimitedSizeStack : LinkedList<string>
{
    private const int _sizeLimit = 10;

    public void Push(string page)
    {
        if (Count == _sizeLimit) RemoveFirst();
        AddLast(page);
    }

    public string? Pop()
    {
        if (Count == 0) return null;

        var last = this.Last();
        RemoveLast();
        return last;
    }
}