from utils import load_data
from collections import Counter
from datetime import datetime

class ReportGenerator:

    @staticmethod
    def generate_report():
        data = load_data()

        if not data:
            print("\n📊 REPORT")
            print("----------------------")
            print("No tickets available")
            return

        total = len(data)
        open_tickets = len([t for t in data if t["status"] == "Open"])
        closed = len([t for t in data if t["status"] == "Closed"])

        # 🔥 Most common issue
        issues = [t["issue"] for t in data]
        common_issue = Counter(issues).most_common(1)

        # 🔥 High priority tickets
        high_priority = len([t for t in data if t["priority"] == "P1"])

        # 🔥 Department with most tickets
        departments = [t["dept"] for t in data]
        top_dept = Counter(departments).most_common(1)

        # 🔥 SLA breaches
        breached = 0
        now = datetime.now()

        for t in data:
            try:
                created = datetime.fromisoformat(t["created_date"])
                sla_limit = created.timestamp() + (t["sla_hours"] * 3600)

                if now.timestamp() > sla_limit and t["status"] != "Closed":
                    breached += 1
            except:
                pass

        # ✅ FINAL OUTPUT
        print("\n📊 REPORT")
        print("----------------------")
        print("Total Tickets:", total)
        print("Open Tickets:", open_tickets)
        print("Closed Tickets:", closed)
        print("High Priority Tickets (P1):", high_priority)
        print("Most Common Issue:", common_issue[0][0] if common_issue else "N/A")
        print("Top Department:", top_dept[0][0] if top_dept else "N/A")
        print("SLA Breaches:", breached)