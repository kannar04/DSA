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

        //Solution 1
        timer.StartTime();
        string result1 = s1.Parse(html);
        timer.StopTime();
        TimeSpan time1 = timer.Result();

        //Solution 2
        timer.StartTime();
        string result2 = s2.Parse(html);
        timer.StopTime();
        TimeSpan time2 = timer.Result();

        Console.WriteLine("=== Solution 1 ===");
        Console.WriteLine("Output (rút gọn): " + result1.Substring(0, Math.Min(80, result1.Length)) + "...");
        Console.WriteLine("Time: " + time1.TotalMilliseconds + " ms");

        Console.WriteLine("\n=== Solution 2 ===");
        Console.WriteLine("Output (rút gọn): " + result2.Substring(0, Math.Min(80, result2.Length)) + "...");
        Console.WriteLine("Time: " + time2.TotalMilliseconds + " ms");

        Console.ReadLine();
    }
}

