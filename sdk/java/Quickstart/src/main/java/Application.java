import com.google.gson.Gson;
import com.sun.net.httpserver.HttpServer;
import it.fattureincloud.sdk.auth.OAuth2AuthorizationCodeManager;
import it.fattureincloud.sdk.auth.OAuth2AuthorizationCodeResponse;
import it.fattureincloud.sdk.auth.Scope;

import java.io.*;
import java.net.InetSocketAddress;
import java.util.Arrays;
import java.util.List;

class Application {

    public static void main(String[] args) throws IOException {
        int serverPort = 8000;
        HttpServer server = HttpServer.create(new InetSocketAddress(serverPort), 0);
        server.createContext("/oauth", (exchange -> {
            OAuth2AuthorizationCodeManager oauth = new OAuth2AuthorizationCodeManager("CLIENT_ID", "CLIENT_SECRET", "http://localhost:8000/oauth");
            List<Scope> scopes = Arrays.asList(Scope.ENTITY_SUPPLIERS_READ);
            String redirectUrl = oauth.getAuthorizationUrl(scopes, "EXAMPLE_STATE");
            String query = exchange.getRequestURI().getQuery();
            if(query == null) query = "";
            if(query.contains("code")){
                int start = query.indexOf("code=") + 5;
                int finish = query.indexOf("&");
                String code = query.substring(start, finish);
                Gson gson = new Gson();
                OAuth2AuthorizationCodeResponse tokenObj = oauth.fetchToken(code).get();
                String token = gson.toJson(tokenObj);

                saveToken(token);
                String respText = "token salvato correttamente";
                exchange.sendResponseHeaders(200, respText.getBytes().length);
                OutputStream output = exchange.getResponseBody();
                output.write(respText.getBytes());
                output.flush();
                exchange.close();
            }else{
                exchange.getResponseHeaders().set("Location", redirectUrl);
                exchange.sendResponseHeaders(302, 0);
                exchange.close();
            }

        }));
        server.createContext("/quickstart", (exchange -> {
            String token = retrieveToken();

            //the following method is defined in the next step
            String respText = Quickstart.getFirstCompanySuppliers(token);

            exchange.sendResponseHeaders(200, respText.getBytes().length);
            OutputStream output = exchange.getResponseBody();
            output.write(respText.getBytes());
            output.flush();
            exchange.close();
        }));
        server.setExecutor(null);
        server.start();
    }

    public static void saveToken(String token) throws IOException {
        BufferedWriter writer = new BufferedWriter(new FileWriter("token.json"));
        writer.write(token); //saving the oAuth access token in the token.json file

        writer.close();
    }

    public static String retrieveToken() throws IOException {
        BufferedReader reader = new BufferedReader(new FileReader("token.json"));
        String json = reader.readLine();
        Gson gson = new Gson();
        OAuth2AuthorizationCodeResponse obj = gson.fromJson(json, OAuth2AuthorizationCodeResponse.class);
        String token = obj.getAccessToken();
        return token;
    }
}