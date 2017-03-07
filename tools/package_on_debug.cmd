@echo off
setlocal
set CONFIGURATION=%1
set BUILD_FOLDER=%2
set VERSION=%3
if  "%CONFIGURATION%"=="Debug" (
    7z a %BUILD_FOLDER%\genmdl-%VERSION%-debug-with-pdb.zip %BUILD_FOLDER%\LICENSE %BUILD_FOLDER%\LICENSE-libyaml %BUILD_FOLDER%\LICENSE-YamlDotNet %BUILD_FOLDER%\src\ModelGenerator\bin\Debug\genmdl.exe %BUILD_FOLDER%\src\ModelGenerator\bin\Debug\genmdl.pdb
)
endlocal
