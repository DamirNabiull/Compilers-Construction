namespace InterpreterOfLisp.Lexer;

public enum TokenCode
{
    /* TYPES */
    IntTk,
    RealTk,
    BoolTk,
    IdentifierTk,
    
    /* QUOTE */
    QuoteTk,
    
    /* NULL */
    NullTk,
    
    /* PARENTHESIS */
    OpenParTk,
    CloseParTk,
    
    /* EOF */
    EofTk
}