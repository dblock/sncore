Type.registerNamespace('SnCore.WebControls');

SnCore.WebControls.DirtyPanelExtenderBehavior = function(element) {
    SnCore.WebControls.DirtyPanelExtenderBehavior.initializeBase(this, [element]);
    this._OnLeaveMessageValue = null;
}

SnCore.WebControls.DirtyObject = function(type, control, newvalue, oldvalue) {
    this.type = type;
    this.control = control;
    this.oldvalue = oldvalue;
    this.newvalue = newvalue;
}

SnCore.WebControls.DirtyPanelExtenderBehavior.prototype = {

    isDirty: function() {
        var dirty;
        dirty = this.getDirty();
        return dirty.length > 0;
    },

    getDirty: function() {
        var result = new Array()
        var values_control = document.getElementById(this.get_element().id + "_Values");
        var values = values_control["value"].split(",");
        for (i in values) {
            var namevalue = values[i];
            var namevaluepair = namevalue.split(":");
            var name = namevaluepair[0];
            var value = (namevaluepair.length > 1 ? namevaluepair[1] : "");
            var control = document.getElementById(name);
            if (control == null) continue;
            // alert(control.id + " -> " + control.type);
            if (control.type == 'checkbox' || control.type == 'radio') {
                var boolvalue = (value == "true" ? true : false);
                if (control.checked != boolvalue) {
                    result[result.length] = new SnCore.WebControls.DirtyObject("checkbox", control, control.checked, boolvalue);
                }
            } else if (control.type == 'select-one' || control.type == 'select-multiple') {
                if (namevaluepair.length > 2) {
                    if (control.options.length > 0) {
                        // control is listbox
                        // there's data:value and selection:value
                        var code = value;
                        value = (namevaluepair.length > 2 ? namevaluepair[2] : "");
                        var optionValues = "";
                        // concat all listbox items
                        for (var cnt = 0; cnt < control.options.length; cnt++) {
                            if (code == 'data') {
                                optionValues += control.options[cnt].text;
                            } else if (code == 'selection') {
                                optionValues += control.options[cnt].selected;
                            }
                            optionValues += "\r\n";
                        }
                        if (encodeURIComponent(optionValues) != value) {
                            // items in the listbox have changed
                            result[result.length] = new SnCore.WebControls.DirtyObject("listbox", control, encodeURIComponent(optionValues), value);
                        }
                    }
                } else if (control.selectedIndex != value) {
                    result[result.length] = new SnCore.WebControls.DirtyObject("dropdown", control, control.selectedIndex, value);
                }
            } else if (control.className == 'ajax__htmleditor_editor_base ajax__htmleditor_editor_default') {
                var controlValue = $find(name).get_content();
                if (controlValue != value) {
                    result[result.length] = new SnCore.WebControls.DirtyObject("html editor", control, control.value, value);
                }
            } else {
                var controlValue = (control.value == null ? "" : encodeURIComponent(control.value));
                // Replace any CRLF with LF only to avoid multi-line values being mistakenly flagged as changed.
                var regex = /%0D%0A/  // CRLF
                controlValue = controlValue.replace(regex, "%0A");
                value = value.replace(regex, "%0A");
                if (controlValue != value) {
                    result[result.length] = new SnCore.WebControls.DirtyObject(control.type, control, control.value, value);
                }
            }
        }
        return result;
    },

    initialize: function() {
        SnCore.WebControls.DirtyPanelExtenderBehavior.callBaseMethod(this, 'initialize');
        DirtyPanelExtender_dirtypanels[DirtyPanelExtender_dirtypanels.length] = this;
    },

    dispose: function() {
        SnCore.WebControls.DirtyPanelExtenderBehavior.callBaseMethod(this, 'dispose');
    },

    get_OnLeaveMessage: function() {
        return this._OnLeaveMessageValue;
    },

    set_OnLeaveMessage: function(value) {
        this._OnLeaveMessageValue = value;
    },

    toString: function() {
        var result = this.get_element().id + ":";
        var dirty = this.getDirty();
        if (dirty.length == 0) {
            result = result + "clean";
        } else {
            for (i in dirty) {
                result = result + "\n" + dirty[i].control.id + " (" + dirty[i].type + ") - ["
                + dirty[i].oldvalue + "][" + dirty[i].newvalue + "]";
            }
        }
        return result;
    }
}

SnCore.WebControls.DirtyPanelExtenderBehavior.registerClass('SnCore.WebControls.DirtyPanelExtenderBehavior', AjaxControlToolkit.BehaviorBase);

var DirtyPanelExtender_dirtypanels = new Array()

function DirtyPanelExtender_SuppressDirtyCheck() {
    window.onbeforeunload = null;
}

function WebForm_OnSubmit() {
    if (theForm != null && ! theForm.onsubmit) {
        return theForm.onsubmit();
    }
}

function __newDoPostBack(eventTarget, eventArgument) {
    // supress prompting on postback
    DirtyPanelExtender_SuppressDirtyCheck();
    return __savedDoPostBack(eventTarget, eventArgument);
}

var __savedDoPostBack = __doPostBack;
__doPostBack = __newDoPostBack;

window.onbeforeunload = function(eventargs) {
    WebForm_OnSubmit();
    for (i in DirtyPanelExtender_dirtypanels) {
        var panel = DirtyPanelExtender_dirtypanels[i];
        if (panel.isDirty()) {
            if (!eventargs) eventargs = window.event;
            eventargs.returnValue = panel.get_OnLeaveMessage();
            break;
        }
    }
}
