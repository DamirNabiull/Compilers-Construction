using InterpreterOfLisp.SyntaxAnalyzer;

namespace InterpreterOfLisp.Evaluation;

public class Environment
{
    private readonly Dictionary<string, dynamic> _localEnv;

    public Environment() {
        _localEnv = new Dictionary<string, dynamic>();
    }
    
    public Environment(Environment env) {
        _localEnv = new Dictionary<string, dynamic>(env.GetEnv());
    }
    
    private Dictionary<string, dynamic> GetEnv()
    {
        return _localEnv;
    }

    public void AddEntry(string id, dynamic t) {
        _localEnv[id] = t;
    }

    public void PopEntry(string id) {
        _localEnv.Remove(id);
    }

    public dynamic GetEntry(string id)
    {
        _localEnv.TryGetValue(id, out var value);
        
        if (value == null)
            throw new Exception("Undefined identifier : " + id);
        
        return value;
    }
}