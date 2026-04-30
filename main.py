from tickets import TicketManager
from monitor import Monitor
from reports import ReportGenerator
from utils import backup_to_csv


def get_int_input(msg):
    try:
        return int(input(msg))
    except ValueError:
        print("❌ Invalid number")
        return None


def menu():
    while True:
        print("\n======== SMART IT SERVICE DESK ========")
        print("1. Create Ticket")
        print("2. View Tickets")
        print("3. Search Ticket")
        print("4. Update Status")
        print("5. Delete Ticket")
        print("6. Monitor System")
        print("7. Generate Report")
        print("8. Check SLA")
        print("9. Exit")
        print("10. Backup Data")

        ch = input("Enter choice: ").strip()

        # ✅ CREATE
        if ch == "1":
            emp = input("Employee: ").strip()
            dept = input("Dept: ").strip()
            issue = input("Issue: ").strip()

            tid = TicketManager.create_ticket(emp, dept, issue)

            if tid:
                print("\n✅ Ticket Created Successfully")
                print(f"Ticket ID: {tid}")
            else:
                print("❌ Failed to create ticket")

        # ✅ VIEW
        elif ch == "2":
            tickets = TicketManager.view_tickets()

            if not tickets:
                print("No tickets found")
            else:
                print("\n📋 ALL TICKETS")
                print("----------------------------------")
                for t in tickets:
                    print(f"ID: {t['ticket_id']}")
                    print(f"Employee: {t['emp_name']}")
                    print(f"Dept: {t['dept']}")
                    print(f"Issue: {t['issue']}")
                    print(f"Category: {t.get('category', 'N/A')}")
                    print(f"Priority: {t['priority']}")
                    print(f"Status: {t['status']}")
                    print(f"Created: {t['created_date']}")
                    print("----------------------------------")

        # ✅ SEARCH
        elif ch == "3":
            tid = get_int_input("Enter Ticket ID: ")
            if tid is None:
                continue

            try:
                t = TicketManager.search_ticket(tid)

                print("\n🔍 TICKET DETAILS")
                print("----------------------------------")
                print(f"ID: {t['ticket_id']}")
                print(f"Employee: {t['emp_name']}")
                print(f"Dept: {t['dept']}")
                print(f"Issue: {t['issue']}")
                print(f"Category: {t.get('category', 'N/A')}")
                print(f"Priority: {t['priority']}")
                print(f"Status: {t['status']}")
                print("----------------------------------")

            except Exception as e:
                print("❌", e)

        # ✅ UPDATE
        elif ch == "4":
            tid = get_int_input("Enter Ticket ID: ")
            if tid is None:
                continue

            status = input("Enter Status (Open/Closed): ").strip()

            if status.lower() not in ["open", "closed"]:
                print("❌ Invalid status")
                continue

            try:
                TicketManager.update_status(tid, status)
            except Exception as e:
                print("❌", e)

        # ✅ DELETE
        elif ch == "5":
            tid = get_int_input("Enter Ticket ID: ")
            if tid is None:
                continue

            confirm = input("Are you sure you want to delete? (y/n): ").lower()

            if confirm == "y":
                try:
                    TicketManager.delete_ticket(tid)
                except Exception as e:
                    print("❌", e)
            else:
                print("❎ Deletion cancelled")

        # ✅ MONITOR
        elif ch == "6":
            Monitor.check_system()

        # ✅ REPORT
        elif ch == "7":
            ReportGenerator.generate_report()

        # ✅ SLA
        elif ch == "8":
            TicketManager.check_sla()

        # ✅ EXIT
        elif ch == "9":
            print("👋 Exiting system...")
            break

        # ✅ BACKUP
        elif ch == "10":
            data = TicketManager.view_tickets()
            backup_to_csv(data)

        else:
            print("❌ Invalid choice, try again")


# Run app
if __name__ == "__main__":
    menu()