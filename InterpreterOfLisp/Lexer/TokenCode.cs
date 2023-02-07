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
    
    /* LIST OPERATIONS */
    HeadTk,
    TailTk,
    ConsTk,
    
    /* COMPARISONS */
    EqualTk,
    NonEqualTk,
    LessTk,
    LessEqTk,
    GreaterTk,
    GreaterEqTk,
    
    /* PREDICATES */
    IsIntTk,
    IsRealTk,
    IsBoolTk,
    IsNullTk,
    IsAtomTk,
    IsListTk,
    
    /* LOGICAL OPERATORS */
    AndTk,
    OrTk,
    XorTk,
    NotTk,
    
    /* EVALUATOR */
    EvalTk,
    
    /* TYPES */
    IntTk,
    RealTk,
    BoolTk,
    IdentifierTk
}