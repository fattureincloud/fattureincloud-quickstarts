const http = require('http')
const url = require('url')
const oauthPath = require("./oauth.js") //import the oauth methods
const quickstart = require("./quickstart.js") //import the quickstart  
const hostname = '127.0.0.1' //set your hostname
const port = 8000 //set your port

const server = http.createServer(async(req, res) => {
    let pathname = url.parse(req.url).pathname

    //url routing
    switch(pathname) {
        case '/oauth': //oauth endpoint 
            res.end( await oauthPath.saveAccessToken(req, res) )
        break
        case '/quickstart': //quickstart endpoint
            res.end( await quickstart.getFirstCompanySuppliers() )
        break
        default:
            res.end()
        break
    }
})

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`)
})