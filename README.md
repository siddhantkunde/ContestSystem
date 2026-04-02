🎯 Contest Participation System
📌 Overview
A role-based backend system where users can participate in contests, answer questions, and compete on a leaderboard.
Supports:
•	VIP, Normal, Admin, Guest roles
•	Multiple question types
•	Scoring & leaderboard
•	Prize tracking
________________________________________
🚀 Tech Stack
•	ASP.NET Core Web API (.NET 8)
•	Entity Framework Core
•	SQLite (No external DB required)
•	JWT Authentication
•	Swagger & Postman
________________________________________
⚙️ Setup Instructions
1️⃣ Clone Repository
git clone https://github.com/siddhantkunde/ContestSystem.git
cd ContestSystem
________________________________________
2️⃣ Restore Packages
dotnet restore
________________________________________
3️⃣ Apply Database Migration
dotnet ef database update
________________________________________
4️⃣ Run Application
dotnet run
________________________________________
5️⃣ Open Swagger
http://localhost:xxxx/swagger
________________________________________
🔐 Authentication
Login
POST /api/auth/login
{
  "username": "user",
  "password": "123"
}
👉 Copy token and use in:
Authorization → Bearer Token
________________________________________
👥 User Roles
Role	Access
Admin	Manage + view all contests
VIP	Access VIP + Normal contests
Normal	Access only normal contests
Guest	View public contests only
________________________________________
📚 API Endpoints
🔓 Public APIs
•	GET /api/contest/public
________________________________________
🔐 Auth APIs
•	POST /api/auth/login
•	POST /api/auth/signup
________________________________________
🏆 Contest APIs
•	GET /api/contest
•	GET /api/contest/{contestId}/questions
•	POST /api/contest/{contestId}/submit
•	GET /api/contest/available
________________________________________
📊 Leaderboard
•	GET /api/leaderboard/{contestId}
________________________________________
👤 User APIs
•	GET /api/user/history
•	GET /api/user/in-progress
•	GET /api/user/prizes
________________________________________
🧠 Features Implemented
✅ Role-based access control
✅ Contest access validation
✅ Multiple question types:
•	Single-select
•	Multi-select
•	True/False
✅ Score calculation
✅ Leaderboard ranking
✅ Prize distribution (winner detection)
✅ In-progress contests tracking
✅ Prevent multiple submissions
✅ Validation checks:
•	Question belongs to contest
•	Options belong to question
✅ Rate limiting (basic)
✅ JWT authentication
________________________________________
🧪 Test Users
Username	Password	Role
admin	123	Admin
vip	123	VIP
user	123	Normal
________________________________________
🗄 Database
•	SQLite (auto-created)
•	EF Core Migrations used
Run:
dotnet ef database update
________________________________________
🎯 How to Test
1.	Login → get token
2.	Add token in Postman
3.	Fetch contests
4.	Get questions
5.	Submit answers
6.	Check leaderboard
7.	Check history / prizes
________________________________________
🏁 Conclusion
This project demonstrates a complete backend system with:
•	Authentication
•	Role-based authorization
•	Contest lifecycle
•	Scoring & leaderboard
•	User tracking
________________________________________
👨‍💻 Author
Siddhant Kunde
