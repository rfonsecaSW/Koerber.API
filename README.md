# koerber

# Notes & Assumptions #

## .CSV Files ##

* Zones: 
    * We can't garantee that the combination of <em> "Borough", "Zone", "service_zone" </em> is unique;
        * <em> Note: LocationId's (56, 57) => "Queens","Corona","Boro Zone"; </em>
    * This will impact the application behavior, since data will not be aggregated by "Zone" but by "LocationId";
* GreenTrips: 
    * This .CSV file contains no headers, the data dictionary provided at https://www1.nyc.gov/site/tlc/about/tlc-trip-record-data.page does not match the .CSV structure.
    * The following assumptions were made:
        * <em> Index[1] => PickUpTime; Index[2] => DropOffTime; Index[5] => PickUpLocationID; Index[6] => DropOffLocationID; </em> 
    * Only PickUpTime, DropOffTime, PickUpLocationID and DropOffLocationID will be parsed, all other fields are optional;
* YellowTrips: 
    * This .CSV file contains no headers, the data dictionary provided at https://www1.nyc.gov/site/tlc/about/tlc-trip-record-data.page does not match the .CSV structure.
    * The following assumptions were made:
        * <em> Index[1] => PickUpTime; Index[2] => DropOffTime; Index[7] => PickUpLocationID; Index[8] => DropOffLocationID; </em> 
    * Only PickUpTime, DropOffTime, PickUpLocationID and DropOffLocationID will be parsed, all other fields are optional;

# Implementation #

## Tech. Stack ##

* ASP.NET CORE v6.0
* Entity Framework CORE
    * Code First Approach: 
        * The data store schema will be adapted to reflect the application data model;
    * ORM:
        * EF CORE offers a database migration strategy. Everytime the data model is updated, we can easily add a corresponding migration describing the updates necessary to keep the schema in synch.
* [Requirement] SQL Server
* [Requirement] Swagger
* [Requirement] Docker

## Third Parties ##

* NewtonSoft.JSON: https://www.newtonsoft.com/json
    * I could have used the default serializer with .NET 6, however I felt more confortable using NewtonSoft.JSON (also more familiar with customizing the serializer options);

* CsvHelper: https://joshclose.github.io/CsvHelper/
    * I've used this dependency to parse the .CSV files, it's configurable, and works well for large data sets (does not load the complete file in memory);

* xUnit: https://xunit.net/
    * I'm using xUnit instead of NUnit for the unit test project (personal choice);

* Moq: https://github.com/moq/moq
    * I'm using Moq to control service execution of injected dependencies for the unit test project;

* FluentAssertions: https://fluentassertions.com/
    * Allows me to more naturally specify the expected outcome (assert) of a unit test;

# Installation #

## Step 1: Configuration ##

Edit the <em> appsettings.json </em> file, to configure the <em> "DatabaseOptions", "DataLoaderOptions" </em> JSON objects:

```
"DatabaseOptions": {
    "ConnectionString": "<INSERT SQL SERVER CONNECTION STRING>"
  },
  "DataLoaderOptions": {
    "ZonesFilePath": "<INSERT ZONES FILE PATH>",
    "GreenTaxiFilePath": "<INSERT GREEN TAXI FILE PATH>",
    "YellowTaxiFilePath": "<INSERT YELLOW TAXI FILE PATH>"
  },
```

## Step 2: Docker Startup ##

Use the following cmds to build, and setup the Docker image:

1. Create Docker Image: ```docker build --rm -t koerber:latest . ```

2. Pick either Method 1 or Method 2 to publish the Docker Image
    * Method 1: Publish Docker Image and Load .CSV Files
        * ```docker run --rm -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 -e LOAD_DATA=YES koerber:latest```
    * Method 2: Publish Docker Image
        * ```docker run --rm -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 -e LOAD_DATA=NO koerber:latest```

### WEB API ###

* /top-zones (CURL Instruction)
```
curl -X 'GET' \
  'https://localhost:5000/top-zones?order=pickups' \
  -H 'accept: */*'
```

* /zone-trips (CURL Instruction)
```
curl -X 'GET' \
  'https://localhost:5000/zone-trips?zone=1&date=2018-01-01' \
  -H 'accept: */*'
```

* /list-yellow (CURL Instruction)
```
curl -X 'POST' \
  'https://localhost:5000/list-yellow' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json-patch+json' \
  -d '{
  "dropOffDateTimeFilter": "2018-01-01",
  "dropOffLocationFilter": [
    1
  ],
  "offset": 0,
  "pagination": 0,
  "pickUpDateTimeFilter": "2018-01-01",
  "pickupLocationFilter": []
}'
```