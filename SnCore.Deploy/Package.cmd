@echo off
pushd "%~dp0"
setlocal

set PROJECTNAME=Savory
set TARGET=%~dp0%PROJECTNAME%
echo Creating %TARGET% ...
if EXIST "%TARGET%" ( rd /s/q "%TARGET%" )

echo Copying Web
xcopy /S /I "%~dp0..\SnCore.Web.Deploy\Release\*.*" %TARGET%

if EXIST %PROJECTNAME%.zip del %PROJECTNAME%.zip
bin\zip32 -r %PROJECTNAME%.zip %PROJECTNAME%\*.*

endlocal
popd
