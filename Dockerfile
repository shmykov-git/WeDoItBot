FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
COPY Starter/bin/Release/net5.0/publish/ App/
COPY Starter/bin/Release/net5.0/publish/settings.deploy.json App/settings.json
WORKDIR /App
ENTRYPOINT ["dotnet", "Starter.dll"]