#!/bin/sh

ORIGINAL_CD="`realpath $PWD`"
SCRIPT_DIR="`realpath ${0%/*}`"

cd "$SCRIPT_DIR"
dotnet run --project "$SCRIPT_DIR/build/ReCap.Builder" -- "$@"
cd "$ORIGINAL_CD"