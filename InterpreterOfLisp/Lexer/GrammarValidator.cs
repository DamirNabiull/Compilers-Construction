using System.Data;
using System.Text.RegularExpressions;

namespace InterpreterOfLisp.Lexer;

public static class GrammarValidator
{
    private static readonly Regex IntegerRegEx = new Regex(@"^[+-]?\d+$");
    private static readonly Regex RealRegEx = new Regex(@"^[+-]?\d+.\d+$");
    private static readonly Regex BoolRegEx = new Regex(@"^(true|false)$");
    private static readonly Regex IdentifierRegEx = new Regex(@"^[a-zA-z][a-zA-Z\d]*$");
    
    public static Token RecognizeToken(Span span, string value)
    {
        if (IntegerRegEx.IsMatch(value))
            return new Token(
                span: span, 
                code: TokenCode.IntTk, 
                value: int.Parse(value));

        if (RealRegEx.IsMatch(value))
            return new Token(
                span: span, 
                code: TokenCode.RealTk, 
                value: double.Parse(value));

        if (BoolRegEx.IsMatch(value))
            return new Token(
                span: span, 
                code: TokenCode.BoolTk, 
                value: bool.Parse(value));

        if (IdentifierRegEx.IsMatch(value))
            return new Token(
                span: span, 
                code: TokenCode.IdentifierTk, 
                value: value);
        
        // Сделать красиво тут
        throw new SyntaxErrorException($"Incorrect syntax: {value==" "}\n{span}");
    }
}