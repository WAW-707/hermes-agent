import os
from dotenv import load_dotenv

with open('/home/kaher/.hermes/.env.test', 'w') as f:
    f.write('export TELEGRAM_BOT_TOKEN=12345:ABC\n')

load_dotenv('/home/kaher/.hermes/.env.test')
print("Token:", os.getenv("TELEGRAM_BOT_TOKEN"))
