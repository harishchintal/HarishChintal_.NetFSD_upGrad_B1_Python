from datetime import datetime, timedelta
from utils import load_data, save_data
from logger import log_info, log_error
from itil import track_problem
import time

PRIORITY_RULES = {
    "Server Down": "P1",
    "Internet Down": "P2",
    "Laptop Slow": "P3",
    "Password Reset": "P4"
}

SLA_RULES = {"P1": 1, "P2": 4, "P3": 8, "P4": 24}

# ✅ Decorator (Advanced Python)
def log_decorator(func):
    def wrapper(*args, **kwargs):
        log_info(f"Executing {func.__name__}")
        return func(*args, **kwargs)
    return wrapper

class Ticket:
    def __init__(self, emp_name, dept, issue):
        self.ticket_id = int(time.time() * 1000)
        self.emp_name = emp_name.title()
        self.dept = dept.upper()
        self.issue = issue
        self.priority = PRIORITY_RULES.get(issue, "P3")
        self.status = "Open"
        self.created_date = datetime.now().isoformat()
        self.sla_hours = SLA_RULES[self.priority]
        self.category = "General"

    def to_dict(self):
        return self.__dict__

# ✅ Inheritance (MANDATORY)
class IncidentTicket(Ticket):
    def __init__(self, emp, dept, issue):
        super().__init__(emp, dept, issue)
        self.category = "Incident"

class ServiceRequest(Ticket):
    def __init__(self, emp, dept, issue):
        super().__init__(emp, dept, issue)
        self.category = "Service Request"

# ✅ Generator (Advanced Python)
def ticket_generator(data):
    for t in data:
        yield t

class TicketManager:

    @staticmethod
    @log_decorator
    def create_ticket(emp, dept, issue):
        try:
            if not emp or not dept or not issue:
                raise ValueError("Fields cannot be empty")

            data = load_data()

            # choose type
            if issue in ["Server Down", "Internet Down"]:
                ticket = IncidentTicket(emp, dept, issue)
            else:
                ticket = ServiceRequest(emp, dept, issue)

            data.append(ticket.to_dict())
            save_data(data)

            # ✅ Problem Management
            track_problem(issue)

            log_info(f"Ticket Created: {ticket.ticket_id}")
            return ticket.ticket_id

        except Exception as e:
            log_error(str(e))
            print("ERROR:", e)
            return None

    @staticmethod
    def view_tickets():
        data = load_data()
        return list(ticket_generator(data))  # using generator

    @staticmethod
    def search_ticket(ticket_id):
        for t in load_data():
            if t["ticket_id"] == ticket_id:
                return t
        raise Exception("Ticket Not Found")

    @staticmethod
    def update_status(ticket_id, status):
        data = load_data()
        for t in data:
            if t["ticket_id"] == ticket_id:
                t["status"] = status.capitalize()
                save_data(data)
                log_info(f"Ticket Updated: {ticket_id}")
                print("✅ Status updated successfully")
                return
        raise Exception("Invalid Ticket ID")

    @staticmethod
    def delete_ticket(ticket_id):
        data = load_data()
        new_data = [t for t in data if t["ticket_id"] != ticket_id]

        if len(data) == len(new_data):
            print("❌ Ticket not found")
            return

        save_data(new_data)
        print("✅ Ticket deleted successfully")

    @staticmethod
    def check_sla():
        data = load_data()
        now = datetime.now()

        for t in data:
            created = datetime.fromisoformat(t["created_date"])
            sla_limit = created + timedelta(hours=t["sla_hours"])

            if t["status"] == "Closed":
                print(f"✅ Ticket {t['ticket_id']} CLOSED")
            elif now > sla_limit:
                print(f"❌ SLA BREACHED {t['ticket_id']} 🚨 Escalated")
            else:
                print(f"⏳ SLA OK {t['ticket_id']}")