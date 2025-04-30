using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Shared.DTOs
{
    public class CreateUserDto
    {
        public string UzivatelskeJmeno { get; set; } = string.Empty;
        public string Heslo { get; set; } = string.Empty;
        public string Role { get; set; } = "Ucitel";
    }
}
