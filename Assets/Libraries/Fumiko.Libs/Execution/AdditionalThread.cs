using System;
using System.Threading;

namespace Fumiko.Systems.Threading
{
    public class AdditionalThread
    {
        public int hash;
        public string identifier;
        public Thread myThread;
        public int priority = 0;
        public Func<bool> threadFunction;
        public int throwAwayTime = -1;

        public AdditionalThread(Func<bool> ThreadFunction, string Identifier)
        {
            threadFunction = ThreadFunction;
            identifier = Identifier;
            hash = Identifier.GetHashCode();
        }

        public void Run()
        {
            try
            {
                threadFunction();
            }
            catch (Exception e)
            {
                ThreadSystem.instance.ThreadFinished(this, false, e);
                return;
            }

            ThreadSystem.instance.ThreadFinished(this);
        }
    }
}