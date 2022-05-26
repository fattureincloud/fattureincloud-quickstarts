const fs = require("fs")
const fattureInCloudSdk = require('@fattureincloud/fattureincloud-js-sdk')
const oauth = new fattureInCloudSdk.OAuth2AuthorizationCodeManager("CLIENT_ID", "CLIENT_SECRET", "http://localhost:8000/oauth")

async function saveAccessToken(req, res) {
    res.statusCode = 200
    res.setHeader('Content-Type', 'text/plain')

    let query = req.url.split('?')[1]
    let params = new URLSearchParams(query)

    if (params.get('code') == null) {
        res.writeHead(302, {
            'Location': oauth.getAuthorizationUrl([fattureInCloudSdk.Scope.ENTITY_SUPPLIERS_READ], 'EXAMPLE_STATE')
        })
        res.end()
    } else {
        try {
            let token = await oauth.fetchToken(params.get('code'))

            fs.writeFileSync("./token.json", JSON.stringify(token, null, 4), (err) => {
                if (err) {
                    console.error(err)
                    return
                }
            })
            res.write("Token succesfully retrived and stored in token.json")

        } catch (e) {
            console.log(e)
        }

        res.end()
    }
}

module.exports = {
    saveAccessToken
}