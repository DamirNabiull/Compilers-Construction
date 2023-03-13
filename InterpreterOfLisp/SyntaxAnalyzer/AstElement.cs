namespace InterpreterOfLisp.SyntaxAnalyzer;

public interface IAstNodeWithChildren {
    public List<AstElementNode> Children {get; set;}
}

public class AstNode
{

}

public class AstProgramNode : AstNode, IAstNodeWithChildren {
    public List<AstElementNode> Children {get; set;}

    public AstProgramNode(List<AstElementNode> children) {
        Children = children;
    }

    public override String ToString() {
        return "AstProgramNode()";
    }
}

public class AstElementNode : AstNode {

}

public class AstListNode : AstElementNode, IAstNodeWithChildren {
    public List<AstElementNode> Children {get; set;}

    public AstListNode(List<AstElementNode> children) {
        Children = children;
    }

    public override String ToString() {
        var childrenStrings = String.Join(",\n\t", Children);

        return "AstListNode()";
    }
}

public class AstIdentifierNode : AstElementNode {
    public Lexer.Token Token;

    public AstIdentifierNode(Lexer.Token token) {
        Token = token;
    }

    public override String ToString() {
        return $"AstIdentifierNode({Token.Value})";
    }
}

public class AstLiteralNode : AstElementNode {
    public Lexer.Token Token;

    public AstLiteralNode(Lexer.Token token) {
        Token = token;
    }

    public override String ToString() {
        return $"AstLiteralNode({Token.Value})";
    }
}
