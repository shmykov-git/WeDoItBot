cd d:\Projects\C#\WeDoItBot
dotnet publish -c Release
cd d:\Projects\devops\_Up_bots

docker-compose down
docker rmi up_bots_we-do-it
rem docker rmi bots_prediction
docker-compose up

rem View->Terminal
rem for PS: cmd /c deploy