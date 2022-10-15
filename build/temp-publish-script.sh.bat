""
#/bin/sh
@echo off
cd src/ReCap.Hub
dotnet clean
dotnet build --no-incremental -c Release -p:PublishTarget="Windows"
dotnet publish --no-build -c Release -p:PublishTarget="Windows" -o ../../publish/demo/"Windows"
cd ../..
