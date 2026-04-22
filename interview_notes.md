
---

### 2. Interview Notes (Keep these handy)

Copy and save these notes — they are written in a way you can naturally say during interviews:

#### When asked: "Tell me about a project you've worked on"

"I built **LarpVault**, a humorous but technically solid Azure Functions application. The idea is you can buy overpriced 'success packs' containing luxury lifestyle B-roll.

The backend consists of:
- A POST endpoint (`PurchasePack`) that accepts purchase requests and returns an order ID
- A GET endpoint (`GetPacks`) that returns available LARP packs
- A background `PurchaseProcessor` using Service Bus Queue Trigger pattern (currently simulated locally)

I used the **Isolated Worker model** in .NET 10, implemented proper CORS for the frontend, added structured logging, and made sure the API handles OPTIONS preflight requests correctly.

This project helped me practice skills directly relevant to production support:
- Troubleshooting function startup issues (Application Insights compatibility, multiple project files)
- Handling CORS and browser security constraints
- Implementing async/background processing patterns
- Monitoring and logging in serverless environments
- Working with local emulators (Azurite)

Even though the theme is fun, the architecture follows production best practices — separation of concerns, clean error handling, and readiness for real Azure Service Bus + SQL integration."

#### Other strong points you can mention:

- "I had to debug several real-world issues: connection refused on Service Bus, double /api routes, and worker process crashes — which gave me good experience in troubleshooting Azure Functions."
- "The frontend is a single HTML file using Tailwind, making it easy to demo without hosting."
- "I deliberately kept the Service Bus part simulated locally because there's no good free emulator, but the code is ready to switch to real Azure Service Bus with minimal changes."

---

Would you like me to also add:
- A small "Future Improvements" section to the README (real SQL, real Service Bus, Application Insights, etc.)?
- Any specific changes to the README tone?

Just say if you're happy with this or want any tweaks.

You're now in a **very solid position** for your interview with this project!