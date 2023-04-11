using InterpreterOfLisp.Lexer;
using InterpreterOfLisp.SyntaxAnalyzer;
using InterpreterOfLisp.FileProcessing;
using InterpreterOfLisp.SemanticsAnalyzer;

namespace InterpreterOfLisp;
class Program
{
    private static void Main(string[] args)
    {
        var sample = "sample_1.lsp";
        
        var dir = Directory.GetCurrentDirectory();
        var path = Path.GetFullPath(dir + @"/../AdditionalFiles/" + sample);
        
        Console.WriteLine(path); 
        var text = FileReader.GetText(path);
        
        var tokenizer = new Tokenizer(text);
        tokenizer.PrintAllTokens();
        var syntaxAnalyzer = new AstParser(tokenizer.GetAllTokens());
        syntaxAnalyzer.PrintNodes();
        var rootNode = syntaxAnalyzer.Parse();
        var semanticsAnalyzer = new Typechecker(rootNode);
        semanticsAnalyzer.TypecheckProgram();
    }
}