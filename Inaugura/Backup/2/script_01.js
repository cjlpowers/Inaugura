/****************************************************
Generic Stuff
****************************************************/
function SetIDVisible(id,visible)
{
    var item = document.getElementById(id);
     if(item)
        item.style.display = visible==true?"block":"none"; 
}

function Close()
{
    self.close();
}

function CloseAndRefreshOpener()
{
    opener.location.reload();
    Close();
}

function OpenLink(url)
{
    newWindow = window.open(url);
	newWindow.focus();
}

function OpenWindow(url, width, height, windowName) 
{
		newWindow = window.open(url, windowName, 'width='+width+',height='+height, 'toolbar=no,menubar=no,locationbar=no');
		newWindow.focus();
}


/****************************************************
Image Rotator
****************************************************/
var imageIndex = 0
var imgArray;
function InitImageRotator(images)
{
    if(images.length > 0 && images.substring(0,1) == "/")
        images = images.substring(1);
        
     imgArray = images.split("/");    
     if(imgArray.length == 0)
        return; 
        
    setTimeout("RotateImages()",4000);
}

function RotateImages()
{  
    if(imgArray.length == 0)
        return;

    var imageUrl = imgArray[ imageIndex % imgArray.length];
    
    var img = document.getElementById? document.getElementById("featureImage") : document.all.featureImage;
    if(img != null)
    {
        if(img.filters)
            img.filters.item(0).Apply();
        img.src = 'ImageHandler.ashx?id=' + imageUrl+'&mode=320';    
        if(img.filters)
            img.filters.item(0).Play()        
    }
    imageIndex++;     
    
    if(imgArray.length>1)
        setTimeout("RotateImages()",4000)  
}

/****************************************************
Search Results Bar
****************************************************/
function ToggleSRMode()
{
    var divMap = document.getElementById("mapHolder");
    var divScroll = document.getElementById("scrollHolder");
    var btn = document.getElementById("srModeToggleCtrl");
    if(divMap.style.display != "block")
    {
        divMap.style.display = "block";	
        divScroll.style.display = "none"; 
        btn.innerHTML = "Show Results List";           
        ShowSearchMap() 
    } 
    else
    {
        divMap.style.display = "none"; 
        divScroll.style.display = "block"; 
        btn.innerHTML = "Show Results Map";
    } 
}
	
function ToggleSRContent()
{	    
    var divContent = document.getElementById("searchResultsContent");
    var btn = document.getElementById("srContentToggleCtrl"); 
    if(divContent.style.display != "block")
    {
        divContent.style.display = "block";	
        btn.className = "searchResultsHide";
    } 
    else
    {
        divContent.style.display = "none";	
        btn.className = "searchResultsShow";
    } 
}

var searchMap = null;
function ShowSearchMap()
{
    if(searchMap == null)
    { 
        var holder = document.getElementById("mapHolder");
        holder.innerHTML = '<div id="searchMap" style="position:relative; width:100%; height:300px;"></div>';
        searchMap = new VEMap('searchMap');        
        var location = new VELatLong(smLat,smLong)       
       try
       {           
           searchMap.LoadMap(location, 13 ,'r' ,false);
           searchMap.SetScaleBarDistanceUnit(VEDistanceUnit.Kilometers); 
       }
       catch(e){}
       finally
       {             
           WebServices.ListingService.GetSearchPin(OnSearchMapGetSearchPinComplete,OnTimeout, OnError);                        
       }           
   } 
}

function OnSearchMapGetSearchPinComplete(result)
{
    WebServices.ListingService.GetSearchResultPins(OnGetPinsComplete,OnTimeout, OnError);
    if(result != null)
        AddPinObject(searchMap, result)
}

function OnGetPinsComplete(result)
{
     for(i=0; i < result.length; i++)
        AddPinObject(searchMap,result[i]);  
}

/****************************************************
Mapping Stuff
****************************************************/
var listingMap = null;
var searchMap = null;
var searchPin = null;
var pinID = 0;
var listingLocation = null;

function Pan(map, longitude, latitude)
{
    var location = new VELatLong(latitude, longitude); 
    map.PanToLatLong(location);
}

function AddPin(map,location, name, description, icon)
{ 
    var pin = new VEPushpin(pinID, location,icon, name, description);
    map.AddPushpin(pin);
    pinID++;
}

function AddPinObject(map,pin)
{ 
    AddPin(map, new VELatLong(pin.latitude, pin.longitude), pin.title, pin.description,pin.icon);
}

// Listing Map ///////////////////////////////
function ShowListingMap(listingID)
{
    // try and get the pin
    WebServices.ListingService.GetListingPin(listingID, OnGetListingPinComplete,OnTimeout, OnError);   
 }

function OnGetListingPinComplete(result)
{    
    SetIDVisible("listingMap",true);
    listingMap = new VEMap('listingMap');    
    listingLocation = new VELatLong(result.latitude, result.longitude)
    listingMap.LoadMap(listingLocation, 15 ,'r' ,false);
    listingMap.SetScaleBarDistanceUnit(VEDistanceUnit.Kilometers);
    AddPinObject(listingMap,result);
    if(searchPin == null)
        WebServices.ListingService.GetSearchPin(OnListingMapGetSearchPinComplete,OnTimeout, OnError);
    else
         OnListingMapGetSearchPinComplete(searchPin);
} 

function OnListingMapGetSearchPinComplete(result)
{
    if(result != null)
    {
        searchPin = result;
       AddPinObject(listingMap, result)
       listingMap.GetRoute(listingLocation, new VELatLong(result.latitude, result.longitude), VEDistanceUnit.Kilometers, VERouteType.Shortest); 
    } 
}

// Generic Methods

function OnTimeout(result)
{
    alert(result);
}

function OnError(error)
{
    debug.trace(error.get_message());
}

// Listing Content
function SetImage(imageID)
{  
    //SetIDVisible("listingMap",false);
    SetIDVisible("listingImage",true);  
    var img =  document.getElementById("listingImage");
    img.setAttribute('src','ImageHandler.ashx?id='+imageID+'&mode=480');    
}