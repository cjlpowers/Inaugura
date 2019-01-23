#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

#endregion

namespace Inaugura.Threading
{
    public static class Helper
    {
        /// <summary>
        /// Invokes a delegate and blocks in a thread-safe mannor (supports multicast delegates). For objects which implement ISynchronizeInvoke, this method invokes the delegate on that objects own main thread. For all other objects this method performs a invoke. 
        /// </summary>
        /// <remarks>If the delegate is null, this method will return</remarks>
        /// <param name="handler">The delegate to invoke</param>
        /// <param name="invokeAsync">If true any object which supports ISynchronizeInvoke will be invoked asynchronously</param>
        /// <param name="args">The parameters</param>
        public static void ThreadSafeInvoke(Delegate handler, bool invokeAsync, params object[] args)
        {
            // return if the delegate is null
            if (handler == null)
                return;

            Delegate[] delegates = handler.GetInvocationList();
            foreach (Delegate eventTarget in delegates)
            {
                if (eventTarget != null)
                {
                    if (eventTarget.Target is ISynchronizeInvoke) // invoke using the targets own thread
                    {
                        ISynchronizeInvoke target = eventTarget.Target as ISynchronizeInvoke;
                        if (invokeAsync) // use begin invoke to run async
                            target.BeginInvoke(eventTarget, args);
                        else // block until the invoke call completes
                            target.Invoke(eventTarget, args);

                    }
                    else // invoke using this thread
                        eventTarget.DynamicInvoke(args);
                }
            }
        }
    }
}
