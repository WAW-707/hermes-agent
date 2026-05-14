import sys
import os
sys.path.append("/home/kaher/.hermes/hermes-agent")
from hermes_cli.config import load_config
from agent.auxiliary_client import _resolve_auto

config = load_config()
model_cfg = config.get('model', {})
if isinstance(model_cfg, str):
    model_name = model_cfg
    provider = ""
else:
    model_name = model_cfg.get("default", model_cfg.get("name", ""))
    provider = model_cfg.get("provider", "")

resolved = _resolve_auto({"task": "main", "provider": provider, "model": model_name})
client, model = resolved
print(f"Client: {client}")
print(f"Model: {model}")
print(f"API Key: {client.api_key}")

