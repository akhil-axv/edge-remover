@echo off
setlocal
cd /d "%~dp0"
where dotnet >nul 2>nul
if errorlevel 1 (
  echo .NET 8 SDK is required. Install it from https://dotnet.microsoft.com/download/dotnet/8.0
  exit /b 1
)
dotnet publish EdgeRemover.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
if errorlevel 1 exit /b %errorlevel%
echo.
echo Built: bin\Release\net8.0-windows\win-x64\publish\EdgeRemover.exe
