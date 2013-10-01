using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace parent_bMedecine.ViewModel.FlyoutViewModel
{
    public class AddUserViewModel : ViewModelBase
    {
        #region Members
        private string _name = String.Empty;
        private string _firstname = String.Empty;
        private string _login = String.Empty;
        private string _role = String.Empty;
        private string _photo = String.Empty;
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

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public string Photo
        {
            get { return _photo; }
            set { _photo = value; RaisePropertyChanged("Photo"); }
        }

        public RelayCommand<object> AddUserCommand { get; private set; }
        public RelayCommand SelectPictureCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        public AddUserViewModel()
        {
            // Commands
            AddUserCommand = new RelayCommand<object>(p => AddUserExecute(((PasswordBox)p).Password));
            SelectPictureCommand = new RelayCommand(SelectPictureExecute);
        }
        #endregion // Constructors

        #region Methods
        private void AddUserExecute(string password)
        {
            Dbo.User newUser = new Dbo.User()
            {
                Connected = false,
                Picture = Utilities.ImageManager.GetBytesFromImage(_photo),
                Role = _role,
                Firstname = _firstname,
                Name = _name,
                Login = _login,
                Pwd = password
            };

            ServiceUser.ServiceUserClient client = new ServiceUser.ServiceUserClient();

            try
            {
                bool res = client.AddUser(newUser);
                if (res)
                    MessengerInstance.Send<Message.OnAddUserMessage>(new Message.OnAddUserMessage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout, veuillez réessayer.", "Erreur");
            }
            finally
            {
                Photo = String.Empty;
                Name = String.Empty;
                Firstname = String.Empty;
                Login = String.Empty;
                Role = String.Empty;
            }
        }

        public void SelectPictureExecute()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // TODO : handle multi-language
            dlg.Filter = "Fichiers Image|*.jpg;*.jpeg;*.png";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Photo = dlg.FileName;
            }
        }
        #endregion // Methods
    }
}
