// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ItemsRepeaterHeaderEffect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        #region NotiBase

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private ObservableCollection<Group> _Groups = new ObservableCollection<Group>();
        public ObservableCollection<Group> Groups
        {
            get { return _Groups; }
            set
            {
                if (_Groups == value)
                    return;

                _Groups = value;
                OnPropertyChanged();
            }
        }

        private Group _CurrentGroup;
        public Group CurrentGroup
        {
            get { return _CurrentGroup; }
            set
            {
                if (_CurrentGroup == value)
                    return;

                _CurrentGroup = value;
                OnPropertyChanged();

                if (value != null)
                {
                    Debug.WriteLine(value.Header);
                }
            }
        }

        private bool _IsControlLoaded = true;
        public bool IsControlLoaded
        {
            get { return _IsControlLoaded; }
            set
            {
                if (_IsControlLoaded == value)
                    return;

                _IsControlLoaded = value;
                OnPropertyChanged();
            }
        }


        public MainPage()
        {
            for (int i = 0; i < 50; i++)
                Groups.Add(new Group()
                {
                    Header = "Header" + i,
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
                });

            this.InitializeComponent();
        }

        private void ItemsRepeater_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            var control = args.Element as GroupControl;
            CreatedFromItemsRepeater(control);
        }

        private void ItemsRepeater_ElementClearing(ItemsRepeater sender, ItemsRepeaterElementClearingEventArgs args)
        {
            var control = args.Element as GroupControl;
            CreatedFromItemsRepeater(control);
        }

        private void CreatedFromItemsRepeater(GroupControl groupControl)
        {
            groupControl.CreatedFromItemsRepeater(ScrollViewer);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var anchor = this.ScrollViewer.CurrentAnchor;
            if (anchor is GroupControl groupControl)
                CurrentGroup = groupControl.Group;
        }



        public void LoadControl()
        {
            IsControlLoaded = true;
        }

        public void UnloadControl()
        {
            IsControlLoaded = false;
        }
    }

    public class Group : INotifyPropertyChanged
    {

        #region NotiBase

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string _Header;
        public string Header
        {
            get { return _Header; }
            set
            {
                if (_Header == value)
                    return;

                _Header = value;
                OnPropertyChanged();
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description == value)
                    return;

                _Description = value;
                OnPropertyChanged();
            }
        }

    }
}
