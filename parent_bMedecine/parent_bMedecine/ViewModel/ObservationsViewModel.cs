using GalaSoft.MvvmLight;
using parent_bMedecine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace parent_bMedecine.ViewModel
{
    public class ObservationsViewModel : ViewModelBase
    {
        #region Members
        private Dbo.Patient _selectedPatient;
        private ObservableCollection<Dbo.Observation> _observations = new ObservableCollection<Dbo.Observation>();
        private int _selectedObservationIndex;
        private ObservableCollection<byte[]> _pictureList = new ObservableCollection<byte[]>();
        #endregion // Members

        #region Properties
        public ObservableCollection<ChartObject> Weights { get; private set; }
        public ObservableCollection<ChartObject> BloodPressures { get; private set; }
        public ObservableCollection<ChartObject> Temperatures { get; private set; }
        public ObservableCollection<ChartObject> Hearts { get; private set; }

        public Dbo.Patient SelectedPatient
        {
            get
            {
                return _selectedPatient;
            }
            set
            {
                _selectedPatient = value;
                RaisePropertyChanged("SelectedPatient");
            }
        }

        public ObservableCollection<Dbo.Observation> Observations
        {
            get
            {
                return _observations;
            }
            set
            {
                _observations = value;
            }
        }

        public ObservableCollection<byte[]> PictureList
        {
            get
            {
                return _pictureList;
            }
            set
            {
                _pictureList = value;
            }
        }

        public int SelectedObservationIndex
        {
            get
            {
                return _selectedObservationIndex;
            }
            set
            {
                _selectedObservationIndex = value;
                RaisePropertyChanged("SelectedObservationIndex");
            }
        }
        #endregion // Properties

        #region Constructors
        private object selectedItem = null;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                // selected item has changed
                selectedItem = value;
            }
        }

        public ObservationsViewModel()
        {
            Weights = new ObservableCollection<ChartObject>();
            BloodPressures = new ObservableCollection<ChartObject>();
            Hearts = new ObservableCollection<ChartObject>();
            Temperatures = new ObservableCollection<ChartObject>();

            // Commands

            // Messages
            MessengerInstance.Register<Message.OnLogoutMessage>(this, m => { Reset(); });
            MessengerInstance.Register<Message.OnPatientSelectionMessage>(this, m => { OnPatientSelectionExecute(m.SelectedPatient); });
        }
        #endregion // Constructors

        #region Methods
        private void OnPatientSelectionExecute(Dbo.Patient patient)
        {
            if (patient == null)
                return;

            Reset();
            SelectedPatient = patient;

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
            //ServiceLive.ServiceLiveClient client2 = new ServiceLive.ServiceLiveClient(null);

            try
            {
                List<Dbo.Observation> res = client.GetPatient(SelectedPatient.Id).Observations;
                if (!res.Any())
                    MessengerInstance.Send<Message.WhenNoObservationMessage>(new Message.WhenNoObservationMessage());
                foreach (var observation in res)
                {
                    Observations.Add(observation);
                    foreach (byte[] img in observation.Pictures)
                        PictureList.Add(img);
                    Weights.Add(new ChartObject() { Category = observation.Date.ToString(), Number = observation.Weight });
                    BloodPressures.Add(new ChartObject() { Category = observation.Date.ToString(), Number = observation.BloodPressure });                       
                }
                SelectedObservationIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des observations, veuillez réessayer.", "Erreur");
            }
        }

        private void Reset()
        {
            Observations.Clear();
            PictureList.Clear();

            Weights.Clear();
            BloodPressures.Clear();
        }
        #endregion // Methors
    }
}
