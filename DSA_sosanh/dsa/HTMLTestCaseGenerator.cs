using System;
using System.IO;
using System.Text;

public static class HTMLTestCaseGenerator
{
    // Tạo file HTML đúng chuẩn
    public static string GenerateLargeHTMLFile(string filePath, int count)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");

            for (int i = 0; i < count; i++)
            {
                w.WriteLine($"<p>Paragraph number {i}</p>");
                w.WriteLine($"<span>Span text number {i}</span>");
            }

            w.WriteLine("</body></html>");
        }

        return filePath;
    }

    // Tạo file HTML phức tạp với nhiều thẻ lồng nhau
    public static string GenerateComplexHTMLFile(string filePath, int depth)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");

            for (int i = 0; i < depth; i++)
            {
                w.WriteLine(new string(' ', i * 2) + $"<div>");
            }

            w.WriteLine(new string(' ', depth * 2) + "Content at deepest level");

            for (int i = depth - 1; i >= 0; i--)
            {
                w.WriteLine(new string(' ', i * 2) + $"</div>");
            }

            w.WriteLine("</body></html>");
        }

        return filePath;
    }

    // ==========================
    // TẠO CÁC TRƯỜNG HỢP LỖI
    // ==========================

    // 1. Sai thẻ đóng (mở <p> nhưng đóng </span>)
    public static string GenerateWrongClosingTag(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");
            w.WriteLine("<p>Hello World</span>"); // sai
            w.WriteLine("</body></html>");
        }

        return filePath;
    }

    // 2. Thiếu thẻ đóng
    public static string GenerateMissingClosingTag(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");
            w.WriteLine("<p>This paragraph has no closing tag");
            w.WriteLine("<span>This is ok</span>");
            w.WriteLine("</body></html>");
        }

        return filePath;
    }

    // 3. Thiếu thẻ mở
    public static string GenerateMissingOpeningTag(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");
            w.WriteLine("This is text without opening tag </p>");
            w.WriteLine("</body></html>");
        }

        return filePath;
    }

    // 4. Lồng sai thứ tự (vi phạm nesting)
    public static string GenerateWrongNesting(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");
            w.WriteLine("<p><span>Wrong order</p></span>");  // sai thứ tự
            w.WriteLine("</body></html>");
        }

        return filePath;
    }

    // 5. Đóng thẻ không tồn tại
    public static string GenerateClosingTagWithoutOpening(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");
            w.WriteLine("</div>"); // thẻ div chưa bao giờ được mở
            w.WriteLine("<p>OK</p>");
            w.WriteLine("</body></html>");
        }

        return filePath;
    }

    // 6. Tạo tổ hợp lỗi nhiều dạng
    public static string GenerateMixedErrors(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<html><body>");

            w.WriteLine("<p>Paragraph start");       // thiếu </p>
            w.WriteLine("<span>OK</span>");

            w.WriteLine("<div>Some text</span>");    // thẻ đóng sai
            w.WriteLine("</body>");                  // thiếu </html>

            // cố tình sai
            w.WriteLine("</unknown>");
        }

        return filePath;
    }
}
