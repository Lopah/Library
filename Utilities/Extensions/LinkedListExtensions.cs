namespace Utilities.Extensions;

public static class LinkedListExtensions
{
    public static LinkedList<T> RemoveWhen<T>(this LinkedList<T> list, Func<T, bool> when)
    {
        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (when is null)
        {
            throw new ArgumentNullException(nameof(when));
        }

        var removed = new LinkedList<T>();

        var node = list.First;
        while (node != null)
        {
            var nextNode = node.Next;
            if (when(node.Value))
            {
                list.Remove(node);
                removed.AddLast(node.Value);
            }

            node = nextNode;
        }

        return removed;
    }
}