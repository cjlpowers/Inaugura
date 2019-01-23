#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Xml;

#endregion

namespace Inaugura.Communication.Messaging
{
	public class TCPMessagePipe : MessagePipe
	{
		#region Variables
		private Socket mSocket;					// The socket used by this pipe
		private int mSendTimeout = 50000;		// Timeout for transmitting data over tcp
		private int mReceiveTimeout = 50000;	// Timeout for receiving data over tcp
		#endregion

		#region Properties	
		public int SendTimeout
		{
			get
			{
				return this.mSendTimeout;
			}
			set
			{
				this.mSendTimeout = value;
			}
		}

		public int ReceiveTimeout
		{
			get
			{
				return this.mReceiveTimeout;
			}
			set
			{
				this.mReceiveTimeout = value;
			}
		}

		private Socket Socket
		{
			get
			{
				return this.mSocket;
			}
			set
			{
				this.mSocket = value;
			}
		}
		#endregion

		#region Methods
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>This constructor will atttempt to create a message pipe by connecting to the target endpoint</remarks>
		/// <param name="target">The remote endpoint of the message pipe</param>
		/// <param name="enableEncryption">True to attempt to nagotiate encryption over the message pipe</param>
		internal TCPMessagePipe(System.Net.EndPoint target, TCPMessagingNetwork network) : base(network)
		{
			this.WriteDebug("Connecting to: " + target.ToString());
			this.Socket = new Socket(target.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			this.Socket.Connect(target);

			this.WriteDebug(string.Format("Socket ({0}) Connected",this.Socket.Handle.ToString()));

			this.EstablishSecurePipe();			
		}		
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="socket">The socket the pipe will transfer data over</param>
		/// <param name="network">The messaging network that contains the pipe</param>
		internal TCPMessagePipe(Socket socket, TCPMessagingNetwork network, Inaugura.Security.RijndaelCrypto privateKey) : base(network,privateKey)
		{			
			this.mSocket = socket;
			this.WriteDebug(string.Format("Socket ({0}) Received", this.Socket.Handle.ToString()));
		}

		/// <summary>
		/// Sends a message over the message pipe and receives a reply
		/// </summary>
		/// <param name="message">The message to send</param>
		/// <param name="useCompression">True to compress large messages. Will only compress messages where there is a benifical size reduction</param>
		/// <param name="useEncryption">True to encrypt the outgoing message</param>
		/// <returns>The reply message</returns>
		public override XmlMessage SendMessage(XmlMessage message)
		{
			if(this.SymmetricCrypto != null)
				message = new EncryptedCompressedMessage(message, this.SymmetricCrypto);

			// Send the message over the tcp socket
			TCPMessagingNetwork.TransmitMessage(message, this.mSocket, this.SendTimeout);

			// receive the response from the tcp socket
			XmlDocument xmlDoc = TCPMessagingNetwork.ReceiveMessage(this.mSocket, this.ReceiveTimeout);

			XmlMessage response = XmlMessage.FromXml(xmlDoc);
			if (response is EncryptedCompressedMessage)
			{
				EncryptedCompressedMessage res = response as EncryptedCompressedMessage;
				response = res.DecryptMessage(this.SymmetricCrypto);
			}
			return response;
		}		

		internal void ProcessReceiveData()
		{
			XmlDocument request = TCPMessagingNetwork.ReceiveMessage(this.Socket, this.ReceiveTimeout);
			XmlMessage requestMessage = XmlMessage.FromXml(request);
			XmlMessage responseMessage = this.ProcessRequest(requestMessage);
			TCPMessagingNetwork.TransmitMessage(responseMessage, this.Socket, this.SendTimeout);
		}	

		#region IDisposable Members
		public override void Dispose()
		{
			this.WriteDebug(string.Format("Socket ({0}) Shutting Down", this.Socket.Handle.ToString()));
			// shutdown and close the socket
			 if(this.mSocket.Connected)
			    this.mSocket.Shutdown(SocketShutdown.Both);
			
			this.mSocket.Close();
			this.WriteDebug("Socket Closed");

			base.Dispose();
		}
		#endregion
		#endregion
	}
}
