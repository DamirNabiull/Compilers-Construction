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
        var childrenStrings = String.Join(",\n\t", Children);

        return "AstListNode()";
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
