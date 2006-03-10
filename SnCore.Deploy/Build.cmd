@echo off

pushd "%~dp0"
cd ..

echo Compiling ...
MSBuild SnCore.sln /t:Rebuild /p:Configuration=Release

popd