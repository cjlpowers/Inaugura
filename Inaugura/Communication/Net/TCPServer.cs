using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using Inaugura.Threading;


namespace Inaugura.Communication.Net
{
	public delegate void ExceptionThrownEventHandler(Exception ex);

	/// <summary>
	/// A thread-pooled TCP server
	/// </summary>
	public class TCPServer
	{
		#region Delegates/Events
        public delegate bool AuthenticationCallback(Socket socket);
        public delegate void ReceiveDataCallback(Socket socket);
		public delegate void ConnectionCreatedHandler(Socket socket);
		public delegate void ConnectionDestroyedHandler(Socket socket);
        public event ExceptionThrownEventHandler Exception;
		public event ConnectionCreatedHandler ConnectionCreated;						// A event fired upon an incomming socket connection being established
		public event ConnectionDestroyedHandler ConnectionDestroyed;					// A event fired upon an established connection being destroyed
		#endregion

		#region Member Variables
		private int mMaxConnections = 128;												// The maximum number of sockets this server will handle at any one time
		private int mConnectionQueueSize = 32;											// The maximum number of incomming connections this server will queue
		private bool mStarted = false;													// The started flag
		private List<Socket> mConnectedSockets = new List<Socket>();					// The list of connected sockets
		private List<Socket> mActiveSockets = new List<Socket>();						// The list of connected sockets currently transfering data
		private Thread mProcessingThread;												// The thread which handles all server processing
		private ManagedThreadPool mThreadPool;											// The thread pool which processes incomming connections
		private IPAddress mIPAddress;													// The ip address which the server listens for incomming connections
		private int mPort = 0;															// The port which the server listens for incomming connections
        private ReceiveDataCallback mReceiveDataHandler = null;							// A pointer to the user defined method which will be called by the thread pool to handle incomming socket data
        private AuthenticationCallback mAuthenticationHandler = null;					// A pointer to the user defined method which will authenticate connection requests
		#endregion

		

		#region Properties
		/// <summary>
		/// Callback method which handles the receiving of data on a socket
		/// </summary>
		/// <value></value>
		public ReceiveDataCallback ReceiveDataHandler
		{
			get
			{
				return this.mReceiveDataHandler;
			}
			set
			{
				this.mReceiveDataHandler = value;
			}
		}

		/// <summary>
		/// Callback method which controls authentication of incomming socket connections
		/// </summary>
		/// <value></value>
		public AuthenticationCallback AuthenticationHandler
		{
			get
			{
				return this.mAuthenticationHandler;
			}
			set
			{
				this.mAuthenticationHandler = value;
			}
		}
		
		/// <summary>
        /// The connection queue size
        /// </summary>
        /// <value>Specifies the number of incomming connections which can be queued</value>
        protected int ConnectionQueueSize
        {
            get
            {                
                return mConnectionQueueSize;
            }
            set
            {
                if (this.Started)
                    throw new InvalidOperationException("The connection queue size can not be set when the server is in the started state");

                this.mConnectionQueueSize = value;                
            }
        }

        /// <summary>
        /// The status of the server
        /// </summary>
        /// <value>Returns true if it has active requests, false otherwise </value>
		public bool Idle
		{
			get
			{
				return this.mThreadPool.Idle;
			}
		}

        /// <summary>
        /// The port the server listens for incomming connections
        /// </summary>
        /// <value></value>
		public int Port
		{
			get
			{
				return this.mPort;
			}
			set
			{
                if (this.Started)
                    throw new InvalidOperationException("The port can not be set while the server is in the started state");
                
                this.mPort = value;
			}
		}

        /// <summary>
        /// Started flag
        /// </summary>
        /// <value>True if started, false otherwise</value>
		public bool Started
		{
			get
			{
				return this.mStarted;
			}
			private set
			{
				this.mStarted = value;
			}
		}

        /// <summary>
        /// List of the connected sockets
        /// </summary>
        /// <value></value>
		protected Socket[] ConnectedSockets
		{
			get
			{
				return this.mConnectedSockets.ToArray();
			}
		}

        /// <summary>
        /// The ip address of the server
        /// </summary>
        /// <value></value>
		public IPAddress IPAddress
		{
			get
			{
				return this.mIPAddress;
			}
			private set
			{
                if (this.Started)
                    throw new InvalidOperationException("The ip address can not be set when the server is in the started state");
                
                this.mIPAddress = value;
			}
		}      

        protected int MaxConnections
        {
            get
            {
                return this.mMaxConnections;
            }
            set
            {
                if (this.Started)
                    throw new InvalidOperationException("The maximum number of connections can not be set when the server is in the started state");

                this.mMaxConnections = value;
            }
        }

		/// <summary>
		/// The number of sockets currently connected to the server
		/// </summary>
		/// <value></value>
		public int Connections
		{
			get
			{
				return this.mConnectedSockets.Count + this.mActiveSockets.Count;
			}
		}
		#endregion

		#region Methods
        /// <summary>
        /// Consctructor
        /// </summary>
        /// <param name="port">The server port</param>
        /// <param name="threads">The number of threads which will handle incomming data</param>
		public TCPServer(int port, int threads) : this(Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString(), port, threads)
		{
        }        

        /// <summary>
        /// Consctructor
        /// </summary>
        /// <param name="address">The server address</param>
        /// <param name="port">The server port</param>
        /// <param name="threads">The number of threads which will handle incomming data</param>       
        public TCPServer(string address, int port, int threads)
        {           
            this.IPAddress = IPAddress.Parse(address);
			this.Port = port;			
			this.mThreadPool = new ManagedThreadPool(threads);
		}

		#region Event Callers
		/*
		private void CallStartingEvent()
		{
			if(this.ServerStarting != null)
				this.ServerStarting(this);
		}
		
		private void CallStartedEvent()
		{
			if (this.ServerStarted != null)
				this.ServerStarted(this);
		}

		private void CallStoppingEvent()
		{
			if (this.ServerStopping != null)
				this.ServerStopping(this);
		}
		private void CallStoppedEvent()
		{
			if (this.ServerStopped != null)
				this.ServerStopped(this);
		}

		private void CallConnectionRequestedEvent(Socket socket)
		{
			if (this.ConnectionRequested != null)
				this.ConnectionRequested(socket);
		}

		private void CallConnectionEstablishedEvent(Socket socket)
		{
			if (this.ConnectionEstablished != null)
				this.ConnectionEstablished(socket);
		}

		private void CallConnectionTerminatedEvent(Socket socket)
		{
			if (this.ConnectionTerminated != null)
				this.ConnectionTerminated(socket);
		}

		private void CallSocketDataAvaliableEvent(Socket socket)
		{
			if (this.SocketDataAvaliable != null)
				this.SocketDataAvaliable(socket);
		}

		private void CallBeginReceivingDataEvent(Socket socket)
		{
			if (this.BeginReceivingData != null)
				this.BeginReceivingData(socket);
		}
		
		private void CallEndReceivingDataEvent(Socket socket)
		{
			if (this.EndReceivingData != null)
				this.EndReceivingData(socket);
		}

		private void CallBeginSendingDataEvent(Socket socket)
		{
			if (this.BeginSendingData != null)
				this.BeginSendingData(socket);
		}
		
		private void CallEndSendingDataEvent(Socket socket)
		{
			if (this.EndSendingData != null)
				this.EndSendingData(socket);
		}
		*/
		#endregion

		#region Starting and Stopping
		/// <summary>
		/// Starts the server
		/// </summary>
		public virtual void Start()
		{
            lock (this)
            {
                if (this.Started)
                    return;

                this.mStarted = true;
                this.mProcessingThread = new Thread(new ThreadStart(this.ProcessServer));
				this.mProcessingThread.Name = "TCPServer Processing Thread";
				this.mProcessingThread.Start();
            }
        }

		/// <summary>
		/// Stops the server and closes any open sockets
		/// </summary>
		public virtual void Stop()
		{
            lock (this)
            {
                if (!this.Started)
                    return;
             
                this.mStarted = false;             
                while (!this.mThreadPool.Idle || this.mProcessingThread.ThreadState != System.Threading.ThreadState.Stopped)
                {
                    Thread.Sleep(10);
                }
            }
        }

		#endregion

		#region Processsing

        /// <summary>
        /// The main processing method of the server
        /// </summary>
        /// <remarks>Tests for new connections as well as activity on the connected sockets</remarks>
        private void ProcessServer()
        {
            // create the list of sockets which listen for incomming connections
            System.Collections.Generic.List<Socket> listenSockets = new System.Collections.Generic.List<Socket>();

            // Create the listening socket(s)
            Debug.WriteLine("Creating listening socket");           
            Socket listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSock.Bind(new IPEndPoint(this.IPAddress, this.Port));
            listenSock.Listen(this.ConnectionQueueSize);
            listenSockets.Add(listenSock);
            Debug.WriteLine("Created Listening Socket " + listenSock.GetHashCode());
            Debug.WriteLine("Listening for connections on " + listenSock.LocalEndPoint.ToString());

            // create the lists used in checking the state of the connected sockets
            List<Socket> readSockets = new List<Socket>();
            List<Socket> errorSockets = new List<Socket>();
            List<Socket> tempSocketList = new List<Socket>();
			List<Socket> removedSockets = new List<Socket>();
			bool incommingConnectionProcessed = false;
            bool connectedSocketProcessed = false;
            bool sleptLastIteration = true;

            #region Propessing loop
            while (this.Started)
            {
                try
                {
                    #region Process Incomming Connections
                    if (true)//(incommingConnectionProcessed || sleptLastIteration) // only process if there is bound to be NEW activity
                    {
						if (tempSocketList.Count == 0) // only look at new connections if all pending connection requests have been processed
						{
							//tempSocketList.Clear();						
							tempSocketList.AddRange(listenSockets);

							// Only the sockets that contain a connection request
							// will remain in listenList after Select returns.
							if (tempSocketList.Count > 0)
								Socket.Select(tempSocketList, null, null, 1000);
						}
						else
						{
							// old connection requests exist... probably because the thread pool cant handle the load, therefore wait a few milliseconds until some connections complete
							Thread.Sleep(50);
						}

						if (tempSocketList.Count > 0) // there are connection requests
                        {
                            incommingConnectionProcessed = true; // skip the sleep phase because we had a connection this time around

							foreach (Socket s in tempSocketList)
							{
								Debug.WriteLine("Incomming Connection Request on socket " + s.GetHashCode());

                                lock (this.mConnectedSockets)
                                {								
                                    if (this.Connections < this.MaxConnections && (this.AuthenticationHandler != null && this.AuthenticationHandler(s) || this.AuthenticationHandler == null))
                                    {
										Socket connectSock = s.Accept();
										removedSockets.Add(s);
										this.mConnectedSockets.Add(connectSock);

										Debug.WriteLine("\tClient Connected on Socket " + connectSock.GetHashCode());

										// call the connection created event
										if (this.ConnectionCreated != null)
											Inaugura.Threading.Helper.ThreadSafeInvoke(this.ConnectionCreated, true, new object[] { connectSock });																		
                                    }                                   								
                                }
							}

							// remove any sockets who's connection request was processed
							foreach (Socket s in removedSockets)
								tempSocketList.Remove(s);
							removedSockets.Clear();
						}
                        else // no new connections
                            incommingConnectionProcessed = false;

						#region Old Code
						/*
						 tempSocketList.Clear();
                        tempSocketList.AddRange(listenSockets);

                        // Only the sockets that contain a connection request
                        // will remain in listenList after Select returns.
                        if (tempSocketList.Count > 0)
                            Socket.Select(tempSocketList, null, null, 1000);

                        if (tempSocketList.Count > 0) // there was a new connection
                        {
                            incommingConnectionProcessed = true; // skip the sleep phase because we had a connection this time around

							foreach (Socket s in tempSocketList)
							{
								Debug.WriteLine("Incomming Connection Request on socket " + s.GetHashCode());

                                lock (this.mConnectedSockets)
                                {								
                                    if (this.Connections < this.MaxConnections && (this.AuthenticationHandler != null && this.AuthenticationHandler(s) || this.AuthenticationHandler == null))
                                    {
										Socket connectSock = s.Accept();
                                        this.mConnectedSockets.Add(connectSock);

										Debug.WriteLine("\tClient Connected on Socket " + connectSock.GetHashCode());

										// call the connection created event
										if (this.ConnectionCreated != null)
											Inaugura.Threading.Helper.ThreadSafeInvoke(this.ConnectionCreated, true, new object[] { connectSock });																		
                                    }
                                    else // 'hang up' on the socket
                                    {									                                       
										Socket connectSock = s.Accept();
                                        connectSock.Shutdown(SocketShutdown.Both);
                                        connectSock.Disconnect(false);
                                        connectSock.Close();
                                        Debug.WriteLine("Connection Request Denied");
                                    }								
                                }
							}

						}
                        else // no new connections
                            incommingConnectionProcessed = false;  
						*/
						#endregion
					}                  
                    #endregion

                    #region Process the connected sockets
					if (this.mConnectedSockets.Count > 0)//(connectedSocketProcessed || sleptLastIteration) // only process if there is bound to be NEW activity
					{
                        lock (this.mConnectedSockets)
                        {
                            readSockets.Clear();
                            readSockets.AddRange(this.mConnectedSockets);
                            errorSockets.Clear();
                            errorSockets.AddRange(this.mConnectedSockets);
                        }

                        // select the sockets
                        if (readSockets.Count > 0 || errorSockets.Count > 0)
                            Socket.Select(readSockets, null, errorSockets, 1000);

                        if (readSockets.Count > 0 || errorSockets.Count > 0)
                        {
                            connectedSocketProcessed = true;
                            #region Process the read sockets
                            foreach (Socket s in readSockets)
                            {
                                if (!s.Connected) // someting is wrong with the socket
                                {
                                    Debug.WriteLine("Destroying Socket " + s.GetHashCode());
                                    this.TerminateSocket(s);
                                }
                                else // read the data
                                {
                                    try
                                    {
                                        if (s.Available == 0) // disconnected at client end
                                        {
                                            Debug.WriteLine("Client disconnected " + s.GetHashCode());
                                            this.TerminateSocket(s);                                            
                                        }
                                        else // processing the incomming data
                                        {
                                            // remove the socket from the connected sockets list until we are done processing the data
                                            lock (this.mConnectedSockets)
                                            {
                                                this.mConnectedSockets.Remove(s);
												this.mActiveSockets.Add(s);
											}

											// use the thread pool to handle the data receiving
											ReceiveDataCallback handler = new ReceiveDataCallback(this.ReceiveData);
											this.mThreadPool.QueueWorkItem(handler, s);
										}
                                    }
                                    catch (System.Net.Sockets.SocketException ex)
                                    {
                                        Debug.WriteLine("Exception on socket " + s.GetHashCode());
                                        Debug.Write(ex);
                                        Debug.WriteLine("Error Code: " + ex.ErrorCode);
                                        // remove the socket with the error										
                                        this.TerminateSocket(s);
                                    }
                                }
                            }
                            #endregion

                            #region Process the error sockets
                            foreach (Socket s in errorSockets)
                            {
                               Debug.WriteLine("Connection " + s.GetHashCode() + " has error");
                               Debug.WriteLine("Closing Socket " + s.GetHashCode());
                               TerminateSocket(s);
                            }
                            #endregion                   
                        }                   
                        else
                            connectedSocketProcessed = false;
                    }
                    #endregion                                 
                }
                catch (Exception ex)
                {
                    if (this.Exception != null)
                        this.Exception(this, new ExceptionThrownEventArgs(ex));
                    Debug.WriteLine(String.Format("Sockets Count {0}, read count {1}, error count {2}", this.mConnectedSockets.Count, readSockets.Count, errorSockets.Count));
                    Debug.Write(ex);
                }

                // Sleep abit before the next iteration
                if (!incommingConnectionProcessed && !connectedSocketProcessed)
                {
                    sleptLastIteration = true;
                    Thread.Sleep(100);
                }
                else
                    sleptLastIteration = false;
            }
            #endregion

            // close all the listening sockets
            foreach (Socket s in listenSockets)
			{
				//if (s.Connected)
				//{
                    Debug.WriteLine("Closing Listening Socket " + s.GetHashCode());
					//s.Shutdown(SocketShutdown.Both);
					s.Close();
				//}
			}
        }
		
		#endregion

		#region Socket Management

        /// <summary>
        /// Terminates a socket and removes it from the Connected Socket List
        /// </summary>
        /// <param name="socket"></param>
		protected void TerminateSocket(Socket socket)
		{
			lock (socket)
			{
				if (this.mConnectedSockets.Contains(socket))
				{
                    if(socket.Connected)
					    socket.Shutdown(SocketShutdown.Both);
					socket.Close();
					lock (this.mConnectedSockets)
					{
						this.mConnectedSockets.Remove(socket);
					}

					// call the connection destroyed event
					if (this.ConnectionDestroyed != null)
						Inaugura.Threading.Helper.ThreadSafeInvoke(this.ConnectionDestroyed, true, new object[] { socket });

					Debug.WriteLine("Socket: " + socket.GetHashCode() + " terminated");
				}
			}
		}

		#region Data Transmition
		/// <summary>
		/// Receives the incomming data for a socket
		/// </summary>
		/// <param name="socket">The socket which has data to receive</param>
		/// <returns>The received socket data</returns>
		protected virtual void ReceiveData(Socket socket)
		{
            try
            {
                // call the receive data handler
				if (this.ReceiveDataHandler != null)
				{
					this.ReceiveDataHandler(socket);
				}				
            }
            catch (Exception ex)
            {
                if (this.Exception != null)
                    this.Exception(this, new ExceptionThrownEventArgs(ex));
            }
            finally
            {
				lock (this.mConnectedSockets)
                {
					// remove the socket from the list of active sockets
					this.mActiveSockets.Remove(socket);
					
					// add the socket back to the connected socket list now that we are done
                    // receiving data from it
                    this.mConnectedSockets.Add(socket);
                }
            }
        }
		#endregion
		#endregion
		#endregion
	}
}
