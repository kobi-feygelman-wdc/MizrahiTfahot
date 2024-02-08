# CalculationAPI - ASP.NET Core 2.0 Server
Calculation API Service
=======================

## Run in Docker
In order to execute CalculationAPI service in docker two files need to be configured:

docker-compose.yml 
------------------
contains the reference to DockerFile, i.e. dockerfile: src\CalculationAPI\Dockerfile

Dockerfile
----------
- resides at the same location as the CalculationAPI project
- defines which dotnet sdk image to be used for docker. i.e. mcr.microsoft.com/dotnet/sdk:7.0 (8.0 tag does not exist for Windows yet)
- important to note that the current path is the docker-compose.yml path, so copying all the required project files are relative to it

to pull the appropriate dotnet docker image run:
	docker pull mcr.microsoft.com/dotnet/sdk:7.0
	
Run the following commands
--------------------------
Just execute 'docker' configuration from VS

or run the following commands

docker build -t CalculationAPI .
docker run -p 5000:5000 CalculationAPI

## Postman collection

Calc.postman_collection.json attached with all the test cases including teh negative ones
workspace.postman_globals.json - please configure base_url1, username, password of your own

## CalculationNUnitTest
Contains the same test cases as in Postman

Note: at the moment a single user can exist in application, that requires Register action (username+password) and then Login, afterwards all the api calls can be made

## appsettings.json
All the arguments below can be configured as well

"JwtSettings": {
  "SecretKey": "This is my secret key for calculation API which should be at least 512 bit long",
  "Issuer": "MyIssuer",
  "Audience": "MyAudience",
  "ExpirationInMinutes":  60
}




