#!/bin/sh
@echo off

SCRIPT_DIR=${0%/*}


cd "$SCRIPT_DIR/src/ReCap.Hub"

dotnet clean
dotnet build --no-incremental -c Release -p:PublishTarget=Windows
dotnet publish --no-build -c Release -p:PublishTarget=Windows -o ../../publish/demo/Windows

cd "$SCRIPT_DIR/publish/demo/Windows"
mv ResurrectionCapsule.Hub ResurrectionCapsule.Hub.exe
cd "$SCRIPT_DIR"