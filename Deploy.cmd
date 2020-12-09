cd C:\Projects\WeDoItBot
dotnet publish -c Release
cd C:\Projects\_Up\bots

docker-compose down
docker rmi bots_we-do-it
docker-compose up