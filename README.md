# han-adb-rdt-datamigration
Migrate StackOverflow SQL DB to MongoDB using C# Console application

## Spinning up MSSQL
To manage download azure data studio if using a Mac M1, otherwise you could use SQL Server Management Studio, links below:
- https://learn.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio
- https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms

```bash
# Mac M1 (SQL Azure Edge)
docker run --cap-add SYS_PTRACE \
--cpus="1.5" \
--memory=2g\
-u root \
-e 'ACCEPT_EULA=1' \
-e 'MSSQL_SA_PASSWORD=P@ssw0rd' \
-p 1433:1433 \
-v azure-sql-storage:/var/opt/mssql \
--name azuresqledge \
-d mcr.microsoft.com/azure-sql-edge
```

## Spinning up MongoDB
To manage download the mongodb compass application, link below:
- https://www.mongodb.com/try/download/compass

```bash
# MongoDb
docker run -u root \
--cpus="1.5" \
--memory=2g \
-p 27017:27017  \
--name mongodb \
-v mongo-data:/data/db \
-d mongo:latest
```