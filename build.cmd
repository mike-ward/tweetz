pushd src
nuget restore
msbuild tweetz.core.sln -t:Rebuild -p:Configuration=Release /verbosity:minimal

popd