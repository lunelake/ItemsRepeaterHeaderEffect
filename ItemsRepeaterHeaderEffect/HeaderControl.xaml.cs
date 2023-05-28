// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ItemsRepeaterHeaderEffect
{
    public sealed partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            this.InitializeComponent();

            this.EffectiveViewportChanged += HeaderControl_EffectiveViewportChanged;
        }

        #region Header Property

        public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(string), typeof(HeaderControl), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion

        private void HeaderControl_EffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
        {
            if (args.EffectiveViewport.Y >= 0 && args.EffectiveViewport.Y < sender.ActualHeight)
            {
                Debug.WriteLine($"{Header} : {args.EffectiveViewport.Y}");

                CompositeTransform.TranslateY = args.EffectiveViewport.Y;
            }
        }



    }
}
