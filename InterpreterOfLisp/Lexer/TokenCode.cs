namespace InterpreterOfLisp.Lexer;

public enum TokenCode
{
    /* SPECIAL FORMS */
    QuoteTk,
    SetqTk,
    FunkTk,
    LambdaTk,
    ProgTk,
    CondTk,
    WhileTk,
    ReturnTk,
    BreakTk,
    
    /* PREDEFINED FUNCTIONS */
    PlusTk,
    MinusTk,
    TimesTk,
    DivideTk,
    
    
}