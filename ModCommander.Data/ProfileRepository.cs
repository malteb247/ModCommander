namespace ModCommander.Data
{
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModCommander.Utils;
    using System.Reflection;

    public static class ProfileRepository
    {
        static Logger log = LogManager.GetCurrentClassLogger();
        static List<Profile> profiles = null;
        static string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "profiles.mc");
        
        public static List<Profile> LoadProfiles()
        {
            if (File.Exists(filePath))
                profiles = File.ReadAllText(filePath).FromXml<List<Profile>>();
            else
                profiles = new List<Profile>();

            return profiles;
        }

        public static void SaveProfile(Profile profile)
        {
            if (profiles == null)
                profiles = new List<Profile>();

            Profile existing = profiles.Where(p => p.Id== profile.Id).FirstOrDefault();

            if (existing != null)
                profiles.Remove(existing);


            profiles.Add(profile);


            File.WriteAllText(filePath, profiles.ToXml<List<Profile>>());
        }

        public static void DeleteProfile(string id)
        {
            Profile existing = profiles.Where(p => p.Id == id).FirstOrDefault();

            if (existing != null)
                profiles.Remove(existing);

            File.WriteAllText(filePath, profiles.ToXml<List<Profile>>());
        }

        public static Profile LoadProfile(string id)
        {
            if (profiles == null)
            {
                if (File.Exists(filePath))
                    profiles = File.ReadAllText(filePath).FromXml<List<Profile>>();
                else
                    profiles = new List<Profile>();
            }

            return profiles.Where(p => p.Id == id).FirstOrDefault();

        }
    }
}
