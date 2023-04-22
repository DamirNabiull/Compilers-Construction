using InterpreterOfLisp.SemanticsAnalyzer.Types;

namespace InterpreterOfLisp.Evaluation;

public class Environment
{
    public static readonly Dictionary<string, AbsType> DEFAULT_ENV = new Dictionary<string, AbsType> {
        // arithmetic
        {
            "plus", 
            new FuncType(new List<AbsType> {new RealType(), new RealType()}, new RealType())
        },
        {
            "minus", 
            new FuncType(new List<AbsType> {new RealType(), new RealType()}, new RealType())
        },
        {
            "times", 
            new FuncType(new List<AbsType> {new RealType(), new RealType()}, new RealType())
        },
        {
            "divide", 
            new FuncType(new List<AbsType> {new RealType(), new RealType()}, new RealType())
        },

        // lists
        {
            "head", 
            new FuncType(new List<AbsType> {new ListType()}, new AbsType())
        },
        {
            "tail", 
            new FuncType(new List<AbsType> {new ListType()}, new ListType())
        },
        {
            "cons", 
            new FuncType(new List<AbsType> {new AbsType(), new ListType()}, new ListType())
        },

        // comparisons 
        // project description says the arguments have to be one of the following: Integer, Real, Boolean 
        // but i dont give a fuck im not typechecking that either way, so its just the base type (basically Any)
        {
            "equal", 
            new FuncType(new List<AbsType> {new AbsType(), new AbsType()}, new BooleanType())
        },
        {
            "nonequal", 
            new FuncType(new List<AbsType> {new AbsType(), new AbsType()}, new BooleanType())
        },
        {
            "less", 
            new FuncType(new List<AbsType> {new AbsType(), new AbsType()}, new BooleanType())
        },
        {
            "lesseq", 
            new FuncType(new List<AbsType> {new AbsType(), new AbsType()}, new BooleanType())
        },
        {
            "greater", 
            new FuncType(new List<AbsType> {new AbsType(), new AbsType()}, new BooleanType())
        },
        {
            "greatereq", 
            new FuncType(new List<AbsType> {new AbsType(), new AbsType()}, new BooleanType())
        },

        // predicates
        {
            "isint", 
            new FuncType(new List<AbsType> {new AbsType()}, new BooleanType())
        },
        {
            "isreal", 
            new FuncType(new List<AbsType> {new AbsType()}, new BooleanType())
        },
        {
            "isbool", 
            new FuncType(new List<AbsType> {new AbsType()}, new BooleanType())
        },
        {
            "isnull", 
            new FuncType(new List<AbsType> {new AbsType()}, new BooleanType())
        },
        {
            "isatom", 
            new FuncType(new List<AbsType> {new AbsType()}, new BooleanType())
        },
        {
            "islist", 
            new FuncType(new List<AbsType> {new AbsType()}, new BooleanType())
        },

        // logical
        {
            "and", 
            new FuncType(new List<AbsType> {new BooleanType(), new BooleanType()}, new BooleanType())
        },
        {
            "or", 
            new FuncType(new List<AbsType> {new BooleanType(), new BooleanType()}, new BooleanType())
        },
        {
            "xor", 
            new FuncType(new List<AbsType> {new BooleanType(), new BooleanType()}, new BooleanType())
        },
        {
            "not", 
            new FuncType(new List<AbsType> {new BooleanType(), new BooleanType()}, new BooleanType())
        },

        // eval
        {
            "eval", 
            new FuncType(new List<AbsType> {new AbsType()}, new AbsType())
        },
    };

    private Dictionary<string, AbsType> currentEnv;
    private List<string> keywordContext;

    public Environment() {
        this.currentEnv = new Dictionary<string, AbsType>(DEFAULT_ENV);
        this.keywordContext = new List<string>{};
    }

    public void AddKeywordContext(string k) {
        this.keywordContext.Add(k);
    }

    public Boolean IsInKeywordContext(List<string> k) {
        foreach (var i in k) {
            if (this.keywordContext.IndexOf(i) != -1)
                return true;
        }
        return false;
    }

    public string? PopKeywordContext(string k) {
        int found = this.keywordContext.IndexOf(k);
        if (found == -1)
            return null;
        var res = this.keywordContext[found];
        this.keywordContext.RemoveAt(found);
        return res;
    }

    public void AddEnvEntry(string id, AbsType t) {
        this.currentEnv.Add(id, t);
    }

    public void EnvPopEntry(string id) {
        this.currentEnv.Remove(id);
    }

    public AbsType? EnvGetEntry(string id) {
        AbsType? value;
        bool hasValue = this.currentEnv.TryGetValue(id, out value);
        if (hasValue)
            return value;
        return null;
    }
}