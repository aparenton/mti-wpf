using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using parent_bMedecine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace parent_bMedecine.ViewModel
{
    /// <summary>
    /// LoginViewModel class for LoginView
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Members
        private string _accountName;
        #endregion // Members

        #region Properties
        /// <summary>
        /// User account name
        /// </summary>
        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                RaisePropertyChanged("AccountName");
            }
        }

        public RelayCommand<object> LoginCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginViewModel()
        {
            // Command
            LoginCommand = new RelayCommand<object>(s => LoginExecute(((PasswordBox)s).Password));
        }
        #endregion // Constructors

        #region Methods
        /// <summary>
        /// Call web service to authenticate user
        /// </summary>
        /// <param name="accountPassword">user password</param>
        private void LoginExecute(string accountPassword)
        {
            ServiceUser.ServiceUserClient client = new ServiceUser.ServiceUserClient();

            try
            {
                bool res = client.Connect(AccountName, accountPassword);
                if (res)
                {
                    string role = client.GetRole(AccountName);
                    MessengerInstance.Send<Message.OnLoginMessage>(new Message.OnLoginMessage(AccountName, RoleManager.IsReadOnlyRole(role)));
                }
                else
                    MessageBox.Show("Identifiant et/ou mot de passe incorrect.", "Alerte");
                client.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors de l'authentification, veuillez réessayer.", "Erreur");
            }
        }
        #endregion // Methors
    }
}
