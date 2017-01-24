@echo off
setlocal
set CONFIGURATION=%1
set BUILD_FOLDER=%2
if  "%CONFIGURATION%"=="Debug" (
    7z a %BUILD_FOLDER%\mdlgen_debug.zip %BUILD_FOLDER%\LICENSE %BUILD_FOLDER%\LICENSE-libyaml %BUILD_FOLDER%\LICENSE-YamlDotNet %BUILD_FOLDER%\src\ModelGenerator\bin\Debug\mdlgen.exe %BUILD_FOLDER%\src\ModelGenerator\bin\Debug\mdlgen.pdb
)
endlocal
