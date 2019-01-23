#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public class TCPMessagingNetwork : MessagingNetwork, IDisposable
	{
		#region Variables
		private Dictionary<Socket, TCPMessagePipe> mMessagePipes;
		private Net.TCPServer mServer;		// The TCP Server used to receive messages
		#endregion

		#region Properties
		public bool IsOpen
		{
			get
			{
				return this.mServer.Started;
			}
		}

		public int Port
		{
			get
			{
				return this.mServer.Port;
			}
			set
			{
				this.mServer.Port = value;
			}
		}
				


		#endregion

		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="port">The port the tcp server will listen for incomming requests</param>
		/// <param name="threads">The number of threads in the thread pool</param>
		public TCPMessagingNetwork(int port, int threads) : this(Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString(),port,threads)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="address">The address of the tcp server</param>
		/// <param name="port">The port the tcp server will listen for incomming requests</param>
		/// <param name="threads">The number of threads in the thread pool</param>
		public TCPMessagingNetwork(string address, int port, int threads) : base()
		{
			this.mMessagePipes = new Dictionary<Socket, TCPMessagePipe>();
			this.mServer = new Net.TCPServer(address, port, threads);
			this.mServer.ReceiveDataHandler = new Inaugura.Communication.Net.TCPServer.ReceiveDataCallback(this.ReceiveDataHandler);
			this.mServer.ConnectionCreated += new Inaugura.Communication.Net.TCPServer.ConnectionCreatedHandler(mServer_ConnectionCreated);
			this.mServer.ConnectionDestroyed += new Inaugura.Communication.Net.TCPServer.ConnectionDestroyedHandler(mServer_ConnectionDestroyed);
		}

		public override MessagePipe CreateMessagePipe(System.Net.EndPoint target)
		{
			return new TCPMessagePipe(target, this);
		}

		public override void Open()
		{
			this.mServer.Start();
		}

		public override void Close()
		{
			this.mServer.Stop();
		}

		private void ReceiveDataHandler(Socket socket)
		{
			TCPMessagePipe pipe = null;
			lock (this.mMessagePipes)
			{
				System.Diagnostics.Debug.Assert(this.mMessagePipes.ContainsKey(socket), "There is no message pipe associated with the socket");
				pipe = this.mMessagePipes[socket];
				pipe.ProcessReceiveData();				
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.mServer.Stop();

			// todo close all the open message pipes
			lock (this.mMessagePipes)
			{
				foreach (KeyValuePair<Socket, TCPMessagePipe> item in this.mMessagePipes)
				{
					item.Value.Dispose();
				}
			}
		}

		#endregion
		#endregion

		private void mServer_ConnectionCreated(Socket socket)
		{
			TCPMessagePipe messagePipe = new TCPMessagePipe(socket, this, new Inaugura.Security.RijndaelCrypto());
			lock (this.mMessagePipes)
			{
				this.mMessagePipes.Add(socket, messagePipe);
			}
		}

		private void mServer_ConnectionDestroyed(Socket socket)
		{
			lock (this.mMessagePipes)
			{
				TCPMessagePipe pipe = this.mMessagePipes[socket];
				this.mMessagePipes.Remove(socket);
				pipe.Dispose();
			}
		}

		#region Static Helper Methods

		internal static void TransmitMessage(XmlDocument message, Socket socket, int timeout)
		{
			using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
			{
				byte[] data = Inaugura.Helper.StringToBytes(message.OuterXml);
				memoryStream.Write(data, 0, data.Length);
				TCPMessagingNetwork.TransmitData(memoryStream, socket, timeout);
				/*
				using (System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter(memoryStream))
				{
					binaryWriter.Write(message.OuterXml);
					binaryWriter.Flush();
					
				}
				*/
			}
		}

		internal static XmlDocument ReceiveMessage(Socket socket, int timeout)
		{
			using (System.IO.MemoryStream memoryStream = TCPMessagingNetwork.ReceiveData(socket, timeout))
			{
				string xml = Inaugura.Helper.BytesToString(memoryStream.ToArray());
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(xml);
				return xmlDoc;
			}
		}


		private static void TransmitData(System.IO.MemoryStream stream, Socket socket, int timeout)
		{
			lock (socket)
			{
				socket.SendTimeout = timeout;

				uint size = (uint)stream.Length;
				byte[] sizeBuffer = new byte[4];

				sizeBuffer[3] = (byte)size;
				size = size >> 8;
				sizeBuffer[2] = (byte)size;
				size = size >> 8;
				sizeBuffer[1] = (byte)size;
				size = size >> 8;
				sizeBuffer[0] = (byte)size;

				// Send the header containing the transmission size
				socket.Send(sizeBuffer);

				// send the transmission data
				socket.Send(stream.ToArray());
			}
		}

		private static System.IO.MemoryStream ReceiveData(Socket socket, int timeout)
		{
			socket.ReceiveTimeout = timeout;

			byte[] sizeBuffer = new byte[4];

			// read the transmission size header
			socket.Receive(sizeBuffer, 0, 4, SocketFlags.None);

			uint byte0 = (uint)sizeBuffer[3];
			uint byte1 = (uint)(sizeBuffer[2] << 8);
			uint byte2 = (uint)(sizeBuffer[1] << 16);
			uint byte3 = (uint)(sizeBuffer[0] << 24);

			uint bytesToRead = byte0 + byte1 + byte2 + byte3;


			System.IO.MemoryStream mem = new System.IO.MemoryStream();
			byte[] myReadBuffer = new byte[8192];
			int numberOfBytesRead = 0;
			uint totalBytesRead = 0;

			// Incoming message may be larger than the buffer size.
			do
			{
				numberOfBytesRead = socket.Receive(myReadBuffer, 0, myReadBuffer.Length, SocketFlags.None);
				mem.Write(myReadBuffer, 0, numberOfBytesRead);
				totalBytesRead += (uint)numberOfBytesRead;
			}
			while (totalBytesRead < bytesToRead);

			return mem;
		}
		#endregion
	}
}
