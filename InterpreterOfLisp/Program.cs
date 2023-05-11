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
        "print_0.lsp", // 0
        "print_1.lsp", // 1
        "sample_1.lsp", // 2
        "sample_2.lsp", // 3
        "sample_3.lsp", // 4
        "sample_4.lsp", // 5
        "sample_5.lsp", // 6
        "sample_6.lsp", // 7
        "sample_7.lsp", // 8
        "code.lsp", // 9
        "lambda.lsp", // 10
        "list.lsp", // 11
        "map.lsp", // 12
        "prog.lsp", // 13
        "quote.lsp", // 14
        "while.lsp", // 15
        "test.lsp", // 16
        "eval_0.lsp", // 17
        "eval_1.lsp", // 18
        "isint.lsp", // 19
        "isreal.lsp", // 20
        "isbool.lsp", // 21
        "isnull.lsp", // 22
        "isatom.lsp", // 23
        "islist.lsp", // 24
        // BAD TESTS
        "bad_sample.lsp", // 25
        "quote_bad.lsp", // 26
        "prog_bad.lsp", // 27
        "bad_sample_1.lsp", // 28
        "bad_print.lsp", // 29
        "bad_eval.lsp", // 30
        "bad_lexer_0.lsp", // 31
        "bad_lexer_1.lsp", // 32
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