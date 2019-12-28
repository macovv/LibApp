
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.DTOs
{
    public class AppUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual string UserName { get; set; }
    }
}
