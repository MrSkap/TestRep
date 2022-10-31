FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

#WORKDIR /WebApplication

COPY WebApplication/WebApplication.csproj /WebApplication/WebApplication.csproj
WORKDIR WebApplication
RUN dotnet restore WebApplication.csproj
COPY . .
COPY Services .
COPY ServiceEntities .


RUN dotnet publish WebApplication.csproj -c release -o out

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /WebApplication
COPY --from=build /WebApplication/out .
ENTRYPOINT ["dotnet", "WebApplication.dll"]
