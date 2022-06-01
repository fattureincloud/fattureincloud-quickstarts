# The following dependency is required
# gem install fattureincloud_ruby_sdk

require 'fattureincloud_ruby_sdk'
require 'json'

def main()
    FattureInCloud_Ruby_Sdk.configure do |config|
    # Here we init the Fatture in Cloud SDK
    # The Access Token is retrieved using the "get_token" method
    config.access_token = get_token()
    end
    
    # In this example we're using the Products API
    products_api_instance = FattureInCloud_Ruby_Sdk::ProductsApi.new
    
    # This is the ID of the company we're working on
    company_id = 2
    # Here we define the parameters for the first request.
    opts = {
        fields: nil,
        fieldset: "detailed",
        sort: nil,
        page: 1, # We're trying to obtain the first page
        per_page: 5 # Every page will contain at most 5 products
    }
    actual_page = 2
    
    result = list_products_with_backoff(company_id, opts, products_api_instance)
    last_page = result.last_page 
    
    # In this example we suppose to export the data to a JSON Lines file.
    # First, we cancel the content of the destination file
    File.delete('./products.jsonl') if File.exist?('./products.jsonl')
    append_products_to_file(result.data)
    
    while actual_page <= last_page do
        opts[:page] = actual_page   
        res = list_products_with_backoff(company_id, opts, products_api_instance)
        append_products_to_file(res.data)
        actual_page += 1
    end
end

def list_products_with_backoff(company_id, opts, products_api_instance)      
    retries = 0
    max_retries = 20
    begin
        puts "Page: #{opts[:page]} Attempt: #{retries} WaitTime(millis): #{2 ** retries * 1000}\n"
        products = products_api_instance.list_products(company_id, opts)
        return products
    rescue FattureInCloud_Ruby_Sdk::ApiError => e
        if retries <= max_retries
            retries += 1
            sleep 2 ** retries
            retry
        else
            raise "Giving up on the server after #{retries} retries. Got error: #{e.message}"
        end
    end
end


def append_products_to_file(products)
    for product in products
        File.write('./products.jsonl', product.to_hash.to_json + "\n", mode: 'a')
    end
end

def get_token()
  return "YOUR_ACCESS_TOKEN"
end

# This code should be executed periodically using a cron library or job scheduler.
main()