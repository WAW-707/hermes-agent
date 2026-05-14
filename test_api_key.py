import requests

api_key = 'AIzaSyBSn2_63CvOqTvTEmgfyWeNIhLz3IXtq1c'
res = requests.get('https://generativelanguage.googleapis.com/v1beta/models', params={'key': api_key})
print(f"Status Code: {res.status_code}")
if res.status_code != 200:
    print(res.text)
