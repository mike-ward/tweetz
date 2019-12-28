echo off
del /f/s/q bin > nul && rmdir /s/q bin
if EXIST bin goto ERROR
pushd src\tweetz.core

dotnet publish -f netcoreapp3.1 -r win10-x86 -c Release --self-contained false --output ../../bin/framework-dependent
if ERRORLEVEL 1 goto ERROR

dotnet publish -f netcoreapp3.1 -r win10-x86 -c Release --self-contained true /p:PublishTrimmed=true --output ../../bin/self-contained
if ERRORLEVEL 1 goto ERROR

popd

iscc tweetz.core.setup.iss
if ERRORLEVEL 1 goto ERROR

pushd bin

7z a -r tweetz-framework-dependent.zip ./framework-dependent/*
if ERRORLEVEL 1 goto ERROR

7z a -r tweetz-self-contained.zip ./self-contained/*
if ERRORLEVEL 1 goto ERROR

popd
goto END

:ERROR
popd
echo Oooooops...
:END