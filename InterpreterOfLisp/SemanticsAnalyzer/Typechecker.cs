using InterpreterOfLisp.SemanticsAnalyzer.Types;
using InterpreterOfLisp.SyntaxAnalyzer;

namespace InterpreterOfLisp.SemanticsAnalyzer;

public class Typechecker {
    private readonly AstProgramNode _astRootNode;

    public Typechecker(AstProgramNode astRootNode) {
        _astRootNode = astRootNode;
    }

    public void TypecheckProgram() {
        var env = new TypecheckerEnv();

        foreach (var i in this._astRootNode.Children) {
            this.TypecheckElement(env, i, new AbsType());
        }
    }

    protected TypecheckerEnv TypecheckSetQ(TypecheckerEnv env, AstSetQNode node, AbsType t) {
        env.AddEnvEntry(node.Assignee.Token.Value.ToString(), new AbsType());
        this.TypecheckElement(env, node.AssignedValue, new AbsType());
        // env.EnvPopEntry(node.Assignee.Token.Value.ToString());
        return env;
    }

    protected TypecheckerEnv TypecheckFunc(TypecheckerEnv env, AstFuncNode node, AbsType t)
    {
        env.AddEnvEntry(
            node.Name.Token.Value.ToString(), 
            new FuncType(
                Enumerable.Repeat(new AbsType(), node.Parameters.Count).ToList(), 
                new AbsType()
            )
        );
        
        var local = new TypecheckerEnv(env);
        
        foreach (var i in node.Parameters) {
            local.AddEnvEntry(i.Token.Value.ToString(), new AbsType());
        }
        local.AddKeywordContext("func");
        this.TypecheckElement(local, node.Body, new AbsType());
        local.PopKeywordContext("func");
        // foreach (var i in node.Parameters) {
        //     env.EnvPopEntry(i.Token.Value.ToString());
        // }
        
        return env;
    }

    protected TypecheckerEnv TypecheckLambda(TypecheckerEnv env, AstLambdaNode node, AbsType t) {
        var local = new TypecheckerEnv(env);
        
        foreach (var i in node.Parameters) {
            local.AddEnvEntry(i.Token.Value.ToString(), new AbsType());
        }
        local.AddKeywordContext("lambda");
        this.TypecheckElement(local, node.Body, new AbsType());
        local.PopKeywordContext("lambda");
        // foreach (var i in node.Parameters) {
        //     env.EnvPopEntry(i.Token.Value.ToString());
        // }

        return env;
    }

    protected TypecheckerEnv TypecheckProg(TypecheckerEnv env, AstProgNode node, AbsType t) {
        // ig ill just leave it like that? i dont really understand what the node does from the project description
        var local = new TypecheckerEnv(env);
        
        foreach (var i in node.Parameters) {
            local.AddEnvEntry(i.Token.Value.ToString(), new AbsType());
        }
        local.AddKeywordContext("prog");
        this.TypecheckElement(local, node.Body, new AbsType());
        local.PopKeywordContext("prog");
        // foreach (var i in node.Parameters) {
        //     env.EnvPopEntry(i.Token.Value.ToString());
        // }

        return env;
    }

    protected TypecheckerEnv TypecheckCond(TypecheckerEnv env, AstCondNode node, AbsType t) {
        var local = new TypecheckerEnv(env);
        
        this.TypecheckElement(local, node.Condition, new AbsType());
        this.TypecheckElement(local, node.TrueArgument, new AbsType());
        if (node.FalseArgument != null)
            this.TypecheckElement(local, node.FalseArgument, new AbsType());

        return env;
    }

    protected TypecheckerEnv TypecheckWhile(TypecheckerEnv env, AstWhileNode node, AbsType t) {
        var local = new TypecheckerEnv(env);
        
        this.TypecheckElement(local, node.Condition, new AbsType());
        local.AddKeywordContext("while");
        this.TypecheckElement(local, node.Body, new AbsType());
        local.PopKeywordContext("while");

        return env;
    }

    protected TypecheckerEnv TypecheckReturn(TypecheckerEnv env, AstReturnNode node, AbsType t) {
        if (!env.IsInKeywordContext(new List<string> {"prog", "lambda", "func"})) {
            throw new Exception("Unexpected return keyword");
        }
        return env;
    }

    protected TypecheckerEnv TypecheckBreak(TypecheckerEnv env, AstBreakNode node, AbsType t) {
        if (!env.IsInKeywordContext(new List<string> {"while"})) {
            throw new Exception("Unexpected break keyword");
        }
        return env;
    }

    protected TypecheckerEnv TypecheckIdentifier(TypecheckerEnv env, AstIdentifierNode node, AbsType t) {
        AbsType? found = env.EnvGetEntry(node.Token.Value.ToString());
        if (found == null) {
            throw new Exception("Unknown indentifier: " + node.Token.Value.ToString());
        }
        return env;
    }

    protected TypecheckerEnv TypecheckList(TypecheckerEnv env, AstListNode node, AbsType t) {
        var local = new TypecheckerEnv(env);
        
        if (node.Children.Count != 0 && node.Children[0] is AstIdentifierNode idnode) {
            this.TypecheckIdentifier(local, idnode, new AbsType());
            AbsType found = local.EnvGetEntry(idnode.Token.Value.ToString())!;
            if (found is FuncType ft) {
                if (ft.argumentTypes.Count != node.Children.Count - 1) {
                    throw new Exception("Argument type mismatch");
                }
            }
            // if its not a function it still technically could be one (eg lambda in a setq expr)
            // so im not going to be throwing an exception here, since im not sure its actually not a function
        }

        foreach (var i in node.Children) {
            this.TypecheckElement(local, i, new AbsType());
        }

        return env;
    }

    protected void TypecheckElement(TypecheckerEnv env, AstElementNode node, AbsType t) {
        if (node is AstSetQNode setQNode) {
            this.TypecheckSetQ(env, setQNode, t);
        } else if (node is AstFuncNode funcNode) {
            this.TypecheckFunc(env, funcNode, t);
        } else if (node is AstLambdaNode lambdaNode) {
            this.TypecheckLambda(env, lambdaNode, t);
        } else if (node is AstProgNode progNode) {
            this.TypecheckProg(env, progNode, t);
        } else if (node is AstCondNode condNode) {
            this.TypecheckCond(env, condNode, t);
        } else if (node is AstWhileNode whileNode) {
            this.TypecheckWhile(env, whileNode, t);
        } else if (node is AstReturnNode returnNode) {
            this.TypecheckReturn(env, returnNode, t);
        } else if (node is AstBreakNode breakNode) {
            this.TypecheckBreak(env, breakNode, t);
        } else if (node is AstIdentifierNode idNode) {
            this.TypecheckIdentifier(env, idNode, t);
        } else if (node is AstListNode listNode) {
            this.TypecheckList(env, listNode, t);
        }
    }
}
