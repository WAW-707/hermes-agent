import os
import requests

api_key = os.getenv('GOOGLE_API_KEY')
if not api_key:
    # Read from .env manually just in case
    with open('/home/kaher/.hermes/.env') as f:
        for line in f:
            if line.startswith('GOOGLE_API_KEY='):
                api_key = line.strip().split('=', 1)[1]

res = requests.get('https://generativelanguage.googleapis.com/v1beta/models', params={'key': api_key})
models = [m['name'] for m in res.json().get('models', []) if 'gemini' in m['name']]
print(models)
