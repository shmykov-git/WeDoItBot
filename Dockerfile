FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
COPY Starter/bin/Release/netcoreapp2.2/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "Starter.dll"]