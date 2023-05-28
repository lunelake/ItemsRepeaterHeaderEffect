// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using System.ComponentModel;
using System.Diagnostics;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ItemsRepeaterHeaderEffect
{
    public sealed partial class GroupControl : UserControl, INotifyPropertyChanged
    {
        #region NotiBase

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public GroupControl()
        {
            this.InitializeComponent();

            this.Loaded += GroupControl_Loaded;
            this.Unloaded += GroupControl_Unloaded;
        }

        private void GroupControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GroupControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("GroupControl_Unloaded");

            scrollViewerVisual?.Dispose();
            itemVisual?.Dispose();
            headerVisual?.Dispose();
            contentVisual?.Dispose();

            scrollViewerVisual = itemVisual = headerVisual = contentVisual = null;
            scrollViewer = null;
        }

        #region Group Property

        public static readonly DependencyProperty GroupProperty =
        DependencyProperty.Register("Group", typeof(Group), typeof(GroupControl), new PropertyMetadata(default(Group)));

        public Group Group
        {
            get { return (Group)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value); }
        }

        #endregion


        ScrollViewer scrollViewer;
        public void CreatedFromItemsRepeater(ScrollViewer scrollViewer)
        {
            this.scrollViewer = scrollViewer;
        }

        Visual scrollViewerVisual, itemVisual, headerVisual, contentVisual;
        private void HeaderGrid_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewerVisual = ElementCompositionPreview.GetElementVisual(scrollViewer);
            itemVisual = ElementCompositionPreview.GetElementVisual(this);
            headerVisual = ElementCompositionPreview.GetElementVisual(HeaderGrid);
            contentVisual = ElementCompositionPreview.GetElementVisual(ContentGrid);

            //Header effect
            var scrollViewerPropertySet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(this.scrollViewer);
            var headerAnimation = scrollViewerPropertySet.Compositor.CreateExpressionAnimation();

            headerAnimation.SetReferenceParameter("scrollViewerVisual", scrollViewerVisual);
            headerAnimation.SetReferenceParameter("scrollViewerPropertySet", scrollViewerPropertySet);
            headerAnimation.SetReferenceParameter("itemVisual", itemVisual);
            headerAnimation.SetReferenceParameter("headerVisual", headerVisual);

            headerAnimation.Expression
                = " -(itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) > 0 && -(itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) < itemVisual.Size.Y - headerVisual.Size.Y"
                + "? - (itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) "
                + ": - (itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) > 0 && -(itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) < itemVisual.Size.Y" +
                "? itemVisual.Size.Y - headerVisual.Size.Y : 0";

            headerVisual.StartAnimation("Offset.Y", headerAnimation);

            //Clip effect
            var clip = contentVisual.Compositor.CreateInsetClip();
            var clipAnimation = scrollViewerPropertySet.Compositor.CreateExpressionAnimation();

            clipAnimation.SetReferenceParameter("scrollViewerVisual", scrollViewerVisual);
            clipAnimation.SetReferenceParameter("scrollViewerPropertySet", scrollViewerPropertySet);
            clipAnimation.SetReferenceParameter("itemVisual", itemVisual);
            clipAnimation.SetReferenceParameter("headerVisual", headerVisual);
            clipAnimation.Expression
                      = " -(itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) > 0 && -(itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) < itemVisual.Size.Y - headerVisual.Size.Y"
                      + "? - (itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) "
                      + ": - (itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) > 0 && -(itemVisual.Offset.Y + scrollViewerPropertySet.Translation.Y) < itemVisual.Size.Y" +
                      "? itemVisual.Size.Y - headerVisual.Size.Y : 0";

            clip.StartAnimation(nameof(InsetClip.TopInset), clipAnimation);

            contentVisual.Clip = clip;
        }

        private void HeaderGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("HeaderGrid_Unloaded");
        }

        private void HeaderGrid_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.UpdateLayout();
            this.StartBringIntoView(new BringIntoViewOptions()
            {
                VerticalAlignmentRatio = 0
            });
        }



        //Code using the old method with the GroupControl_EffectiveViewportChanged event
        //private void GroupControl_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
        //{
        //    if (args.EffectiveViewport.Y >= 0 && args.EffectiveViewport.Y < sender.ActualHeight - HeaderGrid.ActualHeight)
        //    {
        //        Debug.WriteLine($"{Group.Header} : {args.EffectiveViewport.Y}");

        //        CompositeTransform.TranslateY = args.EffectiveViewport.Y;

        //        ContentGrid.Clip = new RectangleGeometry()
        //        {
        //            Rect = new Windows.Foundation.Rect
        //            {
        //                X = 0,
        //                Y = args.EffectiveViewport.Y,
        //                Width = ContentGrid.ActualWidth,
        //                Height = ContentGrid.ActualHeight
        //            }
        //        };
        //    }
        //    else if (args.EffectiveViewport.Y >= 0 && args.EffectiveViewport.Y < sender.ActualHeight)
        //    {
        //        CompositeTransform.TranslateY = sender.ActualHeight - HeaderGrid.ActualHeight;

        //        ContentGrid.Clip = new RectangleGeometry()
        //        {
        //            Rect = new Windows.Foundation.Rect
        //            {
        //                X = 0,
        //                Y = sender.ActualHeight - HeaderGrid.ActualHeight,
        //                Width = ContentGrid.ActualWidth,
        //                Height = ContentGrid.ActualHeight
        //            }
        //        };
        //    }
        //    else
        //    {
        //        CompositeTransform.TranslateY = 0;
        //        ContentGrid.Clip = null;
        //    }
        //}

    }
}
