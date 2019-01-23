using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using ZedGraph;

namespace Inaugura.RealLeads.Graphing
{
	public class VolumeGraph
	{
		#region Variables
		private string mTitle = "Volume";		
		private string mXAxisTitle = "Date/Time";
		private string mY1AxisTitle = "Number of Calls";
        private string mY2AxisTitle = "Number of Web Hits";
		private GraphPane mGraph;
		#endregion

		#region Properties
		public string Title
		{
			get
			{
				return this.mTitle;
			}
			set
			{
				this.mTitle = value;
			}
		}

		public string XAxisTitle
		{
			get
			{
				return this.mXAxisTitle;
			}
			set
			{
				this.mXAxisTitle = value;
			}
		}

		public string Y1AxisTitle
		{
			get
			{
				return this.mY1AxisTitle;
			}
			set
			{
				this.mY1AxisTitle = value;
			}
		}

        public string Y2AxisTitle
		{
			get
			{
				return this.mY2AxisTitle;
			}
			set
			{
				this.mY2AxisTitle = value;
			}
		}
		#endregion

		public VolumeGraph(CallVolume[] callVolumes, Volume[] webVolume)
		{
			this.mGraph =  new GraphPane(new RectangleF(0, 0,100,100), "","","");
            this.mGraph.Y2Axis.IsVisible = true;

			PointPairList points = new PointPairList();
			foreach(CallVolume volume in callVolumes)
			{
				points.Add(new PointPair(new XDate(volume.Time),volume.Value));
			}
            BarItem bar = this.mGraph.AddBar("Call Volume", points, System.Drawing.Color.FromArgb(59, 166, 0));

            points = new PointPairList();
            foreach (Volume volume in webVolume)
            {
                points.Add(new PointPair(new XDate(volume.Time), volume.Value));
            }
            bar = this.mGraph.AddBar("Web Volume", points, System.Drawing.Color.FromArgb(77, 77, 77));
            bar.IsY2Axis = true;
			
		}

        public VolumeGraph(CallVolume[] callVolumes, Volume[] webVolume, DateTime scaleStart, DateTime scaleEnd) : this(callVolumes, webVolume)
        {
            this.mGraph.XAxis.MinAuto = false;
            this.mGraph.XAxis.MaxAuto = false;
            this.mGraph.XAxis.Min = new XDate(scaleStart);
            this.mGraph.XAxis.Max = new XDate(scaleEnd);          
        }

        public VolumeGraph(CallVolume[] callVolumes, Volume[] webVolumes, DateTime scaleStart, DateTime scaleEnd , TimeSpan barWidth) : this(callVolumes, webVolumes, scaleStart, scaleEnd)
        {           
            TimeSpan span = scaleEnd - scaleStart;
            this.mGraph.ClusterScaleWidth = barWidth.TotalMinutes / span.TotalMinutes;
        }

		public void Draw(System.Drawing.Graphics g)
		{
			this.mGraph.PaneRect = new System.Drawing.RectangleF(0,0,g.VisibleClipBounds.Width-1,g.VisibleClipBounds.Height-1);			
			this.mGraph.Title = this.Title;
            this.mGraph.XAxis.Type = AxisType.Date;
			this.mGraph.XAxis.Title = this.XAxisTitle;
			this.mGraph.YAxis.Title = this.Y1AxisTitle;
            this.mGraph.Y2Axis.Title = this.Y2AxisTitle;
            this.mGraph.YAxis.IsOppositeTic = false;
            this.mGraph.YAxis.IsMinorOppositeTic = false;
            this.mGraph.Y2Axis.IsOppositeTic = false;
            this.mGraph.Y2Axis.IsMinorOppositeTic = false;
			this.mGraph.AxisFill = new Fill( Color.White, Color.FromArgb(227,242,219), -45F );          
            this.mGraph.PaneBorder.Color = Color.White;
            this.mGraph.YAxis.IsShowGrid = true;
            this.mGraph.YAxis.GridColor = Color.Gray;
            this.mGraph.FontSpec.FontColor = Color.Gray;
            this.mGraph.XAxis.TitleFontSpec.FontColor = this.mGraph.FontSpec.FontColor;
            this.mGraph.YAxis.TitleFontSpec.FontColor = this.mGraph.FontSpec.FontColor;
            this.mGraph.Y2Axis.TitleFontSpec.FontColor = this.mGraph.FontSpec.FontColor;
            this.mGraph.YAxis.TitleFontSpec.FontColor = this.mGraph.FontSpec.FontColor;
            this.mGraph.XAxis.ScaleFontSpec.Angle = 65F;
            this.mGraph.XAxis.ScaleFormatAuto = true;
            this.mGraph.Legend.Border.IsVisible = false;
			mGraph.AxisChange(g);
            if (this.mGraph.YAxis.Step < 1)
                this.mGraph.YAxis.Step = 1;
			mGraph.Draw(g);
		}

		public System.Drawing.Image CreateImage(int width, int height, System.Drawing.Imaging.ImageFormat format)
		{
			using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height))
			{
				System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;                
				this.Draw(g);

				// get the correct format
				using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
				{
					bmp.Save(mem, format);
					return System.Drawing.Image.FromStream(mem);
				}
			}
		}
	}
}
