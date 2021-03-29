using System;

namespace RBACExample.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class AuthBreadcrumbAttribute : System.Attribute
    {
        public string Name { get; set; }
        public AuthBreadcrumbAttribute(string name)
        {
            this.Name = name;
        }
    }
}