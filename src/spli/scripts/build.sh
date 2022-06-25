type=$1

eval "dotnet publish -c Release -o ../../publish/$type -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true -p:RuntimeIdentifier=$type"
cd "../../publish/$type"
rm *.pdb