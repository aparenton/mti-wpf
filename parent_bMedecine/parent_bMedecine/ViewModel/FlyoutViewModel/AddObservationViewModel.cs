using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace parent_bMedecine.ViewModel.FlyoutViewModel
{
    public class AddObservationViewModel : ViewModelBase
    {
        #region Members
        private Dbo.Patient _selectedPatient;
        private DateTime _date = DateTime.Now;
        private int _weight;
        private int _bloodpressure;
        private string _comment;
        private string[] _prescription;
        #endregion // Members

        #region Properties
        public Dbo.Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { _selectedPatient = value; RaisePropertyChanged("SelectedPatient"); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public int BloodPressure
        {
            get { return _bloodpressure; }
            set { _bloodpressure = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string[] Prescription
        {
            get { return _prescription; }
            set { _prescription = value; }
        }

        public RelayCommand AddObservationCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        public AddObservationViewModel()
        {
            // Messages
            MessengerInstance.Register<Message.OnPatientSelectionMessage>(this, m => { OnPatientSelectionExecute(m.SelectedPatient); });

            // Commands
            AddObservationCommand = new RelayCommand(AddObservationExecute);
        }
        #endregion // Constructors

        #region Methods
        private void AddObservationExecute()
        {
            Dbo.Observation newObservation = new Dbo.Observation()
            {
                BloodPressure = _bloodpressure,
                Comment = _comment,
                Date = _date,
                // Pictures = null, TODO
                Prescription = _prescription,
                Weight = _weight
            };

            ServiceObservation.ServiceObservationClient client = new ServiceObservation.ServiceObservationClient();

            try
            {
                bool res = client.AddObservation(_selectedPatient.Id, newObservation);
                if (res)
                    MessengerInstance.Send<Message.OnAddObservationMessage>(new Message.OnAddObservationMessage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout, veuillez réessayer.", "Erreur");
            }
            finally
            {
                BloodPressure = 0;
                Weight = 0;
                Comment = String.Empty;
                Prescription = null;
                Date = DateTime.Now;
                // Pictures to reset
            }
        }

        private void OnPatientSelectionExecute(Dbo.Patient patient)
        {
            SelectedPatient = patient;
        }
        #endregion // Methods
    }
}
