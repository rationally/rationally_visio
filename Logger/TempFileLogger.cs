using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rationally.Visio.Logger
{
    class TempFileLogger
    {

        public static void Log(string toLog)
        {

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string logFilePath = Path.Combine(appData, "Rationally", "tempLog.txt");
            if (!File.Exists(logFilePath))
            {
                FileStream createdlogFile = File.Create(logFilePath);
                createdlogFile.Close();
            }


            using (StreamWriter logFile = new StreamWriter(logFilePath, true))
            {
                logFile.WriteLine(toLog);
            }
        }
    }
}
