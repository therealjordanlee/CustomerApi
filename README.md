# Customer API [![Build Status](https://dev.azure.com/jordanleenet/Public/_apis/build/status/therealjordanlee.CustomerApi?branchName=master)](https://dev.azure.com/jordanleenet/Public/_build/latest?definitionId=3&branchName=master)

## Problem to Solve
Build a simple Backend Application that allows:
- Adding customers :white_check_mark:
- First name, last name and date of birth fields :white_check_mark:
- Editing customers :white_check_mark:
- Deleting customers :white_check_mark:
- Searching for a customer by partial name match (first or last name) :white_check_mark:
- Add mock APIs to handle the above activities :white_check_mark:

## Tech Stack
- ASP.Net Core 3.1 API :white_check_mark:
- In memory entity framework store :white_check_mark:
- Dependency injection :white_check_mark:
- Basic XUnit tests :white_check_mark:
- Swagger / OpenAPI support :white_check_mark:

## Usage
- Clone this repository: `git clone git@github.com:therealjordanlee/CustomerApi.git`
- Open console and switch to the repository folder
- Build the project: `dotnet build ./src/`
- Run the project: `dotnet run --project ./src/CustomerApi.csproj`

### Swagger
While the application is running, you can browse the SwaggerUI on http://localhost:5001/swagger

### Adding customers
```
curl -X POST "https://localhost:5001/customers" -H "accept: */*" -H "Content-Type: application/json" -d "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"dateOfBirth\":\"2020-06-13T11:09:53.646Z\"}"
```

### Edit customer
```
curl -X PUT "https://localhost:5001/customers/1" -H "accept: */*" -H "Content-Type: application/json" -d "{\"firstName\":\"Jane\",\"lastName\":\"Doe\",\"dateOfBirth\":\"2020-06-13T11:11:14.565Z\"}"
```

### Delete customer
```
curl -X DELETE "https://localhost:5001/customers/1" -H "accept: */*"
```

### Search customer
```
curl -X GET "https://localhost:5001/customers?firstNameIncludes=jo&lastNameIncludes=sn" -H "accept: */*"
```

### Get all customers
```
curl -X GET "https://localhost:5001/customers" -H "accept: */*"
```

## Mock API
A mock (stub) server of Customer API is available in the `stubs` folder.
Usage:
```
dotnet run --project stubs/CustomerApiStub/CustomerApiStub.csproj
```

This will start a mock version of CustomerApi on http://localhost:8080
