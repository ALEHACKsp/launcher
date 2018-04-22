using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace PlutoLauncher.Utils
{
    class Userdata
    {
        public class Gamedata
        {
            public string Branch { get; set; }
            public string Location { get; set; }
        }

        // User data
        public static string UserToken { get; set; }
        public static string Language { get; set; }
        public static Gamedata IW5 { get; set; }
        public static Gamedata T6 { get; set; }

        // API data
        public static string Username { get; set; }
        public static int Uid { get; set; }
        public static string Avatar { get; set; }
        public static List<string> Permissions { get; set; }
        public static string Rank { get; set; }

        // Read data from registry
        public static void ReadData()
        {
            var software = Registry.CurrentUser.OpenSubKey("Software", true);
            if (software == null) return;

            var key = software.CreateSubKey("Plutonium\\Launcher");
            UserToken = (string)key.GetValue("Usertoken", null);
            Language = (string)key.GetValue("Language", "english");

            IW5 = new Gamedata();
            T6 = new Gamedata();
            if (key.GetValue("IW5") != null) IW5 = JsonConvert.DeserializeObject<Gamedata>(key.GetValue("IW5").ToString());
            if (key.GetValue("T6") != null) T6 = JsonConvert.DeserializeObject<Gamedata>(key.GetValue("T6").ToString());
        }

        // Write data to registry
        public static void WriteData()
        {
            var software = Registry.CurrentUser.OpenSubKey("Software", true);
            if (software == null) return;

            var key = software.CreateSubKey("Plutonium\\Launcher");
            if (UserToken != null) key.SetValue("Usertoken", UserToken);
            key.SetValue("Language", Language);

            key.SetValue("IW5", JsonConvert.SerializeObject(IW5));
            key.SetValue("T6", JsonConvert.SerializeObject(T6));
        }
    }
}
