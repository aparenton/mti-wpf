﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace parent_bMedecine.ViewModel.FlyoutViewModel
{
    /// <summary>
    /// AddPatientViewModel class for AddPatientFlyout
    /// </summary>
    public class AddPatientViewModel : ViewModelBase
    {
        #region Members
        private string _name       = String.Empty;
        private string _firstname  = String.Empty;
        private DateTime _birthday = DateTime.Now;
        #endregion // Members

        #region Properties
        /// <summary>
        /// Patient name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Patient firstname
        /// </summary>
        public string Firstname
        {
            get { return _firstname; }
            set 
            {
                _firstname = value;
                RaisePropertyChanged("Firstname");
            }
        }

        /// <summary>
        /// Patient birthday
        /// </summary>
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                RaisePropertyChanged("Birthday");
            }
        }

        public RelayCommand AddPatientCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public AddPatientViewModel()
        {
            // Commands
            AddPatientCommand = new RelayCommand(AddPatientExecute);
        }
        #endregion // Constructors

        #region Methods
        /// <summary>
        /// Create new patient object and call web service to add it
        /// </summary>
        private void AddPatientExecute()
        {
            ServicePatient.Patient newPatient = new ServicePatient.Patient()
            {
                Name         = _name,
                Firstname    = _firstname,
                Birthday     = _birthday,
                Observations = new List<ServicePatient.Observation>()
            };

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();

            try
            {
                bool res = client.AddPatient(newPatient);
                if (res)
                    MessengerInstance.Send<Message.OnAddPatientMessage>(new Message.OnAddPatientMessage());
                client.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors de l'ajout du patient, veuillez réessayer.", "Erreur");
            }
            finally
            {
                Birthday  = DateTime.Now;
                Name      = String.Empty;
                Firstname = String.Empty;
            }
        }
        #endregion // Methods
    }
}
