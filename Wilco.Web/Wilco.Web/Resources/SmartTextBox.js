document.onkeyup = __smartTextBoxTextChanged;

var __smartTextBoxVal;
var __smartTextBoxLastVal;
var __smartTextBoxSelectedSuggestion = 0;
var __smartTextBoxSuggestions;
var __smartTextBoxHasFocus;
var __smartTextBoxCssClass;
var __smartTextBoxSelectedCssClass;

function __smartTextBoxCallback(result, context) {
    a = eval(result);
    r = document.getElementById(context);
    r.style.display = 'block';
    r.innerHTML = '';
    for (i = 0; i < a.length; i++)
    {
		var d = document.createElement('div');
		// What happens if the callback happens after a diff. smarttextbox was selected?
		if (i == 0)
			d.className = __smartTextBoxSelectedCssClass;
		else
			d.className = __smartTextBoxCssClass;
		d.onmousedown = function() { __smartTextBoxSelect(this); };
		d.onmouseover = function() { __smartTextBoxMouse(this, true); }
		d.onmouseout = function() { __smartTextBoxMouse(this, false); }
		d.innerHTML = a[i];
		r.appendChild(d);
    }
    
    __smartTextBoxUpdateSelection(0, false);
}

function __smartTextBoxMouse(el, b)
{
	if (el != __smartTextBoxSuggestions[__smartTextBoxSelectedSuggestion])
	{
		el.className = b ? __smartTextBoxSelectedCssClass : __smartTextBoxCssClass;
	}
}

function __smartTextBoxShouldRefresh()
{
	var sr = (__smartTextBoxLastVal.value != __smartTextBoxVal.value);
	__smartTextBoxLastVal.value = __smartTextBoxVal.value;
	return sr;
}

function __smartTextBoxTextChanged(e)
{
	if (__smartTextBoxHasFocus)
	{
		if (!e)
			e = event;

		if (e.keyCode == 38) // Up.
		{
			__smartTextBoxSelectPreviousSuggestion();
		}
		else if (e.keyCode == 40) // Down.
		{
			__smartTextBoxSelectNextSuggestion();
		}
	}
}

function __smartTextBoxSelectNextSuggestion()
{
	var newSelectedSuggestion;
	if (__smartTextBoxSelectedSuggestion < (__smartTextBoxSuggestions.length - 1))
	{
		newSelectedSuggestion = __smartTextBoxSelectedSuggestion+1;
		__smartTextBoxUpdateSelection(newSelectedSuggestion, true);
	}
}

function __smartTextBoxSelectPreviousSuggestion()
{
	var newSelectedSuggestion;
	if (__smartTextBoxSelectedSuggestion > 0)
	{
		newSelectedSuggestion = __smartTextBoxSelectedSuggestion-1;
		__smartTextBoxUpdateSelection(newSelectedSuggestion, true);
	}
}

function __smartTextBoxUpdateSelection(newSelectedSuggestion, updateInput)
{
	// Reset the current selection.
	__smartTextBoxSuggestions[__smartTextBoxSelectedSuggestion].className = __smartTextBoxCssClass;

	// Set the new selection.
	__smartTextBoxSelectedSuggestion = newSelectedSuggestion;
	var i = __smartTextBoxSuggestions[__smartTextBoxSelectedSuggestion];
	i.className = __smartTextBoxSelectedCssClass;

	if (updateInput)
	{
		__smartTextBoxVal.value = __smartTextBoxLastVal.value = i.innerHTML;
		var l = __smartTextBoxVal.value.length;

		if (__smartTextBoxVal.setSelectionRange)
		{
			__smartTextBoxVal.setSelectionRange(l, l);
		}
		else if (__smartTextBoxVal.createTextRange)
		{
			var tr = __smartTextBoxVal.createTextRange();
			tr.moveStart('character', l);
			tr.select();
		}
	}
}

function __smartTextBoxSelect(item)
{
	__smartTextBoxVal.value = item.innerHTML;
	__smartTextBoxVal.focus();
	
	// Simulate enter.
	if (document.createEventObject)
	{
		var eo = document.createEventObject();
		eo.keyCode = 13;
		__smartTextBoxVal.fireEvent("onkeypress", eo);
	}
}

function __smartTextBoxUpdateFocus(b, val, lastVal, results, css1, css2)
{
	// Reset the selection of the currently shown results.
	if (!b && __smartTextBoxSuggestions && __smartTextBoxSuggestions.length > 0)
		__smartTextBoxSuggestions[__smartTextBoxSelectedSuggestion].className = __smartTextBoxCssClass;

	__smartTextBoxVal = document.getElementById(val);
	__smartTextBoxLastVal = document.getElementById(lastVal);
	var r = document.getElementById(results);
	
	if (!b)
	{
		r.style.display = 'none';
		r.innerHTML = '';
	}
	
	var topOffset = 16;
	if (__smartTextBoxVal.rows)
		topOffset *= __smartTextBoxVal.rows;
	
	var width;
	if (__smartTextBoxVal.width)
		width = __smartTextBoxVal.width;
	else if (__smartTextBoxVal.style.width)
		width = __smartTextBoxVal.stylewidth;
	
	r.style.top = __smartTextBoxGetRealTop(__smartTextBoxVal) + topOffset + 6;
	r.style.left = __smartTextBoxGetRealLeft(__smartTextBoxVal);
	if (width)
		r.style.width = width + 'px';
	
	__smartTextBoxSuggestions = r.getElementsByTagName('div');
	__smartTextBoxSelectedSuggestion = 0;
	__smartTextBoxCssClass = css1;
	__smartTextBoxSelectedCssClass = css2;

	__smartTextBoxHasFocus = b;
}

function __smartTextBoxGetRealTop(el) {
	yPos = el.offsetTop;
	tempEl = el.offsetParent;
	while (tempEl != null)
	{
		yPos += tempEl.offsetTop;
		tempEl = tempEl.offsetParent;
	}
	return yPos;
}

function __smartTextBoxGetRealLeft(el) {
	xPos = el.offsetLeft;
	tempEl = el.offsetParent;
	while (tempEl != null)
	{
		xPos += tempEl.offsetLeft;
		tempEl = tempEl.offsetParent;
	}
	return xPos;
}