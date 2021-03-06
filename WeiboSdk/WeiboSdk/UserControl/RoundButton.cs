﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WeiboSdk
{
    public class RoundButton : Button
    {
        protected ImageBrush OpacityImageBrush;
        private const string OpacityImageBrushName = "OpacityImageBrush";

        protected ContentControl ContentBody;
        private const string ContentBodyName = "ContentBody";

        public RoundButton()
        {
            DefaultStyleKey = typeof(RoundButton);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            OpacityImageBrush = GetTemplateChild(OpacityImageBrushName) as ImageBrush;
            ContentBody = GetTemplateChild(ContentBodyName) as ContentControl;

            if (ImageSource == null)
                ImageSource = new BitmapImage(new Uri("/WeiboSdk;component/Icons/appbar.cancel.png", UriKind.RelativeOrAbsolute));
            else
                SetImageBrush(ImageSource);

            if (ContentBody != null)
            {
                var bottom = -(ContentBody.FontSize / 8.0);
                var top = -(ContentBody.FontSize / 2.0) - bottom;

                ContentBody.Margin = new Thickness(0, top, 0, bottom);
            }
        }

        #region dependency properties
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RoundButton), new PropertyMetadata(Orientation.Vertical));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(RoundButton),
            new PropertyMetadata(
            //new BitmapImage(new Uri("/Coding4Fun.Phone.Controls;component/Media/icons/appbar.check.rest.png", UriKind.RelativeOrAbsolute)),
                OnImageSource));
        #endregion

        #region dp onchange callbacks
        private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as RoundButton;

            if (sender == null || e.NewValue == e.OldValue)
                return;

            sender.SetImageBrush(e.NewValue as ImageSource);
        }

        private void SetImageBrush(ImageSource brush)
        {
            if (OpacityImageBrush == null)
                return;

            OpacityImageBrush.ImageSource = brush;
        }
        #endregion
    }
}