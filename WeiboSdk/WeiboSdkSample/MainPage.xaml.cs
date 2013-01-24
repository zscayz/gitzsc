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
using Newtonsoft.Json.Linq;
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

            // oauth2.0情况下sdk不负责保存key，这里则不需要
            // 此处自定义 AesKey 32位， AesIV 16位
            //SdkData.AesKey = "12345678901234567890123456789012";
            //SdkData.AesIV = "1234567890123456";

            // 您app设置的重定向页,必须一致
            SdkData.RedirectUri = "http://weibo.com";

            //设置授权选项，默认为OAuth1.0
            SdkNetEngine.AuthOption = EumAuth.OAUTH2_0;
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

            oauthControl.OAuthBack = loginBack;
            //test
            /*SdkNetEngine net = new SdkNetEngine();
            cmdFriendShip data = new cmdFriendShip
            {
                sourceId = "1827642611",
                userId = "2361803217",
                acessToken = "2.00d1bgzBAJ9QuC1590c98488QRvHUE"

            };
            net.RequestCmd(SdkRequestType.FRIENDSHIP_SHOW,data,
                delegate(SdkRequestType requestType, SdkResponse response)
                {
                    Debug.WriteLine(response.content);
                });*/

            //test

            // Fired when oauthControl is navigating
            oauthControl.OAuthBrowserNavigating = new EventHandler(oauthBrowserNavigating);

            // Fired when oauthControl is navigated
            oauthControl.OAuthBrowserNavigated = new EventHandler(oauthBrowserNavigated);

            // Fired when oauthControl's cancel button is pressed
            oauthControl.OAuthBrowserCancelled = new EventHandler(oauthBrowserCancelled);

            //客户端方式获取，需要服务器授权
            //ClientOAuth2_0.GetAccessToken("UserName", "PassWord", (e1, e2) => loginBack(e1,e2));
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageViews/SampleTimeline.xaml", UriKind.Relative));
        }

        void loginBack(SdkErrCode errCode, string JsonResponse)
        {

            if (errCode == SdkErrCode.SUCCESS)
            {
                //解析返回的Json数据
                JObject JosnData = JObject.Parse(JsonResponse);
                JToken node1 = JosnData["access_token"];
                if (null != node1)
                    App.AccessToken = node1.ToString();

                JToken node2 = JosnData["refresh_token"];
                if (null != node2)
                    App.RefleshToken = node2.ToString();

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ProgressIndicatorIsVisible = false;
                    NavigationService.Navigate(new Uri("/PageViews/SampleTimeline.xaml", UriKind.Relative));
                    homeButton.Visibility = Visibility.Visible;
                    this.ContentPanel.Children.Remove(oauthControl);
                });
            }
            else if (errCode == SdkErrCode.NET_UNUSUAL)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("检查网络");
                });
            }
            else
            {
                Debug.WriteLine("Other Err.");
            }
        }

        void oauthBrowserCancelled(object sender, EventArgs e)
        {
            ProgressIndicatorIsVisible = false;
            //new Game().Exit();
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