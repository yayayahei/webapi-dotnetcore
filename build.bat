@echo off
cd %~dp0
rd /s/q deploy
dotnet publish ./src/Hello/Hello.csproj -c Release -o ../../deploy 