using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeListViewSample.Models
{
    public class TreeListViewFolderItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private string _name;
        private bool _isExpanded = false;
        private ObservableCollection<TreeListViewFolderItem> _subFolderItems;


        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string FullName { get; set; }
        public bool IsFolder { get; set; } = true;
        public int HierarchyLevel { get; set; } = 0;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));

                if (IsFolder)
                {
                    if (IsExpanded)
                    {
                        AddSubFolderItems();
                    }
                    else
                    {
                        RemoveSubFolderItems();
                    }
                }
            }
        }
        public ObservableCollection<TreeListViewFolderItem> Source { get; set; }
        public ObservableCollection<TreeListViewFolderItem> SubFolderItems
        {
            get => _subFolderItems;
            set
            {
                _subFolderItems = value;
                OnPropertyChanged(nameof(SubFolderItems));
            }
        }


        public TreeListViewFolderItem()
        {
            SubFolderItems = new ObservableCollection<TreeListViewFolderItem>();
        }

        public override string ToString()
        {
            return Name;
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void AddSubFolderItems()
        {
            try
            {
                var subFolderItems = GetSubFolderItems();
                var files = GetFiles();

                var index = Source.IndexOf(this);
                var hierarchyLevel = HierarchyLevel + 1;

                subFolderItems.ForEach(d =>
                {
                    index++;
                    d.HierarchyLevel = hierarchyLevel;
                    Source.Insert(index, d);
                    SubFolderItems.Add(d);
                });

                files.ForEach(f =>
                {
                    index++;
                    f.HierarchyLevel = hierarchyLevel;
                    Source.Insert(index, f);
                    SubFolderItems.Add(f);
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RemoveSubFolderItems()
        {
            try
            {
                for (int i = 0; i < SubFolderItems.Count; i++)
                {
                    SubFolderItems[i].IsExpanded = false;
                    Source.Remove(SubFolderItems[i]);
                }

                SubFolderItems.Clear();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TreeListViewFolderItem> GetSubFolderItems()
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(FullName);
                return directoryInfo.GetDirectories().Select(d =>
                new TreeListViewFolderItem()
                {
                    Name = d.Name,
                    FullName = d.FullName,
                    Source = Source,
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TreeListViewFolderItem> GetFiles()
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(FullName);
                return directoryInfo.GetFiles().Select(f =>
                new TreeListViewFolderItem()
                {
                    Name = f.Name,
                    FullName = f.FullName,
                    IsFolder = false
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
