import { OAuth2AuthorizationCodeManager, Scope } from "@fattureincloud/fattureincloud-ts-sdk"
import fs from "fs"
import http from "http"

export async function getOAuthAccessToken(req: http.IncomingMessage, res: http.ServerResponse) {
	res.statusCode = 200
	res.setHeader('Content-Type', 'text/plain')

	let query = !!req.url && req.url.split('?')[1]
	let params = new URLSearchParams(query || '')

	let oauth = new OAuth2AuthorizationCodeManager('CLIENT_ID', 'CLIENT_SECRET', 'http://localhost:8000/oauth')

	if (params.get('code') == null) {
		res.writeHead(302, {
			'Location': oauth.getAuthorizationUrl([Scope.ENTITY_SUPPLIERS_READ], 'EXAMPLE_STATE')
		});
		res.end()
	} else {
		let code = params.get('code')

		try {
			let token = await oauth.fetchToken(code ?? '')
			// saving the oAuth access token in the token.json file
			fs.writeFileSync("./token.json", JSON.stringify(token, null, 4))
			res.write("Token succesfully retrived and stored in token.json")
		} catch (e) {
			console.log(e);
		}
		res.end();
	}
};