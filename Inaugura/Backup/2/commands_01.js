/****************************************************
Listing Content Methods 
****************************************************/
function GetListingContent(id)
{
    WebServices.ListingService.GetListingContent(id,"", OnGetListingContentComplete,OnGetListingContentTimeout, OnGetListingContentError);
    ShowLoading();
}

function ShowLoading()
{
    var contentElement = document.getElementById("ListingContent");
    if(contentElement)
        contentElement.innerHTML = '<div style="text-align:center;height:1500px;"><span id="loading">Loading</span></div>';          
}

function OnGetListingContentComplete(result)
{
    ShowListing(result);
}

function OnGetListingContentTimeout(result)
{
    alert(result);
}

function OnGetListingContentError(error)
{
    ShowError(error.get_message());
}

function ShowListing(content)
{    
    var contentElement = document.getElementById("ListingContent");
    contentElement.innerHTML = content;
}

/////////////////////////////
function UpdateContentOpenerWindow(id)
{     
    WebServices.ListingService.GetListingContent(id,"", OnUpdateContentOpenerWindowComplete,OnGetListingContentTimeout, OnGetListingContentError);
    var contentElement = opener.document.getElementById("ListingContent");
    contentElement.innerHTML ='<div style="text-align:center;height:1500px;"><span id="loading">Loading</span></div>';
}

function OnUpdateContentOpenerWindowComplete(result)
{
    ShowListingInOpenerWindow(result);    
}

function ShowListingInOpenerWindow(content)
{
    self.close();
    var contentElement = opener.document.getElementById("ListingContent");
    contentElement.innerHTML = content;      
}

/****************************************************
Search Result Methods 
****************************************************/
function GetScrollSearchResults(searchKey, startIndex, endIndex)
{    
    WebServices.ListingService.GetScrollSearchResults(searchKey, startIndex, endIndex, OnGetScrollSearchResultsComplete,OnGetScrollSearchResultsTimeout,OnGetScrollSearchResultsError);    
}

function OnGetScrollSearchResultsComplete(result)
{
    InsertBlock(result);
}

function OnGetScrollSearchResultsTimeout(result)
{
    ShowError(result.get_message());    
}

function OnGetScrollSearchResultsError(result)
{
    ShowError(result.get_message());
}


/****************************************************
Listing Administration Methods
****************************************************/
function ListingOpperation(id,target,opperation)
{  
    ShowLoading();  
    WebServices.ListingService.ListingOpperation(id,target,opperation,OnListingOpperationComplete,OnListingOpperationTimeout,OnListingOpperationError);        
}

function OnListingOpperationComplete(result)
{
    ShowListing(result);
}

function OnListingOpperationTimeout(result)
{
    ShowError(result.get_message());
}

function OnListingOpperationError(result)
{
    ShowError(result.get_message());
}


/****************************************************
Popup
****************************************************/
function PGetListingContent(id)
{
    ShowLoading();
    WebServices.ListingService.GetListingContent(id,"", POnGetListingContentComplete,POnGetListingContentTimeout, POnGetListingContentError);     
}

function POnGetListingContentComplete(result)
{ 
    PShowListing(result);
}

function POnGetListingContentTimeout(result)
{
    alert(result);
}

function POnGetListingContentError(error)
{ 
    ShowError(error.get_message());
}

function PShowListing(content)
{ 
    ClosePopup();
    var contentElement = window.frames.parent.document.getElementById("ListingContent");
    contentElement.innerHTML = content;
}

 function SetFrame(url, width, height)
{  
        if(document.getElementById("srcFrame1") != null)
        {
            document.getElementById("srcFrame1").setAttribute("src",url);    
        }        
        ShowOverlay();
               
        if(document.getElementById("PopupContainer") != null)
        {
            document.getElementById("PopupContainer").style.display = 'block';
            if(height != null && width != null)
            {
                //document.getElementById("Popup").style.height = height + 'px';
                //document.getElementById("Popup").style.width = width + 'px';
            }                            
        }        
}     

function ClosePopup(url)
{      
    window.frames.parent.document.getElementById("Screen").style.display="none"; 
    window.frames.parent.document.getElementById("PopupContainer").style.display="none"; 
    if(window.frames.parent.document.getElementById("srcFrame1") != null)
    {
        //alert(window.frames.parent.document.getElementById("srcFrame1").getAttribute("src"));
        window.frames.parent.document.getElementById("srcFrame1").setAttribute("src","");    
    }
    if(url != null)
        window.frames.parent.location = url;
}

function ClosePopupAndRefresh(id)
{      
    PGetListingContent(id);
}

function PopupLoginRedirect(url)
{
    if(window.frames.parent.document.getElementById("srcFrame1") != null)
        ClosePopup(url);
}

function ShowOverlay()
{
     if(document.getElementById("Screen") != null)
     {                            
            //document.getElementById("Screen").style.height= document.getElementById("mid").scrollHeight+'px';                
            document.getElementById("Screen").style.display="block";          
     }                      
}

function ShowConfirm(message,url)
{
    if(confirm(message))
        if(url != null)
            window.location = url
}

function ShowError(message)
{
    alert(message);
}
