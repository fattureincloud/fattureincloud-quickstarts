# Run the quickstart

Before running the quickstart make sure to fill the placeholders at line 10 of the _oauth.php_ file with your CLIENT_ID and CLIENT_SECRET

Install the dependencies:
```
composer install
````

Start the server:
```
php -S localhost:8000
````

Save the token to the token.json file visiting [localhost:8000/oauth.php](localhost:8000/oauth.php) and finally call our API visitig [localhost:8000/quickstart.php](localhost:8000/quickstart.php)