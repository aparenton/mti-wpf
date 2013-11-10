using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.DataAccess.Observation
{
    public class ObservationDataAccess : IObservationDataAccess
    {
        public bool AddObservation(int idPatient, ServiceObservation.Observation obs)
        {
            ServiceObservation.ServiceObservationClient client = new ServiceObservation.ServiceObservationClient();
            bool res;

            try
            {
                res = client.AddObservation(idPatient, obs);
                client.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PatientDataAccess: GetPatient: Exception:" + ex.Message);
                res = false;
            }

            return res;
        }
    }
}
