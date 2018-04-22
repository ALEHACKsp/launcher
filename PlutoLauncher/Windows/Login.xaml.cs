using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

using PlutoLauncher.Utils;

namespace PlutoLauncher.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        // Charon API instance
        private CharonAPI cAPI = new CharonAPI(CharonAPI.ReleaseChannel.STABLE);

        public Login()
        {
            InitializeComponent();
        }

        private bool DoLogin(string username, string password)
        {
            var res = cAPI.UserAuth(username, password);
            if (res != null)
            {
                // Set user data
                Userdata.UserToken = res.usertoken;
                Userdata.Username = res.username;
                Userdata.Uid = res.uid;
                Userdata.Avatar = res.avatar;
                Userdata.Rank = res.rank;
                Userdata.Permissions = res.permissions;

                return true;
            }

            return false;
        }
    }
}
