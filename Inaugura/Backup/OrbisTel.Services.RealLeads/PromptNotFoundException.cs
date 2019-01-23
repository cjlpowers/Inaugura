using System;
using System.Collections.Generic;
using System.Text;

namespace OrbisTel.Services.RealLeads
{
	class PromptNotFoundException : ApplicationException
	{
		private string mPrompt = string.Empty;

		public string Prompt
		{
			get
			{
				return this.mPrompt;
			}
			private set
			{
				this.mPrompt = value;
			}
		}

		public PromptNotFoundException(string prompt): this(prompt, "The prompt (" + prompt + ") cound not be found")
		{
		}
		public PromptNotFoundException(string prompt, string message): base(message)
		{
		}
	}
}
