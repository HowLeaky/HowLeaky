using HowLeaky.DataModels;
using HowLeaky.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HowLeaky.Factories
{
    public class SimulationFactory
    {
        public static Simulation GenerateSimulationXML(Project Project, XElement simElement, List<InputModel> allModels)
        {
            List<InputModel> simModels = new List<InputModel>();

            int startYear = 0;
            int endYear = 0;
            //Get the models from the filenames of the simualtion pointers
            foreach (XElement element in simElement.Elements())
            {
                if (element.Name.ToString() == "StartYear")
                {
                    startYear = element.Value.ToString() == "default" ? 0 : int.Parse(element.Value.ToString());
                }
                else if (element.Name.ToString() == "EndYear")
                {
                    endYear = element.Value.ToString() == "default" ? 0 : int.Parse(element.Value.ToString());
                }
                else
                {
                    try
                    {
                        string fileName = element.Attribute("href").Value;

                        InputModel model = allModels.Where(im => im.FileName == fileName).FirstOrDefault();

                        InputModel model2 = Cloner.DeepClone<InputModel>(model);

                        string modelDescription = model.GetType().Name;

                        modelDescription += (":" + model.Name);

                        //Check for child nodes
                        foreach (XElement childElement in element.Elements())
                        {
                            if (childElement.Name == "OverrideParameter")
                            {
                                //Add the override to the model
                                model2.Overrides.Add(childElement.Attribute("Keyword").Value, childElement.Element("Value").Value);
                                modelDescription += (";" + childElement.Attribute("Keyword").Value + "=" + childElement.Element("Value").Value);
                            }
                            else
                            {
                                //Probably a climate file override
                                if (element.Name == "ptrStation")
                                {
                                    if (childElement.Attribute("index") != null)
                                    {
                                        model2.Overrides.Add(childElement.Name.ToString(), childElement.Attribute("index").Value);
                                        modelDescription += (";" + childElement.Name.ToString() + "=" + childElement.Attribute("index").Value);
                                    }
                                    else
                                    {
                                        model2.Overrides.Add(childElement.Name.ToString(), childElement.Value);
                                        modelDescription += (";" + childElement.Name.ToString() + "=" + childElement.Value);
                                    }
                                }
                            }
                        }
                        if (element.Name == "ptrStation")
                        {
                            //Map the data to the original model
                            ClimateInputModel cimNew = (ClimateInputModel)model2;
                            ClimateInputModel cimOrig = (ClimateInputModel)model;

                            cimNew.Rain = cimOrig.Rain;
                            cimNew.MaxT = cimOrig.MaxT;
                            cimNew.MinT = cimOrig.MinT;
                            cimNew.PanEvap = cimOrig.PanEvap;
                            cimNew.Radiation = cimOrig.Radiation;
                            cimNew.VP = cimOrig.VP;

                        }
                        model2.ApplyOverrides();

                        model2.LongName = modelDescription;

                        simModels.Add(model2);

                    }
                    catch (Exception e)
                    {

                    }
                }

            }

            return new Simulation(Project, simModels, startYear, endYear);
        }
    }
}
