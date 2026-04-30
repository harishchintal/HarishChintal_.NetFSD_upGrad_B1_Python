import json
import os
from datetime import datetime
from logger import log_info, log_warning

DATA_FOLDER = "data"
PROBLEM_FILE = os.path.join(DATA_FOLDER, "problems.json")

def ensure_file():
    if not os.path.exists(DATA_FOLDER):
        os.makedirs(DATA_FOLDER)

    if not os.path.exists(PROBLEM_FILE):
        with open(PROBLEM_FILE, "w") as f:
            json.dump({}, f)

def track_problem(issue):
    ensure_file()

    try:
        with open(PROBLEM_FILE, "r") as f:
            content = f.read().strip()
            data = json.loads(content) if content else {}
    except Exception:
        data = {}

    # ✅ Increment issue count
    count = data.get(issue, {}).get("count", 0) + 1

    # ✅ Update record
    data[issue] = {
        "count": count,
        "last_seen": datetime.now().isoformat()
    }

    # ✅ Problem record trigger
    if count == 5:
        print(f"⚠ Problem Record Created for: {issue}")
        log_warning(f"Problem Record Created for: {issue}")

    elif count > 5:
        log_info(f"Recurring issue detected: {issue} ({count} times)")

    # ✅ Save data
    with open(PROBLEM_FILE, "w") as f:
        json.dump(data, f, indent=4)