#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

#endregion

namespace Inaugura.Communication.Messaging
{
	/// <summary>
	/// Base Message Hub Class
	/// </summary>
	public abstract class MessagingNetwork
	{
		#region Variables
		private List<IMessageProcessor> mMessageProcessors;		// The list of message processors which handle messages
		private Inaugura.Security.RSACrypto mRSACrypto;			// The asymetric RSA helper object		
		#endregion

		#region Properties
		/// <summary>
		/// List of registered Message Processors
		/// </summary>
		/// <value></value>
		public IMessageProcessor[] MessageProcessors
		{
			get
			{
				lock (this.mMessageProcessors)
				{
					return this.mMessageProcessors.ToArray();
				}
			}
		}

		/// <summary>
		/// The public key encryption object used to initiate secure Message Pipe connections
		/// </summary>
		/// <value></value>
		public Inaugura.Security.RSACrypto AsymmetricCrypto
		{
			get
			{
				return this.mRSACrypto;
			}
			private set
			{
				this.mRSACrypto = value;
			}
		}
		#endregion

		#region Methods
		protected MessagingNetwork()
		{		
			this.mMessageProcessors = new List<IMessageProcessor>();
			this.AsymmetricCrypto = new Inaugura.Security.RSACrypto();
		}

		/// <summary>
		/// Creates a message pipe and attempts to connect to the target.
		/// </summary>
		/// <param name="target">The target address</param>
		/// <param name="supportEncryption">If true the message pipe upon connecting will attempt to nagotiate a secure connection</param>
		/// <returns>A connected message pipe</returns>
		public abstract MessagePipe CreateMessagePipe(EndPoint target);

		//public abstract void BeginCreateMessagePipe(EndPoint target, AsyncCallback requestCallback, object state);
		//public abstract void EndCreateMessagePipe(IAsyncResult asyncResult);
		

		/// <summary>
		/// Opens the messaging network
		/// </summary>
		public abstract void Open();

		/// <summary>
		/// Closes the messaging network
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// Registers a message processor not currently registerd with the message hub
		/// </summary>
		/// <param name="processor">The message processor to register</param>
		public void Register(IMessageProcessor processor)
		{
			lock (this.mMessageProcessors)
			{
				if (!this.mMessageProcessors.Contains(processor))
					this.mMessageProcessors.Add(processor);
			}
		}

		/// <summary>
		/// Unregisters a message processor currently registered with the message hub
		/// </summary>
		/// <param name="processor">The message processor to unregister</param>
		public void Unregister(IMessageProcessor processor)
		{
			lock (this.mMessageProcessors)
			{
				if (this.mMessageProcessors.Contains(processor))
					this.mMessageProcessors.Remove(processor);
			}
		}

		public XmlMessage ProcessRequest(XmlMessage request)
		{
			IMessageProcessor[] messageProcessors = this.MessageProcessors;
			IMessageProcessor messageHandler = null;
			foreach (IMessageProcessor processor in messageProcessors)
			{
				if (processor.SupportsRequest(request))
				{
					messageHandler = processor;
				}
			}
			if (messageHandler == null)
				return new RequestNotSupported("The request is not supported");
			else
			{
				return messageHandler.ProcessRequest(request);
			}
		}		
		#endregion		
	}
}
