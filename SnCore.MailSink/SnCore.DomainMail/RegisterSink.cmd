%WINDIR%\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe SnCore.DomainMail.dll /codebase
cscript smtpreg.vbs /add 1 OnArrival "FoodCandy.DomainMail" FoodCandy.DomainMail.Sink  "rcpt to=*@foodcandy.com"
