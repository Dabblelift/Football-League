# ⚽ Football League API

A RESTful Web API for managing football league matches and teams, built with **ASP.NET Core**, **Entity Framework Core**, and **Clean Architecture** principles.

## 🚀 Features

- CRUD operations for Matches and Teams
- Team statistics auto-calculated from match results
- Global exception handling and custom error messages
- Strategy pattern for result processing and sorting
- DTO-based data access for clean separation
- Swagger documentation

---

## 🛠️ Getting Started

### Prerequisites

- [.NET 8+ SDK](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)

---

### 🚧 Setup

1. **Clone the repository**

```bash
git clone https://github.com/Dabblelift/Football-League.git
cd Football_League
```

2. **Update the DB connection**

In `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=FootballLeagueDB;Trusted_Connection=True;"
}
```

3. **Apply Migrations**

```bash
dotnet ef database update
```

4. **Run the API**

```bash
dotnet run
```

---

## 📘 API Endpoints

Once the app is running, open Swagger UI at:

```
https://localhost:7297/swagger
```

### Examples

- `GET /Match/getAll` – List all matches
- `POST /Match/Add` – Create a match
- `PUT /Match/update` – Update a match
- `DELETE /Match/delete?id={id}` – Delete a match
  
- `GET /Team/getAll?sortBy={criteria}` – Get all teams sorted
- `POST /Team/Add` – Create a team
- `PUT /Team/update` – Update a team
- `DELETE /Team/delete?id={id}` – Delete a team
  
> Use `sortBy` enum: `GoalDifference`, `Wins`, etc.

---

## 🧠 Architecture Highlights

- **Clean Separation**: Controllers → Services → Repositories → EF Models
- **Strategy Pattern**: Easily switch scoring or sorting logic
- **Global Exception Middleware**: All errors handled consistently
- **Validation**: DTO annotations + model validation filters

---

## 📦 Technologies

- ASP.NET Core 8
- Entity Framework Core
- Swagger (Swashbuckle)
- SQL Server

---

## 👨‍💻 Author

Made by Erik Hayrelov

---
