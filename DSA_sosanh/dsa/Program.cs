using System.Text;
class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string bin = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(bin, "input.html");


        // Tạo file HTML lớn để test
        HTMLTestCaseGenerator.GenerateLargeHTMLFile(path, 5000);

        // Tạo file HTML phức tạp để test
        // HTMLTestCaseGenerator.GenerateComplexHTMLFile(path, 500);

        // Test sai 1
        // HTMLTestCaseGenerator.GenerateWrongClosingTag(path);

        // Test sai 2
        // HTMLTestCaseGenerator.GenerateMissingClosingTag(path);

        // Test sai 3
        // HTMLTestCaseGenerator.GenerateMissingOpeningTag(path);

        // Test sai 4
        // HTMLTestCaseGenerator.GenerateWrongNesting(path);

        // Test sai 5
        // HTMLTestCaseGenerator.GenerateClosingTagWithoutOpening(path);

        // Test sai tổng hợp
        // HTMLTestCaseGenerator.GenerateMixedErrors(path);

        AbstractFormData form = new HTMLFileForm(path);
        string html = form.Input();

        HTMLParserSolution1 s1 = new HTMLParserSolution1();
        HTMLParserSolution2 s2 = new HTMLParserSolution2();

        Timing timer = new Timing();

                // Biến lưu kết quả trung gian để truyền sang bước sau
        MyQueue s1_Queue = null;
        List<string> s1_Tags = null;
        List<string> s2_Tags = null;

        Console.WriteLine("{0,-30} | {1,-15} | {2,-15}", "THUẬT TOÁN (STEP)", "SOLUTION 1 (ms)", "SOLUTION 2 (ms)");
        Console.WriteLine(new string('-', 70));


        // SO SÁNH BƯỚC 1: CHUYỂN ĐỔI DỮ LIỆU (PRE-PROCESS)
        
        // Sol 1: Chuyển String -> Queue từng ký tự
        timer.StartTime();
        s1_Queue = s1.CharToQueue(html);
        timer.StopTime();
        double t1_Step0 = timer.Result().TotalMilliseconds;

        // Sol 2: Không cần bước này (0ms)
        double t2_Step0 = 0;

        Console.WriteLine("{0,-30} | {1,-15:F4} | {2,-15:F4}", "1. Convert String->Queue", t1_Step0, t2_Step0);


        // SO SÁNH BƯỚC 2: TÁCH THẺ (TOKENIZING)
        
        // Sol 1: Duyệt Queue để tách thẻ
        // Lưu ý: Phải clone Queue hoặc tạo mới vì Dequeue sẽ làm rỗng hàng đợi
        MyQueue tempQ = s1.CharToQueue(html); 
        timer.StartTime();
        s1_Tags = s1.ExtractTags(tempQ);
        timer.StopTime();
        double t1_Step1 = timer.Result().TotalMilliseconds;

        // Sol 2: Duyệt String (Sliding Window)
        timer.StartTime();
        s2_Tags = s2.SlidingTagScan(html);
        timer.StopTime();
        double t2_Step1 = timer.Result().TotalMilliseconds;

        Console.WriteLine("{0,-30} | {1,-15:F4} | {2,-15:F4}", "2. Extract Tags", t1_Step1, t2_Step1);


        // SO SÁNH BƯỚC 3: KIỂM TRA HỢP LỆ (VALIDATION)
        
        // Sol 1: Dùng Queue đảo vòng (Logic phức tạp)
        timer.StartTime();
        bool v1 = s1.ValidateTags(s1_Tags);
        timer.StopTime();
        double t1_Step2 = timer.Result().TotalMilliseconds;

        // Sol 2: Dùng Queue giả Stack (Logic clean hơn)
        timer.StartTime();
        bool v2 = s2.CheckTags(s2_Tags);
        timer.StopTime();
        double t2_Step2 = timer.Result().TotalMilliseconds;

        Console.WriteLine("{0,-30} | {1,-15:F4} | {2,-15:F4}", "3. Validate Logic", t1_Step2, t2_Step2);


        // SO SÁNH BƯỚC 4: LẤY NỘI DUNG (EXTRACTION)
        
        // Sol 1: Duyệt Queue lấy text
        MyQueue tempQ2 = s1.CharToQueue(html); // Tạo lại queue do cái cũ đã bị dequeue hết
        timer.StartTime();
        string text1 = s1.ExtractText(tempQ2);
        timer.StopTime();
        double t1_Step3 = timer.Result().TotalMilliseconds;

        // Sol 2: Duyệt String lấy text
        timer.StartTime();
        string text2 = s2.ExtractText(html);
        timer.StopTime();
        double t2_Step3 = timer.Result().TotalMilliseconds;

        Console.WriteLine("{0,-30} | {1,-15:F4} | {2,-15:F4}", "4. Extract Text", t1_Step3, t2_Step3);

        Console.WriteLine(new string('-', 70));
        
        // TỔNG KẾT
        double total1 = t1_Step0 + t1_Step1 + t1_Step2 + t1_Step3;
        double total2 = t2_Step0 + t2_Step1 + t2_Step2 + t2_Step3;
        
        Console.WriteLine("{0,-30} | {1,-15:F4} | {2,-15:F4}", "TOTAL TIME", total1, total2);

        Console.ReadLine();
    }
}


