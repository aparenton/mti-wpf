using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.Message
{
    public class OnPatientSelectionMessage : MessageBase
    {
        public ServicePatient.Patient SelectedPatient { get; set; }

        public OnPatientSelectionMessage(ServicePatient.Patient selectedPatient)
        {
            SelectedPatient = selectedPatient;
        }
    }
}
