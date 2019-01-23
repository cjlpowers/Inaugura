using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MeasurementControl : System.Web.UI.UserControl
{
	#region Internal Constructs
    public enum MeasurementType
    {
        InteriorLength = 1,
        InteriorArea = 2,
        ExteriorLength = 3,
        ExteriorArea = 4
    }
	#endregion

	public MeasurementType Mode
	{
        get
        {
            string mode = (string)this.ViewState["mode"];
            if (mode == "1")
                return MeasurementType.InteriorLength;
            else if (mode == "2")
                return MeasurementType.InteriorArea;
            else if (mode == "3")
                return MeasurementType.ExteriorLength;
            else
                return MeasurementType.ExteriorArea;
        }
		set
		{
            if (value == MeasurementType.InteriorLength)
			{
				this.ViewState["mode"] = "1";
				this.SetUnits(Inaugura.RealLeads.Dimensions.InteriorLengthUnits.ToArray());
			}
			else if(value == MeasurementType.InteriorArea)
			{
				this.ViewState["mode"] = "2";
                this.SetUnits(Inaugura.RealLeads.Dimensions.InteriorAreaUnits.ToArray());
			}
            else if (value == MeasurementType.ExteriorLength)
            {
                this.ViewState["mode"] = "3";
                this.SetUnits(Inaugura.RealLeads.Dimensions.ExteriorLengthUnits.ToArray());
            }
            else if (value == MeasurementType.ExteriorArea)
            {
                this.ViewState["mode"] = "4";
                this.SetUnits(Inaugura.RealLeads.Dimensions.ExteriorAreaUnits.ToArray());
            }
            else
                throw new NotSupportedException("Value was not supported");
		}
	}

    public Inaugura.Measurement.Measurement Measurement
    {
        get
        {
            double val = 0;
            double.TryParse(this.mTxtValue.Text, out val);
            return new Inaugura.Measurement.Measurement(val, Inaugura.Measurement.Unit.Parse(this.mUnits.SelectedValue));
        }
        set
        {
            this.mTxtValue.Text = value.Value.ToString();           
            this.SelectUnit(value.Unit);
        }
    }

    public bool Enabled
    {
        get
        {
            return this.mTxtValue.Enabled;
        }
        set
        {
            this.mTxtValue.Enabled = value;
            this.mUnits.Enabled = value;
        }
    }

	protected void Page_Load(object sender, EventArgs e)
	{
	}

	private void SetUnits(Inaugura.Measurement.Unit[] units)
	{
		this.mUnits.Items.Clear();
		foreach (Inaugura.Measurement.Unit unit in units)
		{
			ListItem item = new ListItem(unit.Symbol);
			this.mUnits.Items.Add(item);
		}
	}

    private void SelectUnit(Inaugura.Measurement.Unit unit)
    {
        foreach (ListItem item in this.mUnits.Items)
        {
            if (item.Text == unit.Symbol)
            {
                item.Selected = true;
                return;
            }
        }
    }
}
