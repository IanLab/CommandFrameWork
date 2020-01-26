using CommandCore.Data;
using System;
using System.Globalization;

namespace CommandCore.BroadcastBySharedFolder
{
    public static class CommandNameHelper
    {
        public const string CommandFileExtensionName = ".cmd";
        private static readonly CultureInfo UsCultureInfo = new CultureInfo("en-US");
        private const string Split = " ";
        private static readonly char[] SplitChars = Split.ToCharArray();
        public static string GetFileName(Command cmmd)
        {
            return $"{cmmd.UpdateEntity.GetType().Name}{Split}{cmmd.CurrentUpdateUser}{Split}{cmmd.CurrentUpdateDateTime.ToString(UsCultureInfo)}{CommandFileExtensionName}";
        }

        public static void ParseCmmdFileName(string cmmdFileName,
                                             out string entityTypeName,
                                             out string updateUser,
                                             out DateTime updateDateTime)
        {
            if (string.IsNullOrEmpty(cmmdFileName))
            {
                throw new ArgumentNullException(nameof(cmmdFileName));
            }

            var parts = cmmdFileName.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);

            if(parts.Length != 3)
            {
                throw new CommandFileNameFormatException(cmmdFileName);
            }

            entityTypeName = parts[0];
            updateUser = parts[1];
            try
            {
                updateDateTime = DateTime.Parse(parts[2], UsCultureInfo);
            }catch(FormatException formatExp)
            {
                throw new CommandFileNameFormatException(cmmdFileName, formatExp);
            }
        }
    }
}
