# Run the quickstart

Before running the quickstart make sure to fill the placeholders at line 23 of the _Pages/index.cshtml.cs_ file with your CLIENT_ID and CLIENT_SECRET

Install the dependencies:
```
dotnet restore
```

Start the server:
```
dotnet run
```

Save the token to the token.json file visiting [localhost:5000](localhost:5000) and finally call our API visitig [localhost:5000/Quickstart](localhost:5000/Quickstart)