using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using ZedGraph;

namespace Inaugura.RealLeads.Graphing
{
	public class CallVolumeGraph
	{
		#region Variables
		private string mTitle = string.Empty;		
		private string mXAxisTitle = string.Empty;
		private string mYAxisTitle = string.Empty;
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

		public string YAxisTitle
		{
			get
			{
				return this.mYAxisTitle;
			}
			set
			{
				this.mYAxisTitle = value;
			}
		}
		#endregion

		public CallVolumeGraph(CallVolume[] callVolumes)
		{
			this.mGraph =  new GraphPane(new RectangleF(0, 0,10,10), "","","");
			PointPairList points = new PointPairList();
			foreach(CallVolume volume in callVolumes)
			{
				points.Add(new PointPair(new XDate(volume.CallTime),volume.Volume));
			}
			this.mGraph.AddBar("Call Volume", points, System.Drawing.Color.Green);
		}

		public void Draw(System.Drawing.Graphics g)
		{
			this.mGraph.PaneRect = new System.Drawing.RectangleF(0,0,g.VisibleClipBounds.Width-1,g.VisibleClipBounds.Height-1);			
			this.mGraph.Title = this.Title;
			this.mGraph.XAxis.Title = this.XAxisTitle;
			this.mGraph.YAxis.Title = this.YAxisTitle;
			this.mGraph.AxisFill = new Fill( Color.White, Color.WhiteSmoke, -45F );
			this.mGraph.Legend.Border.IsVisible = false;

		

			mGraph.AxisChange(g);
			mGraph.Draw(g);
		}

		public System.Drawing.Image CreateImage(int width, int height, System.Drawing.Imaging.ImageFormat format)
		{
			using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height))
			{
				System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
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
