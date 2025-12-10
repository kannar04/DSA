using System.Text;

public class HTMLParserSolution2
{
    // 1. QUÉT THẺ THEO CƠ CHẾ TRƯỢT (SLIDING WINDOW)
    // Không cần chuyển sang Queue, duyệt string trực tiếp cho nhanh.
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

    public string CleanTagName(string raw)
    {
        raw = raw.Replace("<", "").Replace(">", "").Replace("/", "").Trim();
        int spaceIndex = raw.IndexOf(' ');
        if (spaceIndex != -1)
            raw = raw.Substring(0, spaceIndex);
        return raw.ToLower();
    }

    //Xử lý thẻ void
    public bool IsVoidTag(string name)
    {
        string[] voids = { "br", "hr", "img", "input", "meta", "link" };
        foreach (string v in voids) if (v == name) return true;
        return false;
    }

    // 2. KIỂM TRA TÍNH HỢP LỆ DÙNG 1 QUEUE (ROTATION TECH)
    // Cải tiến: Chỉ dùng 1 Queue duy nhất để giả lập Stack.
    public bool CheckTags(List<string> tags)
    {
        MyQueue queue = new MyQueue(); 

        foreach (var tag in tags)
        {
            // [NEW] Bỏ qua DOCTYPE và Comment
            if (tag.StartsWith("<!")) continue;

            string cleanName = CleanTagName(tag);
            
            if (tag.EndsWith("/>") || IsVoidTag(cleanName)) continue;

            if (tag.StartsWith("</"))
            {
                if (queue.IsEmpty()) return false;

                // KỸ THUẬT XOAY VÒNG (ROTATION):
                // Muốn lấy thằng cuối? Lấy đầu ném xuống đuôi n-1 lần.
                // Lúc này thằng cuối sẽ trôi lên đầu -> Dequeue ra dùng luôn.
                int size = queue.Count();
                for (int i = 0; i < size - 1; i++)
                {
                    queue.Enqueue(queue.Dequeue());
                }

                string last = (string)queue.Dequeue();

                if (last != cleanName) return false;
            }
            else 
            {
                queue.Enqueue(cleanName);   
            }
        }
        return queue.IsEmpty();
    }

    // 3. TỐI ƯU HÓA TRÍCH XUẤT VĂN BẢN (STRINGBUILDER)
    // Dùng StringBuilder ghép chuỗi nhanh hơn cộng string (+) bình thường
    public string ExtractText(string html)
    {
        StringBuilder sb = new StringBuilder(); 
        bool inside = false;
        foreach (char c in html) 
        {
            if (c == '<') inside = true;
            else if (c == '>') inside = false;
            else if (!inside) sb.Append(c);
        }
        return sb.ToString().Trim();
    }

    // 4. HÀM TỔNG HỢP (PARSE WRAPPER)
    // Chạy lần lượt: Lấy thẻ -> Check lỗi -> Lấy Text
    public string Parse(string html)
    {
        var tags = SlidingTagScan(html);
        if (!CheckTags(tags))
            return "Lỗi HTML không hợp lệ!";
        return ExtractText(html);
    }
}
