import json
import csv
import os
from logger import log_error, log_info

DATA_FOLDER = "data"
TICKET_FILE = os.path.join(DATA_FOLDER, "tickets.json")
BACKUP_FILE = os.path.join(DATA_FOLDER, "backup.csv")

# ✅ Ensure data folder exists
def ensure_data_folder():
    if not os.path.exists(DATA_FOLDER):
        os.makedirs(DATA_FOLDER)


def load_data():
    ensure_data_folder()

    if not os.path.exists(TICKET_FILE):
        return []

    try:
        with open(TICKET_FILE, "r") as f:
            content = f.read().strip()

            if not content:
                return []

            return json.loads(content)

    except json.JSONDecodeError:
        log_error("Invalid JSON format in tickets.json")
        return []

    except Exception as e:
        log_error(f"Error loading data: {e}")
        return []


def save_data(data):
    ensure_data_folder()

    try:
        with open(TICKET_FILE, "w") as f:
            json.dump(data, f, indent=4)

        log_info("Data saved successfully")

    except Exception as e:
        log_error(f"Error saving data: {e}")
        print("❌ Failed to save data")


def backup_to_csv(data):
    ensure_data_folder()

    if not data:
        print("⚠ No tickets available for backup")
        return

    try:
        with open(BACKUP_FILE, "w", newline="") as f:
            writer = csv.DictWriter(f, fieldnames=data[0].keys())
            writer.writeheader()
            writer.writerows(data)

        log_info("Backup created successfully")
        print("✅ Backup saved to CSV")

    except Exception as e:
        log_error(f"Backup error: {e}")
        print("❌ Backup failed")