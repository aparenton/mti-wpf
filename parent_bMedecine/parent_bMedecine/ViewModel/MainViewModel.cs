using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Windows;

namespace parent_bMedecine.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Members
        private ViewModelBase _currentViewModel = new LoginViewModel();
        private string _userAccountName = string.Empty;
        private string _logoutButtonVisibility = "Collapsed";
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

        public string UserAccountName
        {
            get
            {
                return _userAccountName;
            }
            set
            {
                _userAccountName = value;
                RaisePropertyChanged("UserAccountName");
            }
        }

        public string LogoutButtonVisibility
        {
            get
            {
                return _logoutButtonVisibility;
            }
            set
            {
                _logoutButtonVisibility = value;
                RaisePropertyChanged("LogoutButtonVisibility");
            }
        }

        public RelayCommand LogoutCommand { get; private set; }
        #endregion // Properties

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            // Commands
            LogoutCommand = new RelayCommand(OnLogoutExecute);

            // Messages
            MessengerInstance.Register<Message.OnLoginMessage>(this, e => { OnLoginExecute(e.UserAccountName); });
        }

        #region Methods
        private void OnLoginExecute(string userAccountName)
        {
            this.CurrentViewModel = SimpleIoc.Default.GetInstance<MainTabControlViewModel>();

            UserAccountName = userAccountName;
            LogoutButtonVisibility = "Visible";
        }

        private void OnLogoutExecute()
        {
            ServiceUser.ServiceUserClient client = new ServiceUser.ServiceUserClient();

            try
            {
                client.Disconnect(UserAccountName);

                this.CurrentViewModel = SimpleIoc.Default.GetInstance<LoginViewModel>();

                UserAccountName = String.Empty;
                LogoutButtonVisibility = "Collapsed";

                MessengerInstance.Send<Message.OnLogoutMessage>(new Message.OnLogoutMessage());

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la déconnexion, veuillez réessayer.", "Erreur");
            }

        }
        #endregion // Methods
    }
}