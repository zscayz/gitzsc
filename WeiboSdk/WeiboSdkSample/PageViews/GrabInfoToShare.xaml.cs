using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using WeiboSdk;

namespace WeiboSdkSample
{
    public partial class GrabInfoToShare : PhoneApplicationPage
    {
        private bool isLoaded;

        public GrabInfoToShare()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            if (NavigationContext.QueryString.Count > 0)
            {
                string textContent = NavigationContext.QueryString["Text"];
                if (!string.IsNullOrWhiteSpace(textContent))
                    this.messageTextBlock.Text = textContent;
            }
        }

        private void photoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                this.photoPanel.Visibility = Visibility.Visible;
        }

        private void photoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.photoPanel.Visibility = Visibility.Collapsed;
        }

        private void actionButton_Click(object sender, RoutedEventArgs e)
        {

            SdkShare sdkShare = new SdkShare()
            {
                // If this value is set to false,the sdk will ignore PicturePath value
                IsPicStatus = (bool)this.photoCheckBox.IsChecked,

                // Defalt hide applicationbar choose photo button, you can opt them visible
                IsShowChoosePhotoButton = (bool)this.chooseCheckBox.IsChecked,
            };

            //设置OAuth2.0的access_token
            sdkShare.AccessToken = App.AccessToken;
            if (this.localRadioButton.IsChecked == true)
                // Picture path will be set to relative or absolute, can use one that 
                // included in the project or from isolated storage.
                // When you want use the picture in project should set prefix
                // "component://" and set build action to "content",like below.
                sdkShare.PicturePath = "TempJPEG.jpg";
            else
                // Use the picture in isolated storage,just set the normal path.
                sdkShare.PicturePath = "project://Background.png";

            // The text can be edit in the next-show Share Page.
            sdkShare.Message = this.messageTextBlock.Text;

            // The event will be invoked when sharecompleted,with a parameter IsShareSuccess
            sdkShare.Completed += ShareCompleted;

            // Active the Share page to be shown.
            sdkShare.Show();
        }

        void ShareCompleted(object sender, SendCompletedEventArgs e)
        {
            if (e.IsSendSuccess)
                MessageBox.Show("发送成功");
            else
                MessageBox.Show(e.Response, e.ErrorCode.ToString(), MessageBoxButton.OK);
        }
    }
}