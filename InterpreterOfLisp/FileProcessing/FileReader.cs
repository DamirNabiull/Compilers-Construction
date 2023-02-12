namespace InterpreterOfLisp.FileProcessing;

public static class FileReader
{
    public static string GetText(string path)
    {
        var text = File.ReadAllText(path).ReplaceLineEndings("\n");
        return text;
    }
}