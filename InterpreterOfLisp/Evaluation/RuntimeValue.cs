namespace InterpreterOfLisp.Evaluation;

using InterpreterOfLisp.SyntaxAnalyzer;

public interface IRuntimeValue<T> {
    public T ReadValue();
}

public interface IRuntimeNumericValue<T> : IRuntimeValue<T> {
    public dynamic Add(dynamic other);
    public dynamic Subtract(dynamic other);
    public dynamic Multiply(dynamic other);
    public dynamic Divide(dynamic other);
}

public interface IRuntimeComparableValue<T> : IRuntimeValue<T> {
    public int Compare(dynamic other);

    public bool EqualTo(dynamic other);
}

public class RuntimeValue {

    public bool ImplementsNumerical() {
        var interfaceType = typeof(IRuntimeNumericValue<>);
        var type = this.GetType();
        
        return type.GetInterfaces().Any((i => i.GetGenericTypeDefinition() == interfaceType));
    }

    public bool ImplementsComparable() {
        var interfaceType = typeof(IRuntimeComparableValue<>);
        var type = this.GetType();
        
        return type.GetInterfaces().Any((i => i.GetGenericTypeDefinition() == interfaceType));
    }
}

public class RuntimeAtom : RuntimeValue, IRuntimeValue<string> {
    protected readonly string val;
    
    public RuntimeAtom(string val) {
        this.val = val;
    }

    public string ReadValue() {
        return val;
    }
}

public class RuntimeNull : RuntimeValue, IRuntimeValue<RuntimeNull> {
    public RuntimeNull ReadValue() {
        return this;
    }
}

public class RuntimeBool : RuntimeValue, IRuntimeComparableValue<Boolean> {
    protected readonly Boolean val;

    public RuntimeBool(Boolean val) {
        this.val = val;
    }

    public Boolean ReadValue() {
        return val;
    }

    public bool EqualTo(dynamic other) {
        if (other is IRuntimeComparableValue<Boolean> boolval) {
            return boolval.ReadValue() == val;
        }

        return false;
    }

    public int Compare(dynamic other) {
        throw new Exception("cannot compare bool values");
    }
}

// just a simple int representation in runtime
public class RuntimeInt : RuntimeValue, IRuntimeNumericValue<Int64>, IRuntimeComparableValue<Int64> {
    protected readonly Int64 val;

    public RuntimeInt(Int64 val) {
        this.val = val;
    }

    public Int64 ReadValue() {
        return val;
    }

    public dynamic Add(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val + other.ReadValue());
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(other.ReadValue() + val);
        throw new Exception("cannot add a non-numeric to an int");
    }

    public dynamic Subtract(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val - other.ReadValue());
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val - other.ReadValue());
        throw new Exception("cannot subtract a non-numeric from an int");
    }

    public dynamic Multiply(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val * other.ReadValue());
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val * other.ReadValue());
        throw new Exception("cannot multply an int by a non-numeric value");
    }

    public dynamic Divide(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeInt(val / other.ReadValue());
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val / other.ReadValue());
        throw new Exception("cannot divide an int by a non-numeric value");
    }

    public bool EqualTo(dynamic other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            return intval.ReadValue() == val;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            return floatval.ReadValue() == val;
        }

        return false;
    }

    public int Compare(dynamic other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            if (intval.ReadValue() == val) return 0;
            if (intval.ReadValue() < val) return 1;
            if (intval.ReadValue() > val) return -1;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            if (floatval.ReadValue() == val) return 0;
            if (floatval.ReadValue() < val) return 1;
            if (floatval.ReadValue() > val) return -1;
        }

        throw new Exception("cannot compare an int with a non-numeric value");
    }
}

// just a simple real representation in runtime
public class RuntimeReal : RuntimeValue, IRuntimeNumericValue<float>, IRuntimeComparableValue<float> {
    protected readonly float val;

    public RuntimeReal(float val) {
        this.val = val;
    }

    public float ReadValue() {
        return val;
    }

    public dynamic Add(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val + other.ReadValue());

        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(other.ReadValue() + val);

        throw new Exception("cannot add a non-numeric to a real");
    }

    public dynamic Subtract(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val - other.ReadValue());
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val - other.ReadValue());
        throw new Exception("cannot subtract a non-numeric from a real");
    }

    public dynamic Multiply(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val * other.ReadValue());
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val * other.ReadValue());
        throw new Exception("cannot multply a real by a non-numeric value");
    }

    public dynamic Divide(dynamic other) {
        if (other is IRuntimeNumericValue<Int64> otherInt)
            return new RuntimeReal(val / other.ReadValue());
        if (other is IRuntimeNumericValue<float> otherReal)
            return new RuntimeReal(val / other.ReadValue());
        throw new Exception("cannot divide a real by a non-numeric value");
    }

    public bool EqualTo(dynamic other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            return intval.ReadValue() == val;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            return floatval.ReadValue() == val;
        }

        return false;
    }

    public int Compare(dynamic other) {
        if (other is IRuntimeComparableValue<Int64> intval) {
            if (intval.ReadValue() == val) return 0;
            if (intval.ReadValue() < val) return 1;
            if (intval.ReadValue() > val) return -1;
        }

        if (other is IRuntimeComparableValue<float> floatval) {
            if (floatval.ReadValue() == val) return 0;
            if (floatval.ReadValue() < val) return 1;
            if (floatval.ReadValue() > val) return -1;
        }

        throw new Exception("cannot compare an int with a non-numeric value");
    }
}

// just a string representation in the runtime
public class RuntimeString : RuntimeValue, IRuntimeValue<String> {
    protected readonly String val;

    public RuntimeString(String val) {
        this.val = val;
    }

    public String ReadValue() {
        return val;
    }
}

// a list in the runtime can contain any other runtime object
public class RuntimeList : RuntimeValue, IRuntimeValue<List<object>> {
    protected readonly List<dynamic> val;

    public RuntimeList(List<dynamic> val) {
        this.val = val;
    }

    public List<dynamic> ReadValue() {
        return val;
    }
}

// quote in the runtime: wraps another runtime object until its value has to be read
public class RuntimeQuote : RuntimeValue, IRuntimeValue<AstElementNode> {
    protected readonly AstElementNode val;

    public RuntimeQuote(AstElementNode val) {
        this.val = val;
    }

    public AstElementNode ReadValue() {
        return val;
    }
}

// function runtime representation
public class RuntimeFunction : RuntimeValue, IRuntimeValue<(List<string>, AstElementNode)> {
    protected readonly List<string> args;
    protected readonly AstElementNode body;

    public RuntimeFunction(List<string> args, AstElementNode body) {
        this.args = args;
        this.body = body;
    }

    public dynamic Call(List<AstElementNode> callArgs, Environment env) {
        var context = new Environment(env);

        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        List<dynamic> evalutedValues = new List<dynamic>();
        for (var i = 0; i < args.Count; i++)
        {
            dynamic value = Evaluator.Evaluate(context, (dynamic)callArgs[i]);
            context.AddEntry(args[i], value);
            evalutedValues.Add(value);
        }
        try {
            return CallImpl(evalutedValues, context);
        } catch (ReturnException exc) {
            return exc.value;
        }
    }

    protected virtual dynamic CallImpl(List<dynamic> callArgs, Environment env) {
        return Evaluator.Evaluate(env, (dynamic)body);
    }

    public (List<string>, AstElementNode) ReadValue() {
        return (args, body);
    }
}


public class RuntimeProgFunction : RuntimeFunction {
    public RuntimeProgFunction(AstElementNode body) : base(new List<string> {}, body) {}
}

public class RuntimePlusFunction : RuntimeFunction {
    public RuntimePlusFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {   
        if (callArgs[0].ImplementsNumerical() && callArgs[1].ImplementsNumerical())
            return callArgs[0].Add(callArgs[1]);

        throw new Exception("add call with a non-numerical value");
    }
}

public class RuntimeMinusFunction : RuntimeFunction {
    public RuntimeMinusFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (callArgs[0].ImplementsNumerical() && callArgs[1].ImplementsNumerical())
            return callArgs[0].Subtract(callArgs[1]);

        throw new Exception("subtract call with a non-numerical value");
    }
}

public class RuntimeTimesFunction : RuntimeFunction {
    public RuntimeTimesFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");

        if (callArgs[0].ImplementsNumerical() && callArgs[1].ImplementsNumerical())
            return callArgs[0].Multiply(callArgs[1]);

        throw new Exception("times call with a non-numerical value");
    }
}

public class RuntimeDivideFunction : RuntimeFunction {
    public RuntimeDivideFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");

        if (callArgs[0].ImplementsNumerical() && callArgs[1].ImplementsNumerical())
            return callArgs[0].Divide(callArgs[1]);

        throw new Exception("divide call with a non-numerical value");
    }
}

public class RuntimeHeadFunction : RuntimeFunction {
    public RuntimeHeadFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeList rlist)
        {
            var value = rlist.ReadValue();
            if (value.Count > 0) 
                return rlist.ReadValue()[0];

            return new RuntimeNull();
        }

        throw new Exception("head called on a non-list value");
    }
}

public class RuntimeTailFunction : RuntimeFunction {
    public RuntimeTailFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");

        if (callArgs[0] is RuntimeList rlist) {
            var l = rlist.ReadValue();
            return new RuntimeList(l.Skip(1).ToList());
        }
        
        throw new Exception("tail called on a non-list value");
    }
}

public class RuntimeConsFunction : RuntimeFunction {
    public RuntimeConsFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");

        if (callArgs[1] is RuntimeList rlist) {
            var l = new List<dynamic> {callArgs[0]};
            l.AddRange(rlist.ReadValue());
            return new RuntimeList(l);
        }
        
        throw new Exception("cons called on a non-list value");
    }
}

public class RuntimeEqualFunction : RuntimeFunction {
    public RuntimeEqualFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0].ImplementsComparable() && callArgs[1].ImplementsComparable())
            return new RuntimeBool(callArgs[0].EqualTo(callArgs[1]));

        throw new Exception("equal call with a non-comparable value");
    }
}

public class RuntimeNonEqualFunction : RuntimeFunction {
    public RuntimeNonEqualFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0].ImplementsComparable() && callArgs[1].ImplementsComparable())
            return new RuntimeBool(!callArgs[0].EqualTo(callArgs[1]));

        throw new Exception("nonequal call with a non-comparable value");
    }
}

public class RuntimeLessFunction : RuntimeFunction {
    public RuntimeLessFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0].ImplementsComparable() && callArgs[1].ImplementsComparable())
            return new RuntimeBool(
                callArgs[0].Compare(callArgs[1]) < 0
            );

        throw new Exception("less call with a non-comparable value");
    }
}

public class RuntimeLessEqFunction : RuntimeFunction {
    public RuntimeLessEqFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0].ImplementsComparable() && callArgs[1].ImplementsComparable())
            return new RuntimeBool(
                callArgs[0].Compare(callArgs[1] ) <= 0
            );

        throw new Exception("lesseq call with a non-comparable value");
    }
}

public class RuntimeGreaterFunction : RuntimeFunction {
    public RuntimeGreaterFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0].ImplementsComparable() && callArgs[1].ImplementsComparable())
            return new RuntimeBool(
                callArgs[0].Compare(callArgs[1]) > 0
            );

        throw new Exception("greater call with a non-comparable value");
    }
}

public class RuntimeGreaterEqFunction : RuntimeFunction {
    public RuntimeGreaterEqFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0].ImplementsComparable() && callArgs[1].ImplementsComparable())
            return new RuntimeBool(
                callArgs[0].Compare(callArgs[1]) >= 0
            );

        throw new Exception("greatereq call with a non-comparable value");
    }
}

public class RuntimeIsIntFunction : RuntimeFunction {
    public RuntimeIsIntFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        return new RuntimeBool(callArgs[0] is RuntimeInt);
    }
}

public class RuntimeIsRealFunction : RuntimeFunction {
    public RuntimeIsRealFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        return new RuntimeBool(callArgs[0] is RuntimeReal);
    }
}

public class RuntimeIsBoolFunction : RuntimeFunction {
    public RuntimeIsBoolFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        return new RuntimeBool(callArgs[0] is RuntimeBool);
    }
}

public class RuntimeIsNullFunction : RuntimeFunction {
    public RuntimeIsNullFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        return new RuntimeBool(callArgs[0] is RuntimeNull);
    }
}

public class RuntimeIsAtomFunction : RuntimeFunction {
    public RuntimeIsAtomFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        return new RuntimeBool(callArgs[0] is RuntimeAtom);
    }
}

public class RuntimeIsListFunction : RuntimeFunction {
    public RuntimeIsListFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        return new RuntimeBool(callArgs[0] is RuntimeList);
    }
}

public class RuntimeAndFunction : RuntimeFunction {
    public RuntimeAndFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            if (callArgs[1] is RuntimeBool b)
                return new RuntimeBool(a.ReadValue() && b.ReadValue());

        throw new Exception("and call with a non-boolean value");
    }
}

public class RuntimeOrFunction : RuntimeFunction {
    public RuntimeOrFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            if (callArgs[1] is RuntimeBool b)
                return new RuntimeBool(a.ReadValue() || b.ReadValue());

        throw new Exception("or call with a non-boolean value");
    }
}

public class RuntimeXorFunction : RuntimeFunction {
    public RuntimeXorFunction() : base(new List<string> {"_", "_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            if (callArgs[1] is RuntimeBool b)
                return new RuntimeBool(a.ReadValue() ^ b.ReadValue());

        throw new Exception("xor call with a non-boolean value");
    }
}

public class RuntimeNotFunction : RuntimeFunction {
    public RuntimeNotFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment _) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        if (callArgs[0] is RuntimeBool a)
            return new RuntimeBool(!a.ReadValue());

        throw new Exception("not call with a non-boolean value");
    }
}

public class RuntimeEvalFunction : RuntimeFunction {
    public RuntimeEvalFunction() : base(new List<string> {"_"}, new AstElementNode()) {}

    protected override dynamic CallImpl(List<dynamic> callArgs, Environment env) {
        if (args.Count != callArgs.Count)
            throw new Exception("Arguments count mismatch");
        
        var evaled = callArgs[0];
        if (evaled is RuntimeQuote) {
            var toBeEvaluated = ((RuntimeQuote) evaled).ReadValue();
            return Evaluator.Evaluate(env, (dynamic)toBeEvaluated);
        }

        return evaled;
    }
}
