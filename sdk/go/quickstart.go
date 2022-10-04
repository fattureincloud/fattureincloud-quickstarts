package main

import (
	"context"
	"encoding/json"
	"fmt"
	"net/http"
	"os"

	fattureincloudapi "github.com/fattureincloud/fattureincloud-go-sdk/v2/api"
	oauth "github.com/fattureincloud/fattureincloud-go-sdk/v2/oauth2"
)

func getFirstCompanySuppliers(w http.ResponseWriter, r *http.Request) {
	rawData, err := os.ReadFile("token.json")
	if err != nil {
		fmt.Println(err)
	}

	tokenObj := oauth.OAuth2AuthorizationCodeTokenResponse{}
	json.Unmarshal(rawData, &tokenObj)
	accessToken := tokenObj.AccessToken

	// Configure OAuth2 access token for authorization:
	auth := context.WithValue(context.Background(), fattureincloudapi.ContextAccessToken, accessToken)
	configuration := fattureincloudapi.NewConfiguration()
	apiClient := fattureincloudapi.NewAPIClient(configuration)
	// Retrieve the first company id
	userCompaniesResponse, _, err := apiClient.UserApi.ListUserCompanies(auth).Execute()
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error when calling `UserApi.ListUserCompanies``: %v\n", err)
		fmt.Fprintf(os.Stderr, "Full HTTP response: %v\n", r)
		http.Error(w, "500 internal server error.", http.StatusInternalServerError)
		return
	}
	firstCompanyId := userCompaniesResponse.GetData().Companies[0].GetId()
	// Retrieve the list of the Suppliers
	companySuppliers, _, err := apiClient.SuppliersApi.ListSuppliers(auth, firstCompanyId).Execute()
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error when calling `UserApi.ListSuppliers``: %v\n", err)
		fmt.Fprintf(os.Stderr, "Full HTTP response: %v\n", r)
		http.Error(w, "500 internal server error.", http.StatusInternalServerError)
		return
	}
	json.NewEncoder(w).Encode(companySuppliers)

}
