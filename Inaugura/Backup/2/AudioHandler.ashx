<%@ WebHandler Language="C#" Class="AudioHandler" %>

using System;
using System.Web;
using System.Web.SessionState;

public class AudioHandler : BaseHttpHandler, IRequiresSessionState
{
    public override bool ValidateParameters(HttpContext context)
    {
        if (context.Request.Params["id"] != null && context.Request.Params["mode"] != null)
            return true;
        else
            return false;  
    }
    
    public override bool RequiresAuthentication
    {
        get
        {             
            return false; 
        }  
    }   
    
    public override void HandleRequest(HttpContext context)
    {
        string mode = context.Request.Params["mode"];

        if (mode == "voicemail")
        {
            this.HandleVoiceMail(context);
        }
        else if (mode == "file")
        {
            this.HandleFile(context);
        }                         
    }

    private void HandleFile(HttpContext context)
    {       
        Guid id = new Guid(context.Request.Params["id"]);
        Inaugura.RealLeads.File file = Helper.API.ListingManager.GetFile(id);
        if (file == null)
            this.RespondFileNotFound(context);
       
        #region Convert the wav data to MP3
        byte[] mp3Data = Inaugura.Media.Helper.ConvertToMP3(file.Data, 16);
        #endregion   
        
        string fileName = string.Format("{0}.mp3", file.ID.ToString());
        this.SendAudioData(context, mp3Data, fileName);      
    }    

    private void HandleVoiceMail(HttpContext context)
    {
        throw new NotImplementedException();
        /*
        Inaugura.RealLeads.Agent agent = SessionHelper.Agent.ActiveAgent;
        if (agent == null)
        {
            this.RespondForbidden(context);
            return;
        }

        string id = context.Request.Params["id"];

        Inaugura.RealLeads.VoiceMail message = DataHelper.RealLeadsDataStore.VoiceMailStore.GetVoiceMail(id);

        // Message not found
        if (message == null)
        {
            this.RespondFileNotFound(context);
            return;
        }

        // make sure the agent is allowed to hear this voice mail
        if (message.AgentID != agent.ID)
        {
            this.RespondForbidden(context);
            return;
        }

        // get the wav file data
        Inaugura.File file = DataHelper.RealLeadsDataStore.VoiceMailStore.GetFile(message.FileID);
        if (file == null)
            throw new System.Exception("Could not get voice mail file");

        #region Convert the wav data to MP3
        byte[] mp3Data = Inaugura.Media.Helper.ConvertToMP3(file.Data, 16);
        #endregion

        // set the voice mail as old
        if (message.Status == Inaugura.RealLeads.VoiceMail.VoiceMailStatus.New)
        {
            message.Status = Inaugura.RealLeads.VoiceMail.VoiceMailStatus.Old;
            DataHelper.RealLeadsDataStore.VoiceMailStore.Update(message);
        }

        string fileName = string.Format("{0}.mp3", message.Date.ToString("ddMMyyyy_HHmm"));
        this.SendAudioData(context, mp3Data, fileName);      
        */
    }

    private void SendAudioData(HttpContext context, byte[] audioData, string fileName)
    {
        context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
        context.Response.ContentType = "audio/mpeg";
        context.Response.OutputStream.Write(audioData, 0, audioData.Length);      
    }    
    
   
}
