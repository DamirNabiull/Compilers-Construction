using InterpreterOfLisp.Lexer;
using InterpreterOfLisp.FileProcessing;

namespace InterpreterOfLisp;
class Program
{
    private static void Main(string[] args)
    {
        var text = FileReader.GetText(@"C:\Users\Damir\Documents\Compilers Construction\AdditionalFiles\test.lsp");
        
        var tokenizer = new Tokenizer(text);
        tokenizer.PrintAllTokens();
    }
}