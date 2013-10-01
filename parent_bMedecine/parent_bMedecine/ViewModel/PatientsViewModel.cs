using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace parent_bMedecine.ViewModel
{
    public class PatientsViewModel : ViewModelBase
    {
        #region Members
        private string _searchText = String.Empty;
        private ObservableCollection<Dbo.Patient> _patients = new ObservableCollection<Dbo.Patient>();        
        private Dbo.Patient _selectedPatient;
        private ObservableCollection<Dbo.Observation> _observations = new ObservableCollection<Dbo.Observation>();
        private int _selectedObservationIndex;
        private ObservableCollection<byte[]> _pictureList = new ObservableCollection<byte[]>();

        private string _logoVisibility = "Visible";
        private string _tabControlVisibility = "Collapsed";
        #endregion // Members

        #region Properties
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                RaisePropertyChanged("SearchText");
                SearchTextExecute(_searchText);
            }
        }

        public ObservableCollection<Dbo.Patient> Patients
        {
            get
            {
                return _patients;
            }
            set
            {
                _patients = value;
            }
        }

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

        public string LogoVisibility
        {
            get { return _logoVisibility; }
            set { _logoVisibility = value; RaisePropertyChanged("LogoVisibility"); }
        }

        public string TabControlVisibility
        {
            get { return _tabControlVisibility; }
            set { _tabControlVisibility = value; RaisePropertyChanged("TabControlVisibility"); }
        }

        public RelayCommand<Dbo.Patient> SelectPatientCommand { get; private set; }
        public RelayCommand DeletePatientCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        public PatientsViewModel()
        {
            RetrievePatients();

            // Commands
            SelectPatientCommand = new RelayCommand<Dbo.Patient>(p => this.SelectPatientExecute(p));
            DeletePatientCommand = new RelayCommand(DeletePatientExecute);

            // Messages
            MessengerInstance.Register<Message.OnLogoutMessage>(this, m => { Reset(); });
            MessengerInstance.Register<Message.OnAddPatientMessage>(this, m => { RetrievePatients(); });
        }
        #endregion // Constructors

        #region Methods
        private void SelectPatientExecute(Dbo.Patient patient)
        {
            if (patient == null)
                return;

            LogoVisibility = "Collapsed";
            TabControlVisibility = "Visible";

            Observations.Clear();
            PictureList.Clear();
            SelectedPatient = patient;

            MessengerInstance.Send<Message.OnPatientSelectionMessage>(new Message.OnPatientSelectionMessage(patient));

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
            try
            {
                List<Dbo.Observation> res = client.GetPatient(SelectedPatient.Id).Observations;
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

        private void SearchTextExecute(string searchText)
        {
            _patients.Clear();            

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
            try
            {
                List<Dbo.Patient> res = client.GetListPatient();
                foreach (var patient in res)
                {
                    if (patient.Firstname.ToLower().Contains(searchText.ToLower()) ||
                        patient.Name.ToLower().Contains(searchText.ToLower()))
                        Patients.Add(patient);
                }
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la recherche du patient, veuillez réessayer.", "Erreur");
            }
            RaisePropertyChanged("Patients");
        }

        private void RetrievePatients()
        {
            Patients.Clear();

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
            try
            {
                List<Dbo.Patient> res = client.GetListPatient();
                foreach (var patient in res)
                    Patients.Add(patient);
                if (Patients.Any())
                    SelectPatientExecute(Patients.ElementAt(0));
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des patients, veuillez réessayer.", "Erreur");
            }
        }

        private void DeletePatientExecute()
        {
            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
            try
            {
                bool res = client.DeletePatient(SelectedPatient.Id);

                if (res)
                    RetrievePatients();
                else
                    MessageBox.Show("Le patient n'a pas pu être supprimé, veuillez réessayer.", "Alerte");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du patient, veuillez réessayer.", "Erreur");
            }
        }

        private void Reset()
        {
            Patients.Clear();
            Observations.Clear();
            PictureList.Clear();
        }
        #endregion // Methors
    }
}
