using System.Data;
using System.Diagnostics;

namespace InterpreterOfLisp.SyntaxAnalyzer;

public class AstParser
{
    private List<Lexer.Token> _tokens;
    private int _position = 0;
    private AstProgramNode? _rootNode;

    public AstParser(IEnumerable<Lexer.Token> tokens)
    {
        _tokens = tokens.ToList();
    }

    public AstProgramNode Parse() {
        if (_rootNode is null) {
            _rootNode = ParseProgram();
        }
        return _rootNode;
    }
    
    private void PrintNode(AstNode node, string prefix = "") {
        Console.WriteLine(prefix + node);
        
        string new_prefix;

        switch (node) {
            case IAstNodeWithChildren listNode:
                new_prefix = prefix + "|\t";

                foreach (var child in listNode.Children) {
                    PrintNode(child, new_prefix);
                }
                break;
        }
    }

    public void PrintNodes() {
        var node = Parse();
        
        PrintNode(node);
    }

    private Lexer.Token? NextToken() {
        _position++;
        return CurrentToken();
    }

    private Lexer.Token? CurrentToken() {
        return _tokens.Count > _position ? _tokens[_position] : null;
    }

    private AstProgramNode ParseProgram() {
        var currentToken = CurrentToken();

        List<AstElementNode> elements = new List<AstElementNode>();
        
        while (currentToken is not null) {
            switch (currentToken.Code) {
                case Lexer.TokenCode.EofTk:
                    return new AstProgramNode(elements);

                default:
                    var element = ParseElement();
                    elements.Add(element);

                    break;
            }

            currentToken = NextToken();
        }

        throw new InvalidOperationException("Unreachable code by design. Maybe no `EofTk`?");
    }

    private AstListNode ParseQuoteToken() {
        var quoteToken = CurrentToken()!;
        
        Debug.Assert(quoteToken.Code == Lexer.TokenCode.QuoteTk);

        quoteToken = new Lexer.Token(quoteToken.TextSpan, Lexer.TokenCode.IdentifierTk, "quote");

        var nextToken = NextToken()!;
        if (nextToken.Code == Lexer.TokenCode.EofTk) {
            throw new SyntaxErrorException($"Nothing follows after `'`");
        }

        var element = ParseElement();

        return new AstListNode(new List<AstElementNode>{
            new AstIdentifierNode(quoteToken),
            element
        });
    }

    private AstElementNode ParseElement() {
        var currentToken = CurrentToken()!;
        
        switch (currentToken.Code) {
            case Lexer.TokenCode.OpenParTk:
                return ParseList();
            case Lexer.TokenCode.IdentifierTk:
                return new AstIdentifierNode(currentToken);
            case Lexer.TokenCode.BoolTk:
            case Lexer.TokenCode.IntTk:
            case Lexer.TokenCode.RealTk:
            case Lexer.TokenCode.NullTk:
                return new AstLiteralNode(currentToken);
            // I think internally we should treat it like list with first element as `quote` func
            case Lexer.TokenCode.QuoteTk:
                return ParseQuoteToken();
            case Lexer.TokenCode.CloseParTk:
                throw new SyntaxErrorException($"Closing parenthesis without opening one");
        }
        
        throw new SyntaxErrorException($"Can't parse element");
    }

    private AstElementNode ParseList() {
        var currentToken = CurrentToken()!;

        List<AstElementNode> elements = new List<AstElementNode>();

        // do we start from right place?
        Debug.Assert(currentToken is not null && currentToken.Code == Lexer.TokenCode.OpenParTk);

        currentToken = NextToken();
        
        while (currentToken is not null) {
            switch (currentToken.Code) {
                case Lexer.TokenCode.CloseParTk:
                    return new AstListNode(elements);
                case Lexer.TokenCode.EofTk:
                    break;
                default:
                    var element = ParseElement();
                    elements.Add(element);
                    break;
            }
            currentToken = NextToken();
        }

        throw new SyntaxErrorException($"List does not have closing parenthesis");
    }

}
