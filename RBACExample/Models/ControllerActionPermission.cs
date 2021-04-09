using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBACExample.Models
{
    public class ControllerActionPermission : ControllerAction
    {
        public string Permission { get; set; }

        public bool IsAllow { get; set; }
    }
}