using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.Message
{
    /// <summary>
    /// Message sent to ViewModels for initilization on Login
    /// </summary>
    public class OnLoginMessage : MessageBase
    {
        public string UserAccountName { get; set; }

        public OnLoginMessage(string userAccountName)
        {
            UserAccountName = userAccountName;
        }
    }
}
