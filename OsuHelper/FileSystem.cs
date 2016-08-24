// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <FileSystem.cs>
//  Created By: Alexey Golub
//  Date: 24/08/2016
// ------------------------------------------------------------------ 

using System;
using System.IO;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using NegativeLayer.Extensions;
using NegativeLayer.WPFExtensions;

namespace OsuHelper
{
    public static class FileSystem
    {
        private static string TempDirectory
            => Path.Combine(Environment.SpecialFolder.ApplicationData.GetPath(), "OsuHelper", "Temp");

        static FileSystem()
        {
            // Clear temp files on exit
            DispatcherHelper.UIDispatcher.InvokeSafe(() =>
            {
                Application.Current.Exit += (sender, args) =>
                {
                    try
                    {
                        Directory.Delete(TempDirectory, true);
                    }
                    catch
                    {
                        // Oh well
                    }
                };
            });
        }

        private static void CreateTempDirectory()
        {
            try
            {
                Directory.CreateDirectory(TempDirectory);
            }
            catch
            {
                // Already exists or rip
            }
        }

        public static string GetTempFile(string prefix = "osu_helper", string extension = "tmp")
        {
            CreateTempDirectory();
            return Path.Combine(TempDirectory, prefix + "_" + DateTime.Now.ToFileTimeUtc() + "." + extension);
        }
    }
}