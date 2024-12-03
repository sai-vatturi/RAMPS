using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeMeal.Core.DTOs.Auth
{
    public class AssignRoleDto
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public bool IsApproved { get; set; }
    }
}
