using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Inaugura.Reports
{
    /// <summary>
    /// A control for rendering reports
    /// </summary>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)
    ]
    public class ReportHtmlWriter : WebControl
    {
        #region Variables
        private Report mReport;
        #endregion

        #region Properties
        /// <summary>
        /// The report
        /// </summary>
        public Report Report
        {
            get
            {
                return this.mReport;
            }
            set
            {
                this.mReport = value;
            }
        }
        #endregion

        #region Methods
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.Report == null)
                return;

            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "rpt");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
            this.RenderReportSections(this.Report.Sections, writer);
            writer.Indent--;
            writer.WriteEndTag("div");
            writer.WriteLine();
        }

        private void RenderReportSection(ReportSection reportSection, System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                writer.Indent++;
                if (reportSection is Paragraph)
                    this.RenderParagraph(reportSection as Paragraph, writer);
                else if (reportSection is HyperLink)
                    this.RenderHyperLink(reportSection as HyperLink, writer);
                else if (reportSection is Image)
                    this.RenderImage(reportSection as Image, writer);
                else
                    throw new NotSupportedException(string.Format("The Report Section {0} was not supported", reportSection.GetType().Name));
            }
            finally
            {
                writer.Indent--;
            }
        }

        private void RenderReportSections(IEnumerable<ReportSection> reportSections, System.Web.UI.HtmlTextWriter writer)
        {
            foreach (ReportSection section in reportSections)
                this.RenderReportSection(section,writer);
        }

        private void WriteAttributes(ReportSection section, System.Web.UI.HtmlTextWriter writer)
        {
            foreach(KeyValuePair<string,string> attribute in section.Attributes)
                writer.WriteAttribute(attribute.Key, attribute.Value);
        }

        private void RenderParagraph(Paragraph para, System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteBeginTag("p");
            this.WriteAttributes(para, writer);
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(para.Text);            
            writer.WriteEndTag("p");
            writer.WriteLine();
        }

        private void RenderHyperLink(HyperLink link, System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteBeginTag("a");
            this.WriteAttributes(link, writer);
            writer.WriteAttribute("href", link.Uri.ToString());
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(link.Text);
            this.RenderReportSections(link.Sections, writer);            
            writer.WriteEndTag("a");
            writer.WriteLine();
        }

        private void RenderImage(Image img, System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteBeginTag("img");
            this.WriteAttributes(img, writer);
            writer.WriteAttribute("src", img.Uri.ToString());
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("img");
            writer.WriteLine();
        }
        #endregion
    }
}
