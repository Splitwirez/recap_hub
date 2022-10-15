#!/bin/sh
@echo off

cd src/ReCap.Hub
dotnet clean
dotnet build --no-incremental -c Release -p:PublishTarget=INSERT_PUBLISH_TARGET_HERE
dotnet publish --no-build -c Release -p:PublishTarget=INSERT_PUBLISH_TARGET_HERE -o ../../publish/demo/INSERT_PUBLISH_TARGET_HERE
cd ../..