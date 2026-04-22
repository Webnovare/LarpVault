# 🏴‍☠️ LarpVault – Live Like A Legend

**Buy perceived success for $10,000. Get the ultimate LARP content pack.**

A fun but **production-grade** Azure Functions project built to demonstrate real-world skills for Production Support / Azure roles.

![LarpVault](https://via.placeholder.com/800x300/0f172a/60a5fa?text=LarpVault+-+Live+Like+A+Legend)

## ✨ What It Does

When you click "BUY NOW", it:
- Accepts the purchase via HTTP trigger
- "Sends" the order to a background queue (Service Bus style)
- Logs the processing in the background processor

Built to showcase async processing, logging, CORS, and clean architecture.

## 🚀 Quick Start

### 1. Start the services (in separate terminals)

```bash
# Terminal 1 - Storage Emulator
azurite

# Terminal 2 - Azure Functions
cd LarpVault.Api
func start LarpVault_Api.csproj

2. Open the Frontend
Open index.html in your browser and click "BUY NOW - BECOME A LEGEND"
Features

Modern .NET 10 Azure Functions (Isolated Worker)
Clean HTTP Trigger with CORS support
Beautiful single-file Tailwind frontend
Background processing simulation (Service Bus Queue Trigger ready)
Structured logging
Copy Order ID functionality

Project Structure

LarpVault/
├── index.html                 ← Nice frontend
├── LarpVault.Api/
│   ├── PurchaseFunction.cs    ← HTTP Purchase endpoint
│   ├── PurchaseProcessor.cs   ← Background processor
│   ├── Program.cs
│   └── LarpVault_Api.csproj
├── README.md

Tech Stack

.NET 10 + Azure Functions Isolated
Azure Service Bus (Queue Trigger)
Azurite (local storage)
Tailwind CSS (frontend)


Focuses on monitoring, async processing, troubleshooting, and clean code.
Made with sarcasm and real Azure skills.
Star ⭐ if you also believe $10,000 is a fair price for confidence.
Adam Hanafi