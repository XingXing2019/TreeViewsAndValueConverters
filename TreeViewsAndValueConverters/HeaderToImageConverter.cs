﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TreeViewsAndValueConverters
{
    /// <summary>
    /// Converts a full path to a specific image type of a frive, folder or file
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter instance = new HeaderToImageConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get the full path
            var path = (string)value;

            //Check if the path is null
            if (path == null)
                return null;

            //Get the name of the file/folder
            var name = (string)MainWindow.GetFileFolderName(path);

            //By default, we presume an image
            var image = "Images/file.png";

            //If the name is blank, we presume it is a drive as we cannot have a blank file or folder name
            if(string.IsNullOrEmpty(name))
                image = "Images/drive.png";
            else if(new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "Images/folderOpen.png";

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
