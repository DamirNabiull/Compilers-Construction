using InterpreterOfLisp.SyntaxAnalyzer;

namespace InterpreterOfLisp.Evaluation;

public class Evaluator
{   
    public static void Evaluate(AstProgramNode astRootNode)
    {
        var env = new Environment();
        
        Console.WriteLine("\n\n\nEVALUATION:");
        
        foreach (var i in astRootNode.Children) {
            AstElementNode ans = Evaluate(env, (dynamic)i);
            Console.WriteLine(ans.ToString());
        }
    }
    
    public static AstElementNode Evaluate(Environment env, AstIdentifierNode node)
    {
        // Eval Identifier
        Console.WriteLine("Eval Identifier");

        var id = node.Token.Value!.ToString();
        
        return env.GetEntry(id!);
    }
    
    public static AstElementNode Evaluate(Environment env, AstLiteralNode node)
    {
        // Eval Literal
        Console.WriteLine("Eval Literal");
        return node;
    }

    public static AstElementNode Evaluate(Environment env, AstListNode node)
    {
        // Eval ListDeclaration
        if (node.Children.Count != 0 && node.Children[0] is AstIdentifierNode)
        {
            return EvaluateApplication(env, node);
        }

        return EvaluateList(env, node);
    }
    
    public static AstElementNode EvaluateApplication(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval FuncApplicationDeclaration");
        
        var identifier = ((AstIdentifierNode)node.Children[0]).Token.Value!.ToString();
        node.Children.RemoveAt(0);

        return identifier switch
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
    }

    public static AstElementNode EvaluateApplication(Environment env, AstListNode node, string id)
    {
        Console.WriteLine("Eval UserDefined");

        var context = new Environment(env);
        var func = (AstFuncNode)env.GetEntry(id);

        if (node.Children.Count != func.Parameters.Count)
            throw new Exception("Arguments count mismatch");
        
        for (var i = 0; i < func.Parameters.Count; i++)
        {
            var argName = func.Parameters[i].Token.Value!.ToString()!;
            AstElementNode value = Evaluate(context, (dynamic)node.Children[i]);
            context.AddEntry(argName, value);
        }
        
        return Evaluate(context, (dynamic)func.Body);
    }
    
    public static AstElementNode EvaluateList(Environment env, AstListNode node)
    {
        // Eval AtomsListDeclaration
        // Для каждого элемента листа сделать eval
        Console.WriteLine("Eval AtomsListDeclaration");

        for (var i = 0; i < node.Children.Count; i++)
        {
            node.Children[i] = Evaluate(env, (dynamic)node.Children[i]);
        }
        
        return node;
    }

#region KEYWORDS

    public static AstElementNode Evaluate(Environment env, AstQuoteNode node)
    {
        // Eval Quote
        Console.WriteLine("Eval Quote");
        return node;
    }

    public static AstElementNode Evaluate(Environment env, AstSetQNode node)
    {
        // Eval SetQ
        Console.WriteLine("Eval SetQ");
        return new AstElementNode();
    }

    public static AstElementNode Evaluate(Environment env, AstFuncNode node)
    {
        // Eval FuncDeclaration
        Console.WriteLine("Eval FuncDeclaration");
        
        var id = node.Name.Token.Value!.ToString();
        env.AddEntry(id!, node);
        
        return node.Name;
    }

    public static AstElementNode Evaluate(Environment env, AstLambdaNode node)
    {
        // Eval Lambda
        Console.WriteLine("Eval Lambda");

        // Работаем дальше с контекстом, а не env
        // Context - это локальный контекст, создаем новый на основе предыдущего
        // Чтобы hadowing аботал конкретно
        var context = new Environment(env);

        return new AstElementNode();
    }
    
    public static AstElementNode Evaluate(Environment env, AstProgNode node)
    {
        // Eval Prog
        Console.WriteLine("Eval Prog");
        
        // Работаем дальше с контекстом, а не env
        // Context - это локальный контекст, создаем новый на основе предыдущего
        // Чтобы hadowing аботал конкретно
        var context = new Environment(env);
        
        return new AstElementNode();
    }
    
    public static AstElementNode Evaluate(Environment env, AstCondNode node)
    {
        // Eval Cond
        Console.WriteLine("Eval Cond");
        // Evaluate(env, (dynamic)node.TrueArgument);
        // Evaluate(env, (dynamic)node.Condition);
        // Evaluate(env, (dynamic)node.FalseArgument!);
        
        // Работаем дальше с контекстом, а не env
        // Context - это локальный контекст, создаем новый на основе предыдущего
        // Чтобы hadowing аботал конкретно
        var context = new Environment(env);
        
        return new AstElementNode();
    }
    
    public static AstElementNode Evaluate(Environment env, AstWhileNode node)
    {
        // Eval While
        Console.WriteLine("Eval While");
        
        // Работаем дальше с контекстом, а не env
        // Context - это локальный контекст, создаем новый на основе предыдущего
        // Чтобы hadowing аботал конкретно
        var context = new Environment(env);
        
        return new AstElementNode();
    }
    
    public static AstElementNode Evaluate(Environment env, AstReturnNode node)
    {
        // Eval Return
        Console.WriteLine("Eval Return");
        return new AstElementNode();
    }
    
    public static AstElementNode Evaluate(Environment env, AstBreakNode node)
    {
        // Eval Break
        Console.WriteLine("Eval Break");
        return new AstElementNode();
    }

#endregion

#region PREDEFINED

#region Arithmetic

    public static AstElementNode EvaluatePlus(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Plus");
        return new AstElementNode();
    }

    public static AstElementNode EvaluateMinus(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Minus");
        return new AstElementNode();
    }

    public static AstElementNode EvaluateTimes(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Times");
        return new AstElementNode();
    }

    public static AstElementNode EvaluateDivide(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Divide");
        return new AstElementNode();
    }

#endregion

#region ListOperations

    public static AstElementNode EvaluateHead(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Head");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateTail(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Tail");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateCons(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Cons");
        return new AstElementNode();
    }

#endregion

#region Comparisons

    public static AstElementNode EvaluateEqual(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Equal");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateNonEqual(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval NonEqual");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateLess(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Less");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateLessEq(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval LessEq");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateGreater(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Greater");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateGreaterEq(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval GreaterEq");
        return new AstElementNode();
    }

#endregion

#region Predicates

    public static AstElementNode EvaluateIsInt(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsInt");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateIsReal(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsReal");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateIsBool(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsBool");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateIsNull(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsNull");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateIsAtom(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsAtom");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateIsList(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsList");
        return new AstElementNode();
    }

#endregion

#region LogicalOperations

    public static AstElementNode EvaluateAnd(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval And");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateOr(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Or");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateXor(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Xor");
        return new AstElementNode();
    }
    
    public static AstElementNode EvaluateNot(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Not");
        return new AstElementNode();
    }

#endregion

#region Evaluator

    public static AstElementNode EvaluateEval(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Eval");
        return new AstElementNode();
    }

#endregion

#endregion
}