﻿using GalaSoft.MvvmLight;
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
    /// <summary>
    /// PatientsViewModel class for PatientsView
    /// </summary>
    public class PatientsViewModel : ViewModelBase
    {
        #region Members
        private ViewModelBase _currentViewModel                        = SimpleIoc.Default.GetInstance<HomeViewModel>();
        private string _searchText                                     = String.Empty;
        private ObservableCollection<ServicePatient.Patient> _patients = new ObservableCollection<ServicePatient.Patient>();
        private ServicePatient.Patient _selectedPatient;
        private bool _readOnlyUserProfile                              = false;
        #endregion // Members

        #region Properties
        /// <summary>
        /// Current view model used with DataTemplate in PatientsView
        /// </summary>
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

        /// <summary>
        /// Patient search text
        /// </summary>
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

        /// <summary>
        /// Patient list
        /// </summary>
        public ObservableCollection<ServicePatient.Patient> Patients
        {
            get { return _patients; }
            set { _patients = value; }
        }

        /// <summary>
        /// Selected patient by user
        /// </summary>
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

        /// <summary>
        /// Boolean value to know if current user is readonly or not
        /// </summary>
        public bool ReadOnlyUserProfile 
        {
            get
            {
                return _readOnlyUserProfile;
            }
            set
            {
                _readOnlyUserProfile = value;
                RaisePropertyChanged("ReadOnlyUserProfile");
            }
        }

        public RelayCommand<ServicePatient.Patient> SelectPatientCommand { get; private set; }
        public RelayCommand DeletePatientCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PatientsViewModel()
        {
            // Commands
            SelectPatientCommand = new RelayCommand<ServicePatient.Patient>(p => this.SelectPatientExecute(p));
            DeletePatientCommand = new RelayCommand(DeletePatientExecute);

            // Messages
            MessengerInstance.Register<Message.OnLoginMessage>(this, m => { RetrievePatients(); ReadOnlyUserProfile = m.ReadOnlyUserProfile; });
            MessengerInstance.Register<Message.OnLogoutMessage>(this, m => { Reset(); });
            MessengerInstance.Register<Message.OnAddPatientMessage>(this, m => { RetrievePatients(); });
            MessengerInstance.Register<Message.WhenNoObservationMessage>(this, m => { CurrentViewModel = SimpleIoc.Default.GetInstance<HomeViewModel>(); });
        }
        #endregion // Constructors

        #region Methods
        /// <summary>
        /// On patient selection by user
        /// </summary>
        /// <param name="patient">selected patient</param>
        private void SelectPatientExecute(ServicePatient.Patient patient)
        {
            if (patient == null)
                return;

            SelectedPatient = patient;

            CurrentViewModel = SimpleIoc.Default.GetInstance<ObservationsViewModel>();
            MessengerInstance.Send<Message.OnPatientSelectionMessage>(new Message.OnPatientSelectionMessage(patient));
        }

        /// <summary>
        /// On text search, retrieve matching patients
        /// </summary>
        /// <param name="searchText">searched text</param>
        private void SearchTextExecute(string searchText)
        {
            _patients.Clear();

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
            try
            {
                List<ServicePatient.Patient> res = client.GetListPatient();
                foreach (var patient in res)
                {
                    if (patient.Firstname.ToLower().Contains(searchText.ToLower()) ||
                        patient.Name.ToLower().Contains(searchText.ToLower()))
                        Patients.Add(patient);
                }
                client.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors de la recherche du patient, veuillez réessayer.", "Erreur");
            }
            RaisePropertyChanged("Patients");
        }

        /// <summary>
        /// Retrieve patients data from web service
        /// </summary>
        private void RetrievePatients()
        {
            Patients.Clear();

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();
            try
            {
                List<ServicePatient.Patient> res = client.GetListPatient();
                foreach (var patient in res)
                    Patients.Add(patient);
                client.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors de la récupération des patients, veuillez réessayer.", "Erreur");
            }
        }

        /// <summary>
        /// Call web service to delete a patient
        /// </summary>
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
                client.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors de la suppression du patient, veuillez réessayer.", "Erreur");
            }
        }

        /// <summary>
        /// Reset view model properties
        /// </summary>
        private void Reset()
        {
            Patients.Clear();
            CurrentViewModel = SimpleIoc.Default.GetInstance<HomeViewModel>();
        }
        #endregion // Methors
    }
}
