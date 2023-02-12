namespace InterpreterOfLisp.Lexer;

public class Span
{
    private readonly int _start;
    private readonly int _end;
    private readonly int _length;
    private readonly int _line;

    public Span(int start, int end, int line)
    {
        _start = start;
        _end = end;
        _length = end - start;
        _line = line;
    }

    public override string ToString()
    {
        return $"\tLine: {_line}\n\tStart: {_start}\n\tEnd: {_end}\n\tLength: {_length}\n";
    }
}