namespace InterpreterOfLisp.Lexer;

public class Token
{
    public Span TextSpan;
    public TokenCode Code;
    public object Value;

    public Token(Span span, TokenCode code, object value)
    {
        TextSpan = span;
        Code = code;
        Value = value;
    }
    
    public Token(int start, int end, TokenCode code, object value)
    {
        TextSpan = new Span(start, end);
        Code = code;
        Value = value;
    }
}