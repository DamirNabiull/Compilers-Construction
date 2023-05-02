using InterpreterOfLisp.SyntaxAnalyzer;

namespace InterpreterOfLisp.Evaluation;

public class Evaluator
{   
    public static void Evaluate(AstProgramNode astRootNode)
    {
        var env = new Environment();
        
        Console.WriteLine("\n\n\nEVALUATION:");


        try {
            foreach (var i in astRootNode.Children) {
                dynamic ans = Evaluate(env, (dynamic)i);
                Console.WriteLine(ans.ReadValue());
            }
        } catch (ControlException exc) {
            Console.WriteLine($"Catched {exc}. Finishing programm");
        }
    }
    
    public static object Evaluate(Environment env, AstIdentifierNode node)
    {
        // Eval Identifier
        //// Console.WriteLine("Eval Identifier");

        var id = node.Token.Value!.ToString();
        
        return env.GetEntry(id!);
    }
    
    public static dynamic Evaluate(Environment env, AstLiteralNode node)
    {
        // Eval Literal
        //Console.WriteLine("Eval Literal");
        if (node.Token.Value is int) {
            return new RuntimeInt((dynamic) node.Token.Value);
        }
        if (node.Token.Value is bool) {
            return new RuntimeBool((dynamic) node.Token.Value);
        }
        if (node.Token.Value is double) {
            return new RuntimeBool((dynamic) node.Token.Value);
        }
        throw new Exception("literal node contain non-literal value");
    }

    public static dynamic Evaluate(Environment env, AstListNode node)
    {
        // Eval ListDeclaration
        if (node.Children.Count != 0 && node.Children[0] is AstIdentifierNode)
        {
            return EvaluateApplication(env, node);
        }

        return EvaluateList(env, node);
    }
    
    public static dynamic EvaluateApplication(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval FuncApplicationDeclaration");
        
        var identifier = ((AstIdentifierNode)node.Children[0]).Token.Value!.ToString();

        var func = identifier switch
        {
            "plus" => EvaluatePlus(env, node),
            "minus" => EvaluateMinus(env, node),
            "times" => EvaluateTimes(env, node),
            "divide" => EvaluateDivide(env, node),
            "head" => EvaluateHead(env, node),
            "tail" => EvaluateTail(env, node),
            "cons" => EvaluateCons(env, node),
            "equal" => EvaluateEqual(env, node),
            "nonequal" => EvaluateNonEqual(env, node),
            "less" => EvaluateLess(env, node),
            "lesseq" => EvaluateLessEq(env, node),
            "greater" => EvaluateGreater(env, node),
            "greatereq" => EvaluateGreaterEq(env, node),
            "isint" => EvaluateIsInt(env, node),
            "isreal" => EvaluateIsReal(env, node),
            "isbool" => EvaluateIsBool(env, node),
            "isnull" => EvaluateIsNull(env, node),
            "isatom" => EvaluateIsAtom(env, node),
            "islist" => EvaluateIsList(env, node),
            "and" => EvaluateAnd(env, node),
            "or" => EvaluateOr(env, node),
            "xor" => EvaluateXor(env, node),
            "not" => EvaluateNot(env, node),
            "eval" => EvaluateEval(env, node),
            _ => EvaluateApplication(env, node, identifier!)
        };

        return func.Call(node.Children.Skip(1).ToList(), env);
    }

    public static RuntimeFunction EvaluateApplication(Environment env, AstListNode node, string id)
    {
        //Console.WriteLine("Eval UserDefined");

        var context = new Environment(env);
        var runtimeValue = env.GetEntry(id);

        if (runtimeValue is not RuntimeFunction) {
            throw new Exception($"'{id}' is not function");
        }
        RuntimeFunction func = (RuntimeFunction) runtimeValue;
        
        return func;
    }
    
    public static dynamic EvaluateList(Environment env, AstListNode node)
    {
        // Eval AtomsListDeclaration
        // Для каждого элемента листа сделать eval
        //Console.WriteLine("Eval AtomsListDeclaration");

        var evaluated = new List<dynamic>();
        for (var i = 0; i < node.Children.Count; i++)
        {
            evaluated.Add(Evaluate(env, (dynamic)node.Children[i]));
            if (i == 0 && evaluated[0] is RuntimeFunction) {
                return evaluated[0].Call(node.Children.Skip(1).ToList(), env);
            }
        }
        
        return new RuntimeList(evaluated);
    }

#region KEYWORDS

    public static RuntimeQuote Evaluate(Environment env, AstQuoteNode node)
    {
        // Eval Quote
        //Console.WriteLine("Eval Quote");
        return new RuntimeQuote(node.Element);
    }

    public static dynamic Evaluate(Environment env, AstSetQNode node)
    {
        // Eval SetQ
        //Console.WriteLine("Eval SetQ");

        var evaluatedValue = Evaluate(env, (dynamic)node.AssignedValue);
        var id = node.Assignee.Token.Value!.ToString();
        env.AddEntry(id!, evaluatedValue);
        return evaluatedValue;
    }

    public static dynamic Evaluate(Environment env, AstFuncNode node)
    {
        // Eval FuncDeclaration
        //Console.WriteLine("Eval FuncDeclaration");
        
        var id = node.Name.Token.Value!.ToString();

        var func = new RuntimeFunction(node.Parameters.ConvertAll(param => param.Token.Value!.ToString()!), node.Body);
        env.AddEntry(id!, func);
        
        return func;
    }

    public static dynamic Evaluate(Environment env, AstLambdaNode node)
    {
        // Eval Lambda
        //Console.WriteLine("Eval Lambda");

        var func = new RuntimeFunction(node.Parameters.ConvertAll(param => param.Token.Value!.ToString()!), node.Body);

        return func;
    }
    
    public static dynamic Evaluate(Environment env, AstProgNode node)
    {
        // Eval Prog
        //Console.WriteLine("Eval Prog");

        var context = new Environment();


        foreach (var param in node.Parameters) {
            string id = param.Token.Value!.ToString()!;
            context.AddEntry(id, env.GetEntry(id));
        }
        
        return new RuntimeProgFunction(node.Body).Call(new List<AstElementNode>(), context);
    }

    public static RuntimeBool EvaluateCondition(Environment env, AstElementNode node) {
        dynamic cond = Evaluate(env, (dynamic)node);

        if (cond is not RuntimeBool) {
            throw new Exception("Result of condition is not boolean");
        }
        return (dynamic) cond;
    }
    
    public static dynamic Evaluate(Environment env, AstCondNode node)
    {
        // Eval Cond
        //Console.WriteLine("Eval Cond");

        var cond = EvaluateCondition(env, node.Condition);
        if (cond.ReadValue()) {
            return Evaluate(env, (dynamic)node.TrueArgument);
        } else if (node.FalseArgument is not null) {
            return Evaluate(env, (dynamic)node.FalseArgument);
        }
        return new RuntimeNull();
    }
    
    public static RuntimeNull Evaluate(Environment env, AstWhileNode node)
    {
        // Eval While
        //Console.WriteLine("Eval While");
        try {
            while (EvaluateCondition(env, node.Condition).ReadValue()) {
                Evaluate(env, (dynamic) node.Body);
            }
        } catch (BreakException) { }
        
        return new RuntimeNull();
    }
    
    public static RuntimeNull Evaluate(Environment env, AstReturnNode node)
    {
        // Eval Return
        //Console.WriteLine("Eval Return");
        var returnedValue = Evaluate(env, (dynamic)node.ReturnValue);
        throw new ReturnException(returnedValue);
    }
    
    public static RuntimeNull Evaluate(Environment env, AstBreakNode node)
    {
        // Eval Break
        //Console.WriteLine("Eval Break");
        throw new BreakException();
    }

#endregion

#region PREDEFINED

#region Arithmetic

    public static RuntimeFunction EvaluatePlus(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Plus");
        return new RuntimePlusFunction();
    }

    public static RuntimeFunction EvaluateMinus(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Minus");
        return new RuntimeMinusFunction();
    }

    public static RuntimeFunction EvaluateTimes(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Times");
        return new RuntimeTimesFunction();
    }

    public static RuntimeFunction EvaluateDivide(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Divide");
        return new RuntimeDivideFunction();
    }

#endregion

#region ListOperations

    public static RuntimeFunction EvaluateHead(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Head");
        return new RuntimeHeadFunction();
    }
    
    public static RuntimeFunction EvaluateTail(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Tail");
       return new RuntimeTailFunction();
    }
    
    public static RuntimeFunction EvaluateCons(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Cons");
        return new RuntimeConsFunction();
    }

#endregion

#region Comparisons

    public static RuntimeFunction EvaluateEqual(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Equal");
        return new RuntimeEqualFunction();
    }
    
    public static RuntimeFunction EvaluateNonEqual(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval NonEqual");
        return new RuntimeNonEqualFunction();
    }
    
    public static RuntimeFunction EvaluateLess(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Less");
        return new RuntimeLessFunction();
    }
    
    public static RuntimeFunction EvaluateLessEq(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval LessEq");
        return new RuntimeLessEqFunction();
    }
    
    public static RuntimeFunction EvaluateGreater(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Greater");
        return new RuntimeGreaterFunction();
    }
    
    public static RuntimeFunction EvaluateGreaterEq(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval GreaterEq");
        return new RuntimeGreaterEqFunction();
    }

#endregion

#region Predicates

    public static RuntimeFunction EvaluateIsInt(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval IsInt");
        return new RuntimeIsIntFunction();
    }
    
    public static RuntimeFunction EvaluateIsReal(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval IsReal");
        return new RuntimeIsRealFunction();
    }
    
    public static RuntimeFunction EvaluateIsBool(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval IsBool");
        return new RuntimeIsBoolFunction();
    }
    
    public static RuntimeFunction EvaluateIsNull(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval IsNull");
        return new RuntimeIsNullFunction();
    }
    
    public static RuntimeFunction EvaluateIsAtom(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval IsAtom");
        return new RuntimeIsAtomFunction();
    }
    
    public static RuntimeFunction EvaluateIsList(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval IsList");
        return new RuntimeIsListFunction();
    }

#endregion

#region LogicalOperations

    public static RuntimeFunction EvaluateAnd(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval And");
        return new RuntimeAndFunction();
    }
    
    public static RuntimeFunction EvaluateOr(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Or");
        return new RuntimeOrFunction();
    }
    
    public static RuntimeFunction EvaluateXor(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Xor");
        return new RuntimeXorFunction();
    }
    
    public static RuntimeFunction EvaluateNot(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Not");
        return new RuntimeNotFunction();
    }

#endregion

#region Evaluator

    public static RuntimeFunction EvaluateEval(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval Eval");
        return new RuntimeEvalFunction();
    }

#endregion

#endregion
}