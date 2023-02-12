namespace InterpreterOfLisp.Lexer;

public class Tokenizer
{
    private readonly string _text;
    private int _linePos = 1;
    private int _position = 0;
    private int _lineNumber = 1;
    private readonly List<Token> _tokens = new List<Token>();

    public Tokenizer(string text)
    {
        _text = text;
    }

    private IEnumerable<Token> GetAllTokens()
    {
        while (_position < _text.Length)
            _tokens.Add(GetNextToken());

        return _tokens;
    }

    public void PrintAllTokens()
    {
        GetAllTokens();
        foreach (var token in _tokens)
        {
            Console.WriteLine(token.ToString());
        }
    }

    private Token GetNextToken()
    {
        var (start, element) = GetNextSyntaxElement();
        var tokenSpan = new Span(start, _linePos, _lineNumber);
        
        TokenCode code;
        Token recognizedToken;
        
        switch (element)
        {
            case "(":
                recognizedToken = new Token(
                    span: tokenSpan, 
                    code: TokenCode.OpenParTk, 
                    value: element);
                break;
            case ")":
                recognizedToken = new Token(
                    span: tokenSpan, 
                    code: TokenCode.CloseParTk, 
                    value: element);
                break;
            case "\0":
                recognizedToken = new Token(
                    span: tokenSpan, 
                    code: TokenCode.EofTk, 
                    value: element);
                break;
            case "\'":
                recognizedToken = new Token(
                    span: tokenSpan, 
                    code: TokenCode.QuoteTk, 
                    value: element);
                break;
            case "null":
                recognizedToken = new Token(
                    span: tokenSpan, 
                    code: TokenCode.NullTk, 
                    value: element);
                break;
            default:
                recognizedToken = GrammarValidator.RecognizeToken(
                    span: tokenSpan,
                    value: element);
                break;
        }
        
        return recognizedToken;
    }

    private (int, string) GetNextSyntaxElement()
    {
        SkipSpaces();
        var value = GetCurrentChar().ToString();
        var start = _linePos;
        
        _position++;
        _linePos++;

        switch (value)
        {
            case "(":
                break;
            case ")":
                break;
            case "\0":
                break;
            case "\'":
                break;
            default:
                while (GetCurrentChar() != '\0' 
                       && GetCurrentChar() != '(' 
                       && GetCurrentChar() != ')' 
                       && GetCurrentChar() != ' '
                       && GetCurrentChar() != '\n')
                {
                    value += GetCurrentChar();
                    _position++;
                    _linePos++;
                }
                break;
        }
    
        return (start, value);
    }

    private void SkipSpaces()
    {
        while (_text[_position] == ' ' || _text[_position] == '\n')
        {
            if (_text[_position] == '\n')
            {
                _lineNumber++;
                _linePos = 0;
            }
            
            _position++;
            _linePos++;
        }
    }

    private char GetCurrentChar()
    {
        return _text[_position];
    }
}