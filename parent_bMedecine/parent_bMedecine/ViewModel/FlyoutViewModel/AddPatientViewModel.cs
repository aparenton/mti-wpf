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
    public class AddPatientViewModel : ViewModelBase
    {
        #region Members
        private string _name = String.Empty;
        private string _firstname = String.Empty;
        private DateTime _birthday = DateTime.Now;
        #endregion // Members

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public RelayCommand AddPatientCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        public AddPatientViewModel()
        {
            // Commands
            AddPatientCommand = new RelayCommand(AddPatientExecute);
        }
        #endregion // Constructors

        #region Methods
        private void AddPatientExecute()
        {
            Dbo.Patient newPatient = new Dbo.Patient()
            {
                Name = _name,
                Firstname = _firstname,
                Birthday = _birthday,
                Observations = new List<Dbo.Observation>()
            };

            ServicePatient.ServicePatientClient client = new ServicePatient.ServicePatientClient();

            try
            {
                bool res = client.AddPatient(newPatient);
                if (res)
                    MessengerInstance.Send<Message.OnAddPatientMessage>(new Message.OnAddPatientMessage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout du patient, veuillez réessayer.", "Erreur");
            }
            finally
            {
                Birthday = DateTime.Now;
                Name = String.Empty;
                Firstname = String.Empty;
            }
        }
        #endregion // Methods
    }
}