using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using parent_bMedecine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace parent_bMedecine.ViewModel
{
    public class ObservationsViewModel : ViewModelBase, ServiceLive.IServiceLiveCallback
    {
        #region Members
        private ServicePatient.Patient _selectedPatient;
        private ObservableCollection<ServicePatient.Observation> _observations = new ObservableCollection<ServicePatient.Observation>();
        private int _selectedObservationIndex;
        private ObservableCollection<byte[]> _pictureList = new ObservableCollection<byte[]>();
        private ServiceLive.ServiceLiveClient _client;
        #endregion // Members

        #region Properties
        public ObservableCollection<ChartObject> Weights { get; private set; }
        public ObservableCollection<ChartObject> BloodPressures { get; private set; }
        public ObservableCollection<ChartObject> Temperatures { get; private set; }
        public ObservableCollection<ChartObject> Hearts { get; private set; }

        public ServicePatient.Patient SelectedPatient
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

        public ObservableCollection<ServicePatient.Observation> Observations
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

        public RelayCommand StartServiceLiveCommand { get; private set; }
        public RelayCommand StopServiceLiveCommand { get; private set; }
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
            StartServiceLiveCommand = new RelayCommand(StartServiceLiveExecute);
            StopServiceLiveCommand = new RelayCommand(StopServiceLiveExecute);

            // Messages
            MessengerInstance.Register<Message.OnLogoutMessage>(this, m => { Reset(); });
            MessengerInstance.Register<Message.OnPatientSelectionMessage>(this, m => { OnPatientSelectionExecute(m.SelectedPatient); });
        }
        #endregion // Constructors

        #region Methods
        private void OnPatientSelectionExecute(ServicePatient.Patient patient)
        {
            if (patient == null)
                return;

            Reset();
            SelectedPatient = patient;

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();

            try
            {
                List<ServicePatient.Observation> res = client.GetPatient(SelectedPatient.Id).Observations;
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
            Temperatures.Clear();
            Hearts.Clear();

            StopServiceLiveExecute();
        }

        private void StartServiceLiveExecute()
        {
            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(this);
            _client =  new ServiceLive.ServiceLiveClient(context);

            try
            {
                _client.SubscribeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des données Live, veuillez réessayer.", "Erreur");
            }
        }

        private void StopServiceLiveExecute()
        {
            if (_client != null)
            {
                _client.Abort();
                _client.Close();
            }
        }

        public void PushDataHeart(double requestData)
        {
            try
            {
                if (Hearts.Count >= 30)
                    Hearts.RemoveAt(0);

                Hearts.Add(new ChartObject() { Category = DateTime.Now.ToString(), NumberDouble = requestData });
            }
            catch (Exception ex)
            {
            }
        }

        public void PushDataTemp(double requestData)
        {
            try
            {
                if (Temperatures.Count >= 10)
                    Temperatures.RemoveAt(0);

                Temperatures.Add(new ChartObject() { Category = DateTime.Now.ToString(), NumberDouble = requestData });
            }
            catch (Exception ex)
            {
            }
        }
        #endregion // Methods

        
    }
}
