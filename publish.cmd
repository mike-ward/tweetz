@echo off
del /f/s/q bin > nul && rmdir /s/q bin
if EXIST bin goto ERROR
pushd src\tweetz.core
 
echo(
echo --- Build framework-dependent ---
echo(
dotnet publish -f net5.0-windows -r win10-x86 -c Release --self-contained false --output ../../bin/framework-dependent
if ERRORLEVEL 1 goto ERROR

echo(
@echo --- Build self-contained ---
echo(
dotnet publish -f net5.0-windows -r win10-x86 -c Release --self-contained true --output ../../bin/self-contained
if ERRORLEVEL 1 goto ERROR

popd
echo(
echo --- Build installer ---
iscc /Q tweetz.core.setup.iss 
if ERRORLEVEL 1 goto ERROR

pushd bin

echo(
echo(
echo --- Build zip archives ---
7z a -bso0 -bsp0 -r dist/tweetz-framework-dependent.zip ./framework-dependent/*
if ERRORLEVEL 1 goto ERROR

7z a -bso0 -bsp0 -r dist/tweetz-self-contained.zip ./self-contained/*
if ERRORLEVEL 1 goto ERROR

popd
goto END

:ERROR
popd
echo Oooooops...
:END