@echo off
setlocal
set BUILD_FOLDER=%1
set VERSION=%2
set GEN_MDL_PATH=%BUILD_FOLDER%\gen-mdl-%VERSION%-release.zip
set GEN_MDL_RELEASE_PATH=%BUILD_FOLDER%\gen-mdl-%VERSION%-release-with-pdb.zip
set GEN_MDL_DEBUG_PATH=%BUILD_FOLDER%\gen-mdl-%VERSION%-debug-with-pdb.zip

if exist "%GEN_MDL_PATH%" (
    appveyor PushArtifact %GENMDL_PATH% -DeploymentName "gen-mdl release"
)

if exist "%GEN_MDL_RELEASE_PATH%" (
    appveyor PushArtifact %GENMDL_RELEASE_PATH% -DeploymentName "gen-mdl release with pdb"
)

if exist "%GEN_MDL_DEBUG_PATH%" (
    appveyor PushArtifact %GENMDL_DEBUG_PATH% -DeploymentName "gen-mdl debug with pdb"
)
endlocal
