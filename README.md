# BlockChainApi

A .NET Core Web API that fetches blockchain data from BlockCypher and stores historical snapshots in a database.

The project is designed to demonstrate **Clean Architecture**, **SOLID principles**, **async/parallel patterns**, and **enterprise best practices**, as requested in the ICMarkets technical assessment.

---

## 📐 Architecture

The solution follows **Clean / Vertical Architecture**:

```
BlockChainApi
│
├── src
│   ├── BlockChain.Api           # API layer (Controllers, DI, Swagger, Health)
│   ├── BlockChain.Application   # Application layer (Interfaces, Services, DTOs)
│   ├── BlockChain.Infrastructure    # Infrastructure (EF Core, Repositories, Unit of Work)
│   └── BlockChain.Domain        # Shared entities & enums
│
├── tests
│   ├── BlockChain.UnitTests
│   ├── BlockChain.FunctionalTests
│   └── BlockChain.IntegrationTests
│
└── BlockChainApi.sln
```

### Design Patterns Used
- **Repository Pattern** – data access abstraction
- **Unit of Work Pattern** – transaction boundary & persistence control

---

## 🔗 External APIs

The application fetches data from the following BlockCypher endpoints:

- Ethereum: `https://api.blockcypher.com/v1/eth/main`
- Dash: `https://api.blockcypher.com/v1/dash/main`
- Bitcoin (Main): `https://api.blockcypher.com/v1/btc/main`
- Bitcoin (Testnet): `https://api.blockcypher.com/v1/btc/test3`
- Litecoin: `https://api.blockcypher.com/v1/ltc/main`

The **raw JSON response** from each API is stored as-is in the database.

---

## 📦 Data Storage

- Database: **SQLite (EF Core)**
- Each request is stored as a **snapshot** with:
  - `Id`
  - `Chain`
  - `Json` (raw API response)
  - `CreatedAt` (UTC timestamp)

Historical data is always returned in **descending order by CreatedAt**.

---

## 🚀 API Endpoints

### Sync / Fetch (HTTP requests to BlockCypher)

- **Sync single blockchain**
  ```
  POST /api/v1/blockchains/{chain}/sync
  ```

- **Sync all blockchains**
  ```
  POST /api/v1/blockchains/sync
  ```

> External API calls are executed **in parallel**, while database persistence is handled **sequentially** due to SQLite write limitations.

---

### History / Read

- **Get full history**
  ```
  GET /api/v1/blockchains/{chain}/history?page=1&pageSize=50
  ```

- **Get latest snapshot**
  ```
  GET /api/v1/blockchains/{chain}/latest
  ```

Read-only queries use `AsNoTracking()` for better performance.

---

## 🩺 Health & CORS

- **Health Check**
  ```
  GET /health
  ```

- **CORS**
  - Basic permissive policy enabled (AllowAnyOrigin / Header / Method)

---

## 🧪 Testing

The solution includes **three test projects**:

- **Unit Tests**
  - Test application services using mocks

- **Functional Tests**
  - Test API endpoints using `WebApplicationFactory`

- **Integration Tests**
  - Test repositories with a real SQLite database (in-memory)

Run all tests:
```bash
dotnet test
```

---

## 🐳 Docker (Linux)

### Build & Run
```bash
docker compose up --build
```

Swagger will be available at:
```
http://localhost:8080/swagger
```

---

## ▶️ Run Locally

### Requirements
- .NET SDK **8.0+**

### Run API
```bash
dotnet run --project src/BlockChain.Api
```

Swagger:
```
http://localhost:{port}/swagger
```

---

## ⚙️ Technical Highlights

- .NET 8 (LTS)
- Clean Architecture & SOLID
- Repository + Unit of Work patterns
- Async/await throughout
- Parallel external API calls
- Sequential persistence (SQLite-safe)
- Swagger/OpenAPI
- Health checks & CORS
- Structured logging (`ILogger<T>`)
- Automatic request validation
- Docker (Linux)

---

## 📌 Notes

- SQLite does not support concurrent writes.
  For this reason, external API calls are parallelized, while database writes are executed sequentially.
- The architecture allows easy migration to PostgreSQL / SQL Server for full parallel persistence.

---

## 👤 Author
Prepared as part of a technical assessment for ICMarkets.

