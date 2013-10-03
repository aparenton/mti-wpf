using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace parent_bMedecine.ViewModel
{
    public class UsersViewModel : ViewModelBase
    {
        #region Members
        private ObservableCollection<Dbo.User> _users = new ObservableCollection<Dbo.User>();
        #endregion // Members

        #region Properties
        public ObservableCollection<Dbo.User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
            }
        }

        public RelayCommand<Dbo.User> DeleteUserCommand { get; private set; }
        #endregion // Properties

        #region Constructors
        public UsersViewModel()
        {
            RetrieveUsers();

            // Commands
            DeleteUserCommand = new RelayCommand<Dbo.User>(u => { DeleteUserExecute(u); });

            // Messages
            MessengerInstance.Register<Message.OnLoginMessage>(this, m => { RetrieveUsers(); });
            MessengerInstance.Register<Message.OnLogoutMessage>(this, m => { Reset(); });
            MessengerInstance.Register<Message.OnAddUserMessage>(this, m => { RetrieveUsers(); });
        }
        #endregion // Constructors

        #region Methods
        private void Reset()
        {
            Users.Clear();
        }

        private void RetrieveUsers()
        {
            Users.Clear();

            ServiceUser.ServiceUserClient client = new ServiceUser.ServiceUserClient();
            try
            {
                List<Dbo.User> res = client.GetListUser();
                foreach (var user in res)
                    Users.Add(user);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des utilisateurs, veuillez réessayer.", "Erreur");
            }       
        }

        private void DeleteUserExecute(Dbo.User user)
        {
            ServiceUser.ServiceUserClient client = new ServiceUser.ServiceUserClient();
            try
            {
                bool res = client.DeleteUser(user.Login);
                if (res)
                    Users.Remove(user);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression de l'utilisateur, veuillez réessayer.", "Erreur");
            }       
        }
        #endregion // Methors
    }
}
