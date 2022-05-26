# Run the quickstart

Before running the quickstart make sure to fill the placeholders at line 8 of the _oauth.rb_ file with your CLIENT_ID and CLIENT_SECRET

Install the dependencies:
```
bundle
````

Start the server:
```
ruby index.rb
````

Save the token to the token.json file visiting [localhost:8000/oauth](localhost:8000/oauth) and finally call our API visitig [localhost:8000/quickstart](localhost:8000/quickstart)