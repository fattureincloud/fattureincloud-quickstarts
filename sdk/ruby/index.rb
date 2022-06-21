require 'rubygems'
require 'webrick'

require './oauth' # importing the class created in the next tab
require './quickstart' # importing the class created in the previous steps

if $PROGRAM_NAME == __FILE__
	server = WEBrick::HTTPServer.new(Port: 8000)
	server.mount '/quickstart', QuickStart # route that refers to the QuickStary class in the imported quickstart.rb file
	server.mount '/oauth', Oauth # route that refers to the Oauth class in the imported oauth.rb file
	trap 'INT' do server.shutdown end
	server.start
end
