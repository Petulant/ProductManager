# Product Manager System - Technical Documentation

## 1. Project Overview
A full-stack application utilizing **ASP.NET Core MVC** and **Web API** to manage product and categories.

### Tech Stack
*   **Frontend:** ASP.NET Core MVC (Razor Views)
*   **Backend:** ASP.NET Core Web API
*   **Database:** SQL Server (LocalDB)

---

## 2. Set-up Instructions

### Step 1: Database Configuration
1. Open the **Web Project**.
2. Locate `appsettings.json`.
3. Update the `ConnectionStrings` section to point to your local SQL Server instance:
   ```json
   "ProductManagerConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductManagerDB;..."
   ```

### Step 2: Database Migration
1. Open **Package Manager Console**.
2. Set Default Project to **ProductManager.Infrastructure**.
3. Execute: `Update-Database`

### Step 3: Running the Application
1. Right-click the **Solution** > **Properties**.
2. Select **Multiple Startup Projects**.
3. Set **API** and **Web** projects to **Start**.
4. Press **F5**.

---

## 3. Implemented Requirements (Section 5)

*   **5.2 Performance:** Implemented server-side pagination for the Product list.
*   **5.3 Validation:** Added server-side validation using `ModelState` and `asp-validation-summary`.
*   **5.7 Documentation:** Full technical setup guide (this document).

---

## 4. System Architecture
The solution is divided into four main layers:
1. **Domain:** Entities and business logic.
2. **Infrastructure:** Data access and EF Core Context.
3. **API:** RESTful endpoints for data operations.
4. **Web:** The MVC user interface.

## 5. Entity Relationship Diagram (ERD)
The system uses a **One-to-Many** relationship between Categories and Products:
*   One **Category** can have multiple **Products**.
*   Each **Product** belongs to exactly one **Category** via the `CategoryId` Foreign Key.

Entity Relationship Diagram (ERD)
![Database ERD](./ERD.png)

