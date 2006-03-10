var __progressBarInstances;
var __progressBarQueuedItems;

function __progressBarInit(instances)
{
    __progressBarInstances = instances;
    __progressBarQueuedItems = 0;
}

function __progressBarCallback(result, context) {
    var el = document.getElementById(context);
    var i = parseInt(result);
    if (i >= 0 && i <= 100)
        el.style.width = result + '%';
    else if (i < 0)
        el.style.width = '100%';
    
    if (result < 100)
    {
        // Get new progress.
        var inst = __progressBarGetInstanceById(context);
        
        inst.timer = setTimeout('__progressBarUpdate(\'' + context + '\');', inst.refreshInterval);
    }
}

function __progressBarUpdate(id)
{
    var inst = __progressBarGetInstanceById(id);

    if (inst.timer)
        clearTimeout(inst.timer);
    
    __progressBarQueuedItems--;
    
    // Get progress from server.
    eval(inst.callbackFunction);
}

function __progressBarEnqueue(id)
{
    var inst = __progressBarGetInstanceById(id);
    
    __progressBarQueuedItems++;
    
    // Schedule it.
    inst.timer = setTimeout('__progressBarUpdate(\'' + inst.id + '\');', __progressBarQueuedItems * 100);
}

function __progressBarGetInstanceById(id)
{
    for (i = 0; i < __progressBarInstances.length; i++)
    {
        if (__progressBarInstances[i].id == id)
        {
            return __progressBarInstances[i];
        }
    }
    
    return null;
}

function __progressBarInstance(id, callbackFunction, refreshInterval)
{
    this.id = id;
    this.callbackFunction = callbackFunction;
    this.refreshInterval = refreshInterval;
}