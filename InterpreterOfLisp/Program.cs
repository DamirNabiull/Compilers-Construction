using InterpreterOfLisp.Evaluation;
using InterpreterOfLisp.Lexer;
using InterpreterOfLisp.SyntaxAnalyzer;
using InterpreterOfLisp.FileProcessing;
using InterpreterOfLisp.SemanticsAnalyzer;

namespace InterpreterOfLisp;

class Program
{
    private static readonly List<string> Samples = new List<string>
    {
        "sample_1.lsp", // 0
        "sample_3.lsp", // 1
        "sample_4.lsp", // 2
        "sample_5.lsp", // 3
        "sample_6.lsp", // 4
        "sample_7.lsp", // 5
        "code.lsp", // 6
        "lambda.lsp", // 7
        "list.lsp", // 8
        "map.lsp", // 9
        "prog.lsp", // 10
        "quote.lsp", // 11
        "while.lsp", // 12
        "test.lsp", // 13
        // BAD TESTS
        "bad_sample.lsp", // 14
        "quote_bad.lsp", // 15
        "prog_bad.lsp", // 16
        "sample_2.lsp", // 17
        "bad_sample_1.lsp" // 18
    };
    
    private static void Main(string[] args)
    {
        int index = Convert.ToInt32(Console.ReadLine());

        if (index == -1)
        {
            foreach (var sample in Samples)
            {
                var dir = Directory.GetCurrentDirectory();
                var path = Path.GetFullPath(dir + @"/../AdditionalFiles/" + sample);
        
                Console.WriteLine(path); 
                var text = FileReader.GetText(path);
        
                var tokenizer = new Tokenizer(text);
                //tokenizer.PrintAllTokens();
                var syntaxAnalyzer = new AstParser(tokenizer.GetAllTokens());
                //syntaxAnalyzer.PrintNodes();
        
                var rootNode = syntaxAnalyzer.Parse();
                var semanticsAnalyzer = new Typechecker(rootNode);
                semanticsAnalyzer.TypecheckProgram();

                Evaluator.Evaluate(rootNode);

                Console.ReadLine();
            }
        }
        else
        {
            var sample = Samples[index];
            
            var dir = Directory.GetCurrentDirectory();
            var path = Path.GetFullPath(dir + @"/../AdditionalFiles/" + sample);
        
            Console.WriteLine(path); 
            var text = FileReader.GetText(path);
        
            var tokenizer = new Tokenizer(text);
            //tokenizer.PrintAllTokens();
            var syntaxAnalyzer = new AstParser(tokenizer.GetAllTokens());
            //syntaxAnalyzer.PrintNodes();
        
            var rootNode = syntaxAnalyzer.Parse();
            var semanticsAnalyzer = new Typechecker(rootNode);
            semanticsAnalyzer.TypecheckProgram();

            Evaluator.Evaluate(rootNode);
        }
    }
}