namespace InterpreterOfLisp.SemanticsAnalyzer.Types;

public class AbsType {} // Base type, may be used as the Any type

public class VoidType : AbsType {}

public class ListType : AbsType {}

public class FuncType : AbsType { 
    public readonly List<AbsType> argumentTypes;
    public readonly AbsType returnType;

    public FuncType(List<AbsType> argumentTypes, AbsType returnType) {
        this.argumentTypes = argumentTypes;
        this.returnType = returnType;
    }
}

public class RealType : AbsType {}

public class IntegerType : RealType {} // Integers are just a fraction of real numbers

public class BooleanType : AbsType {}
