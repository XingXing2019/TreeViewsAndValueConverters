using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TreeViewsAndValueConverters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

        }
        #endregion

        #region On Loaded
        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get every logical drive on the machine
            foreach (var drive in Directory.GetLogicalDrives())
            {
                //Create a new item for it, Set the header and full path
                var item = new TreeViewItem() { Header = drive, Tag = drive };

                //Add a dummy item
                item.Items.Add(null);

                //Listen out for item expanded
                item.Expanded += Folder_Expanded;

                //Add it to the main treeView
                this.FolderView.Items.Add(item);
            }
        }
        #endregion

        #region Folder Expended
        /// <summary>
        /// When a folder is expanded, fins the sub folders/files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks
            var item = (TreeViewItem)sender;

            //Check if the item only contains the dummy folder
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            //Clear the dummy folder
            item.Items.Clear();
            #endregion

            #region Get Folders
            //Get item full path
            var fullPath = (string)item.Tag;

            //Create a blank lust for directories
            var directories = new List<string>();

            //Try and get directories from the folder
            //igorning any issues doing so 
            try
            {
                var dir = Directory.GetDirectories(fullPath);
                if (dir.Length > 0)
                    directories.AddRange(dir);
            }
            catch (Exception) { }

            directories.ForEach(directoryPath =>
            {
                //Creater directory item, set header as folder name and tag as folder path
                var subitem = new TreeViewItem() { Header = GetFileFolderName(directoryPath), Tag = directoryPath };

                //Add dummy item so we can expand folder
                subitem.Items.Add(null);

                //Handle expending
                subitem.Expanded += Folder_Expanded;

                //Add this item to the parent
                item.Items.Add(subitem);
            });
            #endregion

            #region Get Files
            //Get files
            var files = new List<string>();

            //Try and get files from the folder
            //igorning any issues doing so 
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch (Exception) { }

            files.ForEach(filepath =>
            {
                //Create file item, set header as file name and tage as file path 
                var subItem = new TreeViewItem() { Header = GetFileFolderName(filepath), Tag = filepath };

                //Add this file to the parent
                item.Items.Add(subItem);
            });
            #endregion
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Find the file or folder name from a full path
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static object GetFileFolderName(string directoryPath)
        {
            //If we have no path, return empty
            if (string.IsNullOrEmpty(directoryPath))
                return string.Empty;

            //Make all slashes back slash
            var normalizedPath = directoryPath.Replace('/', '\\');

            //Gat the index of the last back slash
            var lastSlash = normalizedPath.LastIndexOf('\\');

            //If there is no back slash, return the path itself
            if (lastSlash <= 0)
                return directoryPath;

            //Else return the name after the last back slash
            return directoryPath.Substring(lastSlash + 1);
        }
        #endregion
    }
}
