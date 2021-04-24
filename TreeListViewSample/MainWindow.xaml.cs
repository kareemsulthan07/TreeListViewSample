using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TreeListViewSample.Models;

namespace TreeListViewSample
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TreeListViewFolderItem> TreeListViewItems { get; set; }
        private ObservableCollection<TreeViewFolderItem> TreeViewItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            TreeListViewItems = new ObservableCollection<TreeListViewFolderItem>();
            TreeViewItems = new ObservableCollection<TreeViewFolderItem>();

            treeListView.ItemsSource = TreeListViewItems;
            treeView.ItemsSource = TreeViewItems;
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using CommonOpenFileDialog dlg = new CommonOpenFileDialog("Select a folder");
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                dlg.Multiselect = false;
                dlg.AllowNonFileSystemItems = false;
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    TreeListViewItems.Clear();
                    TreeViewItems.Clear();

                    var folderPath = dlg.FileName;

                    var treeListViewFolderItem = new TreeListViewFolderItem()
                    {
                        Name = System.IO.Path.GetFileName(folderPath),
                        FullName = folderPath,
                        IsFolder = true,
                        Source = TreeListViewItems,
                    };
                    TreeListViewItems.Add(treeListViewFolderItem);

                    treeListViewFolderItem.IsExpanded = true;

                    TreeViewFolderItem treeViewFolderItem = new TreeViewFolderItem()
                    {
                        Name = System.IO.Path.GetFileName(folderPath),
                        FullName = folderPath,
                        IsFolder = true,
                    };
                    TreeViewItems.Add(treeViewFolderItem);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                e.Handled = true;

                var listViewItem = (ListViewItem)sender;
                if (listViewItem.DataContext is TreeListViewFolderItem folderItem)
                {
                    folderItem.IsExpanded = !folderItem.IsExpanded;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                e.Handled = true;

                var treeViewItem = (TreeViewItem)sender;
                if (treeViewItem.DataContext is TreeViewFolderItem treeViewFolderItem)
                {
                    if (treeViewFolderItem.IsFolder)
                    {
                        treeViewFolderItem.SubFolderItems.Clear();

                        var subTreeViewFolderItems = GetTreeViewFolderItems(treeViewFolderItem.FullName);
                        var subTreeViewFileItems = GetTreeViewFileItems(treeViewFolderItem.FullName);

                        subTreeViewFolderItems.ForEach(d => treeViewFolderItem.SubFolderItems.Add(d));
                        subTreeViewFileItems.ForEach(f => treeViewFolderItem.SubFolderItems.Add(f)); 
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TreeViewItem_Collapsed(object sender, RoutedEventArgs e)
        {
            try
            {
                e.Handled = true;

                var treeViewItem = (TreeViewItem)sender;
                if (treeViewItem.DataContext is TreeViewFolderItem treeViewFolderItem)
                {
                    if (treeViewFolderItem.IsFolder)
                    {
                        treeViewFolderItem.SubFolderItems.Clear();

                        treeViewFolderItem.SubFolderItems.Add(new TreeViewFolderItem()
                        {
                            Name = ".."
                        });
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TreeViewFolderItem> GetTreeViewFolderItems(string path)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                return directoryInfo.GetDirectories()
                    .Select(d => new TreeViewFolderItem()
                    {
                        Name = d.Name,
                        FullName = d.FullName,
                        IsFolder = true,
                    })
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TreeViewFolderItem> GetTreeViewFileItems(string path)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                return directoryInfo.GetFiles()
                    .Select(f => new TreeViewFolderItem()
                    {
                        Name = f.Name,
                        FullName = f.FullName,
                        IsFolder = false,
                    })
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
