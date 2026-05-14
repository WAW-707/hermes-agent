import requests
import json

api_key = 'AIzaSyBSn2_63CvOqTvTEmgfyWeNIhLz3IXtq1c'
url = f'https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={api_key}'

data = {
    "contents": [{"parts": [{"text": "مرحبا"}]}]
}

res = requests.post(url, headers={'Content-Type': 'application/json'}, data=json.dumps(data))
print(f"Status Code: {res.status_code}")
print(res.text)
