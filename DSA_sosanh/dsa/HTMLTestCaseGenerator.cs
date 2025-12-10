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


    // TẠO CÁC TRƯỜNG HỢP LỖI
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

    // CÁC CASE NÂNG CAO MỚI 
    // 77. Test Cấu Trúc Bảng (Table Structure)
    // Chứa các thẻ lồng nhau: table > thead > tr > th
    public static string GenerateTableTest(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<!DOCTYPE html>");
            w.WriteLine("<html>");
            w.WriteLine("<body style=\"font-family: Arial, sans-serif;\">");

            w.WriteLine("    <h2 class=\"text-center\">Bảng Điểm Sinh Viên (Demo Queue)</h2>");

            w.WriteLine("    ");
            w.WriteLine("    <table border=\"1\" cellpadding=\"10\" cellspacing=\"0\" style=\"width: 100%;\">");
            w.WriteLine("        <thead>");
            w.WriteLine("            <tr style=\"background-color: #f2f2f2;\">");
            w.WriteLine("                <th>STT</th>");
            w.WriteLine("                <th>Họ và Tên</th>");
            w.WriteLine("                <th>Điểm Số</th>");
            w.WriteLine("                <th>Ghi Chú</th>");
            w.WriteLine("            </tr>");
            w.WriteLine("        </thead>");
            w.WriteLine("        <tbody>");
            w.WriteLine("            ");
            w.WriteLine("            <tr>");
            w.WriteLine("                <td>1</td>");
            w.WriteLine("                <td><strong>Nguyễn Văn A</strong></td>");
            w.WriteLine("                <td><span style=\"color: blue;\">8.5</span></td>");
            w.WriteLine("                <td>Đậu</td>");
            w.WriteLine("            </tr>");
            
            w.WriteLine("            ");
            w.WriteLine("            <tr>");
            w.WriteLine("                <td>2</td>");
            w.WriteLine("                <td>Trần Thị <em>Bê</em></td>");
            w.WriteLine("                <td><span style=\"color: red;\">4.0</span></td>");
            w.WriteLine("                <td>");
            w.WriteLine("                    Thi lại <br> (Lý thuyết)");
            w.WriteLine("                </td>");
            w.WriteLine("            </tr>");

            w.WriteLine("            ");
            w.WriteLine("            <tr>");
            w.WriteLine("                <td>3</td>");
            w.WriteLine("                <td>Lê Văn C</td>");
            w.WriteLine("                <td>9.0</td>");
            w.WriteLine("                <td><input type=\"checkbox\" checked disabled> Xuất sắc</td>");
            w.WriteLine("            </tr>");
            w.WriteLine("        </tbody>");
            w.WriteLine("    </table>");

            w.WriteLine("</body>");
            w.WriteLine("</html>");
        }
        Console.WriteLine(">> Đã tạo file: Table Test (input.html)");
        return filePath;
    }

    // 88. Test Trang Blog (Blog Page)
    // Chứa văn bản xen kẽ thẻ, thẻ tự đóng (img, hr, input), attributes phức tạp
    public static string GenerateBlogTest(string filePath)
    {
        using (StreamWriter w = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            w.WriteLine("<!DOCTYPE html>");
            w.WriteLine("<html lang=\"vi\">");
            w.WriteLine("<body>");

            w.WriteLine("    <div id=\"blog-wrapper\" class=\"container\">");
            
            w.WriteLine("        ");
            w.WriteLine("        <header style=\"border-bottom: 1px solid #ccc;\">");
            w.WriteLine("            <h1 class=\"post-title\">Tìm hiểu về <i>Queue</i> và <i>Stack</i></h1>");
            w.WriteLine("            <p class=\"meta\">Đăng bởi <b style=\"color: green;\">Admin</b> lúc <time>10:00 AM</time></p>");
            w.WriteLine("        </header>");

            w.WriteLine("        <hr> ");

            w.WriteLine("        ");
            w.WriteLine("        <article>");
            w.WriteLine("            <p>Xin chào, hôm nay chúng ta sẽ test parser HTML bằng cấu trúc <strong>Queue</strong>.</p>");
            
            w.WriteLine("            <div class=\"image-box\">");
            w.WriteLine("                ");
            w.WriteLine("                <a href=\"/images/queue-full.jpg\" title=\"Xem ảnh lớn\">");
            w.WriteLine("                    <img src=\"queue-thumb.jpg\" alt=\"Mô hình Queue\" width=\"300\" />");
            w.WriteLine("                </a>");
            w.WriteLine("                <br>");
            w.WriteLine("                <span class=\"caption\">Hình 1: Minh họa FIFO</span>");
            w.WriteLine("            </div>");

            w.WriteLine("            <h3>Đặc điểm:</h3>");
            w.WriteLine("            <p>Queue hoạt động theo nguyên tắc <em>Vào trước - Ra trước</em>.</p>");
            
            w.WriteLine("            ");
            w.WriteLine("            <div class=\"comment-section\">");
            w.WriteLine("                <h4>Để lại bình luận:</h4>");
            w.WriteLine("                <form>");
            w.WriteLine("                    <input type=\"text\" name=\"user\" placeholder=\"Tên của bạn...\">");
            w.WriteLine("                    <br>");
            w.WriteLine("                    <textarea rows=\"4\"></textarea>");
            w.WriteLine("                    <br>");
            w.WriteLine("                    <input type=\"submit\" value=\"Gửi đi\">");
            w.WriteLine("                </form>");
            w.WriteLine("            </div>");
            w.WriteLine("        </article>");

            w.WriteLine("        ");
            w.WriteLine("        <footer>");
            w.WriteLine("            <p>Copyright 2024 UEH - All rights reserved.</p>");
            w.WriteLine("        </footer>");

            w.WriteLine("    </div>");

            w.WriteLine("</body>");
            w.WriteLine("</html>");
        }
        Console.WriteLine(">> Đã tạo file: Blog Test (input.html)");
        return filePath;
    }
}

