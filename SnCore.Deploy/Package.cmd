@echo off
pushd "%~dp0"
setlocal

set PROJECTNAME=SnCore
set MOBILEPROJECTNAME=%PROJECTNAME%.mobile
set TARGET=%~dp0%PROJECTNAME%
set MOBILETARGET=%~dp0%MOBILEPROJECTNAME%

echo Creating %TARGET% ...
if EXIST "%TARGET%" ( rd /s/q "%TARGET%" )
echo Creating %MOBILETARGET% ...
if EXIST "%MOBILETARGET%" ( rd /s/q "%MOBILETARGET%" )

echo Copying Web
xcopy /S /I "%~dp0..\SnCore.Web.Deploy\Release\*.*" "%TARGET%"
del "%TARGET%\Web.config"

echo Copying MobileWeb
xcopy /S /I "%~dp0..\SnCore.MobileWeb.Deploy\Release\*.*" "%MOBILETARGET%"
del "%MOBILETARGET%\Web.config"

if EXIST %PROJECTNAME%.zip del %PROJECTNAME%.zip
bin\zip32 -r %PROJECTNAME%.zip %PROJECTNAME%\*.*
bin\zip32 -r %PROJECTNAME%.zip %MOBILEPROJECTNAME%\*.*

endlocal
popd
