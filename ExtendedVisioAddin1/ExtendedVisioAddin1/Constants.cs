using System;

namespace Rationally.Visio
{
    internal class Constants
    {
        public const string TemplateName = "Rationally Template";
        public const string RationallyTypeCell = "User.rationallyType";
        public const string RationallySite = "https://rationally.github.io/";
        public const int CellExists = -1;

        public static readonly string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\"; //<---- test path
        //public static readonly string FolderPath = Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles) + @"\rationally-visio\"; <--- Enable for working add in

        public const int LeftAlignment = 0;
    }
}
