using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WeiboSdk;
using Microsoft.Xna.Framework;
using System.Diagnostics;


namespace WeiboSdkSample
{
    public partial class MainPage : PhoneApplicationPage
    {

        #region ProgressIndicatorIsVisibleProperty
        public static readonly DependencyProperty ProgressIndicatorIsVisibleProperty =
            DependencyProperty.Register("ProgressIndicatorIsVisible",
            typeof(bool),
            typeof(MainPage),
            new PropertyMetadata(false));

        public bool ProgressIndicatorIsVisible
        {
            get { return (bool)GetValue(ProgressIndicatorIsVisibleProperty); }
            set { SetValue(ProgressIndicatorIsVisibleProperty, value); }
        }
        #endregion

        // Constructor
        public MainPage()
        {
            // 此处使用自己 AppKey 和 AppSecret
            SdkData.AppKey = "3959964079";
            SdkData.AppSecret = "d74f9b277601370d2fa9f2ce6bab89d2";

            // 此处自定义 AesKey 32位， AesIV 16位
            SdkData.AesKey = "12345678901234567890123456789012";
            SdkData.AesIV = "1234567890123456";

            // 此处定义 OAuth 授权完成后的返回页面
            SdkData.callBackUrl = "http://weibo.com";

            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "数据传输中";

            Binding bindingData;
            bindingData = new Binding("ProgressIndicatorIsVisible");
            bindingData.Source = this;
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsVisibleProperty, bindingData);
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsIndeterminateProperty, bindingData);

            //oauthControl.ifSave = true;
            oauthControl.OAuthBack = loginBack;

            // Fired when oauthControl is navigating
            oauthControl.OAuthBrowserNavigating = new EventHandler(oauthBrowserNavigating);

            // Fired when oauthControl is navigated
            oauthControl.OAuthBrowserNavigated = new EventHandler(oauthBrowserNavigated);

            // Fired when oauthControl's cancel button is pressed
            oauthControl.OAuthBrowserCancelled = new EventHandler(oauthBrowserCancelled);

            //ClientOAuth2_0.GetAccessToken("kingln@163.com", "12343210");
            //ClientOAuth2_0.OAuthCallBack = (x1, x2) =>
            //{
            //    Debug.WriteLine("成功!");
            //};
            // Xauth Test
            //WeiboXAuth.XAuthLogin("username@email.com", "password", true/*是否由我们帮你保存登录信息*/, (e1, e2, e3) =>
            //{
            //    if (true == e1)
            //    {
            //        //登录成功
            //        Debug.WriteLine(string.Format("userid:{0},accessToken:{1},accessTokenSecret:{2}", e3.userId, e3.acessToken, e3.acessTokenSecret));
            //        Deployment.Current.Dispatcher.BeginInvoke(() =>
            //            {
            //                NavigationService.Navigate(new Uri("/PageViews/SampleTimeline.xaml", UriKind.Relative));
            //            });
            //    }
            //});
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageViews/SampleTimeline.xaml", UriKind.Relative));
        }

        void loginBack(SdkErrCode errCode, string JsonResponse)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ProgressIndicatorIsVisible = false;
                if (null != errCode)
                {
                    //System.Diagnostics.Debug.WriteLine(string.Format("userid:{0},accessToken:{1},accessTokenSecret:{2}", 
                    //    authResponse.userId, 
                    //    authResponse.acessToken,
                    //    authResponse.acessTokenSecret));
                    NavigationService.Navigate(new Uri("/PageViews/SampleTimeline.xaml", UriKind.Relative));
                    homeButton.Visibility = Visibility.Visible;
                    this.ContentPanel.Children.Remove(oauthControl);
                }
            });
        }

        void oauthBrowserCancelled(object sender, EventArgs e)
        {
            ProgressIndicatorIsVisible = false;
            new Game().Exit();
        }

        void oauthBrowserNavigating(object sender, EventArgs e)
        {
            ProgressIndicatorIsVisible = true;
        }

        void oauthBrowserNavigated(object sender, EventArgs e)
        {
            ProgressIndicatorIsVisible = false;
        }
    }
}