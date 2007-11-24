%WINDIR%\Microsoft.NET\Framework\v2.0.50727\RegAsm.exe SnCore.DomainMail.dll /codebase
cscript smtpreg.vbs /add 1 OnArrival "SnCore.DomainMail" SnCore.DomainMail.Sink  "rcpt to=*@sncore.com"
