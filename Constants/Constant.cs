using System;

namespace Rationally.Visio.Constants
{
    internal class Constant
    {
        public const string TemplateName = "Rationally Template";
        public const string RationallySite = "https://rationally.github.io/";
        public const int CellExists = -1;

#if DEBUG
        public static readonly string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\"; //<---- test path
#else
        public static readonly string FolderPath = Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles) + @"\rationally-visio\"; //<--- Enable for working add in
#endif
        public const int LeftAlignment = 0;
    }
}
