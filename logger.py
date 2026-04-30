import logging
import os

LOG_FOLDER = "data"
LOG_FILE = os.path.join(LOG_FOLDER, "logs.txt")

# ✅ Ensure folder exists
if not os.path.exists(LOG_FOLDER):
    os.makedirs(LOG_FOLDER)

# ✅ Configure logging
logging.basicConfig(
    filename=LOG_FILE,
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s"
)

# ✅ Helper functions
def log_info(msg):
    logging.info(msg)
    print(f"INFO: {msg}")   # optional console output

def log_warning(msg):
    logging.warning(msg)
    print(f"WARNING: {msg}")

def log_error(msg):
    logging.error(msg)
    print(f"ERROR: {msg}")

def log_critical(msg):
    logging.critical(msg)
    print(f"CRITICAL: {msg}")