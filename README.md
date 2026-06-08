# TechShop

TechShop is a robust, full-stack .NET 10 MVC e-commerce platform dedicated to electronic devices and tech peripherals. The application features separate user experiences based on role management, server-side data processing, and clean architecture principles.

---

## 👥 Roles & Permissions

The application relies on role-based authorization to secure endpoints and manage order workflows:

### 📱 Customer (User)
Has access to standard e-commerce shopping and checkout functionalities:
* **Product Catalog:** Browse through all available tech products.
* **Advanced Filtering:** Dynamically filter products by **Category** and **Price Range** to find specific hardware easily.
* **Cart Management:** Add products to a persistent shopping cart, modify item quantities, or clear the cart.
* **Order & Checkout:** Proceed to checkout to place an order securely from the cart.

### ⚙️ Administrator
Has full oversight of the platform's ecosystem and fulfillment cycle:
* **Product Management:** Complete CRUD capabilities (Create, Read, Update, Delete) for items, inventory pricing, and categorization.
* **Order Lifecycle Management:** Direct control over the order pipeline. Admins can update incoming orders to the following states:
  * 📥 **Set in Queue:** Mark newly placed orders as received and awaiting processing.
  * ✅ **Set Completed:** Finalize orders once they have been successfully fulfilled/shipped.
  * ❌ **Cancel Order:** Terminate orders due to stock issues or customer requests.
* **Dashboard Control:** Full administrative override for platform operations.

> 📝 **Note on Upcoming Features:** Expanded Administrative delegation (allowing an Admin to grant admin privileges to existing users directly from the UI) is currently under design and will be introduced in the next development cycle.

---

## 🛠️ Tech Stack

* **Backend:** .NET 10 / C# / ASP.NET Core MVC
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First approach)
* **Authentication:** ASP.NET Core Identity (Cookie-based / Role assignment)
* **Frontend:** Razor Views, Bootstrap, HTML5, CSS3, JavaScript

---

## 🏁 Getting Started & Installation

Follow these steps to set up and run a copy of the project locally on your machine for development and testing.

### 1. Prerequisites
Ensure you have the following installed:
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
* [Visual Studio](https://visualstudio.microsoft.com/)

### 2. Clone the Repository
```bash
git clone https://github.com/Thomas-Maged/TechShop.git
cd TechShop
```

### 3. Configure the Database Connection
1. Open the project folder.
2. Locate the base `appsettings.json` file. 
3. Replace the `DefaultConnection` placeholder with your local SQL Server instance details:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_LOCAL_SERVER;Database=TechShopDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Apply Database Migrations
Open your terminal (or Package Manager Console in Visual Studio) at the root of the project and execute the following command to build the schema and seed the roles:

```bash
dotnet ef database update
```

### 5. Default Admin Credentials (Data Seeding)
The application automatically seeds a default system administrator account into the database upon creation so you can test administrative features instantly without manual SQL insertion.
* Default Admin Email: `admin@techshop.com`
* Default Admin Password: `Admin@123` (Ensure you change this in production settings!)

### 6. Run the Application
Run the project using the .NET CLI:
```bash
dotnet run
```
