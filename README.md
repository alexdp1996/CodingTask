# CodingTask

## Overview
This project is designed to retrieve exchange bids and asks data and calculate the price for buying Bitcoin (BTC). It is built using .NET 8 and uses PostgreSQL as the database.

## Getting started

### Prerequisites
- **.NET 8 SDK**: Ensure you have the latest version installed.
- Use an IDE that supports .NET 8.
- PostgreSQL server.

### Build the Solution
1. Restore nuget dependencies
2. Build project
3. Insert Postgre connection string in appsettings.
4. Apply migration to DB
  dotnet ef database update --project CodingTask --startup-project CodingTask
5. Start project