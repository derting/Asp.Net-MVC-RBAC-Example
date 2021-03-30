using System;

namespace RBACExample.Models
{
    public class UserInfo
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public DateTime LoginTime { get; set; }
    }
}