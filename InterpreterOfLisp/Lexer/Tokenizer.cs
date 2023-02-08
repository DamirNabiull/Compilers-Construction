using System.Text.RegularExpressions;

namespace InterpreterOfLisp.Lexer;

public class Tokenizer
{
    private readonly string _text;
    private int _position = 0;
    
    public Tokenizer(string text)
    {
        _text = text;
    }

    public void GetAllTokens()
    {
        while (_position < _text.Length)
            GetNextToken();
    }

    private Token GetNextToken()
    {
        object value = GetNextSyntaxElement();
        var start = _position;
        var code = TokenCode.BadTk;
        
        switch (value)
        {
            
            // ДОПИСАТЬ ВСЕ КЕЙСЫ
            
            case "(":
                code = TokenCode.OpenParTk;
                break;
            case ")":
                code = TokenCode.CloseParTk;
                break;
            case "\0":
                code = TokenCode.EofTk;
                break;
            case "quote":
                code = TokenCode.QuoteTk;
                break;
            case "\'":
                code = TokenCode.QuoteTk;
                break;
            case "setq":
                code = TokenCode.SetqTk;
                break;
            case "func":
                code = TokenCode.FuncTk;
                break;
            case "lambda":
                code = TokenCode.LambdaTk;
                break;
            case "prog":
                code = TokenCode.ProgTk;
                break;
            case "cond":
                code = TokenCode.CondTk;
                break;
            case "while":
                code = TokenCode.WhileTk;
                break;
            case "return":
                code = TokenCode.ReturnTk;
                break;
            case "break":
                code = TokenCode.BreakTk;
                break;
            case "plus":
                code = TokenCode.PlusTk;
                break;
            case "+":
                code = TokenCode.PlusTk;
                break;
            case "minus":
                code = TokenCode.MinusTk;
                break;
            case "-":
                code = TokenCode.MinusTk;
                break;
            case "times":
                code = TokenCode.TimesTk;
                break;
            case "*":
                code = TokenCode.TimesTk;
                break;
            case "divide":
                code = TokenCode.DivideTk;
                break;
            case "/":
                code = TokenCode.DivideTk;
                break;
            case "head":
                code = TokenCode.HeadTk;
                break;
            case "tail":
                code = TokenCode.TailTk;
                break;
            case "cons":
                code = TokenCode.ConsTk;
                break;
            case "equal":
                code = TokenCode.EqualTk;
                break;
            case "nonequal":
                code = TokenCode.NonEqualTk;
                break;
            case "less":
                code = TokenCode.LessTk;
                break;
            case "greater":
                code = TokenCode.GreaterTk;
                break;
            case "lesseq":
                code = TokenCode.LessEqTk;
                break;
            case "greatereq":
                code = TokenCode.GreaterEqTk;
                break;
            case "isint":
                code = TokenCode.IsIntTk;
                break;
            case "isreal":
                code = TokenCode.IsRealTk;
                break;
            case "isbool":
                code = TokenCode.IsBoolTk;
                break;
            case "isnull":
                code = TokenCode.IsNullTk;
                break;
            case "isatom":
                code = TokenCode.IsAtomTk;
                break;
            case "islist":
                code = TokenCode.IsListTk;
                break;
            case "and":
                code = TokenCode.AndTk;
                break;
            case "or":
                code = TokenCode.OrTk;
                break;
            case "xor":
                code = TokenCode.XorTk;
                break;
            case "not":
                code = TokenCode.NotTk;
                break;
            case "eval":
                code = TokenCode.EvalTk;
                break;
            default:
                // Добавить определение типов
                code = TokenCode.IdentifierTk;
                break;
        }
        
        Console.WriteLine($"{value} - {code}");

        return new Token(start, _position, code, value);
    }
    
    // Можно уточнить, нужен ли нам span, если нет, то используем это
    public void GetAllSyntaxElements()
    {
        var matchList = Regex.Matches(_text, @"[()']|[a-zA-Z0-9]+|[+-][1]");
        var list = matchList.Cast<Match>().Select(match => match.Value).ToList();
        foreach (var el in list)
        {
            Console.WriteLine(el);
        }
    }

    private string GetNextSyntaxElement()
    {
        SkipSpaces();
        var value = GetCurrentChar().ToString();
        _position++;
    
        switch (value)
        {

            // Дописать +, - и остальные кейсы

            case "(":
                break;
            case ")":
                break;
            case "\0":
                break;
            case "\'":
                break;
            default:
                while (GetCurrentChar() != '\0' 
                       && GetCurrentChar() != '(' 
                       && GetCurrentChar() != ')' 
                       && GetCurrentChar() != ' ')
                {
                    value += GetCurrentChar();
                    _position++;
                }
                break;
        }
    
        return value;
    }

    private void SkipSpaces()
    {
        while (_text[_position] == ' ')
            _position++;
    }

    private char GetCurrentChar()
    {
        return _text[_position];
    }
}