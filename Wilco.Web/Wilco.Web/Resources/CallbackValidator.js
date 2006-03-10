function __callbackValidatorCallback(result, context)
{
    var val = document.getElementById(context);
    var i = result.indexOf('|');
    val.callbackIsValid = (result.substring(0, i) == 'True');
    val.errormessage = val.innerHTML = result.substring(i+1, result.length);
    ValidatorValidate(val);
}

function __callbackValidatorValidate(val, args)
{
    if (val.previousValue == 'undefined' || val.previousValue != args.Value)
    {
        val.previousValue = args.Value;
        eval(val.callbackfunction);
        args.IsValid = true;
    }
    else
    {
        args.IsValid = val.callbackIsValid;
    }
}