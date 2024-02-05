# IO.Swagger - ASP.NET Core 2.0 Server
Calculation API Service
=======================

## Run in Docker
In order to execute IO.Swagger service in docker two files need to be configured:

docker-compose.yml 
------------------
contains the reference to DockerFile, i.e. dockerfile: src\IO.Swagger\Dockerfile

Dockerfile
----------
- resides at the same location as the IO.Swagger project
- defines which dotnet sdk image to be used for docker. i.e. mcr.microsoft.com/dotnet/sdk:7.0 (8.0 tag does not exist for Windows yet)
- important to note that the current path is the docker-compose.yml path, so copying all the required project files are relative to it

to pull the appropriate dotnet docker image run:
	docker pull mcr.microsoft.com/dotnet/sdk:7.0
	
Run the following commands
--------------------------
cd src/IO.Swagger
docker build -t io.swagger .
docker run -p 5000:5000 io.swagger

## Postman collection

Calc.postman_collection.json attached with all the test cases including teh negative ones
workspace.postman_globals.json - please configure base_url1, username, password of your own

## CalculationNUnitTest
Contains the same test cases as in Postman

## appsettings.json
All the arguments below can be configured as well

"JwtSettings": {
  "SecretKey": "This is my secret key for calculation API which should be at least 512 bit long",
  "Issuer": "MyIssuer",
  "Audience": "MyAudience",
  "ExpirationInMinutes":  60
}




