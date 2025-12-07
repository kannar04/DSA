public class HTMLParserSolution2
{
    public List<string> SlidingTagScan(string html)
    {
        List<string> tags = new List<string>();

        bool insideTag = false;
        string currentTag = "";

        foreach (char c in html)
        {
            if (c == '<')
            {
                insideTag = true;
                currentTag = "<";
            }
            else if (c == '>' && insideTag)
            {
                currentTag += ">";
                tags.Add(currentTag);
                insideTag = false;
            }
            else if (insideTag)
            {
                currentTag += c;
            }
        }

        return tags;
    }

    private string CleanTagName(string raw)
    {
        // BỎ < > /  rồi chỉ lấy từ đầu tới khoảng trắng đầu tiên
        raw = raw.Replace("<", "").Replace(">", "").Replace("/", "").Trim();

        int spaceIndex = raw.IndexOf(' ');
        if (spaceIndex != -1)
            raw = raw.Substring(0, spaceIndex);

        return raw;
    }

    public bool CheckTags(List<string> tags)
    {
        MyQueue queue = new MyQueue(); // queue giả stack

        foreach (var tag in tags)
        {
            // Closing tag 
            if (tag.StartsWith("</"))
            {
                if (queue.IsEmpty()) return false;

                int size = queue.Count();
                for (int i = 0; i < size - 1; i++)
                {
                    var tmp = queue.Dequeue();
                    queue.Enqueue(tmp);
                }

                // pop LIFO
                string last = CleanTagName((string)queue.Dequeue());
                string close = CleanTagName(tag);

                if (last != close) return false;
            }
            else if (!tag.EndsWith("/>")) // Opening tag
            {
                queue.Enqueue(tag);   // giữ nguyên, xử lý clean khi pop
            }
        }

        return queue.IsEmpty();
    }

    public string ExtractText(string html)
    {
        MyQueue queue = new MyQueue();
        foreach (char c in html) queue.Enqueue(c);

        bool inside = false;
    
        StringBuilder sb = new StringBuilder(); 

        while (!queue.IsEmpty())
        {
            char c = (char)queue.Dequeue();
            if (c == '<') inside = true;
            else if (c == '>') inside = false;
            else if (!inside) 
            {
                sb.Append(c); // Nhanh hơn phép cộng chuỗi gấp hàng nghìn lần
            }
        }

        return sb.ToString().Trim();
    }

    public string Parse(string html)
    {
        var tags = SlidingTagScan(html);

        if (!CheckTags(tags))
            return "Lỗi HTML không hợp lệ!";

        return ExtractText(html);
    }
}

