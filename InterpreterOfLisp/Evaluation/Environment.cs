using InterpreterOfLisp.SyntaxAnalyzer;

namespace InterpreterOfLisp.Evaluation;

public class Environment
{
    private readonly Dictionary<string, AstElementNode> _localEnv;

    public Environment() {
        _localEnv = new Dictionary<string, AstElementNode>();
    }
    
    public Environment(Environment env) {
        _localEnv = new Dictionary<string, AstElementNode>(env.GetEnv());
    }
    
    private Dictionary<string, AstElementNode> GetEnv()
    {
        return _localEnv;
    }

    public void AddEntry(string id, AstElementNode t) {
        _localEnv.Add(id, t);
    }

    public void PopEntry(string id) {
        _localEnv.Remove(id);
    }

    public AstElementNode GetEntry(string id)
    {
        _localEnv.TryGetValue(id, out var value);
        
        if (value == null)
            throw new Exception("Undefined identifier : " + id);
        
        return value;
    }
}