using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.Message
{
    /// <summary>
    /// Message sent to ViewModels for reset on Logout
    /// </summary>
    public class OnLogoutMessage : MessageBase
    {
        public OnLogoutMessage()
        {
        }
    }
}
