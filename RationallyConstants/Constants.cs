using System;

namespace Rationally.Visio.RationallyConstants
{
    internal static class Constants
    {

        public const string TemplateName = "Decision View";
        public const string RationallySite = "https://rationally.github.io/";
        public const int CellExists = -1;
        public const double Epsilon = 0.001;
        //
#if DEBUG
        public static readonly string MyShapesFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources"));//<---- test path
#else
        public static readonly string MyShapesFolder = Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles) + @"\rationally-visio\"; //<--- Enable for working add in
#endif
        public const int LeftAlignment = 0; //Visio's own enum is wrong
        public const int SupportedAmountOfAlternatives = 3;
    }

}
