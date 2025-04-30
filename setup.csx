// setup.csx
#r "nuget: Microsoft.Build.Locator, 1.4.1"

using System;
using System.Diagnostics;

string project = "StudentManagement.Api";

string[] packages = new[]
{
    "Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.5",
    "Microsoft.EntityFrameworkCore.SqlServer",
    "Microsoft.EntityFrameworkCore.Tools",
    "BCrypt.Net-Next"
};

foreach (var pkg in packages)
{
    Console.WriteLine($"➡️ Instalace: {pkg}");
    var process = Process.Start(new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = $"add {project} package {pkg}",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false
    });

    process.WaitForExit();
    Console.WriteLine(process.StandardOutput.ReadToEnd());
}
Console.WriteLine("Instalace NuGet balíčků dokončena.");
