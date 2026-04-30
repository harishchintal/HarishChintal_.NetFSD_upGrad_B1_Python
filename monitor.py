import psutil
from tickets import TicketManager
from logger import log_critical
import os

class Monitor:

    @staticmethod
    def check_system():
        cpu = psutil.cpu_percent(interval=1)
        ram = psutil.virtual_memory().percent
        disk = psutil.disk_usage('C:\\' if os.name == 'nt' else '/').percent
        net = psutil.net_io_counters()

        print(f"CPU: {cpu}% | RAM: {ram}% | Disk: {disk}%")
        print(f"Network Sent: {net.bytes_sent} | Received: {net.bytes_recv}")

        if cpu > 90 or ram > 95 or disk > 90:
            print("🚨 ALERT!")
            log_critical("System Alert")
            tid = TicketManager.create_ticket("SYSTEM", "IT", "Server Down")
            print("Auto Ticket:", tid)
        else:
            print("✅ System Normal")