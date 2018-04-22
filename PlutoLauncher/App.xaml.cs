using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Squirrel;
using SharpRaven;

using PlutoLauncher.Windows;
using PlutoLauncher.Utils;

namespace PlutoLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Variables
        bool _noUpdate = false;
        bool _headless = false;
        bool _noClean = false;
        bool _offline = false;
        string _launchGame;

        // Global raven client
        public RavenClient ravenClient;

        // Charon API instance
        private CharonAPI cAPI = new CharonAPI(CharonAPI.ReleaseChannel.STABLE);

        private async void AppStart(object sender, StartupEventArgs args)
        {
            //ravenClient = new RavenClient("https://123:123@sentry.plutonium.pw/123");
            //AppDomain.CurrentDomain.UnhandledException += (s, e) => FatalError((Exception)e.ExceptionObject, true);

            // Check for updates
            /*using (var mgr = new UpdateManager(@"D:\Projects\Plutonium\Code\plutoniumpw\launcher\_Releases"))
            {
                await mgr.UpdateApp();
            }*/

            // Read userdata
            Userdata.ReadData();
            Userdata.WriteData();

            // Parse args
            List<string> unknownArgs = new List<string>();
            foreach (string arg in args.Args)
            {
                if (arg == "--noupdate")
                {
                    _noUpdate = true;
                }
                else if (arg == "--headless")
                {
                    _headless = true;
                }
                /*else if (arg == "--noclean")
                {
                    _noClean = true;
                }*/
                else if (arg == "--offline")
                {
                    _offline = true;
                }
                else if (arg.StartsWith("--game="))
                {
                    var game = arg.Substring(7);
                    if (game == "iw5" || game == "t6" || game == "t6zm") _launchGame = game;
                }
                else
                {
                    unknownArgs.Add(arg);
                }
            }

            // Charon calls
            if (!_offline && Userdata.UserToken != null)
            {
                // TODO: do this in separate thread
                var res = cAPI.ValidateUserToken(Userdata.UserToken);
                if (res != null)
                {
                    // Set user data
                    Userdata.Username = res.username;
                    Userdata.Uid = res.uid;
                    Userdata.Avatar = res.avatar;
                    Userdata.Rank = res.rank;
                    Userdata.Permissions = res.permissions;
                }
                else
                {
                    _offline = true;
                }
            }

            new MainWindow().Show();
            new Login().Show();
        }

        // Report error to Sentry
        public void FatalError(Exception exception, bool informUser = false)
        {
            ravenClient.Capture(new SharpRaven.Data.SentryEvent(exception));
            if (informUser)
            {
                MessageBox.Show(
                    $"Unfortunately, PU ran into an unhandled exception and needs to be closed.{Environment.NewLine}The bug report has been submitted to our staff.{Environment.NewLine}{Environment.NewLine}Error information for your own reference:{Environment.NewLine}{exception.GetType()}: {exception.Message}",
                    "PU - Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(exception.HResult);
            }
        }
    }
}
