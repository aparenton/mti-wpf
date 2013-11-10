using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parent_bMedecine.DataAccess.Patient
{
    public interface IPatientDataAccess
    {
        /// <summary>
        /// Retrieve all patients
        /// </summary>
        /// <returns>a list of patients</returns>
        List<ServicePatient.Patient> GetListPatient();

        /// <summary>
        /// Retrieve a unique patient with id
        /// </summary>
        /// <param name="id">patient's id</param>
        /// <returns>a patient if he exists, null otherwise</returns>
        ServicePatient.Patient GetPatient(int id);

        /// <summary>
        /// Add a patient
        /// </summary>
        /// <param name="patient">the patient to add</param>
        /// <returns>true if added, false otherwise</returns>
        bool AddPatient(ServicePatient.Patient patient);

        /// <summary>
        /// Delete a patient
        /// </summary>
        /// <param name="id">patient's id</param>
        /// <returns>true if deleted, false otherwise</returns>
        bool DeletePatient(int id);
    }
}
