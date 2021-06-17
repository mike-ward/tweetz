pushd src
nuget restore
msbuild tweetz.core.sln -t:Rebuild -p:Configuration=Release
popd