@echo off
cd %~dp0
setlocal
rd /s/q deploy
dotnet publish ./Hello/Hello.csproj -c Release -o ../deploy 
endlocal