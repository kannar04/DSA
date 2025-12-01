using System.Text;
using System.Windows.Forms;
using System.IO;

public class HTMLFromFile : UserControl
{
    private RichTextBox txtInput;
    private RichTextBox txtOutput;
    private Button btnProcess;
    private System.ComponentModel.IContainer components = null;

    public HTMLFromFile()
    {
        InitializeComponent();
        CreateUI();

        // Bắt sự kiện KeyDown để xử lý Ctrl + V dán file
        txtInput.KeyDown += TxtInput_KeyDown;

        // Cho phép drag & drop file
        txtInput.AllowDrop = true;
        txtInput.DragEnter += TxtInput_DragEnter;
        txtInput.DragDrop += TxtInput_DragDrop;
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "HTML from file directory";
    }

    private void CreateUI()
    {
        // Input RichTextBox
        txtInput = new RichTextBox();
        txtInput.Multiline = true;
        txtInput.ScrollBars = RichTextBoxScrollBars.Both;
        txtInput.WordWrap = false;
        txtInput.Font = new Font("Times New Roman", 12);
        txtInput.Location = new Point(20, 20);
        txtInput.Size = new Size(350, 100);
        Controls.Add(txtInput);

        // Output RichTextBox
        txtOutput = new RichTextBox();
        txtOutput.Multiline = true;
        txtOutput.ScrollBars = RichTextBoxScrollBars.Both;
        txtOutput.Location = new Point(20, 180);
        txtOutput.Size = new Size(350, 100);
        txtOutput.Font = new Font("Times New Roman", 12);
        txtOutput.ReadOnly = true;
        Controls.Add(txtOutput);

        // Button
        btnProcess = new Button();
        btnProcess.Text = "Process";
        btnProcess.Location = new Point(20, 130);
        btnProcess.Size = new Size(100, 35);
        btnProcess.Click += BtnProcess_Click;
        Controls.Add(btnProcess);
    }

    // --- Xử lý Ctrl+V khi clipboard chứa file ---
    private void TxtInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.V)
        {
            if (Clipboard.ContainsFileDropList())
            {
                var files = Clipboard.GetFileDropList();
                if (files.Count > 0)
                {
                    txtInput.Text = files[0]; // chỉ lấy file đầu tiên
                    e.SuppressKeyPress = true;
                }
            }
        }
    }

    // Xử lý Drag & Drop 
    private void TxtInput_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effect = DragDropEffects.Copy;
        else
            e.Effect = DragDropEffects.None;
    }

    private void TxtInput_DragDrop(object sender, DragEventArgs e)
    {
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files.Length > 0)
        {
            txtInput.Text = files[0]; // chỉ lấy file đầu tiên
        }
    }

    // Đọc nội dung file HTML 
    public string ReadHtmlFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("HTML file not found.", filePath);

            return File.ReadAllText(filePath, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error reading HTML file:\n" + ex.Message);
            return string.Empty;
        }
    }

    //Button click
    private void BtnProcess_Click(object sender, EventArgs e)
    {
        string input = txtInput.Text.Trim();

        // Loại bỏ dấu " nếu copy as path
        if (input.StartsWith("\"") && input.EndsWith("\""))
        {
            input = input.Substring(1, input.Length - 2);
        }

        if (string.IsNullOrEmpty(input))
        {
            txtOutput.Text = "Vui lòng nhập hoặc dán file HTML!";
            return;
        }

        if (!File.Exists(input))
        {
            txtOutput.Text = "Vui lòng nhập hoặc dán file HTML!";
            return;
        }

        HTMLParserSolution2 parser = new HTMLParserSolution2();
        txtOutput.Text = parser.Parse(ReadHtmlFile(input));
    }
}
