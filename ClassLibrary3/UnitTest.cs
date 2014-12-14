using System;
using System.Threading;
using NUnit.Framework;

namespace ClassLibrary3
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void EnsureGCDoesntHappenIfObjectStillUsed()
        {
            var wasCollected = false;
            var foo = new ExpensiveClass(() => wasCollected = true);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.IsFalse(wasCollected, "This would obviously be tragic");
            Console.WriteLine(foo.GetType());
        }

        [Test]
        public void DetermineWhetherGCCanCollectEarlyWithScopeNoDelay()
        {
            var wasCollected = false;
            {
                var foo = new ExpensiveClass(() => wasCollected = true);
            }
            {
                Bar foo = new Bar(); // impairs readability
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Assert.IsFalse(wasCollected, "@Jeppe's point that reassigning foo has no impact on collection");
        }

        [Test]
        public void DetermineWhetherGCCanCollectIfInScope()
        {
            var wasCollected = false;
            var foo = new ExpensiveClass(() => wasCollected = true);
            GC.Collect();
            GC.WaitForPendingFinalizers();
#if !DEBUG
            Assert.IsTrue(wasCollected, "Thus proving @LasseKarlsens point, viz that collection can happen early irrespective of scope");
#endif
            var bar = new Bar();
        }

        [Test]
        public void DemonstrateGCCanCollectEarlyWithRefactoredAction()
        {
            var wasCollected = false;
            {
                Action action1 = () => { var foo = new ExpensiveClass(() => wasCollected = true); };
                action1();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.IsTrue(wasCollected);
        }
    }
}
