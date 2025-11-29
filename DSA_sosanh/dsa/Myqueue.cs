public class Node
{
    public object data;
    public Node next;
}
public class MyQueue
{
    public Node head;
    public Node tail;

    public bool IsEmpty()
    {
        return head == null;
    }

    public void Enqueue(object x)
    {
        Node n = new Node();
        n.data = x;
        if (tail == null)
            head = tail = n;
        else
        {
            tail.next = n;
            tail = n;
        }
    }

    public object Dequeue()
    {
        if (IsEmpty()) return null;
        object value = head.data;
        head = head.next;
        if (head == null) tail = null;
        return value;
    }
    public int Count()
    {
        int count = 0;
        Node current = head;
        while (current != null)
        {
            count++;
            current = current.next;
        }
        return count;
    }

    public object Peek()
    {
        if (IsEmpty()) return null;
        return head.data;
    }
}
