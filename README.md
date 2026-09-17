\# Movie API



This is a simple Asp.net core web api for searching and browsing movie data using the supplied movie dataset (from kaggle)

Data is downloaded as CSV file and is saved in DataFiles folder.



\## Technologies



\- .NET 8 / ASP.NET Core Web API

\- Entity Framework Core

\- SQL Server

\- CsvHelper

\- xUnit

\- Docker / Docker Compose



\## Running with Docker



Run from the solution directory

(powershell)

docker compose up --build



This starts both the API and SQL server.

Database is created using EF core migrations and populated from the CSV file on first startup.

Once the application has started, open swagger as below



http://localhost:8080/swagger



API also can be called from browser

http://localhost:8080/api/Movies?limit=5



To stop and remove containers:



docker compose down



\##API



Endpoint 1 : GET: /api/Movies : This got below query params



&#x09;title, genre, page, limit, sortby, sortby direction

&#x09;eg : GET /api/Movies?title=Batman\&genre=Action\&limit=5\&page=1



Endpoint 2 : GET: /api/Movies/popular?limit=10



\##Tests



&#x09;You can do 'Run All tests' from menu or 

&#x09;dotnet test (power shell)



\## Additional Notes



The supplied dataset does not contain actor information, so filtering actor not been implemented. No data been changed from CSV.





