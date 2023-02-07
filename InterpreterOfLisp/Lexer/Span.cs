namespace InterpreterOfLisp.Lexer;

public class Span
{
    public int Start { get; }
    public int End { get; }
    public int Length { get; }

    public Span(int start, int end)
    {
        Start = start;
        End = end;
        Length = end - start;
    }
}