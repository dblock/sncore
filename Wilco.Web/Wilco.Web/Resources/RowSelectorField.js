function __rowSelectorField_register(item) {
  var owner = document.getElementById(item.o);
  var child = document.getElementById(item.c);
  if (!owner._rowSelectorChildren) {
    owner._rowSelectorChildren = [];
    owner.checked = true;
  }
  
  if (!child.checked)
    owner.checked = false;
  owner._rowSelectorChildren.push(child);
}

function __rowSelectorField_checkedChanged(ownerID) {
  var owner = document.getElementById(ownerID);
  
  var allChecked = true;
  for (var i = 0; i < owner._rowSelectorChildren.length; i++) {
    if (!owner._rowSelectorChildren[i].checked) {
      allChecked = false;
      break;
    }
  }
  
  owner.checked = allChecked;
}

function __rowSelectorField_selectAll(owner) {
  if (!owner._rowSelectorChildren)
    return;

  for (var i = 0; i < owner._rowSelectorChildren.length; i++) {
    owner._rowSelectorChildren[i].checked = owner.checked;
  }
}