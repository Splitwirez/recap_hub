@echo off

set ORIGINAL_CD=%CD%
set SCRIPT_DIR=%~dp0

cd "%SCRIPT_DIR%"
dotnet publish "%SCRIPT_DIR%/src/ReCap.Hub/ReCap.Hub.csproj" -r win7-x64 -c Release --self-contained true -p:PublishSingleFile=true -o "%SCRIPT_DIR%/publish/Windows"
