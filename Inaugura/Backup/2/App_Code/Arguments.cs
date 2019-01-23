using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Arguments
/// </summary>
public class Arguments : System.Collections.Generic.Dictionary<string, string>
{
    #region Variables
    public const string EditControlKey = "edit";
    public const string ActionKey = "action";
    public const string ListingIDKey = "listing";
    public const string LevelIDKey = "level";
    public const string RoomIDKey = "room";
    public const string ImageIDKey = "image";
    #endregion

    #region Properties    
    public string EditControl
    {
        get
        {
            if (!this.ContainsKey(EditControlKey))
                return null;
            return this[EditControlKey];
        }
    }

    public string Action
    {
        get
        {
            if (!this.ContainsKey(ActionKey))
                return null;
            return this[ActionKey];
        }
    }

    public Guid ListingID
    {
        get
        {
            return this.GetIDFromArgument(ListingIDKey);
        }
    }

    public Guid LevelID
    {
        get
        {
            return this.GetIDFromArgument(LevelIDKey);
        }
    }

    public Guid RoomID
    {
        get
        {
            return this.GetIDFromArgument(RoomIDKey);
        }
    }

    public Guid ImageID
    {
        get
        {
            return this.GetIDFromArgument(ImageIDKey);
        }
    }
    #endregion

    #region Methods
    public Arguments(string args)
    {
        string[] items = args.Split(';');
        foreach (string item in items)
        {
            string[] arg = item.Split(',');
            if (arg.Length != 2)
                throw new ArgumentException(string.Format("The argument '{0}' was not in the correct format", item));
            this.Add(arg[0], arg[1]);
        }
    }

    public Arguments(System.Collections.Generic.Dictionary<string, string> list) : base(list)
    {
    }

    private Guid GetIDFromArgument(string argumentKey)
    {
        if (this.ContainsKey(argumentKey))
            return new Guid(this[argumentKey]);
        return Guid.Empty;
    }
    #endregion
}
