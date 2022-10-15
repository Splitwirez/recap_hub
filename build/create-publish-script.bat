@echo off



set OUT=temp-publish-script.sh.bat
echo "">%OUT%
Setlocal EnableDelayedExpansion
SET NL=&echo,
for /f "Tokens=* Delims=" %%x in (publish-script-template.sh.bat) do (
	SET line=%NL%%%x
	SET modifiedLine=!line:INSERT_PUBLISH_TARGET_HERE=%PUBLISH_TARGET%!
	ECHO !modifiedLine!>>%OUT%
)