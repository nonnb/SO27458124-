using System;

namespace ClassLibrary3
{
    public class ExpensiveClass
    {
        private readonly int[] _memAlloc = new int[1000000];
        private readonly Action _actionWhenDone;

        public ExpensiveClass(Action actionWhenDone)
        {
            _actionWhenDone = actionWhenDone;
        }
        ~ExpensiveClass()
        {
            _memAlloc[0]++; // Some random action to ensure our expensive field isn't optimized out
            _actionWhenDone();
        }
    }
}
