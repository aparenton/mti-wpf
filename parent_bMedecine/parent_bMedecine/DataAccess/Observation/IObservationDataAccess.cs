using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.DataAccess.Observation
{
    public interface IObservationDataAccess
    {
        /// <summary>
        /// Add observation for a given patient
        /// </summary>
        /// <param name="idPatient">the patient's id</param>
        /// <param name="obs">the observation to be added</param>
        /// <returns>true if added, false otherwise</returns>
        bool AddObservation(int idPatient, ServiceObservation.Observation obs);
    }
}
