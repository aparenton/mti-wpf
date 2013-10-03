using GalaSoft.MvvmLight;
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
        public ObservationsViewModel()
        {
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

            Observations.Clear();
            PictureList.Clear();
            SelectedPatient = patient;

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
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
                }
                RaisePropertyChanged("Observations");
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
        }
        #endregion // Methors
    }
}
