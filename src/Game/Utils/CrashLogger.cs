using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;

namespace Game.Utils
{
    public static class CrashLogger
    {
        private const string DIRECTORY_NAME = "CrashReports";

        public static void SaveCrashInfo(ApplicationUnhandledExceptionEventArgs e)
        {
            if (e == null) return;

            var innerMessage = "";
            var innerStackTrace = "";
            var date = DateTime.UtcNow.Date.ToString("yy-MMM-dd");
            var time = DateTime.UtcNow.Ticks;
            var message = e.ExceptionObject.Message;
            var stackTrace = e.ExceptionObject.StackTrace;

            var fileSuffix = message + date + time;

            if (e.ExceptionObject.InnerException != null)
            {
                innerMessage = e.ExceptionObject.InnerException.Message;
                innerStackTrace = e.ExceptionObject.InnerException.StackTrace;
            }

            var fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!fileStorage.GetDirectoryNames().Contains(DIRECTORY_NAME))
            {
                fileStorage.CreateDirectory(DIRECTORY_NAME);
            }
            var fileName = DIRECTORY_NAME + "\\" + fileSuffix + ".txt";

            using (var fileWriter = new StreamWriter(fileStorage.OpenFile(fileName, FileMode.Append)))
            {
                fileWriter.WriteLine(message + stackTrace + innerMessage + innerStackTrace);
            }
        }
    }
}