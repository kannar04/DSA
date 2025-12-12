using System.Text;

public class HTMLParserSolution2
{
    // 1. CHUYỂN ĐỔI CHUỖI INPUT THÀNH HÀNG ĐỢI CHUỖI (STRING -> QUEUE)
    // Giúp nạp dữ liệu vào Queue để các thuật toán bên dưới có cái mà xử lý
    public MyQueue StringToQueue(string html)
    {
        MyQueue q = new MyQueue();
        foreach (char c in html) q.Enqueue(c);
        return q;
    }
    
    // 2. QUÉT THẺ THEO CƠ CHẾ TRƯỢT (SLIDING WINDOW)
    // Không cần chuyển sang Queue, duyệt string trực tiếp cho nhanh.
    public List<string> SlidingTagScan(string html)
    {
        MyQueue queue = StringToQueue(html);
        
        List<string> tags = new List<string>();
        bool insideTag = false;
        string currentTag = "";

        while (!queue.IsEmpty())
        {
            char c = (char)queue.Dequeue(); // Lấy từ đầu hàng đợi

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

    // 3. KIỂM TRA TÍNH HỢP LỆ DÙNG 1 QUEUE (ROTATION TECH)
    // Cải tiến: Chỉ dùng 1 Queue duy nhất để giả lập Stack.
    public bool CheckTags(List<string> tags)
    {
        MyQueue queue = new MyQueue(); 

        foreach (var tag in tags)
        {
            // Bỏ qua DOCTYPE và Comment
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

    // 4. TỐI ƯU HÓA TRÍCH XUẤT VĂN BẢN (STRINGBUILDER)
    // Dùng StringBuilder ghép chuỗi nhanh hơn cộng string (+) bình thường
    public string ExtractText(string html)
    {
        // Nạp vào Queue
        MyQueue queue = StringToQueue(html);
        
        StringBuilder sb = new StringBuilder(); 
        bool inside = false;
        
        // Lấy nội dung thô
        while (!queue.IsEmpty())
        {
            char c = (char)queue.Dequeue();
            
            if (c == '<') inside = true;
            else if (c == '>') inside = false;
            else if (!inside) sb.Append(c);
        }

        // Tách chuỗi thô thành các dòng (Làm sạch, căn trái, xóa dòng trống)
        string rawText = sb.ToString();
        
        // Tách dòng và loại bỏ các dòng trống ngay lập tức
        string[] lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        StringBuilder finalSb = new StringBuilder();
        foreach (string line in lines)
        {
            string cleanLine = line.Trim(); // Căn trái sát lề
            
            if (cleanLine.Length > 0)
            {
                finalSb.AppendLine(cleanLine); // AppendLine tự động thêm \n
            }
        }

        return sb.ToString().Trim();
    }

    // 5. HÀM TỔNG HỢP (PARSE WRAPPER)
    // Chạy lần lượt: Lấy thẻ -> Check lỗi -> Lấy Text
    public string Parse(string html)
    {
        var tags = SlidingTagScan(html);
        if (!CheckTags(tags))
            return "Lỗi HTML không hợp lệ!";
        return ExtractText(html);
    }
}


