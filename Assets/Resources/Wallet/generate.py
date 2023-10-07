import json

with open("wallet_items.json", "r", encoding="utf-8") as f:
    t = json.loads(f.read())
    for item in t["items"]:
        item["show"] = True
        item["finish"] = False

with open("output.json", "w+", encoding="utf-8") as f:
    json.dump(t, f, ensure_ascii=False)