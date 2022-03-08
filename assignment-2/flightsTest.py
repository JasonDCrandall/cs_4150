import requests
import json

key = '61f1e107706cee74ed57a80f'
departure = 'SLC'
arrival = 'LAX'
departure_date = '2022-02-11'
return_date = '2022-02-12'
num_adults = '2'
num_children = '0'
num_infants = '0'
class_type = 'Economy' # Economy, Business, First, Premium_Economy
currency = 'USD'
airport = "slc"

url = f"https://api.flightapi.io/roundtrip/{key}/{departure}/{arrival}/{departure_date}/{return_date}/{num_adults}/{num_children}/{num_infants}/{class_type}/{currency}"

print(url)

# url = f"https://api.flightapi.io/iata/{key}/{airport}/airport"

response = requests.request("GET", url)

data = json.loads(response.text)
print(json.dumps(data, indent=4))
