using System.Linq;
using System;
using System.IO;
using Encoding = System.Text.Encoding;
using System.Globalization;
using System.Collections.Generic;

namespace HowLeaky.DataModels
{
    public class ClimateDataModel : DataModel

    {
        public String StationCode { get; set; }

        public string Country { get; set; }
        
        public String State { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }

        public string ImportedBy { get; set; }

        public DateTime? ImportedDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ClimateDataModel()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        protected void parseDownloadedMetData(string response)
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
        public void downloadMetData(int stationNo)
        {
            //Download the station using SILO API
            string response = "";

            parseDownloadedMetData(response);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public void downloadMetData(double latitude, double longitude)
        {
            //Download the station using SILO API
            string response = "";

            parseDownloadedMetData(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public ClimateDataModel(string fileName, string userName) : base(Guid.NewGuid(), fileName, userName, DateTime.UtcNow)
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
            catch(Exception ex)
            {
                //Handle exception
                //Just throw for now
                throw (new Exception(ex.Message));
            }

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
                bool readfirstdata = false;
                string lastline;
                while ((line = reader.ReadLine()) != null)
                {
                    if (foundheader == false)
                    {
                        if (line.Contains("date") && line.Contains("jday"))
                        {
                            foundheader = true;
                        }
                        else
                        {
                            List<String> items = new List<String>(line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                            if (items.Count() < 8)
                            {
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
                            }
                        }
                    }
                    else
                    {
                        List<String> items = new List<String>(line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                        if (!readfirstdata)
                        {
                            if (items.Count() == 8)
                            {

                                var date = TryParseDate(items[0]);
                                if (date != null)
                                {
                                    StartDate = date;
                                    readfirstdata = true;
                                }
                            }
                        }
                        else
                        {
                            if (items.Count() == 8)
                            {

                                var date = TryParseDate(items[0]);
                                if (date != null)
                                {
                                    EndDate = date;
                                }
                            }
                        }
                    }

                    lastline = line;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private DateTime? TryParseDate(string text)
        {
            if (text.Length == 8)
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(text, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out dateTime))
                {
                    return dateTime;
                }
            }
            return null;
        }
    }

}