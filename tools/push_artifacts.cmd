@echo off
setlocal
set BUILD_FOLDER=%1
set VERSION=%2
set MDLGEN_PATH=%BUILD_FOLDER%\mdlgen-%VERSION%.zip
set MDLGEN_RELEASE_PATH=%BUILD_FOLDER%\mdlgen-%VERSION%-release-with-pdb.zip
set MDLGEN_DEBUG_PATH=%BUILD_FOLDER%\mdlgen-%VERSION%-debug-with-pdb.zip

if exist "%MDLGEN_PATH%" (
    appveyor PushArtifact %MDLGEN_PATH% -DeploymentName "mdlgen"
)

if exist "%MDLGEN_RELEASE_PATH%" (
    appveyor PushArtifact %MDLGEN_RELEASE_PATH% -DeploymentName "mdlgen release with pdb"
)

if exist "%MDLGEN_DEBUG_PATH%" (
    appveyor PushArtifact %MDLGEN_DEBUG_PATH% -DeploymentName "mdlgen debug with pdb"
)
endlocal
