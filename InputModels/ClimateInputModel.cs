using System.Linq;
using System;
using System.IO;
using Encoding = System.Text.Encoding;
using System.Globalization;
using System.Collections.Generic;
using HowLeaky.Tools.Helpers;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    public enum EEvaporationInputOptions { Use_EPan }

    public class ClimateInputModel : InputModel

    {
        public String StationCode { get; set; }

        public string Country { get; set; }

        public String State { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
        public DateTime? EndDate { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;

        [XmlIgnore]
        public List<double> Rain { get; set; }
        [XmlIgnore]
        public List<double> MaxT { get; set; }
        [XmlIgnore]
        public List<double> MinT { get; set; }
        [XmlIgnore]
        public List<double> PanEvap { get; set; }
        [XmlIgnore]
        public List<double> Radiation { get; set; }
        [XmlIgnore]
        public List<double> VP { get; set; }

        public string ImportedBy { get; set; }

        public DateTime? ImportedDate { get; set; }

        public EEvaporationInputOptions _EvaporationInputOptions { get; set; } = EEvaporationInputOptions.Use_EPan;
        public int EvaporationInputOptions
        {
            get
            {
                return (int)_EvaporationInputOptions;
            }
            set
            {
                _EvaporationInputOptions = (EEvaporationInputOptions)value;
            }
        }
        public double PanEvapMultiplier { get; set; } = 1;
        public double RainfallMultiplier { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public ClimateInputModel()
        {
            Rain = new List<double>();
            MaxT = new List<double>();
            MinT = new List<double>();
            PanEvap = new List<double>();
            Radiation = new List<double>();
            VP = new List<double>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        protected void ParseDownloadedMetData(string response)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));

            try
            {
                LoadMetaData(ms);
            }
            catch (Exception ex)
            {
                ms.Close();

                //Handle exception
                //Just throw for now
                throw (new Exception(ex.Message));
            }

            ms.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationNo"></param>
        public void DownloadMetData(int stationNo)
        {
            //Download the station using SILO API
            string response = "";

            ParseDownloadedMetData(response);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public void DownloadMetData(double latitude, double longitude)
        {
            //Download the station using SILO API
            string response = "";

            ParseDownloadedMetData(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public ClimateInputModel(string fileName, string userName) : base(Guid.NewGuid(), fileName, userName, DateTime.UtcNow)
        {
            LoadMetaData(fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadMetaData(string fileName)
        {
            Name = fileName.Replace(".p51", "");

            try
            {
                StreamReader sr = new StreamReader(fileName);
                LoadMetaData(sr.BaseStream);

                sr.Close();
            }
            catch (Exception ex)
            {
                //Handle exception
                //Just throw for now
                throw (new Exception(ex.Message));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            LoadMetaData(new FileStream(FileName, FileMode.Open));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        private void LoadMetaData(Stream stream)
        {

            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                bool foundheader = false;
                while ((line = reader.ReadLine()) != null)
                {
                    List<String> items = new List<String>(line.Trim().Split(new string[] { " ", "\t", "," }, StringSplitOptions.RemoveEmptyEntries));
                    if (items[0] != "//") {
                        if (foundheader == false)
                        {
                            //Parse the header

                            float lat;
                            float lon;
                            if (float.TryParse(items[0], out lat))
                            {
                                Latitude = lat;
                            }
                            if (float.TryParse(items[1], out lon))
                            {
                                Latitude = lon;
                            }

                            items.RemoveRange(0, 2);
                            Comments = String.Join(" ", items.ToArray());

                            foundheader = true;

                        }
                        else
                        {
                            if (items.Count() == 8)
                            {

                                var date = DateUtilities.TryParseDate(items[0]);
                                if (date != null)
                                {
                                    if (StartDate == null)
                                    {
                                        StartDate = date;
                                    }
                                    //Parse the met data
                                    EndDate = date;

                                    MaxT.Add(double.Parse(items[2]));
                                    MinT.Add(double.Parse(items[3]));
                                    Rain.Add(double.Parse(items[4]));
                                    PanEvap.Add(double.Parse(items[5]));
                                    Radiation.Add(double.Parse(items[6]));
                                    VP.Add(double.Parse(items[7]));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}