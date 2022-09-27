using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.ViewModels
{
    internal class LoginPageViewModel:BaseViewModel
    {
        private string m_userName;
        private string m_password;

        public string UserName
        {
            get =>  m_userName; 
            set => Set(ref m_userName, value); 
        }

        public string Password
        {
            get => m_password; 
            set => Set(ref m_password, value); 
        }
    }
}
