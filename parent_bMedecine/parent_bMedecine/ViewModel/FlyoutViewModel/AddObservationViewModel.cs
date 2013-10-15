using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace parent_bMedecine.ViewModel.FlyoutViewModel
{
    public class AddObservationViewModel : ViewModelBase
    {
        #region Members
        private ServicePatient.Patient _selectedPatient;
        private DateTime _date = DateTime.Now;
        private int _weight;
        private int _bloodpressure;
        private string _comment = string.Empty;
        private string _prescription = string.Empty;
        private ObservableCollection<string> _pictures = new ObservableCollection<string>();
        private ObservableCollection<string> _prescriptions = new ObservableCollection<string>();
        #endregion // Members

        #region Properties
        public ServicePatient.Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { _selectedPatient = value; RaisePropertyChanged("SelectedPatient"); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; RaisePropertyChanged("Date"); }
        }

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; RaisePropertyChanged("Weight"); }
        }

        public int BloodPressure
        {
            get { return _bloodpressure; }
            set { _bloodpressure = value; RaisePropertyChanged("BloodPressure"); }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; RaisePropertyChanged("Comment"); }
        }

        public string Prescription
        {
            get { return _prescription; }
            set { _prescription = value; RaisePropertyChanged("Prescription"); }
        }

        public ObservableCollection<string> Pictures
        {
            get { return _pictures; }
            set { _pictures = value; }
        }

        public ObservableCollection<string> Prescriptions
        {
            get { return _prescriptions; }
            set { _prescriptions = value; }
        }

        public RelayCommand AddObservationCommand { get; private set; }
        public RelayCommand AddObservationPictureCommand { get; private set; }
        public RelayCommand AddObservationPrescriptionCommand { get; private set; }
        public RelayCommand<int> DeleteObservationPictureCommand { get; private set; }
        public RelayCommand<int> DeleteObservationPrescriptionCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        public AddObservationViewModel()
        {
            // Messages
            MessengerInstance.Register<Message.OnPatientSelectionMessage>(this, m => { OnPatientSelectionExecute(m.SelectedPatient); });

            // Commands
            AddObservationCommand = new RelayCommand(AddObservationExecute);
            AddObservationPictureCommand = new RelayCommand(AddObservationPictureExecute);
            AddObservationPrescriptionCommand = new RelayCommand(AddObservationPrescriptionExecute);
            DeleteObservationPictureCommand = new RelayCommand<int>(i => DeleteObservationPictureExecute(i));
            DeleteObservationPrescriptionCommand = new RelayCommand<int>(i => DeleteObservationPrescriptionExecute(i));
        }
        #endregion // Constructors

        #region Methods
        private void AddObservationExecute()
        {
            List<Byte[]> pictures = new List<Byte[]>();
            foreach (string picture in Pictures)
            {
                pictures.Add(Utilities.ImageManager.GetBytesFromImage(picture));
            }

            List<string> prescriptions = new List<string>();
            foreach (string prescription in Prescriptions)
            {
                prescriptions.Add(prescription);
            }
            for (int i = 0; i < Prescriptions.Count(); i++)
            {
                prescriptions[i] = Prescriptions.ElementAt(i);
            }

            ServiceObservation.Observation newObservation = new ServiceObservation.Observation()
            {
                BloodPressure = _bloodpressure,
                Comment = _comment,
                Date = _date,
                Pictures = pictures,
                Prescription = prescriptions,
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
                MessageBox.Show("Erreur lors de l'ajout de l'observation, veuillez réessayer.", "Erreur");
            }
            finally
            {
                BloodPressure = 0;
                Weight = 0;
                Comment = string.Empty;
                Prescription = string.Empty;
                Date = DateTime.Now;
                Pictures.Clear();
                Prescriptions.Clear();
            }
        }

        private void AddObservationPictureExecute()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            dlg.Filter = "Fichiers Image|*.jpg;*.jpeg;*.png";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Pictures.Add(dlg.FileName);
            }
        }

        private void AddObservationPrescriptionExecute()
        {
            Prescriptions.Add(Prescription);
            Prescription = string.Empty;
            RaisePropertyChanged("Prescription");
        }

        private void DeleteObservationPrescriptionExecute(int index)
        {
            Prescriptions.RemoveAt(index);
        }

        private void DeleteObservationPictureExecute(int index)
        {
            Pictures.RemoveAt(index);
        }

        private void OnPatientSelectionExecute(ServicePatient.Patient patient)
        {
            SelectedPatient = patient;
        }
        #endregion // Methods
    }
}
