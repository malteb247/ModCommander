namespace ModCommander.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public static class SerializerExtensions
    {
        private static readonly ConcurrentDictionary<Type, XmlSerializer> XmlSerializers = new ConcurrentDictionary<Type, XmlSerializer>();

        public static string ToXml<T>(this T instance) where T : new()
        {
            if (!typeof(T).IsSerializable) { throw new ArgumentException("Type must be serializable", "instance"); }

            using (var memStream = new MemoryStream())
            {
                GetXmlSerializer(typeof(T)).Serialize(memStream, instance);
                return new UTF8Encoding().GetString(memStream.ToArray());
            }
        }

        public static T FromXml<T>(this string xmlString) where T : new()
        {
            if (!typeof(T).IsSerializable) { throw new InvalidOperationException("Type T must be serializable"); }

            using (var memStream = new MemoryStream(new UTF8Encoding().GetBytes(xmlString)))
            {
                return (T)GetXmlSerializer(typeof(T)).Deserialize(memStream);
            }
        }

        /// <summary>
        /// Gets the xml serializer from the concurrent dictionary, if it doesn't exist then add one for the specified type
        /// </summary>
        /// <param name="type">serialzer type</param>
        /// <returns>Instance of <see cref="XmlSerializer"/></returns>
        private static XmlSerializer GetXmlSerializer(Type type)
        {
            return XmlSerializers.GetOrAdd(type, t => new XmlSerializer(t));
        }
    }
}
