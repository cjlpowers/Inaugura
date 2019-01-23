using System;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Collections;
using System.ComponentModel.Design;


namespace Inaugura.Web.Controls
{
	/// <summary>
	/// Provides design time support for the TabBar in VS.NET
	/// </summary>
	internal class TabBarDesigner : System.Web.UI.Design.ControlDesigner
	{
		protected TabBar mTabBar;

		/// <summary>
		/// Initializes the designer
		/// </summary>
		/// <param name="component"></param>
		public override void Initialize(IComponent component)
		{
			// Make sure that this designer is attached to a TabBar
			if(component is TabBar)
			{
				base.Initialize (component);
				this.mTabBar = component as TabBar;
			}
			base.Initialize(component);
		}

		/// <summary>
		/// Writes the HTML used be VS.net to display the control at design-time.
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml()
		{
			return base.GetEmptyDesignTimeHtml();							
		}

		public override string GetPersistenceContent()
		{
			StringWriter sw = new StringWriter();
			HtmlTextWriter html = new HtmlTextWriter(sw);

			if (this.mTabBar != null)
			{
				foreach (Tab tab in this.mTabBar.Tabs)
				{
					IDesignerHost host = (IDesignerHost)this.Component.Site.GetService(typeof(IDesignerHost));
					sw.WriteLine(System.Web.UI.Design.ControlPersister.PersistControl(tab, host));

					/*
					html.WriteBeginTag("Tab");
					html.WriteAttribute("id", tab.ID);					
					html.WriteAttribute("text", tab.Text);	
					html.WriteAttribute("selectable", tab.Selectable.ToString());						
					html.WriteLine(HtmlTextWriter.SelfClosingTagEnd);
					*/
				}
				return sw.ToString();
			}

			return base.GetPersistenceContent();
		}		
	}

	internal class TabBarBuilder : ControlBuilder
	{
		public override Type GetChildControlType(string tagName, IDictionary attribs)
		{
			if (string.Compare(tagName,"Tab", true) == 0)
			{
				return typeof(Tab);
			}
			return base.GetChildControlType (tagName, attribs);
		}
	}				
}
