﻿using InterpreterOfLisp.SyntaxAnalyzer;

namespace InterpreterOfLisp.Evaluation;

public class Evaluator
{
    private readonly AstProgramNode _astRootNode;
    
    public Evaluator(AstProgramNode astRootNode) {
        _astRootNode = astRootNode;
    }
    
    public void Evaluate()
    {
        var env = new Environment();

        foreach (var i in this._astRootNode.Children) {
            Evaluate(env, (dynamic)i);
        }
    }
    
    private AstElementNode Evaluate(Environment env, AstIdentifierNode node)
    {
        // Eval Identifier
        Console.WriteLine("Eval Identifier");

        var id = node.Token.Value!.ToString()!;
        env.GetEntry(id);
        return new AstElementNode();
    }

    private AstElementNode Evaluate(Environment env, AstListNode node)
    {
        // Eval ListDeclaration
        if (node.Children.Count != 0 && node.Children[0] is AstIdentifierNode)
        {
            EvaluateApplication(env, node);
        }
        else
        {
            EvaluateList(env, node);
        }
        
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateApplication(Environment env, AstListNode node)
    {
        // Eval FuncApplicationDeclaration
        Console.WriteLine("Eval FuncApplicationDeclaration");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateList(Environment env, AstListNode node)
    {
        // Eval AtomsListDeclaration
        // Для каждого элемента листа сделать eval
        Console.WriteLine("Eval AtomsListDeclaration");
        return new AstElementNode();
    }

#region KEYWORDS

    private AstElementNode Evaluate(Environment env, AstFuncNode node)
    {
        // Eval FuncDeclaration
        Console.WriteLine("Eval FuncDeclaration");
        return new AstElementNode();
    }

    private AstElementNode Evaluate(Environment env, AstSetQNode node)
    {
        // Eval SetQ
        Console.WriteLine("Eval SetQ");
        return new AstElementNode();
    }
    
    private AstElementNode Evaluate(Environment env, AstLambdaNode node)
    {
        // Eval Lambda
        Console.WriteLine("Eval Lambda");
        return new AstElementNode();
    }
    
    private AstElementNode Evaluate(Environment env, AstProgNode node)
    {
        // Eval Prog
        Console.WriteLine("Eval Prog");
        return new AstElementNode();
    }
    
    private AstElementNode Evaluate(Environment env, AstCondNode node)
    {
        // Eval Cond
        Console.WriteLine("Eval Cond");
        // Evaluate(env, (dynamic)node.TrueArgument);
        // Evaluate(env, (dynamic)node.Condition);
        // Evaluate(env, (dynamic)node.FalseArgument!);
        return new AstElementNode();
    }
    
    private AstElementNode Evaluate(Environment env, AstWhileNode node)
    {
        // Eval While
        Console.WriteLine("Eval While");
        return new AstElementNode();
    }
    
    private AstElementNode Evaluate(Environment env, AstReturnNode node)
    {
        // Eval Return
        Console.WriteLine("Eval Return");
        return new AstElementNode();
    }
    
    private AstElementNode Evaluate(Environment env, AstBreakNode node)
    {
        // Eval Break
        Console.WriteLine("Eval Break");
        return new AstElementNode();
    }

#endregion

#region PREDEFINED

#region Arithmetic

    private AstElementNode EvaluatePlus(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Plus");
        return new AstElementNode();
    }

    private AstElementNode EvaluateMinus(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Minus");
        return new AstElementNode();
    }

    private AstElementNode EvaluateTimes(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Times");
        return new AstElementNode();
    }

    private AstElementNode EvaluateDivide(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Divide");
        return new AstElementNode();
    }

#endregion

#region ListOperations

    private AstElementNode EvaluateHead(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Head");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateTail(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Tail");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateCons(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Cons");
        return new AstElementNode();
    }

#endregion

#region Comparisons

    private AstElementNode EvaluateEqual(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Equal");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateNonEqual(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval NonEqual");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateLess(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Less");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateLessEq(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval LessEq");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateGreater(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Greater");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateGreaterEq(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval GreaterEq");
        return new AstElementNode();
    }

#endregion

#region Predicates

    private AstElementNode EvaluateIsInt(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsInt");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateIsReal(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsReal");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateIsBool(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsBool");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateIsNull(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsNull");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateIsAtom(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsAtom");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateIsList(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval IsList");
        return new AstElementNode();
    }

#endregion

#region LogicalOperations

    private AstElementNode EvaluateAnd(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval And");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateOr(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Or");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateXor(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Xor");
        return new AstElementNode();
    }
    
    private AstElementNode EvaluateNot(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Not");
        return new AstElementNode();
    }

#endregion

#region Evaluator

    private AstElementNode EvaluateEval(Environment env, AstListNode node)
    {
        Console.WriteLine("Eval Eval");
        return new AstElementNode();
    }

#endregion

#endregion
}