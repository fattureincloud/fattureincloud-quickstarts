require 'rubygems'
require 'webrick'
require 'json'
require 'fattureincloud_ruby_sdk'

class Oauth < WEBrick::HTTPServlet::AbstractServlet
	def do_GET(request, response)
		oauth = FattureInCloud_Ruby_Sdk::OAuth2AuthorizationCodeManager.new('CLIENT_ID', 'CLIENT_SECRET', 'http://localhost:8000/oauth')
		if !request.request_uri.query.nil?
			url_obj = URI.decode_www_form(request.request_uri.query).to_h
			if !url_obj['code'].nil?
				token = oauth.fetch_token(url_obj['code'])
				File.open('./token.json', 'w') do |file|
					file.write({"access_token" => token.access_token}.to_json) # saving the oAuth access token in the token.json file
				end
				body = 'Token saved succesfully in ./token.json'
			else
				redirect(response, oauth)
			end
		else redirect(response, oauth)
		end

		response.status = 200
		response['Content-Type'] = 'text/html'
		response.body = body
	end

	def redirect(response, oauth)
		url = oauth.get_authorization_url([FattureInCloud_Ruby_Sdk::Scope::ENTITY_SUPPLIERS_READ], 'EXAMPLE_STATE')
		response.set_redirect(WEBrick::HTTPStatus::TemporaryRedirect, url)
	end
end

if $PROGRAM_NAME == __FILE__
	server = WEBrick::HTTPServer.new(Port: 8000)
	server.mount '/oauth', Oauth
	trap 'INT' do server.shutdown end
	server.start
end
