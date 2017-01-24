@echo off
setlocal
set BUILD_FOLDER=%1
set VERSION=%2
set MDLGEN_PATH=%BUILD_FOLDER%\mdlgen_%VERSION%.zip
set MDLGEN_RELEASE_PATH=%BUILD_FOLDER%\mdlgen_%VERSION%_release.zip
set MDLGEN_DEBUG_PATH=%BUILD_FOLDER%\mdlgen_%VERSION%_debug.zip

if exist "%MDLGEN_PATH%" (
    appveyor PushArtifact %MDLGEN_PATH% -DeploymentName "mdlgen"
)

if exist "%MDLGEN_RELEASE_PATH%" (
    appveyor PushArtifact %MDLGEN_RELEASE_PATH% -DeploymentName "mdlgen with symbols"
)

if exist "%MDLGEN_DEBUG_PATH%" (
    appveyor PushArtifact %MDLGEN_DEBUG_PATH% -DeploymentName "mdlgen with symbols - debug"
)
endlocal
