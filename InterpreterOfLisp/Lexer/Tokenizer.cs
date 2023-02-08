using System.Text.RegularExpressions;

namespace InterpreterOfLisp.Lexer;

public class Tokenizer
{
    private readonly string _text;
    private int _position = 0;
    
    public Tokenizer(string text)
    {
        _text = text;
    }

    public void GetAllTokens()
    {
        while (_position < _text.Length)
            GetToken();
    }

    private Token GetToken()
    {
        object value = GetNextSyntaxElement();
        var start = _position;
        var code = TokenCode.BadTk;
        
        switch (value)
        {
            case "(":
                code = TokenCode.OpenParTk;
                break;
            case ")":
                code = TokenCode.CloseParTk;
                break;
            case "\0":
                code = TokenCode.EofTk;
                break;
            case "quote":
                code = TokenCode.QuoteTk;
                break;
            case "\'":
                code = TokenCode.QuoteTk;
                break;
            default:
                code = TokenCode.IdentifierTk;
                break;
        }
        
        Console.WriteLine($"{value} - {code}");

        return new Token(start, _position, code, value);
    }
    
    // Можно уточнить, нужен ли нам span, если нет, то используем это
    public void GetAllSyntaxElements()
    {
        var matchList = Regex.Matches(_text, @"[()']|[a-zA-Z0-9]+|[+-][1]");
        var list = matchList.Cast<Match>().Select(match => match.Value).ToList();
        foreach (var el in list)
        {
            Console.WriteLine(el);
        }
    }

    private string GetNextSyntaxElement()
    {
        SkipSpaces();
        var value = GetCurrentChar().ToString();
        _position++;
    
        switch (value)
        {
            case "(":
                break;
            case ")":
                break;
            case "\0":
                break;
            default:
                while (GetCurrentChar() != '\0' 
                       && GetCurrentChar() != '(' 
                       && GetCurrentChar() != ')' 
                       && GetCurrentChar() != ' ')
                {
                    value += GetCurrentChar();
                    _position++;
                }
                break;
        }
    
        return value;
    }

    private void SkipSpaces()
    {
        while (_text[_position] == ' ')
            _position++;
    }

    private char GetCurrentChar()
    {
        return _text[_position];
    }
}