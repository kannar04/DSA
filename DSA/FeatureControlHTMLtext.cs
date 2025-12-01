namespace FeatureControlHTMLtext
{
    public class HTMLFromWords : UserControl
    {
        private RichTextBox txtInput;
        private RichTextBox txtOutput;
        private Button btnProcess;
        private System.ComponentModel.IContainer components = null;

        public HTMLFromWords()
        {
            InitializeComponent();
            CreateUI();

            // Cho phép kéo thả file
            txtInput.AllowDrop = true;
            txtInput.DragEnter += TxtInput_DragEnter;
            txtInput.DragDrop += TxtInput_DragDrop;

            // Chặn paste file / ảnh
            txtInput.KeyDown += TxtInput_KeyDown;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "HTML From Words";
        }

        private void CreateUI()
        {
            // Input
            txtInput = new RichTextBox
            {
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                WordWrap = false,
                Font = new Font("Times New Roman", 12),
                Location = new Point(20, 20),
                Size = new Size(350, 100)
            };
            Controls.Add(txtInput);

            // Output
            txtOutput = new RichTextBox
            {
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                Location = new Point(20, 180),
                Size = new Size(350, 100),
                Font = new Font("Times New Roman", 12),
                ReadOnly = true
            };
            Controls.Add(txtOutput);

            // Button
            btnProcess = new Button();
            btnProcess.Text = "Process";
            btnProcess.Location = new Point(20, 130);         // Under input box
            btnProcess.Size = new Size(100, 35);
            btnProcess.Click += BtnProcess_Click;
            Controls.Add(btnProcess);
        }

        private bool LooksLikeFilePath(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            // Loại bỏ dấu " nếu copy as path
            if (input.StartsWith("\"") && input.EndsWith("\""))
                input = input.Substring(1, input.Length - 2).Trim();

            // Windows absolute: C:\...
            if (System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z]:\\"))
                return true;

            // Windows slash: C:/...
            if (System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z]:/"))
                return true;

            // Unix absolute: /home/... hoặc /...
            if (input.StartsWith("/"))
                return true;

            // Unix home: ~/...
            if (input.StartsWith("~/"))
                return true;

            // Relative paths
            if (input.StartsWith("./") || input.StartsWith(".\\") ||
                input.StartsWith("../") || input.StartsWith("..\\"))
                return true;

            return false;
        }

        // Button click 
        private void BtnProcess_Click(object sender, EventArgs e)
        {
            string input = txtInput.Text.Trim();

            // Loại bỏ dấu " nếu copy as path
            if (input.StartsWith("\"") && input.EndsWith("\""))
                input = input.Substring(1, input.Length - 2).Trim();

            if (LooksLikeFilePath(input))
            {
                txtOutput.Text =
                    "Có lẽ bạn đang nhầm lẫn — đây là chức năng xử lý HTML từ văn bản.\n" +
                    "Nếu muốn kiểm tra HTML từ file, vui lòng sử dụng HTML From File.";
                return;
            }

            HTMLParserSolution2 parser = new HTMLParserSolution2();
            txtOutput.Text = parser.Parse(input);
        }

        // Xử lý Drag & Drop 
        private void TxtInput_DragEnter(object sender, DragEventArgs e)
        {
            // Kiểm tra nếu dữ liệu kéo thả là văn bản
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy; // Cho phép thả văn bản
            }
            else
            {
                e.Effect = DragDropEffects.None; // Không cho phép thả dữ liệu khác
            }
        }

        private void TxtInput_DragDrop(object sender, DragEventArgs e)
        {
            // Kiểm tra và lấy văn bản từ dữ liệu kéo thả
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string droppedText = (string)e.Data.GetData(DataFormats.Text);
                txtInput.Text = droppedText;
            }
            else
            {
                txtOutput.Text =
                    "Không thể thả dữ liệu không phải văn bản!\n" +
                    "Vui lòng chỉ kéo thả văn bản.";
            }
        }

        // Chặn paste file / ảnh 
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                // Nếu clipboard chứa file
                if (Clipboard.ContainsFileDropList())
                {
                    txtOutput.Text =
                        "Không thể dán file — chức năng này chỉ xử lý văn bản.";
                    e.SuppressKeyPress = true;
                    return;
                }
            }
        }
    }
}

