package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"

	oauth "github.com/fattureincloud/fattureincloud-go-sdk/oauth2"
)

func getOAuthAccessToken(w http.ResponseWriter, r *http.Request) {
	query := r.URL.Query()
	auth := oauth.NewOAuth2AuthorizationCodeManager("CLIENT_ID", "CLIENT_SECRET", "http://localhost:8000/oauth")

	if query.Get("code") == "" {
		http.Redirect(w, r, auth.GetAuthorizationUrl([]oauth.Scope{oauth.Scopes.ENTITY_SUPPLIERS_READ}, "EXAMPLE_STATE"), http.StatusFound)
	} else {
		code := query.Get("code")

		token, err := auth.FetchToken(code)
		if err != nil {
			log.Println("Error on response.\n[ERROR] -", err)
			http.Error(w, "500 internal server error.", http.StatusInternalServerError)
			return
		}
		jsonObj, _ := json.Marshal(token)
		// saving the oAuth access token in the token.json file
		err = ioutil.WriteFile("token.json", jsonObj, 0644)

		if err != nil {
			log.Println("Error on writing the file.\n[ERROR] -", err)
			http.Error(w, "500 internal server error.", http.StatusInternalServerError)
			return
		}

		fmt.Fprintf(w, "Token succesfully retrived and stored in token.json")

	}
}
