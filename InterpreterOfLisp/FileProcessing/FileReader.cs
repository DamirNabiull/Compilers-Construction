namespace InterpreterOfLisp.FileProcessing;

public class FileReader
{
    private readonly string _text;

    public FileReader(string path)
    {
        _text = File.ReadAllText(path).ReplaceLineEndings(" ");
    }

    public string GetText()
    {
        return _text;
    }
}