using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.Utilities
{
    public class RoleManager
    {
        public static bool IsReadOnlyRole(string role)
        {
            return role.ToLower().Equals("infirmiere") || role.ToLower().Equals("infirmière");
        }
    }
}
