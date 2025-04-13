import { Configuration, ListUserCompaniesResponse, SuppliersApi, UserApi } from "@fattureincloud/fattureincloud-ts-sdk";
import fs from "fs";

export async function getFirstCompanySuppliers() {
	try {
		let rawdata = fs.readFileSync('./token.json');
		let json = JSON.parse(rawdata.toString());
		// Configure OAuth2 access token for authorization: 
		const apiConfig = new Configuration({
			accessToken: json["accessToken"]
		});

		// Retrieve the first company id
		let userApiInstance = new UserApi(apiConfig);
		let userCompaniesResponse: ListUserCompaniesResponse = await (await userApiInstance.listUserCompanies()).data;
		let firstCompanyId = userCompaniesResponse?.data?.companies?.[0]?.id;

		if (firstCompanyId) {
			// Retrieve the list of the Suppliers
			let suppliersApiInstance = new SuppliersApi(apiConfig);
			let companySuppliers = await suppliersApiInstance.listSuppliers(firstCompanyId);

			return (JSON.stringify(companySuppliers.data));
		}

	} catch (e) {
		return (JSON.stringify(e));
	}
}