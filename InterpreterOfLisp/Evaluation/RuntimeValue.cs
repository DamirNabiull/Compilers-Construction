namespace InterpreterOfLisp.Evaluation;

using InterpreterOfLisp.SyntaxAnalyzer;

public interface IRuntimeValue<T> {
    public T ReadValue();
}

public interface IRuntimeNumericValue<T> : IRuntimeValue<T> {
    public IRuntimeNumericValue<dynamic> Add(IRuntimeNumericValue<dynamic> other);
    public IRuntimeNumericValue<dynamic> Subtract(IRuntimeNumericValue<dynamic> other);
    public IRuntimeNumericValue<dynamic> Multiply(IRuntimeNumericValue<dynamic> other);
    public IRuntimeNumericValue<dynamic> Divide(IRuntimeNumericValue<dynamic> other);
}

public interface IRuntimeComparableValue<T> : IRuntimeValue<T> {
    public int Compare(IRuntimeComparableValue<dynamic> other);

    public bool EqualTo(IRuntimeComparableValue<dynamic> other);
}

public class RuntimeAtom : IRuntimeValue<string> {
    protected readonly string val;
    
    public RuntimeAtom(string val) {
        this.val = val;
    }

    public string ReadValue() {
        return val;
    }
}

public class RuntimeNull : IRuntimeValue<RuntimeNull> {
    public RuntimeNull ReadValue() {
        return this;
    }
}

public class RuntimeBool : IRuntimeComparableValue<Boolean> {
    protected readonly Boolean val;

    public RuntimeBool(Boolean val) {
        this.val = val;
    }

    public Boolean ReadValue() {
        return val;
    }

    public bool EqualTo(IRuntimeComparableValue<dynamic> other) {
        if (other is IRuntimeComparableValue<Boolean> boolval) {
            return boolval.ReadValue() == val;
        }

        return false;
    }

    public int Compare(IRuntimeComparableValue<dynamic> other) {
        throw new Exception("cannot compare bool values");
    }
}

// just a simple int representation in runtime
public class RuntimeInt : IRuntimeNumericValue<Int64>, IRuntimeComparableValue<Int64> {
    protected readonly Int64 val;

    public RuntimeInt(Int64 val) {
        this.val = val;
    }

    public Int64 ReadValue() {
        return val;
    }

    public IRuntimeNumericValue<dynamic> Add(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val + other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(other.ReadValue() + val) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot add a non-numeric to an int");
    }

    public IRuntimeNumericValue<dynamic> Subtract(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val - other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val - other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot subtract a non-numeric from an int");
    }

    public IRuntimeNumericValue<dynamic> Multiply(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val * other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val * other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot multply an int by a non-numeric value");
    }

    public IRuntimeNumericValue<dynamic> Divide(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val / other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val / other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot divide an int by a non-numeric value");
    }

    public bool EqualTo(IRuntimeComparableValue<dynamic> other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            return intval.ReadValue() == val;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            return floatval.ReadValue() == val;
        }

        return false;
    }

    public int Compare(IRuntimeComparableValue<dynamic> other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            if (intval.ReadValue() == val) return 0;
            if (intval.ReadValue() < val) return -1;
            if (intval.ReadValue() > val) return 1;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            if (floatval.ReadValue() == val) return 0;
            if (floatval.ReadValue() < val) return -1;
            if (floatval.ReadValue() > val) return 1;
        }

        throw new Exception("cannot compare an int with a non-numeric value");
    }
}

// just a simple real representation in runtime
public class RuntimeReal : IRuntimeNumericValue<float>, IRuntimeComparableValue<float> {
    protected readonly float val;

    public RuntimeReal(float val) {
        this.val = val;
    }

    public float ReadValue() {
        return val;
    }

    public IRuntimeNumericValue<dynamic> Add(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val + other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(other.ReadValue() + val) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot add a non-numeric to a real");
    }

    public IRuntimeNumericValue<dynamic> Subtract(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val - other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val - other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot subtract a non-numeric from a real");
    }

    public IRuntimeNumericValue<dynamic> Multiply(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val * other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val * other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot multply a real by a non-numeric value");
    }

    public IRuntimeNumericValue<dynamic> Divide(IRuntimeNumericValue<dynamic> other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val / other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val / other.ReadValue()) as IRuntimeNumericValue<dynamic>;
        throw new Exception("cannot divide a real by a non-numeric value");
    }

    public bool EqualTo(IRuntimeComparableValue<dynamic> other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            return intval.ReadValue() == val;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            return floatval.ReadValue() == val;
        }

        return false;
    }

    public int Compare(IRuntimeComparableValue<dynamic> other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            if (intval.ReadValue() == val) return 0;
            if (intval.ReadValue() < val) return -1;
            if (intval.ReadValue() > val) return 1;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            if (floatval.ReadValue() == val) return 0;
            if (floatval.ReadValue() < val) return -1;
            if (floatval.ReadValue() > val) return 1;
        }

        throw new Exception("cannot compare an int with a non-numeric value");
    }
}

// just a string representation in the runtime
public class RuntimeString : IRuntimeValue<String> {
    protected readonly String val;

    public RuntimeString(String val) {
        this.val = val;
    }

    public String ReadValue() {
        return val;
    }
}

// a list in the runtime can contain any other runtime object
public class RuntimeList : IRuntimeValue<List<IRuntimeValue<object>>> {
    protected readonly List<IRuntimeValue<object>> val;

    public RuntimeList(List<IRuntimeValue<object>> val) {
        this.val = val;
    }

    public List<IRuntimeValue<object>> ReadValue() {
        return val;
    }
}

// quote in the runtime: wraps another runtime object until its value has to be read
public class RuntimeQuote : IRuntimeValue<IRuntimeValue<object>> {
    protected readonly IRuntimeValue<object> val;

    public RuntimeQuote(IRuntimeValue<object> val) {
        this.val = val;
    }

    public IRuntimeValue<object> ReadValue() {
        return val;
    }
}

// function runtime representation
public class RuntimeFunction : IRuntimeValue<(List<string>, AstElementNode)> {
    protected readonly List<string> args;
    protected readonly AstElementNode body;

    public RuntimeFunction(List<string> args, AstElementNode body) {
        this.args = args;
        this.body = body;
    }

    public IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment env) {
        var context = new Environment(env);

        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        for (var i = 0; i < args.Count; i++)
        {
            IRuntimeValue<object> value = Evaluator.Evaluate(context, (dynamic)callArgs[i]);
            context.AddEntry(args[i], value);
        }
        
        return Evaluator.Evaluate(context, (dynamic)body);
    }

    public (List<string>, AstElementNode) ReadValue() {
        return (args, body);
    }
}

public class RuntimePlusFunction : RuntimeFunction {
    public RuntimePlusFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        return (callArgs[0] as IRuntimeNumericValue<dynamic>).Add(callArgs[1] as IRuntimeNumericValue<dynamic>);
    }
}

public class RuntimeMinusFunction : RuntimeFunction {
    public RuntimeMinusFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        return (callArgs[0] as IRuntimeNumericValue<dynamic>).Subtract(callArgs[1] as IRuntimeNumericValue<dynamic>);
    }
}

public class RuntimeTimesFunction : RuntimeFunction {
    public RuntimeTimesFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        return (callArgs[0] as IRuntimeNumericValue<dynamic>).Multiply(callArgs[1] as IRuntimeNumericValue<dynamic>);
    }
}

public class RuntimeDivideFunction : RuntimeFunction {
    public RuntimeDivideFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        return (callArgs[0] as IRuntimeNumericValue<dynamic>).Divide(callArgs[1] as IRuntimeNumericValue<dynamic>);
    }
}

public class RuntimeHeadFunction : RuntimeFunction {
    public RuntimeHeadFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeList rlist) {
            return rlist.ReadValue()[0];
        }

        throw new Exception("head called on a non-list value");
    }
}

public class RuntimeTailFunction : RuntimeFunction {
    public RuntimeTailFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");

        if (callArgs[0] is RuntimeList rlist) {
            var l = rlist.ReadValue();
            return new RuntimeList(l.GetRange(1, l.Count)) as IRuntimeValue<object>;
        }
        
        throw new Exception("tail called on a non-list value");
    }
}

public class RuntimeConsFunction : RuntimeFunction {
    public RuntimeConsFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");

        if (callArgs[1] is RuntimeList rlist) {
            var l = new List<IRuntimeValue<object>> {callArgs[0]};
            l.AddRange(rlist.ReadValue());
            return new RuntimeList(l) as IRuntimeValue<object>;
        }
        
        throw new Exception("cons called on a non-list value");
    }
}

public class RuntimeEqualFunction : RuntimeFunction {
    public RuntimeEqualFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is IRuntimeComparableValue<dynamic> comparable)
            return new RuntimeBool(comparable.EqualTo(callArgs[1] as IRuntimeComparableValue<dynamic>)) as IRuntimeValue<object>;

        throw new Exception("equal call with a non-comparable value");
    }
}

public class RuntimeNonEqualFunction : RuntimeFunction {
    public RuntimeNonEqualFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is IRuntimeComparableValue<dynamic> comparable)
            return new RuntimeBool(!comparable.EqualTo(callArgs[1] as IRuntimeComparableValue<dynamic>)) as IRuntimeValue<object>;

        throw new Exception("nonequal call with a non-comparable value");
    }
}

public class RuntimeLessFunction : RuntimeFunction {
    public RuntimeLessFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is IRuntimeComparableValue<dynamic> comparable)
            return new RuntimeBool(
                comparable.Compare(callArgs[1] as IRuntimeComparableValue<dynamic>) < 0
            ) as IRuntimeValue<object>;

        throw new Exception("less call with a non-comparable value");
    }
}

public class RuntimeLessEqFunction : RuntimeFunction {
    public RuntimeLessEqFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is IRuntimeComparableValue<dynamic> comparable)
            return new RuntimeBool(
                comparable.Compare(callArgs[1] as IRuntimeComparableValue<dynamic>) <= 0
            ) as IRuntimeValue<object>;

        throw new Exception("lesseq call with a non-comparable value");
    }
}

public class RuntimeGreaterFunction : RuntimeFunction {
    public RuntimeGreaterFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is IRuntimeComparableValue<dynamic> comparable)
            return new RuntimeBool(
                comparable.Compare(callArgs[1] as IRuntimeComparableValue<dynamic>) > 0
            ) as IRuntimeValue<object>;

        throw new Exception("greater call with a non-comparable value");
    }
}

public class RuntimeGreaterEqFunction : RuntimeFunction {
    public RuntimeGreaterEqFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is IRuntimeComparableValue<dynamic> comparable)
            return new RuntimeBool(
                comparable.Compare(callArgs[1] as IRuntimeComparableValue<dynamic>) >= 0
            ) as IRuntimeValue<object>;

        throw new Exception("greatereq call with a non-comparable value");
    }
}

public class RuntimeIsIntFunction : RuntimeFunction {
    public RuntimeIsIntFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (callArgs[0] is RuntimeInt) 
            return new RuntimeBool(true) as IRuntimeValue<object>;
        return new RuntimeBool(false) as IRuntimeValue<object>;
    }
}

public class RuntimeIsRealFunction : RuntimeFunction {
    public RuntimeIsRealFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (callArgs[0] is RuntimeReal) 
            return new RuntimeBool(true) as IRuntimeValue<object>;
        return new RuntimeBool(false) as IRuntimeValue<object>;
    }
}

public class RuntimeIsBoolFunction : RuntimeFunction {
    public RuntimeIsBoolFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (callArgs[0] is RuntimeBool) 
            return new RuntimeBool(true) as IRuntimeValue<object>;
        return new RuntimeBool(false) as IRuntimeValue<object>;
    }
}

public class RuntimeIsNullFunction : RuntimeFunction {
    public RuntimeIsNullFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (callArgs[0] is RuntimeNull) 
            return new RuntimeBool(true) as IRuntimeValue<object>;
        return new RuntimeBool(false) as IRuntimeValue<object>;
    }
}

public class RuntimeIsAtomFunction : RuntimeFunction {
    public RuntimeIsAtomFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (callArgs[0] is RuntimeAtom) 
            return new RuntimeBool(true) as IRuntimeValue<object>;
        return new RuntimeBool(false) as IRuntimeValue<object>;
    }
}

public class RuntimeIsListFunction : RuntimeFunction {
    public RuntimeIsListFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (callArgs[0] is RuntimeList) 
            return new RuntimeBool(true) as IRuntimeValue<object>;
        return new RuntimeBool(false) as IRuntimeValue<object>;
    }
}

public class RuntimeAndFunction : RuntimeFunction {
    public RuntimeAndFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            if (callArgs[1] is RuntimeBool b)
                return new RuntimeBool(a.ReadValue() && b.ReadValue()) as IRuntimeValue<object>;

        throw new Exception("and call with a non-boolean value");
    }
}

public class RuntimeOrFunction : RuntimeFunction {
    public RuntimeOrFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            if (callArgs[1] is RuntimeBool b)
                return new RuntimeBool(a.ReadValue() || b.ReadValue()) as IRuntimeValue<object>;

        throw new Exception("or call with a non-boolean value");
    }
}

public class RuntimeXorFunction : RuntimeFunction {
    public RuntimeXorFunction() : base(new List<string> {"a", "b"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            if (callArgs[1] is RuntimeBool b)
                return new RuntimeBool(a.ReadValue() ^ b.ReadValue()) as IRuntimeValue<object>;

        throw new Exception("xor call with a non-boolean value");
    }
}

public class RuntimeNotFunction : RuntimeFunction {
    public RuntimeNotFunction() : base(new List<string> {"a"}, new AstElementNode()) {}

    public new IRuntimeValue<object> Call(List<IRuntimeValue<object>> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            return new RuntimeBool(!a.ReadValue()) as IRuntimeValue<object>;

        throw new Exception("not call with a non-boolean value");
    }
}
