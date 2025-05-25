# 🧪 Technical Assessment – Forum API

This project is a technical submission for the Backend Software Engineer assessment. It implements a simple web forum API using ASP.NET Core with authentication, post/comment/reaction functionality, and moderation capabilities.

## 🛠️ Tech Stack

- **.NET 9** – ASP.NET Core Web API
- **Entity Framework Core** – ORM for SQL
- **SQL Server** – Using `MSSQLLocalDB`
- **Authentication** – In-built Identity + JWT token handling
- **Docker** – Optional API hosting
- **Postman** – API testing collection provided
- **Testing** – xUnit + WebApplicationFactory + InMemory DB

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) (recommended)
- [Docker](https://www.docker.com/) (optional, for container run)
- Postman (for API testing)

### ⚙️ Setup & Run

1. **Clone the Repository**
   ```bash
   git clone https://github.com/GarethMonty/iidentifiiblog.git
   cd iidentifiiblog_GarethM
   ```

2. **Run the App via Visual Studio**

   Launch profiles provided in `launchSettings.json`:

   - **Http/Https** – Starts the API server only.
   - **UI** – Starts a basic UI.
   - **Docs** – Opens Redoc documentation in browser at `/docs`.
   - **Container (.NET SDK)** – For Docker run (requires Docker installed).

3. **Database Seeding**

   On startup:
   - The MSSQLLocalDB is used (`(localdb)\MSSQLLocalDB`)
   - Existing data is wiped.
   - A mix of **constant** and **random** data is seeded.
   - Known GUIDs are used to facilitate Postman testing.

> 📌 Ensure MSSQLLocalDB is installed and accessible. If needed, change the DB connection in `appsettings.json`.

---

## 📬 API Documentation

### 🔴 Redoc

Start the project with `Docs` launch profile or navigate to:

```
https://localhost:7113/docs
```

---
## 🖥️ Basic UI Features

While this is primarily an API-first project, a minimal UI is available when running the application with the **Login UI** launch profile. After logging in through the Identity UI, users will be redirected to a basic post listing interface.

### UI Capabilities:
- Lists all available posts
- Provides basic search and filtering options
- Supports paging through the post list
- Allows authenticated users to react to posts (e.g., like/dislike)

### Known Issues:
- Selecting a post that has no reactions will currently throw an error
- The UI is intentionally minimal and was implemented only to demonstrate login flow and basic interaction

---

## 🔑 Authentication
The service will dynamically use either the JWT token or cookie authentication based on the request context.

- **Cookie Authentication** – Used for login through the UI (Login UI launch profile).
- **JWT Token Authentication** – Used for secure access to API endpoints.
- Login credentials are included in the Postman collection.
- Roles: `User`, `Moderator`.

---

## 🧲 Postman Collection

### File: `IIdentifii_Blog.postman_collection.json`

- Includes authentication flows.
- Runs pre-request scripts to fetch fresh tokens.
- Uses seeded GUIDs to interact with known test data.
- Endpoints cover:
  - 🔐 Auth
  - 📬 Blog Posts
  - 💬 Comments
  - ❤️ Reactions (extensible: like, dislike, etc.)
  - 🚨 Moderator Tags

> 🛠 Pre-filled variables (user credentials, tokens, GUIDs) provided for convenience.

---

# 🧱 Project Architecture – Forum API

This architecture supports:
- Clear separation of concerns
- Strong testability
- Configurable features
- API-centric extensibility

The design is kept intentionally simple for clarity, but can scale up with additional requirements (e.g., caching, external services, logging, etc.).

## 🧩 Layered Architecture Overview

The project is organized into clearly defined layers to promote maintainability, testability, and separation of concerns.

```plaintext
┌────────────┐
│    Tests   │ ◄──────────── Integration testing via WebApplicationFactory & InMemory DB
└────┬───────┘
     │
┌────▼───────┐
│    API     │ ◄──────────── REST controllers, routing, auth, error handling, feature flags
└────┬───────┘
     │
┌────▼────────────┐
│  BusinessLogic   │ ◄──────────── Service layer, validation, transformation (entity ⇄ DTO)
└────┬─────────────┘
     │
┌────▼────────┐
│ Repository  │ ◄──────────── EF Core + Repository Pattern
└────┬────────┘
     │
┌────▼──────┐
│  Shared   │ ◄──────────── Enums, constants, DTOs, interfaces
└───────────┘
```

---

## 🏗️ Applied Design Patterns

### ✅ Repository Pattern
Encapsulates all data access logic, abstracting the EF Core DbContext behind interfaces.

### ✅ DTO Pattern
Used to decouple internal data models from API responses and requests.

### ✅ Options Pattern
Used to bind and validate settings from `appsettings.json`, e.g., for feature flags.

---

## 🧠 Architectural Considerations

### Authentication Strategies
- **API Access**: Secured using JWT Bearer Tokens
- **UI Access**: Secured using ASP.NET Core Cookie Authentication (via Identity)

### Data Seeding
- On startup, the database is cleared and seeded with:
  - Constant, predictable values (for use in tests/Postman)
  - Randomized data for broader coverage

### Feature Flags
- Toggle certain API functionality via configuration (e.g., enabling/disabling routes)
- Implemented using ASP.NET Options pattern

### Testing
- Use InMemoryDatabase and seeded data
- Tests mimic real client behavior using HTTP requests

---

## 🔄 Extensibility

### Reactions System
- Designed as a flexible mechanism (not limited to 'likes')
- Easily extendable with new reaction types (e.g., love, haha, dislike)

### Moderation Tags
- Tags such as "Misleading" or "False Information" are defined via enums
- System can be extended with new moderation categories

### API-first Design
- Built as a headless API with Postman Collection as the main interaction method
- No frontend UI provided except for ASP.NET Identity login page (Login UI profile)

---

## 🔧 Configuration

- All settings handled via `appsettings.json`.
- Feature flags toggle specific API behaviors.
- Uses ASP.NET Core's built-in **Options pattern**.

---

## 🧪 Integration Testing

- Located in `Tests` project.
- Based on `WebApplicationFactory`.
- Uses **InMemoryDatabase** for fast isolated test runs.
- Mimics the app's data seeding process.
- Tests focus on high-level API behavior, not exhaustive.

Run tests using:

```bash
dotnet test
```

or directly via Visual Studio Test Explorer.

---

## 💡 Functional Highlights

- **Authentication & Authorization**
  - Login, token issuance
  - Role-based access: User vs Moderator

- **Posts**
  - Create, retrieve, filter, sort
  - Pagination & author/date/tag filtering

- **Comments**
  - Add, retrieve, update, delete (no editing on posts per spec)

- **Reactions**
  - Flexible like/dislike/love system
  - One reaction per user per post

- **Moderation**
  - Tag posts with "misleading"/"false information"
  - Extensible tag types

- **Feature Flags**
  - Toggle API features from config

---

## 🦪 Example Credentials (From Postman Variables)

```json
"email": "user@example.com"
"password": "User123!"
"moderatorEmail": "moderator@example.com"
"moderatorPassword": "Moderator123!"
```

---