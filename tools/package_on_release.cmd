@echo off
setlocal
set CONFIGURATION=%1
set BUILD_FOLDER=%2
set VERSION=%3
if  "%CONFIGURATION%"=="Release" (
    7z a %BUILD_FOLDER%\mdlgen-%VERSION%.zip %BUILD_FOLDER%\LICENSE %BUILD_FOLDER%\LICENSE-libyaml %BUILD_FOLDER%\LICENSE-YamlDotNet %BUILD_FOLDER%\src\ModelGenerator\bin\Release\mdlgen.exe
    7z a %BUILD_FOLDER%\mdlgen-%VERSION%-release-with-pdb.zip %BUILD_FOLDER%\LICENSE %BUILD_FOLDER%\LICENSE-libyaml %BUILD_FOLDER%\LICENSE-YamlDotNet %BUILD_FOLDER%\src\ModelGenerator\bin\Release\mdlgen.exe %BUILD_FOLDER%\src\ModelGenerator\bin\Release\mdlgen.pdb
)
endlocal
