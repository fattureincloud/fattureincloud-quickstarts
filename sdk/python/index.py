from http.server import BaseHTTPRequestHandler, HTTPServer
from oauth import Oauth #import the Oauth class
from quickstart import Quickstart #import the Quickstart class

class testHTTPServer_RequestHandler(BaseHTTPRequestHandler):
	def do_GET(self):
		#url routing
		if self.path.startswith('/oauth'): #oauth endpoint
			Oauth.get_oauth_access_token(self)
		elif self.path == '/quickstart': #quickstart endpoint
			Quickstart.get_first_company_suppliers(self)
		return
	
def run():
	print('Starting the server...')
	server_address = ('127.0.0.1', 8000) #set your hostname and port
	httpd = HTTPServer(server_address, testHTTPServer_RequestHandler)
	print('Server running...')
	httpd.serve_forever()
run()