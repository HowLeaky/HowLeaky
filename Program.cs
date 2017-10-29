using HowLeaky.DataModels;
using HowLeaky.Models;
using HowLeakyModels.Deserialiser;
using System.Collections.Generic;

namespace HowLeaky
{
    class Program
    {
        static void Main(string[] args)
        {
            // var rootAttribute = new XmlRootAttribute();
            // rootAttribute.ElementName = "TillageType";
            // rootAttribute.IsNullable = true;

            // //XmlSerializer xmlSerialiser = new XmlSerializer(typeof(TillageModel), rootAttribute);
            //// XmlSerializer xmlSerialiser = new XmlSerializer(typeof(TillageModel), rootAttribute);
            // XmlSerializer xmlSerialiser = new XmlSerializer(typeof(TillageModel));


            // StreamReader sr = new StreamReader(@"C: \Users\Al\Desktop\Hard chisel_at_harvest.till2");


            // XmlDocument xmlDoc = new XmlDocument();
            // xmlDoc.Load(sr);
            // XmlNodeList address = xmlDoc.GetElementsByTagName("TillageType");

            // //Serialiser.Deserialise<TillageModel>

            // new MemoryStream()
            //Test all tillage models
            #region Load tillage Models
            List<TillageObjectDataModel> tillageModels = new List<TillageObjectDataModel>();
            tillageModels.Add((TillageObjectDataModel)Serialiser.Deserialise<TillageObjectDataModel>(@"C:\Users\Al\Desktop\Hard chisel_at_harvest.till2", "TillageType"));
            tillageModels.Add((TillageObjectDataModel)Serialiser.Deserialise<TillageObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Tillage\Tyned scarifier.till", "TillageType"));
            tillageModels.Add((TillageObjectDataModel)Serialiser.Deserialise<TillageObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Tillage\ZT harvest -all crops.till", "TillageType"));
            tillageModels.Add((TillageObjectDataModel)Serialiser.Deserialise<TillageObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Tillage\ZT planter.till", "TillageType"));
            tillageModels.Add((TillageObjectDataModel)Serialiser.Deserialise<TillageObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Tillage\Disc opener planter.till", "TillageType"));
            tillageModels.Add((TillageObjectDataModel)Serialiser.Deserialise<TillageObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Tillage\Hard chisel_at_harvest.till", "TillageType"));
            tillageModels.Add((TillageObjectDataModel)Serialiser.Deserialise<TillageObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Tillage\Multi-tyned planter.till", "TillageType"));
            #endregion
            #region Load Pest Models
            //Test all pest models
            List<PesticideObjectDataModel> pestModels = new List<PesticideObjectDataModel>();
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\24-D - fallowA - WS.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\24-D - fallowAB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\24-D - fallowC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\24-D - fallowD.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\24-D - wheatABC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\24-D - wheatD.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Aciflurofen-MungbeanB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Atrazine - sorghumA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Atrazine - sorghumB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Atrazine - sorghumC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Atrazine - sorghumD.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Chlorsulfuron-WheatBC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Fluroxypyr - sorghumA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Fluroxypyr - sorghumD.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Fluroxypyr-Fallow.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - fallowA - WS.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - fallowAB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - fallowC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - fallowD.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - Mungbean.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - sorghumA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - sorghumB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - sorghumC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - sorghumD.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Glyphosate - WheatABCD.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Haloxyfop-Chickpea.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Haloxyfop-MungbeanA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Haloxyfop-MungbeanB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Haloxyfop-SunflowerAB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Haloxyfop-SunflowerC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Imazethapyr-Mungbean.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Isoxaflutole-Chickpea.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\MCPA-Wheat.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Metsulfuronmethyl-Fallow.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Metsulfuronmethyl-FallowC.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Metsulfuronmethyl-Wheat.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Paraquat-ChickpeaA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Paraquat-Fallow.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Paraquat-FallowA - WS.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Paraquat-SorghumA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Paraquat-Sunflower.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Simazine-Chickpea.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Smetolachlor - MungbeanB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Smetolachlor - SorghumA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Smetolachlor - SorghumB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Smetolachlor - Sunflower.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Smetolachlor-FallowA.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Smetolachlor-FallowB.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Terbuthylazine-Chickpea.pest", "PesticideType"));
            pestModels.Add((PesticideObjectDataModel)Serialiser.Deserialise<PesticideObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Pesticides\Trifluralin-Sunflower.pest", "PesticideType"));
            #endregion
            #region Load Irrigation models
            //Test all pest models
            List<IrrigationDataModel> irrigModels = new List<IrrigationDataModel>();
            irrigModels.Add((IrrigationDataModel)Serialiser.Deserialise<IrrigationDataModel>(@"C:\projects\HLTest\Irrigation\Banana D Class.irri", "IrrigationType"));
            irrigModels.Add((IrrigationDataModel)Serialiser.Deserialise<IrrigationDataModel>(@"C:\projects\HLTest\Irrigation\Banana B-C Class.irri", "IrrigationType"));
            #endregion
            #region Load Phosphorus models
            List<PhosphorusDataModel> pmodels = new List<HowLeaky.DataModels.PhosphorusDataModel>();
            pmodels.Add((PhosphorusDataModel)Serialiser.Deserialise<PhosphorusDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Phosphorus\Phosphorus.phos", "PhosphorusType"));
            #endregion
            #region  SoilDataModel 
            List<SoilDataModel> SDmodels = new List<HowLeaky.DataModels.SoilDataModel>();
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_41_Dermosols_(structured_clay_or_clay_loam_surface)_with_Mod_deep_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_70_Dermosols_(sealing_loamy_surface)_with_Mod_deep_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_109_Dermosols_(sandy_surface)_with_Very_deep_A_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_209_Sodosols_(loamy_surface)_with_Mod_deep_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_210_Sodosols_(loamy_surface)_with_Mod_deep_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_211_Sodosols_(loamy_surface)_with_Mod_deep_A_deep_B_that_are_Moderately_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_213_Sodosols_(loamy_surface)_with_Deep_A_shallow_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_217_Sodosols_(loamy_surface)_with_Deep_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_219_Sodosols_(loamy_surface)_with_Deep_A_deep_B_that_are_Moderately_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_221_Sodosols_(loamy_surface)_with_Very_deep_A_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_249_Sodosols_(mod_deep_with_Very_deep_A_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_273_Sodosols_(shallow_sandy_surface)_with_Deep_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_277_Sodosols_(shallow_sandy_surface)_with_Very_deep_A_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_300_Tenosols_and_Rudosol_and_Podosols_(sandy)_with_Deep_A_shallow_B_that_are_Highly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_303_Tenosols_and_Rudosol_and_Podosols_(sandy)_with_Deep_A_deep_B_that_are_Moderately_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_304_Tenosols_and_Rudosol_and_Podosols_(sandy)_with_Deep_A_deep_B_that_are_Highly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_322_Rudosols_and_Tenosols_(loamy)_with_Mod_deep_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_402_Medium_Vertosols_with_Mod_deep_A_shallow_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_406_Medium_Vertosols_with_Mod_deep_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_413_Medium_Vertosols_with_Deep_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_425_Light_Vertosols_with_Shallow_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_442_Light_Vertosols_with_Deep_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_5_Heavy_Vertosols_with_Shallow_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_6_Heavy_Vertosols_with_Shallow_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_13_Heavy_Vertosols_with_Mod_deep_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_14_Heavy_Vertosols_with_Mod_deep_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_18_Heavy_Vertosols_with_Deep_A_shallow_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_21_Heavy_Vertosols_with_Deep_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_22_Heavy_Vertosols_with_Deep_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_33_Dermosols_(structured_clay_or_clay_loam_surface)_with_Shallow_A_deep_B_that_are_Very_slowly_permeable.soil", "SoilType"));
            //SDmodels.Add((SoilDataModel)Serialiser.Deserialise<SoilDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Soils\grp_34_Dermosols_(structured_clay_or_clay_loam_surface)_with_Shallow_A_deep_B_that_are_Slowly_permeable.soil", "SoilType"));
            #endregion
            #region SolutesDataModel 
            List<SolutesDataModel> Smodels = new List<HowLeaky.DataModels.SolutesDataModel>();
            //Have no test solutes
            #endregion
            #region ClimateDataModel
            List<ClimateDataModel> CDmodels = new List<HowLeaky.DataModels.ClimateDataModel>();
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2450_14745_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2450_14780_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2450_14800_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2455_14745_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2455_14760_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2455_14790_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2455_14805_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2465_14705_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2465_14755_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2465_14770_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2470_14720_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2470_14755_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2475_14765_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2295_14825_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2300_14820_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2300_14830_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2305_14835_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2310_14825_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2310_14840_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2315_14815_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2315_14825_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2320_14835_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2325_14820_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2325_14830_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2330_14815_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2330_14835_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2330_14840_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2335_14835_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2340_14840_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2345_14815_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2345_14820_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2345_14825_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2345_14830_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2350_14850_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2355_14815_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2355_14820_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2355_14830_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2360_14770_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2360_14785_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2360_14795_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2365_14785_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2365_14795_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2365_14810_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2370_14795_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2370_14810_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2375_14765_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2375_14785_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2380_14775_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2380_14795_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2380_14805_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2385_14765_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2385_14800_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2390_14785_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2390_14790_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2395_14775_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2400_14790_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2405_14775_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2405_14790_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2410_14790_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2415_14780_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2415_14800_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2420_14785_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2420_14800_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2420_14805_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2425_14760_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2425_14790_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2425_14810_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2430_14765_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2430_14800_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2430_14810_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2435_14755_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2435_14785_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2440_14795_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2445_14750_ADJ.p51",null));
            //CDmodels.Add(new ClimateDataModel(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Climate\2445_14810_ADJ.p51",null));

            #endregion
            #region Project.cs
            List<Project> Pmodels = new List<HowLeaky.Models.Project>();

            #endregion
            #region LAIVegModel
            List<LAIVegObjectDataModel> LAUVmodels = new List<HowLeaky.DataModels.LAIVegObjectDataModel>();
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\C management Chickpeas CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\C management Mungbeans JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\C management Sunflowers JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\D management Chickpeas CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\D management Forage Sorghum CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\D management Oats - CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\D management Sorghum CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\D management Wheat - CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\Maize dynamic model JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\Mungbeans JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\Mungbeans JO_monocrop.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\Sunflowers JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\Sunflowers JO_monocrop.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\A and B management Chickpeas CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\A and B management Chickpeas CQ 1 JO_monocrop.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\A and B management Sorghum CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\A and B management Sorghum CQ 1 JO_monocrop.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\A and B management Wheat CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\A and B management Wheat CQ 1 JO_monocrop.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\C and D management Sorghum CQ 1 JO.vege", "VegetationType"));
            LAUVmodels.Add((LAIVegObjectDataModel)Serialiser.Deserialise<LAIVegObjectDataModel>(@"C:\Users\Al\Dropbox\HL Testing\FI_Nogoa_HL_files\Vegetation\C and D management Wheat - CQ 1 JO.vege", "VegetationType"));

            #endregion
            #region CoverVegModel 
            List<CoverVegObjectDataModel> CVmodels = new List<HowLeaky.DataModels.CoverVegObjectDataModel>();
            CVmodels.Add((CoverVegObjectDataModel)Serialiser.Deserialise<CoverVegObjectDataModel>(@"C:\projects\HLTest\Vegetation\Banana interrow (cover model) D Class2.vege", "VegetationType"));
            CVmodels.Add((CoverVegObjectDataModel)Serialiser.Deserialise<CoverVegObjectDataModel>(@"C:\projects\HLTest\Vegetation\Banana interrow (cover model) D Class.vege", "VegetationType"));
            CVmodels.Add((CoverVegObjectDataModel)Serialiser.Deserialise<CoverVegObjectDataModel>(@"C:\projects\HLTest\Vegetation\Banana row (cover model) plant and 4 ratoons_B Class.vege", "VegetationType"));
            CVmodels.Add((CoverVegObjectDataModel)Serialiser.Deserialise<CoverVegObjectDataModel>(@"C:\projects\HLTest\Vegetation\Banana row (cover model) plant and 4 ratoons_C Class.vege", "VegetationType"));
            CVmodels.Add((CoverVegObjectDataModel)Serialiser.Deserialise<CoverVegObjectDataModel>(@"C:\projects\HLTest\Vegetation\Banana row (cover model) plant and 4 ratoons_D Class.vege", "VegetationType"));
            CVmodels.Add((CoverVegObjectDataModel)Serialiser.Deserialise<CoverVegObjectDataModel>(@"C:\projects\HLTest\Vegetation\Banana interrow (cover model) B Class.vege", "VegetationType"));
            CVmodels.Add((CoverVegObjectDataModel)Serialiser.Deserialise<CoverVegObjectDataModel>(@"C:\projects\HLTest\Vegetation\Banana interrow (cover model) C Class.vege", "VegetationType"));

            #endregion
            #region NitrateDataModel 
            List<NitrateDataModel> Nmodels = new List<HowLeaky.DataModels.NitrateDataModel>();
            //Nmodels.Add((NitrateDataModel)Serialiser.Deserialise<NitrateDataModel>(@"C:\projects\HLTest\Nitrate\Interrow.nitr", "NitratesType"));
            //Nmodels.Add((NitrateDataModel)Serialiser.Deserialise<NitrateDataModel>(@"C:\projects\HLTest\Nitrate\mnth250.nitr", "NitratesType"));
            //Nmodels.Add((NitrateDataModel)Serialiser.Deserialise<NitrateDataModel>(@"C:\projects\HLTest\Nitrate\mnth350.nitr", "NitratesType"));
            //Nmodels.Add((NitrateDataModel)Serialiser.Deserialise<NitrateDataModel>(@"C:\projects\HLTest\Nitrate\mnth450.nitr", "NitratesType"));
            Nmodels.Add((NitrateDataModel)Serialiser.Deserialise<NitrateDataModel>(@"C:\projects\HLTest\Nitrate\fnt250.nitr", "NitratesType"));
            //Nmodels.Add((NitrateDataModel)Serialiser.Deserialise<NitrateDataModel>(@"C:\projects\HLTest\Nitrate\fnt350.nitr", "NitratesType"));
            //Nmodels.Add((NitrateDataModel)Serialiser.Deserialise<NitrateDataModel>(@"C:\projects\HLTest\Nitrate\fnt450.nitr", "NitratesType"));

            #endregion

        }
    }
}
