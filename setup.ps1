Write-Host "Instalace NuGet balíèkù pro StudentManagement.Api..."

$project = "StudentManagement.Api"

$packages = @(
    "Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.5",
    "Microsoft.EntityFrameworkCore.SqlServer",
    "Microsoft.EntityFrameworkCore.Tools",
    "BCrypt.Net-Next"
)

foreach ($pkg in $packages) {
    Write-Host "Instalace balíèku: $pkg"
    dotnet add $project package $pkg
}

Write-Host "Hotovo – všechny balíèky byly nainstalovány."
