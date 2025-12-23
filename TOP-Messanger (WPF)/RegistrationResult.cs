using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOP_Messanger
{
    public class RegistrationResult
    {
        public bool IsValid { get; set; }
        public bool IsGuest { get; set; }
        public bool IsServer { get; set; }
        public string Login { get; set; }
    }
}
