
var visibleSize = 3;
var visibleIndex = 1;
var visibleBuffer = 3;
var blockSize = 15;
var leftIndex = 1;
var loadingLeftBlock = false;
var loadingRightBlock = false;
var rightIndex = blockSize;
var leftBuffer;
var rightBuffer;
var frameWidth = 705;
var terminator = '<!---->';
var blockTerminator = '<!--block-->';


fx.Test = Class.create();
		fx.Test.prototype = Object.extend(new fx.Base(), {
			initialize: function(el, options) {
				this.el = $(el);
				this.setOptions(options);
				this.now = 0;
			},
			increase: function() {
				this.el.style.left  = this.now + "px";
			},						
            ScrollRight: function()       
            { 
                 if(visibleIndex >= rightIndex-visibleBuffer)
                    return; 
            
                trace("Left Index: "+leftIndex);               
                trace("Right Index: "+rightIndex);                
                trace("Visible Index (before): "+visibleIndex);
    
                var list = document.getElementById? document.getElementById("ItemList") : document.all.ItemList;
                var html = list.innerHTML;
                if(visibleIndex == rightIndex - visibleSize)
                    return;                     
           
                searchKeyCtrl = document.getElementById? document.getElementById("SearchKey") : document.all.SearchKey      
                visibleIndex = visibleIndex + visibleSize;                
                this.custom(this.now, this.now - frameWidth); 
                trace("Visible Index (after): "+visibleIndex);
            },
            ScrollLeft: function()       
            { 
                if(visibleIndex ==1)
                    return;
                    
                trace("Left Index: "+leftIndex);               
                trace("Right Index: "+rightIndex);                
                trace("Visible Index (before): "+visibleIndex);
    
                var list = document.getElementById? document.getElementById("ItemList") : document.all.ItemList;
                var html = list.innerHTML;
                visibleIndex = visibleIndex - visibleSize;                
                this.custom(this.now, this.now + frameWidth); 
                trace("Visible Index (after): "+visibleIndex);                
            }
		});		

/* test */
function GetScrollRight(searchKey, startIndex, endIndex)
{
    trace("Getting Results "+startIndex+" "+endIndex);
    WebServices.ListingService.GetScrollSearchResults(searchKey, startIndex, endIndex, OnGetGetScrollRightComplete,OnGetGetScrollLeftTimeout,OnGetScrollSearchResultsError);    
}

function OnGetGetScrollRightComplete(result)
{
    //var list = document.getElementById? document.getElementById("ItemList") : document.all.ItemList;
    AppendResults(result,true);    
}

function OnGetGetScrollLeftTimeout(result)
{
    alert(result.get_message());
}

function OnGetScrollLeftError(result)
{
    trace(result.get_message());
}

function AppendResults(results, right)
{    
    var cross_scroll=document.getElementById? document.getElementById("ninersMove") : document.all.ninersMove;
    var list = document.getElementById? document.getElementById("ItemList") : document.all.ItemList                
    var html;     
    
    html = list.innerHTML;
    for(i=0; i < results.length; i++)
    {
        if(results[i] !=terminator && results[i] != blockTerminator)
        {
            if(right)
                rightIndex++;
            else
                leftIndex--;               
        }
        if(right)
        {
            html = html + results[i];
        }
        else
            html = results[i] + html;        
    }    
    cross_scroll.innerHTML = '<table cellpadding="0" cellspacing="0"><tr id="ItemList">'+  html+'</tr></table>';
}


function trace(msg)
{
    //debug.trace(msg)
}

function ScrollInit()
{
        myNewEffect = new fx.Test('ninersMove', {duration: 1400, onComplete: function()
			    {			    
                    //GetScrollRight(searchKeyCtrl.innerHTML, 1, 40);  
                    if(visibleIndex >= rightIndex - visibleBuffer)
                    {
                        searchKeyCtrl = document.getElementById? document.getElementById("SearchKey") : document.all.SearchKey      
                        trace("Search Key: "+searchKeyCtrl.innerHTML);                                        
                        GetScrollRight(searchKeyCtrl.innerHTML, rightIndex+1, rightIndex+blockSize); 
                    }                 
			    }
			} );
}
