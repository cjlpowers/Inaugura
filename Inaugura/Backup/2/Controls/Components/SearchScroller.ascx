<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchScroller.ascx.cs"
    Inherits="Controls_Components_SearchScroller" %>
<script type="text/javascript">
    var smLong = <% Response.Write(this.SearchLongitude); %>;
	var smLat = <% Response.Write(this.SearchLatitude); %>;
    window.onload = function() {ScrollInit();	}    
    function _ListingSelected(id)
    {
       __doPostBack('<%= this.ClientID.Replace('_','$') %>',id);
       listingMap = null;
    }
</script>
<div id="searchResultsBar">
    <h1>Search Results</h1>
    <a id="srContentToggleCtrl" onclick="ToggleSRContent()" class="searchResultsHide"></a>
    <div id="searchResultsContent" style="display:block;">
        <div id="mapHolder">
        </div>
        <div id="scrollHolder">
            <table cellpadding="0" cellspacing="0" style="margin-top: 5px;">
                <tr>
                    <td id="searchLeft" class="searchLeft" runat="server">
                    </td>
                    <td id="searchFill">
                        <div style="display: none" id="SearchKey">
                            <% Response.Write(this.SearchKey); %>
                        </div>
                        <div id="ninersWrap" style="width: 700px;">
                            <div id="ninersBox" style="position: relative; width: 700px; height: 120px; overflow: hidden;">
                                <div id="ninersMove" style="position: absolute; left: 0; top: 0;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr id="ItemList">
                                            <% Response.Write(this.InitialResults); %>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td id="searchRight" class="searchRight" runat="server">
                    </td>
                </tr>
            </table>
        </div>
       <div>
        <a id="srModeToggleCtrl" onclick="ToggleSRMode()" class="linkSearchResultsToggle">Show Results Map</a>
       </div>  
    </div>
</div>

