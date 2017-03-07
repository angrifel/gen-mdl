@echo off
setlocal
set BUILD_FOLDER=%1
set VERSION=%2
set GENMDL_PATH=%BUILD_FOLDER%\genmdl-%VERSION%-release.zip
set GENMDL_RELEASE_PATH=%BUILD_FOLDER%\genmdl-%VERSION%-release-with-pdb.zip
set GENMDL_DEBUG_PATH=%BUILD_FOLDER%\genmdl-%VERSION%-debug-with-pdb.zip

if exist "%GENMDL_PATH%" (
    appveyor PushArtifact %GENMDL_PATH% -DeploymentName "genmdl release"
)

if exist "%GENMDL_RELEASE_PATH%" (
    appveyor PushArtifact %GENMDL_RELEASE_PATH% -DeploymentName "genmdl release with pdb"
)

if exist "%GENMDL_DEBUG_PATH%" (
    appveyor PushArtifact %GENMDL_DEBUG_PATH% -DeploymentName "genmdl debug with pdb"
)
endlocal
