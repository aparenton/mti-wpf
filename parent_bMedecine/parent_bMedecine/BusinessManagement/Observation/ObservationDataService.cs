using parent_bMedecine.DataAccess.Observation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
