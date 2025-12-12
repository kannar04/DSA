public class HTMLParserSolution1
{
    // 1. CHUYỂN ĐỔI CHUỖI INPUT THÀNH HÀNG ĐỢI KÝ TỰ (CHAR TO QUEUE)
    // Thay vì xử lý string, ném từng ký tự vào Queue để thao tác sau này.
    public MyQueue CharToQueue(string html)
    {
        MyQueue queue = new MyQueue();
        foreach (char c in html)
            queue.Enqueue(c);
        return queue;
    }

    // 2. TÁCH CÁC THẺ HTML TỪ HÀNG ĐỢI (EXTRACT TAGS)
    public List<string> ExtractTags(MyQueue charQueue)
    {
        List<string> tags = new List<string>(); 

        while (!charQueue.IsEmpty())
        {
            // PHA 1: SKIP (BỎ QUA RÁC)
            // Lấy từng ký tự ra xem, nếu chưa phải '<' thì vứt đi
            char c = (char)charQueue.Dequeue();
            
            if (c != '<') 
            {
                continue; 
            }

            // PHA 2: CAPTURE (GOM THẺ)
            string currentTag = "<";
            
            while (!charQueue.IsEmpty())
            {
                char nextChar = (char)charQueue.Dequeue();
                currentTag += nextChar;

                if (nextChar == '>')
                {
                    // Đã tìm thấy đáy của thẻ -> Thêm vào List
                    tags.Add(currentTag);
                    break; // Thoát vòng lặp con, quay lại Pha 1
                }
            }
        }
        return tags;
    }

    // 3. LÀM SẠCH VÀ LỌC THẺ (HELPER FUNCTIONS)
    // Xóa mấy dấu thừa (<, >, /) để lấy tên thẻ sạch (ví dụ: <div> -> div)
    private string CleanTagName(string raw)
    {
        string s = raw.Replace("<", "").Replace(">", "").Replace("/", "").Trim();
        int idx = s.IndexOf(' '); 
        if (idx > 0) s = s.Substring(0, idx);
        return s.ToLower();
    }

    // Check thẻ đặc biệt không cần đóng (br, img...) -> Gặp bọn này thì bỏ qua
    private bool IsVoidTag(string name)
    {
        string[] voids = { "br", "hr", "img", "input", "meta", "link" };
        foreach (string v in voids) if (v == name) return true;
        return false;
    }

    // 4. KIỂM TRA TÍNH HỢP LỆ DÙNG 2 QUEUE (PING-PONG TECH)
    // Đây là phần khó nhất: Queue là FIFO, nhưng check thẻ cần LIFO (Stack).
    // Giải pháp: Dùng 2 Queue (open & temp). Đổ qua đổ lại để lấy phần tử cuối cùng.
    public bool ValidateTags(List<string> tags)
    {
        MyQueue open = new MyQueue();    
        MyQueue temp = new MyQueue();    

        foreach (string tag in tags)
        {
            // Bỏ qua DOCTYPE và Comment ()
            if (tag.StartsWith("<!")) continue; 

            string cleanName = CleanTagName(tag);
            
            // Nếu là thẻ tự đóng thì bỏ qua
            if (tag.EndsWith("/>") || IsVoidTag(cleanName)) continue;

            if (tag.StartsWith("</")) // Thẻ đóng
            {
                if (open.IsEmpty()) return false;

                string lastOpen = null;
                
                // KỸ THUẬT PING-PONG:
                // Đổ hết từ 'open' sang 'temp' để tìm thằng cuối cùng (đáy Queue)
                while (!open.IsEmpty())
                {
                    object x = open.Dequeue();
                    if (open.IsEmpty()) lastOpen = (string)x; 
                    else temp.Enqueue(x);
                }

                // So sánh thẻ đóng hiện tại với thẻ mở vừa tìm được
                if (lastOpen != cleanName) return false;

                // Đổ ngược lại từ 'temp' về 'open' để tiếp tục vòng sau
                while (!temp.IsEmpty()) open.Enqueue(temp.Dequeue());
            }
            else // Thẻ mở
            {
                open.Enqueue(cleanName);
            }
        }
        // Nếu Queue rỗng tức là tất cả thẻ đã được đóng cặp hoàn hảo
        return open.IsEmpty();
    }

    // 5. TRÍCH XUẤT NỘI DUNG VĂN BẢN (EXTRACT TEXT)
    // Chỉ lấy chữ cái không nằm trong cặp ngoặc < >
    public string ExtractText(MyQueue queue)
    {
        string result = "";
        bool inside = false;

        // Lấy toàn bộ nội dung thô (bao gồm cả khoảng trắng thừa)
        while (!queue.IsEmpty())
        {
            char c = (char)queue.Dequeue();
            if (c == '<') inside = true;
            else if (c == '>') inside = false;
            else if (!inside) result += c;
        }

        // Tách chuỗi thô thành các dòng (Làm sạch, căn trái, xóa dòng trống)
        string[] lines = rawResult.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        string finalResult = "";
        foreach (string line in lines)
        {
            // Cắt khoảng trắng 2 đầu (Trim) -> Giúp thụt lùi về sát bên trái
            string cleanLine = line.Trim();
            
            // Nếu dòng có dữ liệu thì mới lấy
            if (cleanLine.Length > 0)
            {
                finalResult += cleanLine + "\n"; // Xuống dòng kiểu Shift+Enter
            }
        }
        return result.Trim();
    }
}

