@echo off

SET publish_target=%1
if ["%1"]==[""] (
	SET publish_target="Windows"
)

SET return_dir=%cd%

SET repo_dir=%~dp0
cd %repo_dir%




cd build
SET PUBLISH_TARGET=%publish_target%
call create-publish-script.bat

cd %repo_dir%

call build\temp-publish-script.sh.bat
PAUSE