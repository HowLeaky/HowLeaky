using HowLeaky.DataModels;
using HowLeaky.Tools;
using HowLeaky.Tools.Serialiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HowLeaky.Factories
{
    public class SimInputModelFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public SimInputModelFactory() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rawInputModels"></param>
        /// <returns></returns>
        public static List<InputModel> GenerateSimInputModels(XElement element, List<InputModel> rawInputModels)
        {
            List<InputModel> simInputModels = new List<InputModel>();

            foreach (XElement xe in element.Elements())
            {
                //Firstly find the target
                InputModel rawInputModel = rawInputModels.Where(x => x.FileName == xe.Attribute("href").Value.ToString()).FirstOrDefault();

                //Then clone the raw model
                InputModel simInputModel = Cloner.Clone(rawInputModel);
                simInputModels.Add(simInputModel);

                //Then apply any overrides
                foreach (XElement child in xe.Elements())
                {
                    if (child.Elements().Count() == 0)
                    {
                        if (child.Value != null)
                        {
                            simInputModel.Overrides.Add(child.Name.ToString(), child.Value);
                        }
                        else
                        {
                            //Should have an index
                            simInputModel.Overrides.Add(child.Name.ToString(), child.Attribute("index").Value);
                        }
                    }
                    else
                    {
                        //Should be override parameters
                        //Check-----
                        if (child.Attribute("Active").Value == "true")
                        {
                            simInputModel.Overrides.Add(child.Name.ToString(), child.Element("Value").Value);
                        }
                    }
                }

                //Apply any overrides
                simInputModel.ApplyOverrides();
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GenerateSimInputModelDescription(XElement element)
        {
            List<string> modelDescriptions = new List<string>();
            foreach (XElement xe in element.Elements())
            {
                string modelDescription = xe.Name.ToString();

                string[] ModelNameParts = xe.Attribute("href").Value.ToString().Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

                modelDescription += ("=" + ModelNameParts[ModelNameParts.Length - 1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0]);

                //Then apply any overrides
                foreach (XElement child in xe.Elements())
                {
                    if (child.Elements().Count() == 0)
                    {
                        if (child.Value != null)
                        {
                            modelDescription += (";" + child.Name.ToString() + "=" + child.Value.ToString());
                        }
                        else
                        {
                            //Should have an index
                            modelDescription += (";" + child.Name.ToString() + "=" + child.Attribute("index").Value.ToString());
                         }
                    }
                    else
                    {
                        //Should be override parameters
                        //Check-----
                        if (child.Attribute("Active").Value == "true")
                        {
                            modelDescription += (";" + child.Name.ToString() + "=" + child.Element("Value").Value.ToString());
                        }
                    }
                }
                modelDescriptions.Add(modelDescription);
            }
            return String.Join(",",modelDescriptions.ToArray());
        }
    }
}
