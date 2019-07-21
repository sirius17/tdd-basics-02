using System;
namespace ConsoleCalculator
{
    public class Add : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA + opB;
    }

    public class Subtract : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA - opB;
    }

    public class Multiply : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA * opB;
    }

    public class Divide : IBinaryOp
    {
        public int Apply(int opA, int opB) => opA / opB;
    }
}
