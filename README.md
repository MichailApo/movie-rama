# MovieRama
A simple ASP.NET core razor pages web application , where users submit movies with their descriptions and other users can vote with a like or a hate

## Authentication
Utilizes cookie based authentication provided by ASP.NET Core Identity.

## Persistence
Database is created code first with Entity Framework 6.

The database is [mssql server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-2022) running on a local container.

# Installation
Suppose you have [docker](https://docs.docker.com/get-started/overview/) installed on you pc.

Clone the repo and navigate to folder containing the docker-compose.yaml

Run
```
docker-compose -f .\docker-compose.yaml up -d
```

and wait for the app image to be built and the containers to start.

The application consinsts of 2 container services

- db
- webapp

# Further development (for production level)
- Drop razor pages and move to a modern FE framework and the backend could be a webApi in order to support reactiveness on the web page
- Pagination to the main page , as essentially retries the whole table
- Liveness/Readiness probes
- Structured Logging
- Metrics