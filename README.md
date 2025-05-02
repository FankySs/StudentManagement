# StudentManagement

Víceprojektová .NET 8 aplikace pro správu studentů, předmětů, známek a výuky. Vytvořeno podle zadání z VUT-FEKT BPC-OOP.

## 🏗️ Architektura řešení

Navigace v projektech a vzájemné vztahy:

```
Solution: StudentManagement.sln
├─ StudentManagement.Shared      # Sdílené DTOs, entitní typy a rozhraní
├─ StudentManagement.Api         # REST API (Web API)  
└─ StudentManagement.Web         # Front-end (Razor Pages)
```

1. **Sdílená vrstva (Shared)**

   * **DTOs & Entita**: Definuje objekty (`StudentDto`, `CourseDto`, `GradeDto`) a mapování na databázové entity.
   * **Rozhraní**: `IRepository<T>`, `IUnitOfWork` pro abstrakci přístupu k datům.
   * **Společné knihovny**: Validace (Data Annotations), chybové třídy a helpery.

2. **API vrstva (StudentManagement.Api)**

   * **Architektura**: Onion/Hexagonální styl s vrstvením:

     * Controllers – přijímají HTTP požadavky, volají Servisy, vrací DTO.
     * Services (Business Logic) – implementují use-cases (CRUD operace s validacemi a pravidly).
     * Repositories (Data Access) – Entity Framework Core, `DbContext` s `DbSet<Student>`, `DbSet<Course>`, `DbSet<Grade>`.
     * Shared Interfaces – závisí na rozhraních z Shared projektu.
   * **Bezpečnost**: JWT autentizace + autorizace.
   * **Konfigurace**: `appsettings.json` + proměnné prostředí (DB, JWT klíče).

3. **Webová vrstva (StudentManagement.Web)**

   * **Razor Pages**: Frontální UI s překrytím těch nejčastějších stránek: `Index`, `Students`, `Courses`, `Grades`.
   * **Volání API**: `HttpClient` nastavený na base URL API, serializace JSON pomocí `System.Text.Json`.
   * **Validace**: jQuery Validation + Unobtrusive pro client-side checks.
   * **Styling**: základní CSS + Bootstrap.

4. **Databázová vrstva**

   * **EF Core Migrations**: migrace v Api projektu, samostatná složka `Migrations`.
   * **Context**: `StudentManagementDbContext`, injektovaný přes DI.


![{D979A733-BC31-4D9F-9684-D8588144D197}](https://github.com/user-attachments/assets/86b939f2-a622-47aa-9409-76498ad5c606)

