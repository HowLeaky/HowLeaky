
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    public enum SoilImportFormat
    {
        HowLeaky,
        ApsoilApsimFormat
    };

    public class SoilDataModel : DataModel
    {

        ////public virtual ParameterModel CnReductionAtFullCover { get; set; }
        ////public virtual ParameterModel PAWC { get; set; }
        ////public virtual ParameterModel RunoffCurveNumber { get; set; }
        ////public virtual ParameterModel Stage1EvapLimitU { get; set; }
        ////public virtual ParameterModel LayerCount { get; set; }
        ////public virtual ParameterModel Stage2EvapCona { get; set; }

        ////public virtual ParameterModel Depths { get; set; }
        ////public virtual ParameterModel AirDryLimits { get; set; }
        ////public virtual ParameterModel FieldCapacities { get; set; }
        ////public virtual ParameterModel MaxDrainageFromLayer { get; set; }
        ////public virtual ParameterModel WiltingPoints { get; set; }
        ////public virtual ParameterModel SaturationLimits { get; set; }
        ////public virtual ParameterModel PAWCs { get; set; }

        //////public virtual ParameterModel MaxDailyDrainageVolume { get; set; }
        ////public virtual ParameterModel BulkDensity { get; set; }

        //////not in use
        ////public virtual ParameterModel MaxReductionInCNDueToTill { get; set; }
        ////public virtual ParameterModel RainfallToRemoveRoughness { get; set; }
        ////public virtual ParameterModel USLEK { get; set; }
        ////public virtual ParameterModel USLEP { get; set; }
        ////public virtual ParameterModel FieldSlope { get; set; }
        ////public virtual ParameterModel SlopeLength { get; set; }
        ////public virtual ParameterModel RillRatio { get; set; }
        ////public virtual ParameterModel SimulateSoilCracking { get; set; }
        ////public virtual ParameterModel MaxInfiltrationIntoCracks { get; set; }
        ////public virtual ParameterModel SedimentDeliveryRatio { get; set; }

        ////public virtual ParameterModel OrganicCarbon { get; set; }
        ////public virtual ParameterModel CarbonNitrogenRatio { get; set; }
        ////public virtual ParameterModel NitrogenMineralisationCoefficient { get; set; }

        //////public virtual ICollection<GeoRegion> Regions { get; set; }
        ////public virtual ICollection<ChangeRecord> EditHistory { get; set; }

        ////public virtual ICollection<ApplicationUser> AdditionalPermissions { get; set; }

        ////public SoilModel()
        ////{
        ////    //CnReductionAtFullCover = new ParameterModel(ParameterModelType.Float);
        ////    //PAWC = new ParameterModel(ParameterModelType.Float);
        ////    //RunoffCurveNumber = new ParameterModel(ParameterModelType.Float);
        ////    //Stage1EvapLimitU = new ParameterModel(ParameterModelType.Float);
        ////    //LayerCount = new ParameterModel(ParameterModelType.Int);
        ////    //Stage2EvapCona = new ParameterModel(ParameterModelType.Float);

        ////    //Depths = new ParameterModel(ParameterModelType.FloatVector);
        ////    //AirDryLimits = new ParameterModel(ParameterModelType.FloatVector);
        ////    //FieldCapacities = new ParameterModel(ParameterModelType.FloatVector);
        ////    //MaxDrainageFromLayer = new ParameterModel(ParameterModelType.FloatVector);
        ////    //WiltingPoints = new ParameterModel(ParameterModelType.FloatVector);
        ////    //SaturationLimits = new ParameterModel(ParameterModelType.FloatVector);
        ////    //PAWCs = new ParameterModel(ParameterModelType.FloatVector);
        ////    //MaxDailyDrainageVolume = new ParameterModel(ParameterModelType.FloatVector);
        ////    //BulkDensity = new ParameterModel(ParameterModelType.FloatVector   

        ////    //MaxReductionInCNDueToTill = new ParameterModel(ParameterModelType.Float);
        ////    //RainfallToRemoveRoughness = new ParameterModel(ParameterModelType.Float);
        ////    //USLEK = new ParameterModel(ParameterModelType.Float);
        ////    //USLEP = new ParameterModel(ParameterModelType.Float);
        ////    //FieldSlope = new ParameterModel(ParameterModelType.Float);
        ////    //SlopeLength = new ParameterModel(ParameterModelType.Float);
        ////    //RillRatio = new ParameterModel(ParameterModelType.Float);
        ////    //SimulateSoilCracking = new ParameterModel(ParameterModelType.Bool);
        ////    //MaxInfiltrationIntoCracks = new ParameterModel(ParameterModelType.Float);
        ////    //SedimentDeliveryRatio = new ParameterModel(ParameterModelType.Float);

        ////    //Regions=new HashSet<GeoRegion>();
        ////    EditHistory = new HashSet<ChangeRecord>();
        ////    AdditionalPermissions = new HashSet<ApplicationUser>();
        ////}

        ////public SoilModel(ApplicationDbContext.ApplicationDbContext db, Stream stream, string filename, ApplicationUser appUser, SoilImportFormat format)
        ////{
        ////    if (format == SoilImportFormat.HowLeaky)
        ////        LoadFromHowLeakyXMLStream(db, stream, filename);
        ////    else if (format == SoilImportFormat.ApsoilApsimFormat)
        ////        LoadFromApsoilApsimXMLStream(db, stream, filename);
        ////    Id = Guid.NewGuid();
        ////    CreatedBy = appUser.UserName;
        ////    CreatedDate = DateTime.UtcNow;
        ////    ModifiedBy = CreatedBy;
        ////    ModifiedDate = CreatedDate;
        ////}

        ////public bool LoadFromHowLeakyXMLStream(ApplicationDbContext.ApplicationDbContext db, Stream stream, string filename)
        ////{
        ////    try
        ////    {
        ////        string name = filename.Replace(".soil", "");
        ////        string source = "HowLeaky File-import:  " + filename;
        ////        XmlDocument Doc = new XmlDocument();
        ////        Doc.Load(stream);
        ////        XmlElement Root = Doc.DocumentElement;
        ////        if (Root.HasChildNodes)
        ////        {
        ////            XmlNode header = Root.SelectSingleNode("SoilType");
        ////            if (header != null)
        ////            {
        ////                Name = (header.Attributes != null && header.Attributes["text"] != null) ? header.Attributes["text"].Value : name;
        ////                Summary = (header.Attributes != null && header.Attributes["Description"] != null) ? header.Attributes["Description"].Value : "";
        ////                Comments = (header.Attributes != null && header.Attributes["Comments"] != null) ? header.Attributes["Comments"].Value : "";
        ////                Comments = Comments + "\n" + source;

        ////                LayerCount = InitialiseFromXMLNode(db, header, "HorizonCount", ParameterModelType.Int, source);
        ////                var layerCount = LayerCount.getIntValue();
        ////                var depth = ExtractFloatArrayFromNode(header, layerCount, "LayerDepth");
        ////                var airDryLimit = ExtractFloatArrayFromNode(header, layerCount, "InSituAirDryMoist");
        ////                var wiltingPoint = ExtractFloatArrayFromNode(header, layerCount, "WiltingPoint");
        ////                var fieldCapacity = ExtractFloatArrayFromNode(header, layerCount, "FieldCapacity");
        ////                var saturationLimit = ExtractFloatArrayFromNode(header, layerCount, "SatWaterCont");
        ////                var maxDrainageFromLayer = ExtractFloatArrayFromNode(header, layerCount, "MaxDailyDrainRate");
        ////                var bulkDensity = ExtractFloatArrayFromNode(header, layerCount, "BlukDensity");

        ////                Depths = Initialise(db, depth, "", source);
        ////                AirDryLimits = Initialise(db, airDryLimit, "", source);
        ////                WiltingPoints = Initialise(db, wiltingPoint, "", source);
        ////                FieldCapacities = Initialise(db, fieldCapacity, "", source);
        ////                SaturationLimits = Initialise(db, saturationLimit, "", source);
        ////                MaxDrainageFromLayer = Initialise(db, maxDrainageFromLayer, "", source);
        ////                BulkDensity = Initialise(db, bulkDensity, "", source);

        ////                //****************
        ////                CnReductionAtFullCover = InitialiseFromXMLNode(db, header, "RedInCNAtFullCover", ParameterModelType.Float, source);
        ////                PAWC = InitialiseFromXMLNode(db, header, "****", ParameterModelType.Float, source);
        ////                RunoffCurveNumber = InitialiseFromXMLNode(db, header, "RunoffCurveNumber", ParameterModelType.Float, source);
        ////                Stage1EvapLimitU = InitialiseFromXMLNode(db, header, "Stage1SoilEvap_U", ParameterModelType.Float, source);
        ////                Stage2EvapCona = InitialiseFromXMLNode(db, header, "Stage2SoilEvap_Cona", ParameterModelType.Float, source);
        ////                MaxReductionInCNDueToTill = InitialiseFromXMLNode(db, header, "MaxRedInCNDueToTill", ParameterModelType.Float, source);
        ////                RainfallToRemoveRoughness = InitialiseFromXMLNode(db, header, "RainToRemoveRough", ParameterModelType.Float, source);
        ////                USLEK = InitialiseFromXMLNode(db, header, "USLE_K", ParameterModelType.Float, source);
        ////                USLEP = InitialiseFromXMLNode(db, header, "USLE_P", ParameterModelType.Float, source);
        ////                FieldSlope = InitialiseFromXMLNode(db, header, "FieldSlope", ParameterModelType.Float, source);
        ////                SlopeLength = InitialiseFromXMLNode(db, header, "SlopeLength", ParameterModelType.Float, source);
        ////                RillRatio = InitialiseFromXMLNode(db, header, "RillRatio", ParameterModelType.Float, source);
        ////                SimulateSoilCracking = InitialiseFromXMLNode(db, header, "SoilCrack", ParameterModelType.Bool, source);
        ////                MaxInfiltrationIntoCracks = InitialiseFromXMLNode(db, header, "MaxInfiltIntoCracks", ParameterModelType.Float, source);
        ////                SedimentDeliveryRatio = InitialiseFromXMLNode(db, header, "SedDelivRatio", ParameterModelType.Float, source);
        ////                OrganicCarbon = InitialiseFromXMLNode(db, header, "OrganicCarbon", ParameterModelType.Float, source);
        ////                CarbonNitrogenRatio = InitialiseFromXMLNode(db, header, "CarbonNitrogenRatio", ParameterModelType.Float, source);
        ////                NitrogenMineralisationCoefficient = InitialiseFromXMLNode(db, header, "NitrateMineralisationCoefficient", ParameterModelType.Float, source);

        ////                //CalculateTotals();
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ErrorLogger.HandleError(ex, "", false);
        ////    }

        ////    return false;
        ////}

        ////public bool LoadFromApsoilApsimXMLStream(ApplicationDbContext.ApplicationDbContext db, Stream stream, string filename)
        ////{
        ////    try
        ////    {

        ////        XmlDocument Doc = new XmlDocument();
        ////        Doc.Load(stream);
        ////        XmlElement Root = Doc.DocumentElement;
        ////        if (Root.HasChildNodes)
        ////        {
        ////            var RecordNumber = ExtractElementText(Root, "", "RecordNumber");//>9</RecordNumber>
        ////            var ASCOrder = ExtractElementText(Root, "", "ASCOrder");//>Vertosol</ASCOrder>
        ////            var ASCSubOrder = ExtractElementText(Root, "", "ASCSubOrder");//>Grey</ASCSubOrder>
        ////            var SoilType = ExtractElementText(Root, "", "SoilType");//>Clay</SoilType>
        ////            var Site = ExtractElementText(Root, "", "Site");//>Condamine</Site>
        ////            //var NearestTown = ExtractElementText(Root, "", "NearestTown");//>Condamine, Q 4416</NearestTown>
        ////            var Region = ExtractElementText(Root, "", "Region");//>Darling Downs and Granite Belt</Region>
        ////            var State = ExtractElementText(Root, "", "State");//>Queensland</State>
        ////            //var Country = ExtractElementText(Root, "", "Country");//>Australia</Country>
        ////            //var NaturalVegetation = ExtractElementText(Root, "", "NaturalVegetation");//>Brigalow, Belah</NaturalVegetation>
        ////            var ApsoilNumber = ExtractElementText(Root, "", "ApsoilNumber");//>105</ApsoilNumber>
        ////            //var Latitude = ExtractElementText(Root, "", "Latitude");//>-27.091</Latitude>
        ////            //var Longitude = ExtractElementText(Root, "", "Longitude");//>149.942</Longitude>
        ////            //var LocationAccuracy = ExtractElementText(Root, "", "LocationAccuracy");//>+/- 20m</LocationAccuracy>
        ////            //var YearOfSampling = ExtractElementText(Root, "", "YearOfSampling");//>0</YearOfSampling>
        ////            var DataSource = ExtractElementText(Root, "", "DataSource");//>CSIRO Sustainable Ecosystems, Toowoomba</DataSource>

        ////            Name = String.Format("{2} {0} {1}", ASCSubOrder, ASCOrder, Site);
        ////            var descriptor = String.Format("{4} {0} {1} ({2} {3} {5})", ASCSubOrder, ASCOrder, Site, SoilType, Region, State);
        ////            Summary = String.Format("APSOIL No. {0} (Record {1}) {2}", ApsoilNumber, RecordNumber, descriptor);
        ////            Comments = String.Format("Extracted from APOIL file (APSIM format) {0} ({1})", filename, DataSource);

        ////            List<string> Thickness = ExtractElementArrayText(Root, "Water", "Thickness");
        ////            List<string> BD = ExtractElementArrayText(Root, "Water", "BD");
        ////            List<string> AirDry = ExtractElementArrayText(Root, "Water", "AirDry");
        ////            List<string> LL15 = ExtractElementArrayText(Root, "Water", "LL15");
        ////            List<string> DUL = ExtractElementArrayText(Root, "Water", "DUL");
        ////            List<string> SAT = ExtractElementArrayText(Root, "Water", "SAT");


        ////            var SummerCona = ExtractElementText(Root, "SoilWater", "SummerCona");//>3.5</SummerCona>
        ////            var SummerU = ExtractElementText(Root, "SoilWater", "SummerU");//>6</SummerU>
        ////            //var SummerDate = ExtractElementText(Root, "SoilWater", "SummerDate");//>1-Nov</SummerDate>
        ////            //var WinterCona = ExtractElementText(Root, "SoilWater", "WinterCona");//>2.5</WinterCona>
        ////            //var WinterU = ExtractElementText(Root, "SoilWater", "WinterU");//>4</WinterU>
        ////            //var WinterDate = ExtractElementText(Root, "SoilWater", "WinterDate");//>1-Apr</WinterDate>
        ////            //var DiffusConst = ExtractElementText(Root, "SoilWater", "DiffusConst");//>40</DiffusConst>
        ////            //var DiffusSlope = ExtractElementText(Root, "SoilWater", "DiffusSlope");//>16</DiffusSlope>
        ////            //var Salb = ExtractElementText(Root, "SoilWater", "Salb");//>0.12</Salb>
        ////            var CN2Bare = ExtractElementText(Root, "SoilWater", "CN2Bare");//>73</CN2Bare>
        ////            var CNRed = ExtractElementText(Root, "SoilWater", "CNRed");//>20</CNRed>
        ////            //var CNCov = ExtractElementText(Root, "SoilWater", "CNCov");//>0.8</CNCov>
        ////            var Slope = ExtractElementText(Root, "SoilWater", "Slope");//>NaN</Slope>
        ////                                                                       //var DischargeWidth = ExtractElementText(Root, "SoilWater", "DischargeWidth");//>NaN</DischargeWidth>
        ////                                                                       //var CatchmentArea = ExtractElementText(Root, "SoilWater", "CatchmentArea");//>NaN</CatchmentArea>
        ////                                                                       //var MaxPond = ExtractElementText(Root, "SoilWater", "MaxPond");//>NaN</MaxPond>

        ////            LayerCount = ExtractFromString(db, "HorizonCount", ParameterModelType.Int, Thickness.Count.ToString());
        ////            var layerCount = LayerCount.getIntValue();

        ////            // var maxDrainageFromLayer = ExtractFromStringList(header, layerCount, "MaxDailyDrainRate");

        ////            Thickness = ScaleThickness(Thickness);
        ////            Depths = ExtractFromStringList(db, Thickness, layerCount, "LayerDepth");

        ////            AirDryLimits = ExtractFromStringList(db, AirDry, layerCount, "InSituAirDryMoist");
        ////            WiltingPoints = ExtractFromStringList(db, LL15, layerCount, "WiltingPoint");
        ////            FieldCapacities = ExtractFromStringList(db, DUL, layerCount, "FieldCapacity");
        ////            SaturationLimits = ExtractFromStringList(db, SAT, layerCount, "SatWaterCont");
        ////            MaxDrainageFromLayer = ExtractFromStringList(db, null, layerCount, "SatWaterCont");
        ////            BulkDensity = ExtractFromStringList(db, BD, layerCount, "BlukDensity");

        ////            CnReductionAtFullCover = ExtractFromString(db, "RedInCNAtFullCover", ParameterModelType.Float, CNRed);
        ////            PAWC = ExtractFromString(db, "****", ParameterModelType.Float, null);
        ////            RunoffCurveNumber = ExtractFromString(db, "RunoffCurveNumber", ParameterModelType.Float, CN2Bare);
        ////            Stage1EvapLimitU = ExtractFromString(db, "Stage1SoilEvap_U", ParameterModelType.Float, SummerU);
        ////            Stage2EvapCona = ExtractFromString(db, "Stage2SoilEvap_Cona", ParameterModelType.Float, SummerCona);
        ////            MaxReductionInCNDueToTill = ExtractFromString(db, "MaxRedInCNDueToTill", ParameterModelType.Float, null);
        ////            RainfallToRemoveRoughness = ExtractFromString(db, "RainToRemoveRough", ParameterModelType.Float, null);
        ////            USLEK = ExtractFromString(db, "USLE_K", ParameterModelType.Float, null);
        ////            USLEP = ExtractFromString(db, "USLE_P", ParameterModelType.Float, null);
        ////            FieldSlope = ExtractFromString(db, "FieldSlope", ParameterModelType.Float, Slope);
        ////            SlopeLength = ExtractFromString(db, "SlopeLength", ParameterModelType.Float, null);
        ////            RillRatio = ExtractFromString(db, "RillRatio", ParameterModelType.Float, null);
        ////            SimulateSoilCracking = ExtractFromString(db, "SoilCrack", ParameterModelType.Bool, null);
        ////            MaxInfiltrationIntoCracks = ExtractFromString(db, "MaxInfiltIntoCracks", ParameterModelType.Float, null);
        ////            SedimentDeliveryRatio = ExtractFromString(db, "SedDelivRatio", ParameterModelType.Float, null);
        ////            OrganicCarbon = ExtractFromString(db, "OrganicCarbon", ParameterModelType.Float, null);
        ////            CarbonNitrogenRatio = ExtractFromString(db, "CarbonNitrogenRatio", ParameterModelType.Float, null);
        ////            NitrogenMineralisationCoefficient = ExtractFromString(db, "NitrateMineralisationCoefficient", ParameterModelType.Float, null);

        ////            //CalculateTotals();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ErrorLogger.HandleError(ex, "", false);
        ////    }

        ////    return false;
        ////}

        ////private List<string> ScaleThickness(List<string> Thickness)
        ////{
        ////    var cumdepth = 0.0f;
        ////    var newlist = new List<float>();
        ////    foreach (var depth in Thickness)
        ////    {
        ////        var depthval = float.Parse(depth);
        ////        cumdepth = cumdepth + depthval;
        ////        newlist.Add(cumdepth);

        ////    }
        ////    var newlist2 = new List<string>();
        ////    foreach (var depth in newlist)
        ////    {
        ////        newlist2.Add(depth.ToString("F0"));
        ////    }
        ////    return newlist2;
        ////}

        ////private ParameterModel ExtractFromStringList(ApplicationDbContext.ApplicationDbContext db, List<string> list, int layerCount, string p)
        ////{
        ////    if (list != null)
        ////    {
        ////        var values = String.Join(",", list.ToArray());
        ////        return new ParameterModel(db, ParameterModelType.FloatVector, values);
        ////    }
        ////    return new ParameterModel(db, ParameterModelType.FloatVector, "");
        ////}

        ////private ParameterModel ExtractFromString(ApplicationDbContext.ApplicationDbContext db, string name, ParameterModelType type, string value)
        ////{
        ////    return new ParameterModel(db, type, value);
        ////}

        ////private List<String> ExtractElementArrayText(XmlElement parentnode, string basepath, string finalpath)
        ////{
        ////    var list = new List<string>();
        ////    XmlNode node;
        ////    if (!String.IsNullOrEmpty(basepath))
        ////    {
        ////        node = parentnode.SelectSingleNode(basepath);
        ////        if (node.HasChildNodes)
        ////        {
        ////            node = node.SelectSingleNode(finalpath);

        ////        }
        ////    }
        ////    else
        ////    {
        ////        node = parentnode.SelectSingleNode(finalpath);

        ////    }
        ////    if (node != null && node.HasChildNodes)
        ////    {
        ////        foreach (XmlNode child in node.ChildNodes)
        ////        {
        ////            list.Add(child.InnerText);
        ////        }
        ////    }
        ////    return list;
        ////}

        ////private string ExtractElementText(XmlElement parentnode, string basepath, string finalpath)
        ////{
        ////    XmlNode node;
        ////    if (!String.IsNullOrEmpty(basepath))
        ////    {
        ////        node = parentnode.SelectSingleNode(basepath);
        ////        if (node.HasChildNodes)
        ////        {
        ////            node = node.SelectSingleNode(finalpath);
        ////            return node.InnerText;
        ////        }
        ////    }
        ////    else
        ////    {
        ////        node = parentnode.SelectSingleNode(finalpath);
        ////        return node.InnerText;
        ////    }
        ////    return "";

        ////}

        ////private List<float> ExtractFloatArrayFromNode(XmlNode header, int count, string key)
        ////{
        ////    try
        ////    {
        ////        if (header != null && String.IsNullOrEmpty(key) == false)
        ////        {

        ////            XmlNode node = header.SelectSingleNode(key);
        ////            if (node != null)
        ////            {
        ////                XmlAttributeCollection attributes = node.Attributes;
        ////                if (attributes.Count > 0)
        ////                {
        ////                    List<float> values = new List<float>();
        ////                    for (int i = 0; i < count; ++i)
        ////                    {
        ////                        String attributekey = "value" + (i + 1);
        ////                        XmlNode attribute = attributes.GetNamedItem(attributekey);
        ////                        if (attribute != null)
        ////                        {
        ////                            float result;
        ////                            if (float.TryParse(attribute.Value, out result))
        ////                                values.Add(result);
        ////                            else
        ////                                values.Add(0);
        ////                        }
        ////                    }
        ////                    return values;
        ////                }
        ////            }
        ////        }
        ////        return null;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ErrorLogger.HandleError(ex, "", true);
        ////    }
        ////    return null;
        ////}

        ////internal void RemoveFromDatabase(ApplicationDbContext.ApplicationDbContext db)
        ////{
        ////    if (CnReductionAtFullCover != null) CnReductionAtFullCover.RemoveFromDatabase(db);
        ////    if (PAWC != null) PAWC.RemoveFromDatabase(db);
        ////    if (RunoffCurveNumber != null) RunoffCurveNumber.RemoveFromDatabase(db);
        ////    if (Stage1EvapLimitU != null) Stage1EvapLimitU.RemoveFromDatabase(db);
        ////    if (LayerCount != null) LayerCount.RemoveFromDatabase(db);
        ////    if (Stage2EvapCona != null) Stage2EvapCona.RemoveFromDatabase(db);
        ////    if (Depths != null) Depths.RemoveFromDatabase(db);
        ////    if (AirDryLimits != null) AirDryLimits.RemoveFromDatabase(db);
        ////    if (FieldCapacities != null) FieldCapacities.RemoveFromDatabase(db);
        ////    if (MaxDrainageFromLayer != null) MaxDrainageFromLayer.RemoveFromDatabase(db);
        ////    if (WiltingPoints != null) WiltingPoints.RemoveFromDatabase(db);
        ////    if (SaturationLimits != null) SaturationLimits.RemoveFromDatabase(db);
        ////    if (BulkDensity != null) BulkDensity.RemoveFromDatabase(db);
        ////    if (MaxReductionInCNDueToTill != null) MaxReductionInCNDueToTill.RemoveFromDatabase(db);
        ////    if (RainfallToRemoveRoughness != null) RainfallToRemoveRoughness.RemoveFromDatabase(db);
        ////    if (USLEK != null) USLEK.RemoveFromDatabase(db);
        ////    if (FieldSlope != null) FieldSlope.RemoveFromDatabase(db);
        ////    if (USLEP != null) USLEP.RemoveFromDatabase(db);
        ////    if (SlopeLength != null) SlopeLength.RemoveFromDatabase(db);
        ////    if (RillRatio != null) RillRatio.RemoveFromDatabase(db);
        ////    if (SimulateSoilCracking != null) SimulateSoilCracking.RemoveFromDatabase(db);
        ////    if (MaxInfiltrationIntoCracks != null) MaxInfiltrationIntoCracks.RemoveFromDatabase(db);
        ////    if (SedimentDeliveryRatio != null) SedimentDeliveryRatio.RemoveFromDatabase(db);
        ////    if (OrganicCarbon != null) OrganicCarbon.RemoveFromDatabase(db);
        ////    if (CarbonNitrogenRatio != null) CarbonNitrogenRatio.RemoveFromDatabase(db);
        ////    if (NitrogenMineralisationCoefficient != null) NitrogenMineralisationCoefficient.RemoveFromDatabase(db);
        ////    //Regions.Clear();
        ////    AdditionalPermissions.Clear();
        ////    if (EditHistory != null)
        ////    {
        ////        foreach (var record in EditHistory.ToList())
        ////        {
        ////            record.RemoveFromDatabase(db);
        ////        }
        ////        EditHistory.Clear();
        ////    }

        ////}
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}