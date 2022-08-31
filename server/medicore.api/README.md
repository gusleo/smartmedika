# README #

This is API for MediCare apps

### What is this repository for? ###

* API for provide data MediCare apps
* Version: 0.0.1

### Configuration ###

* clone Dna.Core: *git clone git@bitbucket.org:medicoreid/dna.core.git*
* clone MediCore.Layer: *git clone git@bitbucket.org:medicoreid/medicore.layer.git*

### Database Migrations ###
* type on terminal *dotnet ef migrations add Initial -c MediCoreContext -o Migrations/MediCore.Data*
* type *dotnet ef database update -c MediCoreContext*

### Copy Heroku database to local Postgree ###
1. ```heroku pg:backups:capture --app smartmedika```
2. ```heroku pg:backups:download --app smartmedika```
3. ```pg_restore --verbose --clean --no-acl --no-owner -h localhost -d SmartMedika latest.dump```