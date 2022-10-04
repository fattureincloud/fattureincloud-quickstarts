package main

import (
	"fmt"
	"log"
	"net/http"
)

func main() {
	http.HandleFunc("/oauth", getOAuthAccessToken)
	http.HandleFunc("/quickstart", getFirstCompanySuppliers)

	fmt.Printf("Starting server at port 8000\n")
	if err := http.ListenAndServe(":8000", nil); err != nil {
		log.Fatal(err)
	}
}
