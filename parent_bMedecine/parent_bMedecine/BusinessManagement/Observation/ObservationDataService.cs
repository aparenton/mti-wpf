using parent_bMedecine.DataAccess.Observation;

namespace parent_bMedecine.BusinessManagement.Observation
{
    public class ObservationDataService : IObservationDataService
    {
        private readonly IObservationDataAccess _observationDataAccess;

        public ObservationDataService(IObservationDataAccess observationDataAccess)
        {
            _observationDataAccess = observationDataAccess;
        }

        public bool AddObservation(int idPatient, ServiceObservation.Observation obs)
        {
            return _observationDataAccess.AddObservation(idPatient, obs);
        }
    }
}