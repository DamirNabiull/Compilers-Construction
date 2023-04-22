using InterpreterOfLisp.SemanticsAnalyzer.Types;
using InterpreterOfLisp.SyntaxAnalyzer;

namespace InterpreterOfLisp.Evaluation;

public class Environment
{
    public static readonly List<string> PredefinedEnv = new()
    {
        // arithmetic
        "plus",
        "minus",
        "times",
        "divide",
        // lists
        "head", 
        "tail", 
        "cons", 
        // comparisons
        "equal", 
        "nonequal", 
        "less", 
        "lesseq", 
        "greater", 
        "greatereq", 
        // predicates
        "isint", 
        "isreal", 
        "isbool", 
        "isnull", 
        "isatom", 
        "islist", 
        // logical
        "and", 
        "or", 
        "xor", 
        "not", 
        // eval
        "eval"
    };

    private readonly Dictionary<string, AstElementNode> _currentEnv;

    public Environment() {
        _currentEnv = new Dictionary<string, AstElementNode>();
    }

    public void AddEnvEntry(string id, AstElementNode t) {
        _currentEnv.Add(id, t);
    }

    public void EnvPopEntry(string id) {
        _currentEnv.Remove(id);
    }

    public AstElementNode EnvGetEntry(string id)
    {
        _currentEnv.TryGetValue(id, out var value);
        
        if (value == null)
            throw new Exception("Undefined identifier : " + id);
        
        return value;
    }
}