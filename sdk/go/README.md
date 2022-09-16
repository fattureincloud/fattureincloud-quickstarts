# Run the quickstart

Before running the quickstart make sure to fill the placeholders at line 15 of the _oauth.go_ file with your CLIENT_ID and CLIENT_SECRET

Install the dependencies:
```
go mod tidy
````

Start the server:
```
go run .
````

Save the token to the token.json file visiting [localhost:8000/oauth](localhost:8000/oauth) and finally call our API visitig [localhost:8000/quickstart](localhost:8000/quickstart)