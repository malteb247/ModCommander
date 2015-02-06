namespace ModCommander.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    [Serializable]
    public sealed class Profile : IEquatable<Profile>
    {
        [XmlAttribute]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Path { get; set; }
        
        public string User { get; set; }

        public string Password { get; set; }

        public string Uri { get; set; }

        public Profile()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Profile CreateEmptyProfile()
        {
            return new Profile() { Id = Guid.NewGuid().ToString() };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Profile other = obj as Profile;

            if (other == null)
                return false;
            else
                return Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public bool Equals(Profile other)
        {
            return this.Id == other.Id;
        }

        public bool IsActive { get; set; }
    }
}