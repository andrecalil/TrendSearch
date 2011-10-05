using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TrendSearch.Domain;

namespace TrendSearch.Web.App
{
    public class Serialization
    {
        #region XML encoding
        
        //private static Encoding _encoding = Encoding.UTF8;
        private static Encoding _encoding = Encoding.GetEncoding("iso-8859-1");
        
        #endregion

        #region Serializacao objeto
        /// <summary>
        /// recebe objeto e retorna um xml.
        /// </summary>
        /// <param name="__Objeto"></param>
        /// <returns>retorna arquivo xml</returns>
        public static string Serialize(object __Objeto)
        {
            String _content;
            MemoryStream stream = new MemoryStream();

            XmlSerializer xmlSerializer = new XmlSerializer(__Objeto.GetType());

            //CustomXmlTextWriter _writer = new CustomXmlTextWriter(stream, _encoding);
            XmlTextWriter _writer = new XmlTextWriter(stream, _encoding);
            
            xmlSerializer.Serialize(_writer, __Objeto);

            _writer.Flush();

            StreamReader _reader = new StreamReader(stream, _encoding);
            stream.Position = 0;

            _content = _reader.ReadToEnd();

            return _content;
        }
        #endregion

        #region Deserialize objeto
        /// <summary>
        /// recebe um XML e retorna um objeto defino pelo _tipo
        /// </summary>
        /// <param name="__xml"></param>
        /// <param name="__type"></param>
        /// <returns></returns>
        public static T Deserialize <T>(string xml)
        {
            // The object to be returned
            object source;

            // Create a memorystream into which the object string will be written
            using (MemoryStream stream = new MemoryStream())
            {
                // Create the sream writer with the specified encoding
                using (StreamWriter writer = new StreamWriter(stream, _encoding))
                {
                    // Write the object content into the stream
                    writer.Write(xml);
                    writer.Flush();
                    stream.Position = 0;

                    XmlTextReader leitor = new XmlTextReader(stream);
                    //CustomXmlTextReader leitor = new CustomXmlTextReader(stream);

                    // Create the serializer, we must provide the object type to the constructor
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                    //source = xmlSerializer.Deserialize(stream);
                    source = xmlSerializer.Deserialize(leitor);
                }
            }
            
            return (T)source;
        }
        #endregion
    }
}