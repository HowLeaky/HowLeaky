using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HowLeaky.Tools.Serialiser
{
    public class Serialiser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public string Serialise<T>(T obj)
        where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            using (StringWriter output = new StringWriter())
            {
                ser.Serialize(output, obj);
                return output.ToString();
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="fileName"></param>
        ///// <param name="innerTag"></param>
        ///// <returns></returns>
        public static T Deserialise<T>(string fileName, string innerTag)
            where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            XmlNode node = null;
            try
            {
                node = xmlDoc.GetElementsByTagName(innerTag)[0];
            }
            catch (Exception e)
            {
                //Throw something here
                throw new Exception(e.Message);
            }

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(node.OuterXml)))
                return (T)ser.Deserialize(ms);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="innerTag"></param>
        /// <returns></returns>
        public static T Deserialise<T>(string file, Type type, bool loadFromFile = true)
           where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(type);

            XDocument xmlDoc;

            if (loadFromFile == true)
            {
                //File name
                xmlDoc = XDocument.Load(file);
            }
            else
            {
                //File contents
                xmlDoc = XDocument.Parse(file);
            }
            XElement node = null;
            try
            {
                //node = xmlDoc.GetElementsByTagName(innerTag)[0];
                node = xmlDoc.Root.Elements().First();
            }
            catch (Exception e)
            {
                //Throw something here
                throw new Exception(e.Message);
            }

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(node.ToString())))
                return (T)ser.Deserialize(ms);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="innerTag"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T Deserialise<T>(Stream stream, string innerTag, Type type)
           where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(type);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(stream);

            XmlNode node = null;
            try
            {
                node = xmlDoc.GetElementsByTagName(innerTag)[0];
            }
            catch (Exception e)
            {
                //Throw something here
                throw new Exception(e.Message);
            }

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(node.OuterXml)))
                return (T)ser.Deserialize(ms);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public T Deserialise<T>(string fileName)
                where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            FileInfo file = new FileInfo(fileName);
            if (!file.Exists)
            {
                throw new Exception("File " + fileName + " does not exist.");
            }

            using (StreamReader sr = new StreamReader(file.FullName))
                return (T)ser.Deserialize(sr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        static public T Deserialise<T>(Stream stream)
        where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            return (T)ser.Deserialize(stream);
        }
    }
}
