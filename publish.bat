@echo off

set ORIGINAL_CD=%CD%
set SCRIPT_DIR=%~dp0

cd "%SCRIPT_DIR%"
dotnet run --project "%SCRIPT_DIR%\build\ReCap.Builder\ReCap.Builder.csproj" -- %*
cd "%ORIGINAL_CD%"