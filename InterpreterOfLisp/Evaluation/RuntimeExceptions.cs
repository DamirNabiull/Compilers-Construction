namespace InterpreterOfLisp.Evaluation;

class ControlException : Exception {
    public ControlException() : base("Control exception. If not catched, TypeChecker is broken") {}
}

class BreakException : ControlException {}

class ReturnException : ControlException {
    public readonly dynamic value;

    public ReturnException(dynamic value) : base() {
        this.value = value;
    }
}
