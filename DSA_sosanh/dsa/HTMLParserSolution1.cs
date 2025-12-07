public class HTMLParserSolution1
{
    public MyQueue CharToQueue(string html)
    {
        MyQueue queue = new MyQueue();
        foreach (char c in html)
            queue.Enqueue(c);
        return queue;
    }

    // Tách toàn bộ tag
    public List<string> ExtractTags(MyQueue queue)
    {
        List<string> tags = new List<string>();
        string cur = "";
        bool inside = false;

        while (!queue.IsEmpty())
        {
            char c = (char)queue.Dequeue();

            if (c == '<')
            {
                inside = true;
                cur = "<";
            }
            else if (c == '>' && inside)
            {
                cur += ">";
                inside = false;
                tags.Add(cur);
            }
            else if (inside)
                cur += c;
        }

        return tags;
    }

    // Check hợp lệ bằng Queue, code dễ hiểu nhất
    public bool ValidateTags(List<string> tags)
    {
        MyQueue open = new MyQueue();    // chứa tên thẻ mở
        MyQueue temp = new MyQueue();    // phục vụ lấy phần tử cuối

        foreach (string tag in tags)
        {
            if (tag.StartsWith("</"))
            {
                string closeName = tag.Replace("</", "").Replace(">", "");

                // Không có thẻ mở nào → sai
                if (open.IsEmpty()) return false;

                // Lấy thẻ mở cuối cùng (mô phỏng stack)
                string lastOpen = null;

                // Di chuyển từ open sang temp, giữ lastOpen
                while (!open.IsEmpty())
                {
                    object x = open.Dequeue();

                    if (open.IsEmpty())       // x là phần tử cuối
                        lastOpen = (string)x;
                    else
                        temp.Enqueue(x);      // chưa phải cuối, đưa vào tạm
                }

                // Khớp sai → trả false
                if (lastOpen != closeName)
                {
                    // Restore rồi trả false
                    temp.Enqueue(lastOpen);
                    while (!temp.IsEmpty())
                        open.Enqueue(temp.Dequeue());
                    return false;
                }

                // Khớp đúng → bỏ lastOpen, restore phần còn lại
                while (!temp.IsEmpty())
                    open.Enqueue(temp.Dequeue());
            }
            else
            {
                string openName = tag.Replace("<", "").Replace(">", "");
                open.Enqueue(openName);
            }
        }

        // Còn thẻ mở dư → sai
        if (!open.IsEmpty()) return false;

        return true;
    }

    public string ExtractText(MyQueue queue)
    {
        string result = "";
        bool inside = false;

        while (!queue.IsEmpty())
        {
            char c = (char)queue.Dequeue();

            if (c == '<') inside = true;
            else if (c == '>') inside = false;
            else if (!inside) result += c;
        }

        return result.Trim();
    }

    public string Parse(string html)
    {
        MyQueue charQ = CharToQueue(html);

        // copy
        string all = "";
        while (!charQ.IsEmpty())
            all += (char)charQ.Dequeue();

        // tách tag
        List<string> tags = ExtractTags(CharToQueue(all));

        // validate
        if (!ValidateTags(tags))
            return "HTML không hợp lệ!";

        // extract text
        string text = ExtractText(CharToQueue(all));

        return text;
    }
}

