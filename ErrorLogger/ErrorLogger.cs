using System;
using System.Reflection;

namespace HowLeaky.ErrorLogger
{
    public class ErrorLogger
    {
        static readonly object newlock = new object();

        public static void HandleError(Exception ex, String title, bool showtouser)
        {
            try
            {
                lock (newlock)
                {
                    String LogText = GenerateMessage(ex, title);
                    System.Diagnostics.Debug.WriteLine(LogText);

                    //ApplicationDbContext db = new ApplicationDbContext();

                    //ApplicationUser user = null;
                    //String username = HttpContext.Current.User.Identity.Name;
                    //if (String.IsNullOrEmpty(username) == false)
                    //    user = db.Users.SingleOrDefault(u => u.UserName == username);

                    //AnalyticsErrorRecord record = new AnalyticsErrorRecord();
                    //record.AspNetUser = user;
                    //record.timeStamp = DateTime.UtcNow;
                    //record.deviceType = "Website";
                    //record.connectionType = "Online";
                    //record.appVersionNumber = "N.A.";
                    //record.iOSVersionNumber = "N.A.";
                    //record.deviceID = "N.A.";
                    //record.exceptionName = ex.Message;
                    //record.exceptionReason = ex.ToString();
                    //record.userInfo = "";
                    //record.stackSymbols = ex.StackTrace;
                    //record.stackReturnAddresses = "";
                    //record.developerMsg = title;
                    //db.ErrorLogs.Add(record);
                    //db.SaveChanges();

                    // SaveToLogFile(LogText);
                    //if (ShowToUser)
                    //    MessageBox.Show(LogText, "An error has occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
            catch (Exception ex2)
            {
                //SaveToLogFile(ex2.ToString());
                //MessageBox.Show(ex2.ToString(), "Error logger crashed", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private static string GenerateMessage(Exception ex, string title)
        {
            try
            {
                String msg = "";
                String versioninfo = GetVersionInfo();

                if (String.IsNullOrEmpty(title) == false)
                    msg = $"Title: {title}\r\n";
                if (ex != null)
                {
                    msg = $"{msg}Message: {ex.Message}\r\n";
                    msg = $"{msg}Version: {versioninfo}\r\n";
                    msg = $"{msg}Source: {ex.Source}\r\n";
                  //  msg = String.Format("{0}Target Site: {1}\r\n", msg, ex.TargetSite);
                    msg = $"{msg}StackTrace: {ex.StackTrace}\r\n";
                    msg = $"{msg}Inner Exception: {ex.InnerException}\r\n";
                    msg = $"{msg}Data: {ex.Data}\r\n";
                }
                msg = $"{msg}Date/Time: {DateTime.Now}\r\n";
                return msg;
            }
            catch (Exception ex2)
            {
                return ex2.ToString();
            }
            // return "";
        }

        private static string GetVersionInfo()
        {
            try
            {
                //Assembly assem = Assembly.GetEntryAssembly();
                //if (assem != null)
                //{
                //    AssemblyName assemname = assem.GetName();
                //    if (assemname != null)
                //    {
                //        Version version = assemname.Version;
                //        return String.Format("{0} V{1}", assemname.Name, version.ToString());
                //    }
                //}
            }
            catch (Exception ex2)
            {
                return ex2.ToString();
            }
            return "No Version Information Found";
        }

        //private static void SaveToLogFile(String LogText)
        //{
        //    try
        //    {

        //        String basedirectory = HttpContext.Current.Server.MapPath(String.Format("~/App_Data"));

        //        FileStream fs = new FileStream(basedirectory + "/ErrorLog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //        StreamWriter s = new StreamWriter(fs);
        //        s.Close();
        //        fs.Close();
        //        FileStream fs1 = new FileStream(basedirectory + "/ErrorLog.txt", FileMode.Append, FileAccess.Write);
        //        StreamWriter s1 = new StreamWriter(fs1);
        //        s1.Write(LogText);
        //        s1.Write("===========================================================================================\r\n");
        //        s1.Close();
        //        fs1.Close();

        //    }
        //    catch
        //    {
        //        //MessageBox.Show("An error has occurred and then whoops... the error logger also crashed when trying to save the log file.", "Error logger Problem", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        //    }
        //}
    }
}
