const fattureInCloudSdk = require("@fattureincloud/fattureincloud-js-sdk")
const fs = require("fs")

async function getFirstCompanySuppliers() {
    try {
        let rawdata = fs.readFileSync(__dirname + '/token.json')

        let json = JSON.parse(rawdata)

        let defaultClient = fattureInCloudSdk.ApiClient.instance
        let OAuth2AuthenticationCodeFlow = defaultClient.authentications['OAuth2AuthenticationCodeFlow']
        OAuth2AuthenticationCodeFlow.accessToken = json["access_token"]

        // Retrieve the first company id
        let userApiInstance = new fattureInCloudSdk.UserApi()
        let userCompaniesResponse = await userApiInstance.listUserCompanies()
        let firstCompanyId = userCompaniesResponse.data.companies[0].id

        // Retrieve the list of the Suppliers
        let suppliersApiInstance = new fattureInCloudSdk.SuppliersApi()
        let companySuppliers = await suppliersApiInstance.listSuppliers(firstCompanyId)
 
        return(JSON.stringify(companySuppliers.data)) 

    } catch(e) {
        return (JSON.stringify(e))
    }
}

module.exports = {
    getFirstCompanySuppliers
}