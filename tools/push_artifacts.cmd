@echo off
setlocal
set CONFIGURATION=%1
set BUILD_FOLDER=%2
set VERSION=%3
set MDLGEN_PATH=%BUILD_FOLDER%\mdlgen_%VERSION%.zip
set MDLGEN_RELEASE_PATH=%BUILD_FOLDER%\mdlgen_%VERSION%_release.zip
set MDLGEN_DEBUG_PATH=%BUILD_FOLDER%\mdlgen_%VERSION%_debug.zip

if exists "%MDLGEN_PATH%" (
    appveyor PushArtifact %MDLGEN_PATH% -name "mdlgen"
)

if exists "%MDLGEN_RELEASE_PATH%" (
    appveyor PushArtifact %MDLGEN_RELEASE_PATH% -name "mdlgen with symbols"
)

if exists "%MDLGEN_DEBUG_PATH%" (
    appveyor PushArtifact %MDLGEN_DEBUG_PATH% -name "mdlgen with symbols - debug"
)
endlocal
