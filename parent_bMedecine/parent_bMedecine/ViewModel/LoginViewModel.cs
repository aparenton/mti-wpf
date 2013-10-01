using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace parent_bMedecine.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Members
        private string _accountName;
        #endregion // Members

        #region Properties
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
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<object>(s => LoginExecute(((PasswordBox)s).Password));
        }
        #endregion // Constructors

        #region Methods
        private void LoginExecute(string accountPassword)
        {
            ServiceUser.ServiceUserClient client = new ServiceUser.ServiceUserClient();

            try
            {
                bool res = client.Connect(AccountName, accountPassword);
                if (res)
                    MessengerInstance.Send<Message.OnLoginMessage>(new Message.OnLoginMessage(AccountName));
                else
                    MessageBox.Show("Identifiant et/ou mot de passe incorrect.", "Alerte");
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'authentification, veuillez réessayer.", "Erreur");
            }            
        }
        #endregion // Methors
    }
}
