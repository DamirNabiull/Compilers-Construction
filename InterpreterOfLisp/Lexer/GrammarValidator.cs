using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace InterpreterOfLisp.Lexer;

public static class GrammarValidator
{
    private static readonly Regex IntegerRegEx = new Regex(@"^[+-]?\d+$");
    private static readonly Regex RealRegEx = new Regex(@"^[+-]?\d+.\d+$");
    private static readonly Regex BoolRegEx = new Regex(@"^(true|false)$");
    private static readonly Regex IdentifierRegEx = new Regex(@"^[a-zA-z][a-zA-Z\d]*$");
    
    public static (TokenCode, object) RecognizeToken(Span span, object element)
    {
        var value = element.ToString()!;
            
        if (IntegerRegEx.IsMatch(value))
            return (TokenCode.IntTk, int.Parse(value));

        if (RealRegEx.IsMatch(value))
            return (TokenCode.RealTk, double.Parse(value, CultureInfo.InvariantCulture));

        if (BoolRegEx.IsMatch(value))
            return (TokenCode.BoolTk, bool.Parse(value));

        if (IdentifierRegEx.IsMatch(value))
            return (TokenCode.IdentifierTk, value);

        // Сделать красиво тут
        throw new SyntaxErrorException($"Incorrect syntax:\n{span}");
    }
}