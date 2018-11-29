using HowLeaky.DataModels;
using HowLeaky.InputModels;
using HowLeaky.Tools.Serialiser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HowLeaky.Factories
{
    public class RawInputModelFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public RawInputModelFactory() { }
        /// <summary>
        /// 
        /// </summary>
        static Dictionary<string, Type> inputModelMap = new Dictionary<string, Type>
        {
            { "TillageType", typeof(TillageInputModel) },
            { "PesticideType", typeof(PesticideInputModel)},
            { "IrrigationType", typeof(IrrigationInputModel)},
            { "PhosphorusType", typeof(PhosphorusInputModel) },
            { "SoilType", typeof(SoilInputModel)},
            { "SolutesType", typeof(SolutesInputModel)},
            { "DataFile", typeof(ClimateInputModel) },
            { "NitratesType", typeof(NitrateInputModel) },
            { "DINNitratesType", typeof(DINNitrateInputModel) },
            //Vegetation models
            { "VegetationType", null },
            { "LAIVegDataTemplate", typeof(LAIVegInputModel) },
            { "CoverVegTemplate", typeof(CoverVegInputModel) }
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static InputModel GenerateRawInputModel(string homeDir, XElement element)
        {
            string elementName = element.Name.ToString();

            Type elementType = inputModelMap.FirstOrDefault(x => x.Key == elementName).Value;

            XAttribute fileNameAttribute = element.Attribute("href");

            string fileName = fileNameAttribute == null ? "" : fileNameAttribute.Value.ToString().Replace("\\", "/");

            if (fileName.Contains("./"))
            {
                fileName = homeDir + "/" + fileName;
            }

            if (elementType == null)
            {
                //We need to get the type from the parameter file
                XDocument xdoc = XDocument.Load(fileName);

                string childTypeName = xdoc.Root.Name.ToString();

                elementType = inputModelMap.FirstOrDefault(x => x.Key == childTypeName).Value;
            }

            InputModel model = (InputModel)Serialiser.Deserialise<InputModel>(fileName, elementType);

            model.FileName = fileName;

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static InputModel GenerateRawInputModelFromContents(string contents, string type)
        {

            Type T = Type.GetType(type);

            string elementName = "";

            InputModel model = (InputModel)Serialiser.Deserialise<InputModel>(contents, T, false);

            return model;
        }
    }
}
