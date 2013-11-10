using parent_bMedecine.DataAccess.Patient;
using System.Collections.Generic;

namespace parent_bMedecine.BusinessManagement.Patient
{
    public class PatientDataService : IPatientDataService
    {
        private readonly IPatientDataAccess _patientDataAccess;

        public PatientDataService(IPatientDataAccess patientDataAccess)
        {
            _patientDataAccess = patientDataAccess;
        }

        public List<ServicePatient.Patient> GetListPatient()
        {
            return _patientDataAccess.GetListPatient();
        }

        public ServicePatient.Patient GetPatient(int id)
        {
            return _patientDataAccess.GetPatient(id);
        }

        public bool AddPatient(ServicePatient.Patient patient)
        {
            return _patientDataAccess.AddPatient(patient);
        }

        public bool DeletePatient(int id)
        {
            return _patientDataAccess.DeletePatient(id);
        }
    }
}