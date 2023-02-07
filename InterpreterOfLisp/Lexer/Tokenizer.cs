namespace InterpreterOfLisp.Lexer;

public class Tokenizer
{
    private readonly string _text;
    private int _position = 0;
    private char _currentChar;
    
    public Tokenizer(string text)
    {
        _text = text;
    }

    public Token GetToken()
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
        }
        
        Console.WriteLine($"{value} - {code}");

        return new Token(start, _position, code, value);
    }

    private string GetNextSyntaxElement()
    {
        ReadNextChar();
        var value = _currentChar.ToString();

        switch (value)
        {
            case "(":
                break;
            case ")":
                break;
            case "\0":
                break;
            default:
                while (_currentChar != '\0' && _currentChar != '(' && _currentChar != ')')
                {
                    ReadNextChar();
                    value += _currentChar;
                }
                break;
        }

        return value;
    }

    private void ReadNextChar()
    {
        while (_text[_position] == ' ')
            _position++;
        
        _currentChar = _text[_position];
        _position++;
    }
}