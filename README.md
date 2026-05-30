# E-Commerce Book Application (BulkyBook)

ASP.NET Core MVC bookstore built on the [BulkyBook](https://www.udemy.com/course/complete-aspnet-core-21-course) tutorial pattern: layered architecture, Entity Framework Core, ASP.NET Core Identity, and an admin area for catalog management.

## Features

| Area | Status |
|------|--------|
| Product catalog (browse, details) | Done |
| Admin CRUD — categories, products, companies | Done |
| Product images (upload to `wwwroot/images/product/`) | Done |
| ASP.NET Core Identity (register, login, roles) | Done |
| Company accounts (B2B registration) | Done |
| Shopping cart, checkout, orders | Planned |

### Roles

Defined in `BulkyBook.Utility.SD`:

- `Customer` — default for new users
- `Company` — linked to a `Company` record at registration
- `Admin`
- `Employee`

## Tech stack

- **.NET 10** — `BulkyBookWeb`
- **ASP.NET Core MVC** with Areas (`Customer`, `Admin`, `Identity`)
- **Entity Framework Core 10** + **PostgreSQL** (Npgsql)
- **Repository pattern** + **Unit of Work**
- **ASP.NET Core Identity** (Razor Pages under `Areas/Identity`)

## Solution structure

```
e-commerce-book-application/
├── BulkyBookWeb/              # Main web application
├── BulkyBook.Models/          # Domain models and view models
├── BulkyBook.DataAccess/      # DbContext, repositories, migrations
├── BulkyBook.Utility/         # Shared constants (roles) and EmailSender
├── DI_Service_Lifetime/       # DI lifetime demo (referenced by web project)
├── RazorPagesApp/             # Standalone Razor Pages sample
└── BulkyBookWeb.slnx          # Solution file
```

Open the solution:

```bash
dotnet restore BulkyBookWeb.slnx
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) (local or remote)

## Database setup

1. Create a PostgreSQL database (default name in config: `Bulky`).

2. Set the connection string in `BulkyBookWeb/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=Bulky;Username=YOUR_USER;Password=YOUR_PASSWORD"
  }
}
```

3. Apply migrations from the repository root:

```bash
dotnet ef database update \
  --project BulkyBook.DataAccess/BulkyBook.DataAccess.csproj \
  --startup-project BulkyBookWeb/BulkyBookWeb.csproj
```

Seed data (categories, sample books, companies) is applied via `ApplicationDBContext` in `BulkyBook.DataAccess/Data/ApplicationDBContext.cs`.

### Add a new migration

```bash
dotnet ef migrations add MigrationName \
  --project BulkyBook.DataAccess/BulkyBook.DataAccess.csproj \
  --startup-project BulkyBookWeb/BulkyBookWeb.csproj
```

## Run the application

```bash
cd BulkyBookWeb
dotnet run
```

Or from the solution root:

```bash
dotnet run --project BulkyBookWeb/BulkyBookWeb.csproj
```

Default URLs (see `BulkyBookWeb/Properties/launchSettings.json`):

- HTTP: `http://localhost:5243`
- HTTPS: `https://localhost:7061`

The default route lands on the **Customer** area product list: `{area=Customer}/{controller=Home}/{action=Index}`.

## Application areas

| Area | Purpose |
|------|---------|
| **Customer** | Public catalog — product list and details |
| **Admin** | Manage categories, products (`Upsert` + images), and companies |
| **Identity** | Account registration, login, password reset, 2FA |

Admin endpoints are not restricted with `[Authorize]` yet; add role-based authorization before production use.

## Architecture notes

- Controllers use `IUnitOfWork` instead of injecting `ApplicationDBContext` directly.
- Generic `Repository<T>` supports optional EF `Include` via the `includeProperties` argument (comma-separated navigation paths).
- Identity is registered with `IdentityUser` in `Program.cs`; `ApplicationUser` exists in the models layer for extended profile/company fields and may be wired in later.

## Configuration

| File | Purpose |
|------|---------|
| `BulkyBookWeb/appsettings.json` | Base logging and host settings |
| `BulkyBookWeb/appsettings.Development.json` | Connection string (development) |

Email sending uses `IEmailSender` → `BulkyBook.Utility.EmailSender`; configure SMTP or a provider before relying on confirmation emails in production.

## Related projects in this repo

- **RazorPagesApp** — minimal Razor Pages template, not part of the main storefront.
- **DI_Service_Lifetime** — dependency injection scope demo referenced by `BulkyBookWeb`.

## License

No license file is included in this repository. Add one if you plan to distribute or open-source the project.
