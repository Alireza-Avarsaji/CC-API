# Build image
ARG REPO=mcr.microsoft.com/dotnet/sdk
ARG REPORUN=mcr.microsoft.com/dotnet/aspnet
ARG POJECT_NAME=CC-API
ARG AppName=${POJECT_NAME}.dll

FROM $REPO:5.0-alpine AS build

WORKDIR /src

#RUN ./builder.sh
COPY CC_Proj1_Alireza.csproj .
RUN dotnet restore
COPY ./ ./

RUN dotnet publish "./CC_Proj1_Alireza.csproj" -c Release -o "/src/dist" --no-restore

# app as iamge
FROM $REPORUN:5.0-alpine 

	
WORKDIR /app
COPY --from=build /src/dist ./

# Fix Globalization Error
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
ENTRYPOINT ["dotnet", "CC_Proj1_Alireza.dll"]

