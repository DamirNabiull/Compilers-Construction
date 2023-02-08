using InterpreterOfLisp.Lexer;
using InterpreterOfLisp.FileProcessing;

namespace InterpreterOfLisp;
class Program
{
    static void Main(string[] args)
    {
        var fr = new FileReader(@"C:\Users\Damir\Documents\Compilers Construction\AdditionalFiles\test.lsp");
        var text = fr.GetText();
        
        var tokenizer = new Tokenizer(text);
        tokenizer.GetAllTokens();
    }
}
