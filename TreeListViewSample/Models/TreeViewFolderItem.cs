using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TreeListViewSample.Models
{
    public class TreeViewFolderItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private string _name;
        private bool _isFolder;
        private ObservableCollection<TreeViewFolderItem> _subTreeViewFolderItems;


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
        public bool IsFolder 
        {
            get => _isFolder;
            set
            {
                _isFolder = value;

                if (IsFolder)
                {
                    SubFolderItems = new ObservableCollection<TreeViewFolderItem>()
                    {
                        new TreeViewFolderItem()
                        {
                            Name = ".."
                        },
                    };
                }
                else
                {
                    SubFolderItems = new ObservableCollection<TreeViewFolderItem>();
                }
            }
        }
        public ObservableCollection<TreeViewFolderItem> SubFolderItems
        {
            get => _subTreeViewFolderItems;
            set
            {
                _subTreeViewFolderItems = value;
                OnPropertyChanged(nameof(SubFolderItems));
            }
        }


        public TreeViewFolderItem()
        {
            
        }

        public override string ToString()
        {
            return Name;
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
