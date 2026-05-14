import sqlite3

c = sqlite3.connect('/home/kaher/.hermes/state.db')
print(c.execute("SELECT DISTINCT model FROM sessions").fetchall())
