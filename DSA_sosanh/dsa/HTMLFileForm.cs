public class HTMLFileForm : AbstractFormData
{
    private string path;

    public HTMLFileForm(string p)
    {
        path = p;
    }

    public override string Input()
    {
        return File.ReadAllText(path);
    }

    public override void Output(string result)
    {
        Console.WriteLine("Kết quả: " + result);
    }
}