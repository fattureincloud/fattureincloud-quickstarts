import * as http from "http"
import url from "url"
import { getOAuthAccessToken } from "./oauth";
import { getFirstCompanySuppliers } from "./quickstart";

const hostname = '127.0.0.1'; //set your hostname
const port = 8000; //set your port

const server = http.createServer(async (req, res) => {
	let pathname = url.parse(req.url ?? '').pathname;

	//url routing
	switch (pathname) {
		case '/oauth': //oauth endpoint 
			res.end(await getOAuthAccessToken(req, res));
			break;
		case '/quickstart': //quickstart endpoint
			res.end(await getFirstCompanySuppliers());
			break;
		default:
			res.end();
			break;
	}
	res.end();
});

server.listen(port, hostname, () => {
	console.log(`Server running at http://${hostname}:${port}/`);
});