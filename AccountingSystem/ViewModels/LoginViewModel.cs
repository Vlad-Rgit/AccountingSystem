using AccountingSystem.Commands;
using AccountingSystem.Models;
using AccountingSystem.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private SecureString _secureString;

        public SecureString SecurePassword
        {
            get => _secureString;
            set
            {
                _secureString = value.Copy();
                _secureString.MakeReadOnly();
            }
        }

        public RelayCommand SignInCommand { get; }
        public event Action SignInConfirmed;

        public string Login { get; set; }
        
        public User LoggedUser { get; set; }

        public LoginViewModel()
        {
            SignInCommand = new RelayCommand(SignIn);
        }

        public void SignIn()
        {
            IntPtr unmanagedString = IntPtr.Zero;
            string password;

            unmanagedString =
                Marshal.SecureStringToGlobalAllocUnicode(SecurePassword);
            password = Marshal.PtrToStringUni(unmanagedString);



            using (var context = new Context())
            {
                User user = context.Users.FirstOrDefault(u => u.Login == Login && u.Password == password);

                if(user == null)
                {
                    CustomMessageBox messageBox = new CustomMessageBox("Ошибка", "Неправильный логин или пароль");
                    messageBox.ShowDialog();
                }
                else
                {
                    LoggedUser = user;
                    SignInConfirmed?.Invoke();
                }

            }
        }
    }
}
