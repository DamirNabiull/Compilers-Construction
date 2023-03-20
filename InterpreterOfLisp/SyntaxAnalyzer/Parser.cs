using System.Data;
using System.Diagnostics;
using Token = InterpreterOfLisp.Lexer.Token;
using TokenCode = InterpreterOfLisp.Lexer.TokenCode;

namespace InterpreterOfLisp.SyntaxAnalyzer;

public class AstParser
{
    private readonly List<Token> _tokens;
    private int _position = 0;
    private AstProgramNode? _rootNode;

    public AstParser(IEnumerable<Token> tokens)
    {
        _tokens = tokens.ToList();
    }

    public AstProgramNode Parse() => _rootNode ??= ParseProgram();

    private void PrintNode(AstNode node, string prefix = "") {
        Console.WriteLine(prefix + node);

        switch (node) {
            case IAstNodeWithChildren listNode:
                var newPrefix = prefix + "|\t";

                foreach (var child in listNode.Children) {
                    PrintNode(child, newPrefix);
                }
                break;
        }
    }

    public void PrintNodes() {
        var node = Parse();
        
        PrintNode(node);
    }

    private Token? NextToken() {
        _position++;
        return CurrentToken();
    }

    private Token? CurrentToken() {
        return _tokens.Count > _position ? _tokens[_position] : null;
    }

    private AstProgramNode ParseProgram() {
        var currentToken = CurrentToken();

        var elements = new List<AstElementNode>();
        
        while (currentToken is not null) {
            switch (currentToken.Code) {
                case TokenCode.EofTk:
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
        
        Debug.Assert(quoteToken.Code == TokenCode.QuoteTk);

        quoteToken = new Token(quoteToken.TextSpan, TokenCode.IdentifierTk, "quote");

        var nextToken = NextToken()!;
        if (nextToken.Code == TokenCode.EofTk) {
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
            case TokenCode.OpenParTk:
                return ParseList();
            case TokenCode.IdentifierTk:
                return new AstIdentifierNode(currentToken);
            case TokenCode.BoolTk:
            case TokenCode.IntTk:
            case TokenCode.RealTk:
            case TokenCode.NullTk:
                return new AstLiteralNode(currentToken);
            // I think internally we should treat it like list with first element as `quote` func
            case TokenCode.QuoteTk:
                return ParseQuoteToken();
            case TokenCode.CloseParTk:
                throw new SyntaxErrorException($"Closing parenthesis without opening one");
        }
        
        throw new SyntaxErrorException($"Can't parse element");
    }

    private AstListNode ParseList() {
        var currentToken = CurrentToken();

        var elements = new List<AstElementNode>();

        // do we start from right place?
        Debug.Assert(currentToken is not null && currentToken.Code == TokenCode.OpenParTk);

        currentToken = NextToken();
        
        while (currentToken is not null) {
            switch (currentToken.Code) {
                case TokenCode.CloseParTk:
                    return new AstListNode(elements);
                case TokenCode.EofTk:
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
