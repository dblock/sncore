var inDesignMode = false;
var currentDragMode = 0;
var zoneDragOver = 0;
var rowToDrop = 0;
var cellToDrop = 0;
var dropLocation = 0;
var splitter = document.createElement("div");
var horzZoneSplitter = 0;
var vertZoneSplitter = 0;
var moveObject = 0;
var maintainOriginalZone = 0;
var topObject = document.body;
var unsavedChanges = new Array();
var cssWebPartUserCellSelected;
var cssWebPartZoneSplitter;

// Sets up the layout flags.
function WebPartSetupLayoutFlags(webPartUserCellSelectedCssClass, webPartZoneSplitterCssClass)
{
    cssWebPartUserCellSelected = webPartUserCellSelectedCssClass;
    cssWebPartZoneSplitter = webPartZoneSplitterCssClass;
	inDesignMode = true;
	topObject = document.body;
	
	for (i = 0; i < webPartZones.length; i++)
	{
	    var a = webPartZones[i];
	    WebPartSetupZone(a[0], a[1], a[2]);
	}
}

function WebPartSetupZone(zone, normalClassName, selectedClassName)
{
    z = document.getElementById(zone);
    z.normalClassName = normalClassName;
    z.selectedClassName = selectedClassName;
}

// Gets the real offset for the specified element.
function GetRealOffset(startingObject, offsetType, endParent)
{
	var realValue = 0;
	if (!endParent)
		endParent = document.body;
	
	// Traverse the tree and calculate the real offset.
	for (var currentObject = startingObject; currentObject != endParent && currentObject != document.body; currentObject = currentObject.offsetParent)
	{
		realValue += eval('currentObject.offset' + offsetType);
	}
	
	return realValue;
}

// Get the parent table of the specified table cell.
function GetParentTable(tableCell)
{
	for (var currentObject = tableCell; currentObject.tagName != 'TABLE'; currentObject = currentObject.parentNode)
	{
		if (currentObject == document.body)
			return 0;
	}
	
	return currentObject;
}

// Gets the parent row of a table cell.
function GetParentRow(tableCell)
{
	var parentRow = tableCell.parentNode;
	while(parentRow.tagName != "TR" && parentRow.tagName != "BODY") parentRow = parentRow.parentNode;
	if (parentRow.tagName != "TR")
	{
		return null;
	}
	else
	{
		return parentRow;
	}
}

// Starts moving a webpart.
function MoveWebPartStart(zoneTableCellID, webPartCaption)
{
    var zoneTableCell = document.getElementById(zoneTableCellID);
    
    // Only move a webpart if the left mouse button was clicked.
	if (window.event && window.event.button != 1)
		return;
	
	currentDragMode = 'move';
	if (document.selection)
	    document.selection.empty();
	
	// Create drag-related objects.
	CreateDragObject(webPartCaption);
	CreateSplitter();
	
	dropLocation = zoneTableCell;
	maintainOriginalZone = (zoneTableCell.allowZoneChange == '0') ? GetParentTable(zoneTableCell) : '0';
	
	splitter.goodDrop = 'false';
	var zone = GetParentTable(zoneTableCell);
	if (zone.getAttribute('name') == 'webPartZone')
	{
		zoneDragOver = zone;
		zoneDragOver.className = zoneDragOver.selectedClassName;
	}
	
	MoveSplitter(zoneTableCell);
	
	if (document.body.attachEvent)
	    document.body.attachEvent('ondragover', MoveWebPartBodyDragOver);
	else
	    document.body.ondragover = MoveWebPartBodyDragOver;
	
	var oldDragEnd = document.body.ondragend;
	var oldDrop = document.body.ondrop;
	document.body.ondragend = new Function("window.event.returnValue = false;");
	document.body.ondrop = new Function("splitter.goodDrop = 'true';");
	zoneTableCell.ondragstart = new Function("try {event.dataTransfer.effectAllowed = 'move';} catch (exception) {}");
	if (zoneTableCell.attachEvent)
	    zoneTableCell.attachEvent("ondrag",MoveDragObject);
	else
	    zoneTableCell.ondrag = MoveDragObject;
	
	if (zoneTableCell.dragDrop)
	    zoneTableCell.dragDrop();
	
	if (document.body.detachEvent)
	    document.body.detachEvent('ondragover', MoveWebPartBodyDragOver);
	else
	    document.body.ondragover = null;
	
	document.body.ondragend = oldDragEnd;
	document.body.ondrop = oldDrop;
	if (zoneTableCell.detachEvent)
	    zoneTableCell.detachEvent("ondrag",MoveDragObject);
	else
	    zoneTableCell.ondrag = null;
	
	moveObject.style.display = 'none';
	currentDragMode = 0;
	if (navigator.userAgent.toLowerCase().indexOf("msie 5.5") != -1)
	{
		zoneTableCell.swapNode(zoneTableCell);
	}
	
	if (window.event)
	    window.event.returnValue = false;
	else
	    return false;
}

// Handles the webpart zone's drag enter event.
function MoveWebPartDragZoneEnter(zoneTable)
{
	if (currentDragMode != 'move')
		return;
	
	if (zoneTable != zoneDragOver)
	{
		zoneDragOver.className = zoneDragOver.normalClassName;
		zoneDragOver = zoneTable;
		event.dataTransfer.dropEffect = 'move';
	}
	
	MoveWebPartStopEventBubble()
}

// Handles the webpart's drag enter event.
function MoveWebPartDragEnter(zoneTableCell)
{
	if (currentDragMode != 'move')
		return;
	
	event.dataTransfer.dropEffect = 'move';
	cellToDrop = zoneTableCell.cellIndex;
	rowToDrop = GetParentRow(zoneTableCell).rowIndex;
}

// Handles the webpart's drag over event.
function MoveWebPartDragOver(zoneTableCell, needsSetup)
{
	if (currentDragMode != 'move')
		return;
	
	event.dataTransfer.dropEffect = 'move';
	SetupDropLocation(zoneTableCell, needsSetup);
	
	if (zoneDragOver.rows[rowToDrop])
	{
		dropLocation = zoneDragOver.rows[rowToDrop].cells[cellToDrop];
		MoveSplitter(dropLocation);
		MoveWebPartStopEventBubble()
	}
}

// Handles the webpart body's drag over event.
function MoveWebPartBodyDragOver()
{
	if (currentDragMode != 'move') return;
	event.dataTransfer.dropEffect = 'none';
	splitter.style.display = 'none';
	if (zoneDragOver.className != zoneDragOver.normalClassName)
		zoneDragOver.className = zoneDragOver.normalClassName;
	
	window.event.returnValue = false;
}

// Stops event bubbling.
function MoveWebPartStopEventBubble()
{
	if (currentDragMode != 'move' || splitter.style.display == 'none')
		return;
	
	window.event.returnValue = false;
	window.event.cancelBubble = true;
}

// Moves the webpart from its original zone to the destination zone.
function MoveWebPart(originalTableCell, destinationTableCell)
{
	splitter.style.display = 'none';
	zoneDragOver.className = zoneDragOver.normalClassName;
	if ((currentDragMode != 'move') || (splitter.goodDrop != 'true') || (originalTableCell == destinationTableCell))
		return;
	
	var newTableCell;
	var originalZone = GetParentTable(originalTableCell);
	var originalIndex = (originalTableCell.orientation == 'Horizontal') ? originalTableCell.cellIndex : originalTableCell.parentNode.rowIndex;
	var destinationZone;
	var destinationIndex;
	destinationZone = GetParentTable(destinationTableCell);
	var zonesChanged = (destinationZone != originalZone);
	if (destinationTableCell.orientation == 'Horizontal')
	{
		destinationIndex = destinationTableCell.cellIndex;
		newTableCell = GetParentRow(destinationTableCell).insertCell(destinationIndex);
	}
	else
	{
		destinationIndex = destinationTableCell.parentNode.rowIndex;
		newTableCell = destinationZone.insertRow(GetParentRow(destinationTableCell).rowIndex).insertCell();
	}
	
	newTableCell.swapNode(originalTableCell);
	if (originalTableCell.orientation == 'Horizontal')
		newTableCell.removeNode(true);
	else
		GetParentRow(newTableCell).removeNode(true);
	
	originalTableCell.orientation = destinationTableCell.orientation;
	if (zonesChanged)
	{
		var originalEmptyZoneText;
		var destinationEmptyZoneText;

		// Get the empty webpart zones.
		var tempTable;
		var emptyZones = document.getElementsByTagName('span');
		for (var i = 0; i < emptyZones.length; i++)
		{
			if (emptyZones[i].name == 'emptyWebPartZone')
			{
				tempTable = GetParentTable(emptyZones[i]);
				if (tempTable == originalZone)
					originalEmptyZoneText = emptyZones[i];
				else if (tempTable == destinationZone)
					destinationEmptyZoneText = emptyZones[i];
			}
		}

		if (originalEmptyZoneText)
		{
			var webPartsInZone = originalEmptyZoneText.getAttribute('webPartsInZone');
			originalEmptyZoneText.setAttribute('webPartsInZone', --webPartsInZone);
			if (webPartsInZone == 0)
			{
				originalEmptyZoneText.style.display = '';
			}
		}
		if (destinationEmptyZoneText != null)
		{
		    var webPartsInZone = destinationEmptyZoneText.getAttribute('webPartsInZone');
			destinationEmptyZoneText.setAttribute('webPartsInZone', ++webPartsInZone);
			destinationEmptyZoneText.style.display = 'none';
		}
	}
	
	if (zonesChanged || ((destinationIndex != originalIndex) && (destinationIndex != (originalIndex + 1))))
	{
		if (originalZone != destinationZone) 
		{
			AddChange(eval(originalTableCell.relatedWebPart), "ZoneID", destinationZone.zoneID);
			UpdatePartOrderAfterMove(originalZone, 0);
		}
		UpdatePartOrderAfterMove(destinationZone, 0);
	}
}

// Updates the order of the parts within the specified zone.
function UpdatePartOrderAfterMove(zone, startingIndex)
{
	var index;
	var offset = 0;
	if (zone.orientation == 'Horizontal')
	{
		var parentRow = zone.rows[0];
		for(index = startingIndex; index < parentRow.cells.length; index++)
		{
		    if (parentRow.cells[index].style.display == 'none')
		        offset++;
			AddChange(eval(parentRow.cells[index].relatedWebPart), "PartOrder", index - offset);
		}
	}
	else
	{
		for(index = startingIndex; index < zone.rows.length; index++)
		{
		    if (zone.rows[index].cells[0].style.display == 'none')
		        offset++;
			AddChange(eval(zone.rows[index].cells[0].relatedWebPart), "PartOrder", index - offset);
		}
	}
}

// Creates a drag object.
function CreateDragObject(webPartTitle)
{
	var titleText;
	if (!moveObject)
	{
	    if (document.body.insertAdjacentElement)
		    moveObject = document.body.insertAdjacentElement("afterBegin", document.createElement('div'));
		else
		    moveObject = document.body.appendChild(document.createElement('div'));
		moveObject.className = cssWebPartUserCellSelected;
		moveObject.style.cssText= "font-size:8pt;position:absolute;overflow:hidden;display:none;z-index:100";
		moveObject.style.filter = "progid:DXImageTransform.Microsoft.Alpha(opacity=75)";
		titleText = moveObject.appendChild(document.createElement('NOBR'));
		titleText.style.cssText = "padding-top:2px;width:147px;height:1.5em;overflow:hidden;text-overflow:ellipsis";
	}
	else
	    titleText = moveObject.childNodes[0];
	titleText.innerText = webPartTitle;
}

// Moves the drag object.
function MoveDragObject()
{
	if (currentDragMode != 'move')
		return;
	
	if (moveObject.style.display == 'none')
		moveObject.style.display = '';
	
	if (moveObject.style.width == '')
	{
		moveObject.realWidth = moveObject.offsetWidth;
		moveObject.realHeight = moveObject.offsetHeight;
	}
	
	var newWidth = moveObject.realWidth;
	var newHeight = moveObject.realHeight;
	var newLeft = event.clientX + document.body.scrollLeft - (newWidth / 2);
	var newTop = event.clientY + document.body.scrollTop + 1;
	
	if (newLeft + newWidth > document.body.scrollWidth)
		newWidth -= (newLeft + newWidth - document.body.scrollWidth);
	
	if (newTop + newHeight > document.body.scrollHeight)
		newHeight -= (newTop + newHeight - document.body.scrollHeight);
	
	if (newHeight <= 0 || newWidth <= 0) 
	{
		moveObject.style.display = 'none'; 
		newWidth = newHeight = 0;
	}
	else
	{
		moveObject.style.display = '';
	}
	
	moveObject.style.width = newWidth;
	moveObject.style.height = newHeight;
	moveObject.style.pixelLeft = newLeft;
	moveObject.style.pixelTop = newTop;
}

// Creates the splitter.
function CreateSplitter()
{
	if (!vertZoneSplitter || !horzZoneSplitter)
	{
		var splitterBuilder = document.createElement('table');
		splitterBuilder.style.cssText = "font-size:1pt; position:absolute; display:none; border-collapse:collapse";
		splitterBuilder.className = cssWebPartZoneSplitter;
		splitterBuilder.cellSpacing = '0';
		splitterBuilder.cellPadding = '0';
		if (splitterBuilder.attachEvent)
		{
			splitterBuilder.attachEvent('ondragenter', MoveWebPartStopEventBubble);
		    splitterBuilder.attachEvent('ondragover', MoveWebPartStopEventBubble);
		}
		else
		{
		    splitterBuilder.ondragenter = MoveWebPartStopEventBubble;
		    splitterBuilder.ondragover = MoveWebPartStopEventBubble;
		}
		var insideSplitterCell = splitterBuilder.insertRow(0).insertCell(0);
		insideSplitterCell.align = 'center';
		var insideSplitter = insideSplitterCell.appendChild(document.createElement('div'));
		insideSplitter.id = "insideSplitter";
		insideSplitter.className = cssWebPartZoneSplitter;
		insideSplitter.style.backgroundColor = splitterBuilder.currentStyle ? splitterBuilder.currentStyle.borderColor : splitterBuilder.style.borderColor;
		insideSplitter.style.background = "transparent";
		insideSplitter.style.borderWidth = "2px";
		insideSplitter.style.position = "relative";
		horzZoneSplitter = topObject.appendChild(splitterBuilder.cloneNode(true));
		vertZoneSplitter = topObject.appendChild(splitterBuilder.cloneNode(true));
		var insideHorzSplitter = horzZoneSplitter.all ? horzZoneSplitter.all["insideSplitter"] : getElemenyById(horzZoneSplitter, 'insideSplitter');
	    var insideVertSplitter = vertZoneSplitter.all ? vertZoneSplitter.all["insideSplitter"] : getElemenyById(vertZoneSplitter, 'insideSplitter');
		horzZoneSplitter.style.width = 6;
		horzZoneSplitter.style.borderStyle = "solid none";
		insideHorzSplitter.style.height = '100%';
		insideHorzSplitter.style.width = '33%';
		insideHorzSplitter.style.borderStyle = "none solid none none";
		insideHorzSplitter.style.posTop = 0;
		vertZoneSplitter.style.height = 6;
		vertZoneSplitter.style.borderStyle = "none solid";
		insideVertSplitter.style.width = '100%';
		insideVertSplitter.style.height = '2';
		insideVertSplitter.style.borderStyle = "solid none none none";
		insideVertSplitter.style.posTop = 1;
	}
	splitter = vertZoneSplitter;
}

// Moves the splitter.
function MoveSplitter(zoneTableCell)
{
	if (splitter)
		splitter.style.display = 'none';
	
	if (maintainOriginalZone == '0' || GetParentTable(zoneTableCell) == maintainOriginalZone)
	{
		var insideSplitter;
		if (zoneTableCell.orientation == 'Horizontal')
		{
			var rightOffset = ((document.dir == "rtl") ? zoneTableCell.offsetWidth - ((zoneTableCell.cellIndex == 0) ? 3 : 0) : 0); 
			splitter = horzZoneSplitter;
			insideSplitter = splitter.all ? splitter.all["insideSplitter"] : getElemenyById(splitter, 'insideSplitter');;
			splitter.style.pixelLeft = GetRealOffset(zoneTableCell, 'Left', topObject) - ((zoneTableCell.cellIndex == 0) ? 0 : 3);
			splitter.style.pixelLeft += rightOffset;
			splitter.style.pixelTop = GetRealOffset(zoneDragOver, 'Top', topObject) + 1;
			splitter.style.height = zoneDragOver.clientHeight;
		}
		else
		{
			splitter = vertZoneSplitter;
			insideSplitter = splitter.all ? splitter.all["insideSplitter"] : getElemenyById(splitter, 'insideSplitter');;
			splitter.style.pixelLeft = GetRealOffset(zoneDragOver, 'Left', topObject) + 1;
			splitter.style.pixelTop = GetRealOffset(zoneTableCell, 'Top', topObject) - ((GetParentRow(zoneTableCell).rowIndex == 0) ? 0 : 4);
			splitter.style.width = zoneDragOver.clientWidth;
		}
		
		if (zoneDragOver.className != zoneDragOver.selectedClassName)
			zoneDragOver.className = zoneDragOver.selectedClassName;
		splitter.style.display = 'inline';
	}
}

// Sets up the drop location.
function SetupDropLocation(zoneTableCell, checkSize)
{
	if (zoneTableCell.orientation == 'Vertical')
	{
		var parentRow = GetParentRow(zoneTableCell);
		if (!parentRow) return;
		if (checkSize && (event.clientY + topObject.scrollTop - GetRealOffset(zoneTableCell, 'Top')) > (zoneTableCell.offsetHeight / 2))
			rowToDrop = parentRow.rowIndex + 1;
		else 
			rowToDrop = parentRow.rowIndex;
	}
	else
	{
		var rtlPage = (document.dir == "rtl"), maxCells = zoneTableCell.parentNode.childNodes.length, nextCellIndex = zoneTableCell.cellIndex + 1;
		if (checkSize && (event.clientX + topObject.scrollLeft - GetRealOffset(zoneTableCell, 'Left')) > (zoneTableCell.offsetWidth / 2))
			cellToDrop = (rtlPage) ? zoneTableCell.cellIndex : zoneTableCell.cellIndex + 1;
		else 
		{
			if (rtlPage)	
				cellToDrop = (nextCellIndex >= maxCells) ? zoneTableCell.cellIndex : zoneTableCell.cellIndex + 1;
			else
				cellToDrop = zoneTableCell.cellIndex;
		}
	}
}

// Minimizes/restores a webpart.
function MinimizeRestore(webPartID, minimizeButtonID, minimizeToolTip, restoreToolTip, minimizeImageUrl, restoreImageUrl)
{
    var webPart = document.getElementById(webPartID);
    var minimizeButton = document.getElementById(minimizeButtonID);
	var newValue;
	var newValueIndex;
	if (webPart.style.display != 'none')
	{
		newValue = 'Minimized';
		newValueIndex = 1;
		webPart.style.display = 'none';
		minimizeButton.title = restoreToolTip;
		minimizeButton.src = restoreImageUrl;
	}
	else
	{
		newValue = 'Normal';
		newValueIndex = 0;
		webPart.style.display = '';
		minimizeButton.title = minimizeToolTip;
		minimizeButton.src = minimizeImageUrl;
	}
	AddChange(webPart, "frameState", newValueIndex)
}

// Removes a webpart.
function RemoveWebPart(webPartID, deleteConfirmMessage)
{
    var webPart = document.getElementById(webPartID);
    var relatedWebPart = document.getElementById(webPart.getAttribute('relatedWebPart'));
	if (!deleteConfirmMessage || deleteConfirmMessage.length == 0 || confirm(deleteConfirmMessage))
	{
		webPart.style.display = 'none';
		var originalZone = GetParentTable(webPart);
		var originalEmptyZoneText;

		// Get the empty webpart zones.
		var emptyZones = document.getElementsByTagName('span');
		for (var i = 0; i < emptyZones.length; i++)
		{
			if (emptyZones[i].getAttribute('name') == 'emptyWebPartZone' && GetParentTable(emptyZones[i]) == originalZone)
				originalEmptyZoneText = emptyZones[i];
		}

		if (originalEmptyZoneText)
		{
			var webPartsInZone = originalEmptyZoneText.getAttribute('webPartsInZone');
			originalEmptyZoneText.setAttribute('webPartsInZone', --webPartsInZone);
			if (webPartsInZone == 0)
			{
				originalEmptyZoneText.style.display = '';
			}
		}
		
		UpdatePartOrderAfterMove(originalZone, 0);
		AddChange(relatedWebPart, "isIncluded", "False")
	}
}

// Registers a change.
function AddChange(webPart, property, newValue)
{
	if (!webPart)
		return;
	var webPartGUID = webPart.getAttribute('WebPartID');
	if (webPart.webPartLayoutChanges)
	{
		var propertyIndex = SearchArray(webPart.webPartLayoutChanges, property);
		if (propertyIndex != -1)
		{
			webPart.webPartLayoutChanges[propertyIndex + 1] = newValue;
		}
		else
		{
			webPart.webPartLayoutChanges.push(property);
			webPart.webPartLayoutChanges.push(newValue);
		}
	}
	else
	{
		webPart.webPartLayoutChanges = new Array();
		webPart.webPartLayoutChanges.push(property);
		webPart.webPartLayoutChanges.push(newValue);
	}
	
	if (SearchArray(unsavedChanges, webPartGUID) == -1)
	{
		unsavedChanges.push(((unsavedChanges.length) ? "|" : "") + webPartGUID);
		unsavedChanges.push(webPart.webPartLayoutChanges);
	}
	
	document.forms[webPartPageFormName].webPartLayoutChanges.value = unsavedChanges;
}

// Searches the specified array for the specified value.
function SearchArray(searchArray, value)
{
	for (var index = 0; index < searchArray.length; index++)
	{
		if ((searchArray[index] == value) || (searchArray[index] == "#" + value))
			return index;
	}
	
	return -1;
}

// Gets an element by its id recursively, starting at target's child nodes.
function getElemenyById(target, elementID)
{
    var res;
    for (i = 0; i < target.childNodes.length; i++)
    {
        if (target.childNodes[i].id == elementID)
            return target.childNodes[i];
        else
        {
            res = getElemenyById(target.childNodes[i], elementID);
            if (res)
                return res;
        }
    }
    
    return res;
}