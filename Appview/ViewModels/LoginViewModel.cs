using System.ComponentModel;
using System.Windows.Input;
using Appview.Models;

namespace Appview.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _message;
        private bool _isLoggedIn;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged("IsLoggedIn");
            }
        }

        // ICommand for Login
        public ICommand LoginCommand { get; set; }

        private User _user;

        public LoginViewModel()
        {
            _user = new User();
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            // Simulasi login
            _user.Username = Username;
            _user.Password = Password;

            if (_user.Username == "admin" && _user.Password == "password") // Contoh validasi
            {
                _user.Login();
                IsLoggedIn = true;
                Message = "Login berhasil!";
            }
            else
            {
                IsLoggedIn = false;
                Message = "Login gagal. Coba lagi.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
