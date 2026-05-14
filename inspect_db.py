import sqlite3

c = sqlite3.connect('/home/kaher/.hermes/state.db')
try:
    models = c.execute("SELECT * FROM models").fetchall()
    print("Models Table:", models)
except Exception as e:
    print("No models table:", e)

try:
    settings = c.execute("SELECT * FROM settings").fetchall()
    print("Settings Table:", settings)
except Exception as e:
    print("No settings table:", e)
