
# Chat-Project-Server-Side (C# ASP.NET)

#### Advanced project for the Advanced Programming course: Web Development 

This document provides instructions and highlights regarding the installation and execution of the server-side of our chat application.

## Prerequisites
Ensure the following installations are completed before running the project:

1. .NET Core SDK
2. A code editor (e.g., Visual Studio or Visual Studio Code)

## Running the Server

### Step 1: Clone the Repository
Clone the ASP.NET project repository from the following link:
[ASP.NET Project Repository](https://github.com/noampdut/Chat-Project-Server-Side-.NET)

### Step 2: Navigate to the Project Directory
Open a terminal and navigate to the project directory where the `Chat-Project-Server-Side` solution file is located.

### Step 3: Restore Dependencies
Run the following command to restore the project dependencies:

```bash
dotnet restore
```

### Step 4: Run the Server
Start the server by running the following command:

```bash
dotnet run
```

> Note: Ensure the server runs on port 5001 to maintain compatibility with the front-end.

## API Endpoints
Simple Example for API endpoints:
- **POST /api/users/login**: Authenticate a user.
The Swagger also will open, after running the server.

## User Credentials

#### Admin Account:
- **Username:** admin
- **Password:** n123456

#### Additional Registered Users:
- **Username:** noampdut
- **Password:** n123456
- **Username:** naama
- **Password:** n123456
- **Username:** ofek
- **Password:** n123456

### Registration Instructions
To register a new user, send a POST request to `/api/users/register` with a username without spaces and a password that contains at least 6 characters, including both letters and digits.

### Thank You and Enjoy!
