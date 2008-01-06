@echo off
pushd "%~dp0"
setlocal

set CONFIG=%~1
if "%CONFIG%"=="" set CONFIG=Release
set PROJECTNAME=SnCore
set MOBILEPROJECTNAME=%PROJECTNAME%.mobile
set ROOT=Target\%CONFIG%
if EXIST "%ROOT%" ( rd /s/q "%ROOT%" )
echo Creating %ROOT% ...
mkdir %ROOT%
set TARGET=%~dp0%ROOT%\%PROJECTNAME%
set MOBILETARGET=%~dp0%ROOT%\%MOBILEPROJECTNAME%

echo Creating %TARGET% ...
mkdir %TARGET%
echo Creating %MOBILETARGET% ...
mkdir %MOBILETARGET%

echo Copying Web
xcopy /EXCLUDE:XCopy.exclude /S /I "%~dp0..\SnCore.Web.Deploy\%CONFIG%\*.*" "%TARGET%"

echo Copying MobileWeb
xcopy /EXCLUDE:XCopy.exclude /S /I "%~dp0..\SnCore.MobileWeb.Deploy\%CONFIG%\*.*" "%MOBILETARGET%"

if EXIST %PROJECTNAME%.zip del %PROJECTNAME%.zip
pushd %ROOT%
..\..\bin\zip32 -r ..\..\%PROJECTNAME%.zip %PROJECTNAME%\*.*
..\..\bin\zip32 -r ..\..\%PROJECTNAME%.zip %MOBILEPROJECTNAME%\*.*
popd

endlocal
popd
