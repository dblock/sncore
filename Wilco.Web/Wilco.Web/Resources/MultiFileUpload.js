function __multiFileUploadAddFile(containerID, counterID, containerCssClass, inputCssClass)
{
    var container = document.getElementById(containerID);
    var counter = document.getElementById(counterID);
    if (counter.value.length == 0)
        counter.value = 1;
    var fileCount = parseInt(counter.value);
    counter.value = fileCount + 1;
    var id = container.id + fileCount;
    var inputContainer = document.createElement('inputContainer');
    inputContainer.className = containerCssClass;
    var file = document.createElement('input');
    file.type = 'file';
    file.id = file.name = id;
    file.className = inputCssClass;
    inputContainer.appendChild(file);
    var br = document.createElement('br');
    inputContainer.appendChild(br);
    container.appendChild(inputContainer);
}