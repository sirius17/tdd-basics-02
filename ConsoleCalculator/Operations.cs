using System;
namespace ConsoleCalculator
{
    public class Add : IBinaryOp
    {
        public float Apply(float opA, float opB) => opA + opB;
    }

    public class Subtract : IBinaryOp
    {
        public float Apply(float opA, float opB) => opA - opB;
    }

    public class Multiply : IBinaryOp
    {
        public float Apply(float opA, float opB) => opA * opB;
    }

    public class Divide : IBinaryOp
    {
        public float Apply(float opA, float opB)
        {
            if (Math.Abs(opB) < float.Epsilon)
                throw new DivideByZeroException();
            return opA / opB;
        }
    }
}
