public class HTMLStringForm : AbstractFormData
{
    private string content;

    public HTMLStringForm(string s)
    {
        content = s;
    }

    public override string Input()
    {
        return content;
    }

    public override void Output(string result)
    {
        Console.WriteLine("Kết quả: " + result);
    }
}
