@echo off
setlocal
set SnCoreProjectName=SnCore
set BUILD_TARGET=%~1
set BUILD_CONFIGURATION=%~2
set BUILD_SVNDIR=%ProgramFiles%\svn
if "%BUILD_TARGET%"=="" set BUILD_TARGET=all
if "%BUILD_CONFIGURATION%"=="" set BUILD_CONFIGURATION=Debug
if "%FrameworkVersion%"=="" set FrameworkVersion=v2.0.50727
if "%FrameworkDir%"=="" set FrameworkDir=%SystemRoot%\Microsoft.NET\Framework
echo Framework: %FrameworkDir%
PATH=%PATH%;%FrameworkDir%\%FrameworkVersion%;%ProgramFiles%\Microsoft Visual Studio 8\VC\BIN;%ProgramFiles%\Microsoft Visual Studio 8\Common7\Tools;%ProgramFiles%\NUnit 2.4.1\bin
echo %PATH%
msbuild.exe SnCore.proj /t:%BUILD_TARGET% /p:"Configuration=%BUILD_CONFIGURATION%"
endlocal
