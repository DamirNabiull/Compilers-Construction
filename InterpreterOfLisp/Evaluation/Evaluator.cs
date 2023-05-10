using InterpreterOfLisp.SyntaxAnalyzer;
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace InterpreterOfLisp.Evaluation;

public static class Evaluator
{
    public static void Evaluate(AstProgramNode astRootNode)
    {
        var env = new Environment();
        
        Console.WriteLine();

        try {
            foreach (var i in astRootNode.Children) {
                Evaluate(env, (dynamic)i);
            }
        } catch (ControlException exc) {
            Console.WriteLine($"Catch {exc}. Finishing program");
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
        return node.Token.Value switch
        {
            // Eval Literal
            //Console.WriteLine("Eval Literal");
            int => new RuntimeInt((dynamic)node.Token.Value),
            bool => new RuntimeBool((dynamic)node.Token.Value),
            double => new RuntimeReal((dynamic)node.Token.Value),
            null => new RuntimeNull(),
            _ => throw new Exception("literal node contain non-literal value")
        };
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
    
    private static dynamic EvaluateApplication(Environment env, AstListNode node)
    {
        //Console.WriteLine("Eval FuncApplicationDeclaration");
        
        var identifier = ((AstIdentifierNode)node.Children[0]).Token.Value!.ToString();

        var func = identifier switch
        {
            "plus" => EvaluatePlus(),
            "minus" => EvaluateMinus(),
            "times" => EvaluateTimes(),
            "divide" => EvaluateDivide(),
            "head" => EvaluateHead(),
            "tail" => EvaluateTail(),
            "cons" => EvaluateCons(),
            "equal" => EvaluateEqual(),
            "nonequal" => EvaluateNonEqual(),
            "less" => EvaluateLess(),
            "lesseq" => EvaluateLessEq(),
            "greater" => EvaluateGreater(),
            "greatereq" => EvaluateGreaterEq(),
            "isint" => EvaluateIsInt(),
            "isreal" => EvaluateIsReal(),
            "isbool" => EvaluateIsBool(),
            "isnull" => EvaluateIsNull(),
            "isatom" => EvaluateIsAtom(),
            "islist" => EvaluateIsList(),
            "and" => EvaluateAnd(),
            "or" => EvaluateOr(),
            "xor" => EvaluateXor(),
            "not" => EvaluateNot(),
            "eval" => EvaluateEval(),
            "print" => EvaluatePrint(),
            _ => EvaluateApplication(env, identifier!)
        };

        return func.Call(node.Children.Skip(1).ToList(), env);
    }

    private static RuntimeFunction EvaluateApplication(Environment env, string id)
    {
        //Console.WriteLine("Eval UserDefined");
        var runtimeValue = env.GetEntry(id);

        if (runtimeValue is not RuntimeFunction func) {
            throw new Exception($"'{id}' is not function");
        }

        return func;
    }

    private static dynamic EvaluateList(Environment env, AstListNode node)
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
            var id = param.Token.Value!.ToString()!;
            context.AddEntry(id, env.GetEntry(id));
        }
        
        return new RuntimeProgFunction(node.Body).Call(new List<AstElementNode>(), context);
    }

    private static RuntimeBool EvaluateCondition(Environment env, AstElementNode node) {
        var cond = Evaluate(env, (dynamic)node);

        if (cond is not RuntimeBool) {
            throw new Exception("Result of condition is not boolean");
        }
        
        return cond;
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

    private static RuntimeFunction EvaluatePlus()
    {
        //Console.WriteLine("Eval Plus");
        return new RuntimePlusFunction();
    }

    private static RuntimeFunction EvaluateMinus()
    {
        //Console.WriteLine("Eval Minus");
        return new RuntimeMinusFunction();
    }

    private static RuntimeFunction EvaluateTimes()
    {
        //Console.WriteLine("Eval Times");
        return new RuntimeTimesFunction();
    }

    private static RuntimeFunction EvaluateDivide()
    {
        //Console.WriteLine("Eval Divide");
        return new RuntimeDivideFunction();
    }

#endregion

#region ListOperations

    private static RuntimeFunction EvaluateHead()
    {
        //Console.WriteLine("Eval Head");
        return new RuntimeHeadFunction();
    }
    
    private static RuntimeFunction EvaluateTail()
    {
        //Console.WriteLine("Eval Tail");
       return new RuntimeTailFunction();
    }
    
    private static RuntimeFunction EvaluateCons()
    {
        //Console.WriteLine("Eval Cons");
        return new RuntimeConsFunction();
    }

#endregion

#region Comparisons

    private static RuntimeFunction EvaluateEqual()
    {
        //Console.WriteLine("Eval Equal");
        return new RuntimeEqualFunction();
    }
    
    private static RuntimeFunction EvaluateNonEqual()
    {
        //Console.WriteLine("Eval NonEqual");
        return new RuntimeNonEqualFunction();
    }
    
    private static RuntimeFunction EvaluateLess()
    {
        //Console.WriteLine("Eval Less");
        return new RuntimeLessFunction();
    }
    
    private static RuntimeFunction EvaluateLessEq()
    {
        //Console.WriteLine("Eval LessEq");
        return new RuntimeLessEqFunction();
    }
    
    private static RuntimeFunction EvaluateGreater()
    {
        //Console.WriteLine("Eval Greater");
        return new RuntimeGreaterFunction();
    }
    
    private static RuntimeFunction EvaluateGreaterEq()
    {
        //Console.WriteLine("Eval GreaterEq");
        return new RuntimeGreaterEqFunction();
    }

#endregion

#region Predicates

    private static RuntimeFunction EvaluateIsInt()
    {
        //Console.WriteLine("Eval IsInt");
        return new RuntimeIsIntFunction();
    }
    
    private static RuntimeFunction EvaluateIsReal()
    {
        //Console.WriteLine("Eval IsReal");
        return new RuntimeIsRealFunction();
    }
    
    private static RuntimeFunction EvaluateIsBool()
    {
        //Console.WriteLine("Eval IsBool");
        return new RuntimeIsBoolFunction();
    }
    
    private static RuntimeFunction EvaluateIsNull()
    {
        //Console.WriteLine("Eval IsNull");
        return new RuntimeIsNullFunction();
    }
    
    private static RuntimeFunction EvaluateIsAtom()
    {
        //Console.WriteLine("Eval IsAtom");
        return new RuntimeIsAtomFunction();
    }
    
    private static RuntimeFunction EvaluateIsList()
    {
        //Console.WriteLine("Eval IsList");
        return new RuntimeIsListFunction();
    }

#endregion

#region LogicalOperations

    private static RuntimeFunction EvaluateAnd()
    {
        //Console.WriteLine("Eval And");
        return new RuntimeAndFunction();
    }
    
    private static RuntimeFunction EvaluateOr()
    {
        //Console.WriteLine("Eval Or");
        return new RuntimeOrFunction();
    }
    
    private static RuntimeFunction EvaluateXor()
    {
        //Console.WriteLine("Eval Xor");
        return new RuntimeXorFunction();
    }
    
    private static RuntimeFunction EvaluateNot()
    {
        //Console.WriteLine("Eval Not");
        return new RuntimeNotFunction();
    }

#endregion

#region Evaluator

    private static RuntimeFunction EvaluateEval()
    {
        //Console.WriteLine("Eval Eval");
        return new RuntimeEvalFunction();
    }

#endregion

#region Print

    private static RuntimeFunction EvaluatePrint()
    {
        //Console.WriteLine("Eval Print");
        return new RuntimePrintFunction();
    }

#endregion

#endregion
}