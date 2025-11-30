namespace FeatureControlHTMLtext 
{
    public class HTMLFromWords : UserControl
    {
        
        private RichTextBox txtInput;
        private RichTextBox txtOutput;
        private Button btnProcess;
        private System.ComponentModel.IContainer components = null; //khai báo container rỗng để sử dụng
        public HTMLFromWords()
        {
            InitializeComponent();
            CreateUI();


        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();// sử dụng container rỗng để tạo ra form
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";

            
        }

    
        private void CreateUI()
        {
            // Input TextBox
            txtInput = new RichTextBox();
            txtInput.Multiline = true;
            txtInput.ScrollBars = RichTextBoxScrollBars.Both;   // con lăn ngang,dọc
            txtInput.WordWrap = false;               // keep HTML formatting
            txtInput.Font = new Font("Times New Roman", 12); // good for coding text
            txtInput.Location = new Point(20, 20);
            txtInput.Size = new Size(350, 100);        // large area
            Controls.Add(txtInput); 

            // Output TextBox
            txtOutput = new RichTextBox();
            txtOutput.Multiline = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.Both; 
            txtOutput.Location = new Point(20, 180); 
            txtOutput.Size = new Size(350, 100);
            txtOutput.Font = new Font("Time New Roman", 12);
            txtOutput.ReadOnly = true;           // Make output non-editable
            Controls.Add(txtOutput);

            // Button
            btnProcess = new Button();
            btnProcess.Text = "Process";
            btnProcess.Location = new Point(20, 130);         // Under input box
            btnProcess.Size = new Size(100, 35);
            btnProcess.Click += BtnProcess_Click;
            Controls.Add(btnProcess);
        }

        // What happens when pressing the button
        private void BtnProcess_Click(object sender, EventArgs e) //hàm event 
        {
            string input = txtInput.Text;
            HTMLParserSolution2 Parsed_HTLM = new HTMLParserSolution2();
            
            txtOutput.Text =  Parsed_HTLM.Parse(input);;
        }
    }
}

