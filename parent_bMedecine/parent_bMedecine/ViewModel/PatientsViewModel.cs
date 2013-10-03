using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
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
        private ViewModelBase _currentViewModel = SimpleIoc.Default.GetInstance<HomeViewModel>();
        private string _searchText = String.Empty;
        private ObservableCollection<Dbo.Patient> _patients = new ObservableCollection<Dbo.Patient>();        
        private Dbo.Patient _selectedPatient;
        #endregion // Members

        #region Properties
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

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
            MessengerInstance.Register<Message.OnLoginMessage>(this, m => { RetrievePatients(); });
            MessengerInstance.Register<Message.OnLogoutMessage>(this, m => { Reset(); });
            MessengerInstance.Register<Message.OnAddPatientMessage>(this, m => { RetrievePatients(); });
            MessengerInstance.Register<Message.WhenNoObservationMessage>(this, m => { CurrentViewModel = SimpleIoc.Default.GetInstance<HomeViewModel>(); });
        }
        #endregion // Constructors

        #region Methods
        private void SelectPatientExecute(Dbo.Patient patient)
        {
            if (patient == null)
                return;

            SelectedPatient = patient;

            CurrentViewModel = SimpleIoc.Default.GetInstance<ObservationsViewModel>();
            MessengerInstance.Send<Message.OnPatientSelectionMessage>(new Message.OnPatientSelectionMessage(patient));
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
        }
        #endregion // Methors
    }
}
