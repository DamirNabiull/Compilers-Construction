using InterpreterOfLisp.Lexer;
using InterpreterOfLisp.FileProcessing;

namespace InterpreterOfLisp;
class Program
{
    private static void Main(string[] args)
    {
        var sample = "bad_sample.lsp";
        
        var dir = Directory.GetCurrentDirectory();
        var path = Path.GetFullPath(dir + @"..\..\..\..\..\AdditionalFiles\" + sample);
        
        Console.WriteLine(path); 
        var text = FileReader.GetText(path);
        
        var tokenizer = new Tokenizer(text);
        tokenizer.PrintAllTokens();
    }
}