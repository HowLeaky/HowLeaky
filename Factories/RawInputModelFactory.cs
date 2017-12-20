using HowLeaky.DataModels;
using HowLeaky.ModelControllers.Veg;
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
            { "TillageType", typeof(TillageObjectDataModel) },
            { "PesticideType", typeof(PesticideObjectDataModel)},
            { "IrrigationType", typeof(IrrigationInputModel)},
            { "PhosphorusType", typeof(PhosphorusInputModel) },
            { "SoilType", typeof(SoilInputModel)},
            { "SolutesType", typeof(SolutesInputModel)},
            { "DataFile", typeof(ClimateInputModel) },
            { "NitratesType", typeof(NitrateInputDataModel) },
            //Vegetation models
            { "VegetationType", null },
            { "LAIVegDataTemplate", typeof(LAIVegObjectDataModel) },
            { "CoverVegTemplate", typeof(CoverVegObjectDataModel) }
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static InputModel GenerateRawInputModel(XElement element)
        {
            string elementName = element.Name.ToString();

            Type elementType = inputModelMap.FirstOrDefault(x => x.Key == elementName).Value;

            XAttribute fileNameAttribute = element.Attribute("href");

            string fileName = fileNameAttribute == null ? "" : fileNameAttribute.Value.ToString();

            if (elementType == null)
            {
                //We need to get the type from the parameter file
                XDocument xdoc = XDocument.Load(fileName);

                string childTypeName = xdoc.Root.Name.ToString();

                elementType = inputModelMap.FirstOrDefault(x => x.Key == childTypeName).Value;
            }

            InputModel model = (InputModel)Serialiser.Deserialise<InputModel> (fileName, elementName, elementType);

            model.FileName = fileName;

            return model;
        }
    }
}
