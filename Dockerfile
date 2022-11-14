FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY WebApplication/WebApplication.csproj /project/WebApplication/WebApplication.csproj
WORKDIR project
COPY Services/Services.csproj Services/Services.csproj
COPY ServiceEntities/ServiceEntities.csproj ServiceEntities/ServiceEntities.csproj

RUN dotnet restore WebApplication/WebApplication.csproj
RUN dotnet restore ServiceEntities/ServiceEntities.csproj
RUN dotnet restore Services/Services.csproj
COPY . .

RUN dotnet publish WebApplication/WebApplication.csproj -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "WebApplication.dll"]


FROM node:latest AS build
WORKDIR /usr/src/app
COPY ./ClientApp/client-app .
RUN npm install
RUN npm run build --prod

FROM nginx:latest
COPY --from=build /usr/src/app/dist/client-app /usr/share/nginx/html