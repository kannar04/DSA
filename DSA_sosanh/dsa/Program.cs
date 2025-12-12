using System.Text;
class Program
{
    static void Main()
    {
        // 1. CẤU HÌNH CƠ BẢN (SETUP)
        Console.Clear();
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string bin = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(bin, "input.html");

        // 2. KHU VỰC TẠO DỮ LIỆU TEST
        // Muốn test trường hợp nào thì bỏ comment dòng đó rồi chạy lại

        // CÁC TRƯỜNG HỢP ĐÚNG (VALID) 

        // HTMLTestCaseGenerator.GenerateTableTest(path);
        // HTMLTestCaseGenerator.GenerateBlogTest(path);

        // Test hiệu năng: Tạo file cực lớn (15.000 dòng)
        // HTMLTestCaseGenerator.GenerateLargeHTMLFile(path, 15000);

        // Test độ sâu: Tạo file lồng nhau 500 cấp
        // HTMLTestCaseGenerator.GenerateComplexHTMLFile(path, 500);


        //  CÁC TRƯỜNG HỢP LỖI (INVALID) 

        // Lỗi 1: Sai thẻ đóng (Mở <p> nhưng đóng </span>)
        // HTMLTestCaseGenerator.GenerateWrongClosingTag(path);

        // Lỗi 2: Thiếu thẻ đóng (Mở <p> nhưng quên đóng)
        // HTMLTestCaseGenerator.GenerateMissingClosingTag(path);

        // Lỗi 3: Thiếu thẻ mở (Tự dưng có </p> mà không có mở)
        // HTMLTestCaseGenerator.GenerateMissingOpeningTag(path);

        // Lỗi 4: Lồng sai thứ tự (Vi phạm quy tắc Nesting)
        // HTMLTestCaseGenerator.GenerateWrongNesting(path);

        // Lỗi 5: Đóng thẻ không tồn tại (Đóng </div> mà chưa mở bao giờ)
        // HTMLTestCaseGenerator.GenerateClosingTagWithoutOpening(path);

        // Lỗi 6: Tổng hợp nhiều lỗi linh tinh
        // HTMLTestCaseGenerator.GenerateMixedErrors(path);
        
        
        // 3. ĐỌC FILE INPUT
        // Kiểm tra xem file input.html có tồn tại không. Nếu không có thì báo lỗi và dừng luôn.
        AbstractFormData form = new HTMLFileForm(path);
        
        if (!File.Exists(path))
        {
            Console.WriteLine("Lỗi: Không tìm thấy file input.html!");
            Console.WriteLine("Hãy bỏ comment một trong các hàm Generate... ở trên để tạo file.");
            Console.ReadLine();
            return;
        }

        string html = form.Input();

        // Khởi tạo 2 giải pháp (Solution 1 & Solution 2)
        HTMLParserSolution1 s1 = new HTMLParserSolution1();
        HTMLParserSolution2 s2 = new HTMLParserSolution2();


        // 4. KIỂM TRA TÍNH HỢP LỆ TRƯỚC
        // Dùng Solution 2 để quét nhanh xem HTML có lỗi thẻ không.
        // Nếu lỗi thì dừng chương trình, không cần so sánh hiệu năng làm gì.
        Console.WriteLine(">> Đang kiểm tra file HTML...");
        string checkResult = s2.Parse(html);

        if (checkResult.StartsWith("Lỗi"))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">> KẾT QUẢ: FILE KHÔNG HỢP LỆ (INVALID)");
            Console.ResetColor();
            Console.ReadLine();
            return; 
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(">> KẾT QUẢ: FILE HỢP LỆ (VALID)");
            Console.ResetColor();
        }


        // 5. IN KẾT QUẢ CUỐI CÙNG 
        // Hiển thị nội dung văn bản đã tách được ra màn hình cho người dùng xem.
        Console.WriteLine();
        Console.WriteLine(new string('=', 65));
        Console.WriteLine("KẾT QUẢ TRÍCH XUẤT VĂN BẢN (PREVIEW)");
        Console.WriteLine(new string('-', 65));

        // Lấy text
        string finalText = s2.ExtractText(html);

        if (string.IsNullOrEmpty(finalText))
        {
            Console.WriteLine("(Không có nội dung text nào được tìm thấy)");
        }
        else
        {
            Console.WriteLine(finalText);
        }
        
        Console.WriteLine(new string('=', 65));


        // 6. SO SÁNH HIỆU NĂNG GIỮA 2 CÁCH GIẢI 
        // Đo thời gian chạy từng bước của Solution 1 (2 Queue) và Solution 2 (1 Queue)
        Timing timer = new Timing();
        
        List<string> s1_Tags = null;
        List<string> s2_Tags = null;

        Console.WriteLine("{0,-25} | {1,-15} | {2,-15}", "CÁC BƯỚC", "SOL 1 (2-Queue)", "SOL 2 (1-Queue)");
        Console.WriteLine(new string('-', 65));

        // BƯỚC 6.1: TÁCH THẺ 
        // Đo xem thuật toán nào bóc tách thẻ HTML nhanh hơn.
        MyQueue tempQ = s1.CharToQueue(html); 
        
        timer.StartTime();
        s1_Tags = s1.ExtractTags(tempQ);
        timer.StopTime();
        double t1_S1 = timer.Result().TotalMilliseconds;

        timer.StartTime();
        s2_Tags = s2.SlidingTagScan(html);
        timer.StopTime();
        double t2_S1 = timer.Result().TotalMilliseconds;

        Console.WriteLine("{0,-25} | {1,-15:F4} | {2,-15:F4}", "2. Extract Tags", t1_S1, t2_S1);

        // BƯỚC 6.2: KIỂM TRA LOGIC 
        // So sánh 2 kỹ thuật Queue: Ping-Pong vs Rotation.
        
        // Sol 1: Kỹ thuật Ping-Pong
        timer.StartTime();
        s1.ValidateTags(s1_Tags);
        timer.StopTime();
        double t1_S2 = timer.Result().TotalMilliseconds;

        // Sol 2: Kỹ thuật Rotation 
        timer.StartTime();
        s2.CheckTags(s2_Tags);
        timer.StopTime();
        double t2_S2 = timer.Result().TotalMilliseconds;

        Console.WriteLine("{0,-25} | {1,-15:F4} | {2,-15:F4}", "3. Validate Logic", t1_S2, t2_S2);

        // BƯỚC 6.3: LẤY NỘI DUNG VĂN BẢN (EXTRACT TEXT)
        // Đo thời gian lọc bỏ thẻ để lấy text thuần.
        MyQueue tempQ2 = s1.CharToQueue(html);
        
        timer.StartTime();
        s1.ExtractText(tempQ2);
        timer.StopTime();
        double t1_S3 = timer.Result().TotalMilliseconds;

        timer.StartTime();
        s2.ExtractText(html);
        timer.StopTime();
        double t2_S3 = timer.Result().TotalMilliseconds;

        Console.WriteLine("{0,-25} | {1,-15:F4} | {2,-15:F4}", "4. Extract Text", t1_S3, t2_S3);


        // 7. TỔNG KẾT THỜI GIAN
        // Cộng tổng thời gian thực thi của từng giải pháp.
        double total1 = t1_S1 + t1_S2 + t1_S3;
        double total2 = t2_S1 + t2_S2 + t2_S3;
        
        Console.WriteLine(new string('-', 65));
        Console.WriteLine("{0,-25} | {1,-15:F4} | {2,-15:F4}", "TỔNG THỜI GIAN (ms)", total1, total2);

        Console.ReadLine();
    }
}

