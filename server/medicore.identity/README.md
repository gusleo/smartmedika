# README #

This README would normally document whatever steps are necessary to get your application up and running.

### What is this repository for? ###

* Quick summary
* Version
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### How do I get set up? ###

* Summary of set up
* Configuration
* Dependencies
* Database configuration
* How to run tests
* Deployment instructions

### Contribution guidelines ###

* Writing tests
* Code review
* Other guidelines

## Setting Docker Quikstart Terminal ##
1. run command ```docker-machine env```
2. on the last of the output you will see command #### Run this command to configure your shell:
3. run that command
4. after that run the command bellow

### Compose Docker ###

1. ```docker-compose -f ./docker-compose.ci.build.yml up```
2. If #1 not working or still output an error, inside ** identity ** folder run ``` dotnet restore ./MediCore.Identity.sln ``` then ``` dotnet publish ./MediCore.Identity.sln -c Release -o ./obj/Docker/publish ``` 
3. Inside ** api ** folder run ``` dotnet restore ./MediCore.Api.sln ``` then ``` dotnet publish ./MediCore.Api.sln -c Release -o ./obj/Docker/publish ```
4. ```docker-compose build```, more info about docker-compose https://stackoverflow.com/a/39988980/661739
2. ```docker tag <your-image-name> registry.heroku.com/<heroky-app-name>/web```
3. ```docker push registry.heroku.com/<heroky-app-name>/web```

### Common Issues ###
1. ** dotnet command not found. ** Download sdk installer from here: https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.0.0-download.md (for dotnet core 2.0.0)
2. ** MSBuild error: "The SDK 'Microsoft.NET.Sdk.Web' specified could not be found. ** " Copy sdk from *'C:\Program Files\dotnet\sdk\2.0.2\Sdks\Microsoft.Docker.Sdk* to *C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\Sdks* 'https://github.com/dotnet/cli/issues/6178#issuecomment-319322361