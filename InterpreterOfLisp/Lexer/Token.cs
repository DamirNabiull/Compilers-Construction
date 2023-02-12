namespace InterpreterOfLisp.Lexer;

public class Token
{
    public Span TextSpan;
    public TokenCode Code;
    public object? Value;

    public Token(Span span, TokenCode code, object value)
    {
        TextSpan = span;
        Code = code;
        Value = value;
    }
    
    public Token(int start, int end, int line, TokenCode code, object value)
    {
        TextSpan = new Span(start, end, line);
        Code = code;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Value} - {Code}\n{TextSpan}";
    }
}