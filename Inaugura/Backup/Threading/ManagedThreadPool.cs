// Stephen Toub
// stoub@microsoft.com
//
// Very simple threadpool in C#.
// 4/27/04

#region Namespaces
using System;
using System.Threading;
using System.Collections;
#endregion

namespace Inaugura.Threading
{
    public delegate void ThreadPoolExceptionHandler(Exception ex, object[] parameters);


	/// <summary>Implementation of Dijkstra's PV Semaphore based on the Monitor class.</summary>
	public class Semaphore
	{
		#region Member Variables
		/// <summary>The number of units alloted by this semaphore.</summary>
		private int _count;
		/// <summary>Lock for the semaphore.</summary>
		private object _semLock = new object();
		#endregion

		#region Construction
		/// <summary> Initialize the semaphore as a binary semaphore.</summary>
		public Semaphore() : this(1) 
		{
		}

		/// <summary> Initialize the semaphore as a counting semaphore.</summary>
		/// <param name="count">Initial number of threads that can take out units from this semaphore.</param>
		/// <exception cref="ArgumentException">Throws if the count argument is less than 0.</exception>
		public Semaphore(int count) 
		{
			if (count < 0) throw new ArgumentException("Semaphore must have a count of at least 0.", "count");
			_count = count;
		}
		#endregion

		#region Synchronization Operations
		/// <summary>V the semaphore (add 1 unit to it).</summary>
		public void AddOne() { V(); }

		/// <summary>P the semaphore (take out 1 unit from it).</summary>
		public void WaitOne() { P(); }

		/// <summary>P the semaphore (take out 1 unit from it).</summary>
		public void P() 
		{
			// Lock so we can work in peace.  This works because lock is actually
			// built around Monitor.
			lock(_semLock) 
			{
				// Wait until a unit becomes available.  We need to wait
				// in a loop in case someone else wakes up before us.  This could
				// happen if the Monitor.Pulse statements were changed to Monitor.PulseAll
				// statements in order to introduce some randomness into the order
				// in which threads are woken.
				while(_count <= 0) Monitor.Wait(_semLock, Timeout.Infinite);
				_count--;
			}
		}

		/// <summary>V the semaphore (add 1 unit to it).</summary>
		public void V() 
		{
			// Lock so we can work in peace.  This works because lock is actually
			// built around Monitor.
			lock(_semLock) 
			{
				// Release our hold on the unit of control.  Then tell everyone
				// waiting on this object that there is a unit available.
				_count++;
				Monitor.Pulse(_semLock);
			}
		}

		/// <summary>Resets the semaphore to the specified count.  Should be used cautiously.</summary>
		public void Reset(int count)
		{
			lock(_semLock) { _count = count; }
		}
		#endregion
	}

	/// <summary>Managed thread pool.</summary>
	public class ManagedThreadPool
	{
		
		#region Member Variables
		/// <summary>Queue of all the callbacks waiting to be executed.</summary>
		private Queue _waitingCallbacks;
		/// <summary>
		/// Used to signal that a worker thread is needed for processing.  Note that multiple
		/// threads may be needed simultaneously and as such we use a semaphore instead of
		/// an auto reset event.
		/// </summary>
		private Semaphore _workerThreadNeeded;
		/// <summary>List of all worker threads at the disposal of the thread pool.</summary>
		private ArrayList _workerThreads;
		/// <summary>Number of threads currently active.</summary>
		private int _inUseThreads;
		/// <summary>Lockable object for the pool.</summary>
		private object _poolLock = new object();
		private int _maxWorkerThreads = 25;
		#endregion

		#region Construction and Finalization
		/// <summary>Initialize the thread pool.</summary>
		public ManagedThreadPool(int numberOfThreads) 
		{
			this._maxWorkerThreads = numberOfThreads;
			Initialize(); 
		}

		/// <summary>Initializes the thread pool.</summary>
		private void Initialize()
		{
			// Create our thread stores; we handle synchronization ourself
			// as we may run into situtations where multiple operations need to be atomic.
			// We keep track of the threads we've created just for good measure; not actually
			// needed for any core functionality.
			_waitingCallbacks = new Queue();
			_workerThreads = new ArrayList();
			_inUseThreads = 0;

			// Create our "thread needed" event
			_workerThreadNeeded = new Semaphore(0);
			
			// Create all of the worker threads
			for(int i=0; i<_maxWorkerThreads; i++)
			{
				// Create a new thread and add it to the list of threads.
				Thread newThread = new Thread(new ThreadStart(ProcessQueuedItems));
				_workerThreads.Add(newThread);

				// Configure the new thread and start it
				newThread.Name = "ManagedPoolThread #" + i.ToString();
				newThread.IsBackground = true;
				newThread.Start();
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Queues a work item to the thread pool.
		/// </summary>
		/// <param name="workItem">A delegate to be invoked by the thread pool.</param>
		/// <param name="list">The list of parameters</param>
		public void QueueWorkItem(Delegate workItem, params object[] list)
		{
			this.QueueErrorHandledWorkItem(workItem, list, null, null);
		}

		/// <summary>Queues a user work item to the thread pool.</summary>
        /// <param name="workItem">
        /// A delegate to be invoked by the thread pool.
        /// </param>
        /// <param name="para">
        /// The object that is passed to the delegate when serviced from the thread pool.
        /// </param>
        /// <param name="exceptionCallback">Callback delegate for any exception that might occur</param>
        /// <param name="exceptionPara">Parameters with which to call the exception callback delegate.</param>
		public void QueueErrorHandledWorkItem(Delegate workItem, object[] para, ThreadPoolExceptionHandler exceptionCallback, object[] exceptionPara)
		{
			// make sure that the number of parameters matches the delegate
			if (workItem.Method.GetParameters().Length != para.Length)
				throw new ArgumentException("The number of parameters does not match that reqired by the work item delegate");

			// Create a waiting callback that contains the delegate and its state.
            // At it to the processing queue, and signal that data is waiting.
            WaitingCallback waiting = new WaitingCallback(workItem, para, exceptionCallback, exceptionPara);
            lock (_poolLock) { _waitingCallbacks.Enqueue(waiting); }
            _workerThreadNeeded.AddOne();
        }
        
        /// <summary>
        /// Queues a user work item to the thread pool.
        /// </summary>
        /// A WaitCallback representing the delegate to invoke when the thread in the 
        /// thread pool picks up the work item.
        /// </param>
        /// <param name="para">
        /// The object that is passed to the delegate when serviced from the thread pool.
        /// </param>
        /// <param name="exceptionCallback">Callback delegate for any exception that might occur</param>
		public void QueueErrorHandledWorkItem(Delegate callback, object[] para, ThreadPoolExceptionHandler exceptionCallback)
		{
            this.QueueErrorHandledWorkItem(callback, para, exceptionCallback, para);
        }
                

		/// <summary>Empties the work queue of any queued work items.  Resets all threads in the pool.</summary>
		public void Reset()
		{
			lock(_poolLock) 
			{ 
				// Cleanup any waiting callbacks
				try 
				{
					// Try to dispose of all remaining state
					foreach(object obj in _waitingCallbacks)
					{
						WaitingCallback callback = (WaitingCallback)obj;
						foreach (object para in callback.Params)
						{
							if (para is IDisposable) ((IDisposable)para).Dispose();
						}
					}
				} 
				catch{}

				// Shutdown all existing threads
				try 
				{
					foreach(Thread thread in _workerThreads) 
					{
						if (thread != null) thread.Abort("reset");
					}
				}
				catch{}

				// Reinitialize the pool (create new threads, etc.)
				Initialize();
			}
		}
		#endregion

		#region Properties
		/// <summary>Gets the number of threads at the disposal of the thread pool.</summary>
		public int MaxThreads { get { return _maxWorkerThreads; } }
		/// <summary>Gets the number of currently active threads in the thread pool.</summary>
		public int ActiveThreads { get { return _inUseThreads; } }
		/// <summary>Gets the number of delegates currently awaiting processing by the thread pool.</summary>
		public int QueuedCount{ get { lock(_poolLock) { return _waitingCallbacks.Count; } } }

		/// <summary>
		/// The processing state of the thread pool
		/// </summary>
		/// <value>Returns true if the thread pool has no queued work items and no active threads, otherwise false</value>
		public bool Idle
		{
			get
			{
				return this.QueuedCount == 0 && this.ActiveThreads == 0;
			}
		}
		#endregion

		#region Thread Processing
		/// <summary>Event raised when there is an exception on a threadpool thread.</summary>
		public event UnhandledExceptionEventHandler UnhandledException;

		/// <summary>A thread worker function that processes items from the work queue.</summary>
		private void ProcessQueuedItems()
		{
			// Process indefinitely
			while(true)
			{
				_workerThreadNeeded.WaitOne();

				// Get the next item in the queue.  If there is nothing there, go to sleep
				// for a while until we're woken up when a callback is waiting.
				WaitingCallback callback = null;

				// Try to get the next callback available.  We need to lock on the 
				// queue in order to make our count check and retrieval atomic.
				lock(_poolLock)
				{
					if (_waitingCallbacks.Count > 0)
					{
						try { callback = (WaitingCallback)_waitingCallbacks.Dequeue(); } 
						catch{} // make sure not to fail here
					}
				}

				if (callback != null)
				{
					// We now have a callback.  Execute it.  Make sure to accurately
					// record how many callbacks are currently executing.
					try 
					{
						Interlocked.Increment(ref _inUseThreads);
						callback.Callback.DynamicInvoke(callback.Params);
					} 
					catch(Exception exc)
					{
						try
						{
                            if (callback.ExceptionCallback != null)
                            {
                                callback.ExceptionCallback(exc,callback.ExceptionParams);
                            }
                            else
                            {
                                UnhandledExceptionEventHandler handler = UnhandledException;
                                if (handler != null)
                                    handler(typeof(ManagedThreadPool), new UnhandledExceptionEventArgs(exc, false));
                            }
                        }
						catch{}
					}
					finally
					{
						Interlocked.Decrement(ref _inUseThreads);
					}
				}
			}
		}
		#endregion

		/// <summary>Used to hold a callback delegate and the state for that delegate.</summary>
        private class WaitingCallback
        {
            #region Member Variables
            /// <summary>Callback delegate for the callback.</summary>
            private Delegate mCallback;
            /// <summary>Parameters with which to call the callback delegate.</summary>
            private object[] mParams;
            /// <summary>Exception delgate called for exceptions that occur when processing the async operation</summary>
            private ThreadPoolExceptionHandler mExceptionCallback;
            /// <summary>Parameters with which to call the exception callback delegate.</summary>
            private object[] mExceptionParams;
            #endregion

            #region Construction
            /// <summary>Initialize the callback holding object.</summary>
            /// <param name="callback">Callback delegate for the callback.</param>
            /// <param name="para">Parameters with which to call the callback delegate.</param>
            /// <param name="exceptionCallback">The exception handler</param>
            public WaitingCallback(Delegate callback, object[] para, ThreadPoolExceptionHandler exceptionCallback) : this(callback,para,exceptionCallback, para)
            {
                
            }

            /// <summary>
            /// Initialize the callback holding object.
            /// </summary>
            /// <param name="callback">Callback delegate for the callback.</param>
            /// <param name="para">Parameters with which to call the callback delegate.</param>
            /// <param name="exceptionCallback">Callback delegate for any exception that might occur</param>
            /// <param name="exceptionPara">Parameters with which to call the exception callback delegate.</param>
            public WaitingCallback(Delegate callback, object[] para, ThreadPoolExceptionHandler exceptionCallback, object[] exceptionPara)
            {
                this.mCallback = callback;
                this.mParams = para;
                this.mExceptionCallback = exceptionCallback;
                this.mExceptionParams = exceptionPara;
            }
            #endregion

            #region Properties
            /// <summary>Gets the callback delegate for the callback.</summary>
            public Delegate Callback { get { return this.mCallback; } }
            /// <summary>Gets the callback delegate for the callback.</summary>
            public ThreadPoolExceptionHandler ExceptionCallback { get { return this.mExceptionCallback; } }
            /// <summary>Gets the state with which to call the callback delegate.</summary>
            public object[] Params { get { return this.mParams; } }
            /// <summary>Gets the state with which to call the exception callback delegate.</summary>
            public object[] ExceptionParams { get { return this.mExceptionParams; } }
            #endregion
        }
    }
}
