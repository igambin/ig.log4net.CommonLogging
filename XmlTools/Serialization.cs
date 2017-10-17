using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ig.log4net.XmlTools
{
    /// <summary>
    /// Helper zu Xml
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// Serialize 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException">Wenn eingaben leer sind.</exception>
        /// <exception cref="SerializationException">Wenn das TObject nicht serialisierbar ist.</exception>
        /// <returns></returns>
        public static string Serialize<T>(T value)
        {

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!value.IsSerializable())
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "Bei der Serialisierung ist ein Fehler aufgetreten!"));
            }


            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false, false),
                    Indent = true,
                    OmitXmlDeclaration = false
                };

            using (var textWriter = new Utf8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Deserialize 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <exception cref="SerializationException">Wenn das TObject nicht serialisierbar ist.</exception>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlReaderSettings();
            // No settings need modifying here

            using (var textReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        /// <summary>
        /// Prüft ob das Objekt serializiert werden kann.
        /// </summary>
        /// <param name="obj">zu prüfende Eingabe</param>
        /// <returns></returns>
        public static bool IsSerializable(this object obj)
        {
            var t = obj.GetType();

            return Attribute.IsDefined(t, typeof(DataContractAttribute)) || t.IsSerializable || (obj is IXmlSerializable) || Attribute.IsDefined(t, typeof(SerializableAttribute));

        }

        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
 }
