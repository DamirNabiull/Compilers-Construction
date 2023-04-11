using SyntaxErrorException = System.Data.SyntaxErrorException;
using Token = InterpreterOfLisp.Lexer.Token;

namespace InterpreterOfLisp.SyntaxAnalyzer;

public interface IAstNodeWithChildren {
    public List<AstElementNode> Children { get; set; }
}

public class AstNode { }

public class AstProgramNode : AstNode, IAstNodeWithChildren {
    public List<AstElementNode> Children {get; set;}

    public AstProgramNode(List<AstElementNode> children) {
        Children = children;
    }

    public override string ToString() {
        return "AstProgramNode()";
    }
}

public class AstElementNode : AstNode { }

public class AstListNode : AstElementNode, IAstNodeWithChildren {
    public List<AstElementNode> Children {get; set;}

    public AstListNode(List<AstElementNode> children) {
        Children = children;
    }

    public override string ToString() {
        return "AstListNode()";
    }
}

public class AstKeywordNode : AstListNode
{
    public AstKeywordNode(List<AstElementNode> children) : base(children.Skip(1).ToList()) {}
}

public class AstQuoteNode : AstKeywordNode {
    public AstElementNode Element;

    public AstQuoteNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 2) {
            throw new SyntaxErrorException("Invalid number of arguments for `quote` keyword");
        }

        Element = children[1];
    }

    public override string ToString() {
        return "AstQuoteNode()";
    }
}

public class AstSetQNode : AstKeywordNode {
    public AstIdentifierNode Assignee;
    public AstElementNode AssignedValue;

    public AstSetQNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 3) {
            throw new SyntaxErrorException("Invalid number of arguments for `setq` keyword");
        }

        if (children[1] is not AstIdentifierNode) {
            throw new SyntaxErrorException("First argument of `setq` must be Atom");
        }

        Assignee = (AstIdentifierNode) children[1];
        AssignedValue = children[2];
    }

    public override string ToString() {
        return "AstSetQNode()";
    }
}

public class AstFuncNode : AstKeywordNode {
    public AstIdentifierNode Name;
    public List<AstIdentifierNode> Parameters;
    public AstElementNode Body;

    public AstFuncNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 4) {
            throw new SyntaxErrorException("Invalid number of arguments for `func` keyword");
        }

        if (children[1] is not AstIdentifierNode) {
            throw new SyntaxErrorException("First argument of `func` must be Atom");
        }
        Name = (AstIdentifierNode) children[1];

        if (children[2] is not AstListNode) {
            throw new SyntaxErrorException("Second argument of `func` must be List");
        }
        var listNode = ((AstListNode) children[2]);

        if (!listNode.Children.All(child => child is AstIdentifierNode)) {
            throw new SyntaxErrorException($"Second parameter of `prog` must be list of Atoms, but it's not");
        }

        Parameters = listNode.Children.Cast<AstIdentifierNode>().ToList();

        Body = children[3];
    }

    public override string ToString() {
        return "AstFuncNode()";
    }
}

public class AstLambdaNode : AstKeywordNode {
    public List<AstIdentifierNode> Parameters;
    public AstElementNode Body;

    public AstLambdaNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 3) {
            throw new SyntaxErrorException("Invalid number of arguments for `lambda` keyword");
        }
        
        if (children[1] is not AstListNode) {
            throw new SyntaxErrorException("Second argument of `lambda` must be List");
        }
        
        var listNode = ((AstListNode) children[1]);

        if (!listNode.Children.All(child => child is AstIdentifierNode)) {
            throw new SyntaxErrorException($"Second parameter of `lambda` must be list of Atoms, but it's not");
        }

        Parameters = listNode.Children.Cast<AstIdentifierNode>().ToList();

        Body = children[2];
    }

    public override string ToString() {
        return "AstLambdaNode()";
    }
}

public class AstProgNode : AstKeywordNode {
    public List<AstIdentifierNode> Parameters;
    public AstElementNode Body;

    public AstProgNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 3) {
            throw new SyntaxErrorException("Invalid number of arguments for `prog` keyword");
        }
        
        if (children[1] is not AstListNode) {
            throw new SyntaxErrorException("Second argument of `prog` must be List");
        }
        var listNode = ((AstListNode) children[1]);

        if (!listNode.Children.All(child => child is AstIdentifierNode)) {
            throw new SyntaxErrorException($"Second parameter of `prog` must be list of Atoms, but it's not");
        }

        Parameters = listNode.Children.Cast<AstIdentifierNode>().ToList();

        Body = children[2];
    }

    public override string ToString() {
        return "AstProgNode()";
    }
}

public class AstCondNode : AstKeywordNode {
    public AstElementNode Condition;
    public AstElementNode TrueArgument;
    public AstElementNode? FalseArgument;

    public AstCondNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 3 && children.Count != 4) {
            throw new SyntaxErrorException("Invalid number of arguments for `cond` keyword");
        }

        Condition = children[1];
        TrueArgument = children[2];
        FalseArgument = children.ElementAtOrDefault(3);
    }

    public override string ToString() {
        return "AstCondNode()";
    }
}

public class AstWhileNode : AstKeywordNode {
    public AstElementNode Condition;
    public AstElementNode Body;

    public AstWhileNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 3) {
            throw new SyntaxErrorException("Invalid number of arguments for `while` keyword");
        }
        
        Condition = children[1];
        Body = children[2];
    }

    public override string ToString() {
        return "AstWhileNode()";
    }
}

public class AstReturnNode : AstKeywordNode {
    public AstElementNode ReturnValue;

    public AstReturnNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 2) {
            throw new SyntaxErrorException("Invalid number of arguments for `return` keyword");
        }
        ReturnValue = children[1];
    }

    public override string ToString() {
        return "AstReturnNode()";
    }
}

public class AstBreakNode : AstKeywordNode {
    public AstBreakNode(List<AstElementNode> children) : base(children) {
        if (children.Count != 1) {
            throw new SyntaxErrorException("Invalid number of arguments for `break` keyword");
        }
    }

    public override string ToString() {
        return "AstBreakNode()";
    }
}

public class AstIdentifierNode : AstElementNode {
    public Token Token;

    public AstIdentifierNode(Token token) {
        Token = token;
    }

    public override string ToString() {
        return $"AstIdentifierNode({Token.Value})";
    }
}

public class AstLiteralNode : AstElementNode {
    public Token Token;

    public AstLiteralNode(Token token) {
        Token = token;
    }

    public override string ToString() {
        return $"AstLiteralNode({Token.Value})";
    }
}
