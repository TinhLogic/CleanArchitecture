
#!/bin/bash
#########################################
# Packaging CleanArchitecture module, and push to NUGET
# Usage: ./build-nuget.sh [API_KEY]
#########################################
ROOT_PATH=..
BUILD_TARGET_PATH=.sln
NUGET=https://api.nuget.org/v3/index.json
BUILD_CONFIG=Release
ROOT_NAMESPACE=CleanArchitecture

# Get API Key from parameter or prompt
if [ -n "$1" ]; then
    NUGET_API_KEY=$1
    echo "Using API key from parameter"
else
    read -p "Enter NuGet API Key: " NUGET_API_KEY
    if [ -z "$NUGET_API_KEY" ]; then
        echo "ERROR: API Key is required!"
        echo "Get your API key from: https://www.nuget.org/account/apikeys"
        exit 1
    fi
fi

read -p "Enter Version ${ROOT_NAMESPACE}: " P_VERSION
if [[ -z $P_VERSION ]]
then
	VERSION=$(echo '111 222 33' | awk -F'[<>]' '/<Version>/{print $3}' common.props)
else
	VERSION=$P_VERSION
fi

echo "VERSION ${VERSION}"
dotnet pack ./src/${ROOT_NAMESPACE}.Api -c:Release -p:TargetFramework=net10.0 -p:PackageVersion=$VERSION
dotnet pack ./src/${ROOT_NAMESPACE}.BO -c:Release -p:TargetFramework=net10.0 -p:PackageVersion=$VERSION
dotnet pack ./src/${ROOT_NAMESPACE}.DTOs -c:Release -p:TargetFramework=net10.0 -p:PackageVersion=$VERSION
dotnet pack ./src/${ROOT_NAMESPACE}.Entities -c:Release -p:TargetFramework=net10.0 -p:PackageVersion=$VERSION
dotnet pack ./src/${ROOT_NAMESPACE}.EntityFrameworkCore -c:Release -p:TargetFramework=net10.0 -p:PackageVersion=$VERSION
# Change directory to code-base root path
#cd ${ROOT_PATH}
# Set privileges __________________________
# chmod 777
# Push _____________________________________________________
echo -e "> Push to Nuget..."
dotnet nuget push ./src/${ROOT_NAMESPACE}.Api/bin/${BUILD_CONFIG}/${ROOT_NAMESPACE}.Api.${VERSION}.nupkg --source ${NUGET} --api-key ${NUGET_API_KEY}
dotnet nuget push ./src/${ROOT_NAMESPACE}.BO/bin/${BUILD_CONFIG}/${ROOT_NAMESPACE}.BO.${VERSION}.nupkg --source ${NUGET} --api-key ${NUGET_API_KEY}
dotnet nuget push ./src/${ROOT_NAMESPACE}.DTOs/bin/${BUILD_CONFIG}/${ROOT_NAMESPACE}.DTOs.${VERSION}.nupkg --source ${NUGET} --api-key ${NUGET_API_KEY}
dotnet nuget push ./src/${ROOT_NAMESPACE}.Entities/bin/${BUILD_CONFIG}/${ROOT_NAMESPACE}.Entities.${VERSION}.nupkg --source ${NUGET} --api-key ${NUGET_API_KEY}
dotnet nuget push ./src/${ROOT_NAMESPACE}.EntityFrameworkCore/bin/${BUILD_CONFIG}/${ROOT_NAMESPACE}.EntityFrameworkCore.${VERSION}.nupkg --source ${NUGET} --api-key ${NUGET_API_KEY}


read -p "Press [Enter] key to exit..." 