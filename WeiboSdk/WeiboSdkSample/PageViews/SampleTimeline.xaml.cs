using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml;
using Microsoft.Phone.Controls;
using WeiboSdk;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;

namespace WeiboSdkSample
{
    public partial class SampleTimeline : PhoneApplicationPage
    {
        #region DependencyProperties

        #region FriendTimelineListProperty
        public static readonly DependencyProperty FriendTimelineListProperty = 
            DependencyProperty.Register(
            "FriendTimelineList",
            typeof(ObservableCollection<StatusItem>),
            typeof(SampleTimeline),
            new PropertyMetadata((ObservableCollection<StatusItem>)null));

        public ObservableCollection<StatusItem> FriendTimelineList
        {
            get { return (ObservableCollection<StatusItem>)GetValue(FriendTimelineListProperty); }
            set { SetValue(FriendTimelineListProperty, value); }
        }
        #endregion

        #region ProgressIndicatorIsVisibleProperty
        public static readonly DependencyProperty ProgressIndicatorIsVisibleProperty =
            DependencyProperty.Register("ProgressIndicatorIsVisible",
            typeof(bool),
            typeof(SampleTimeline),
            new PropertyMetadata(false));

        public bool ProgressIndicatorIsVisible
        {
            get { return (bool)GetValue(ProgressIndicatorIsVisibleProperty); }
            set { SetValue(ProgressIndicatorIsVisibleProperty, value); }
        }
        #endregion

        #endregion

        private ApplicationBarIconButton _shareIconButton;
        private SdkCmdBase cmdBase;
        private SdkNetEngine netEngine;

        public SampleTimeline()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            _shareIconButton = this.ApplicationBar.Buttons[2] as ApplicationBarIconButton;

            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "数据传输中";

            Binding bindingData;
            bindingData = new Binding("ProgressIndicatorIsVisible");
            bindingData.Source = this;
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsVisibleProperty, bindingData);
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsIndeterminateProperty, bindingData);

            RefreshTimeline();
        }

        private void RefreshTimeline()
        {
            ProgressIndicatorIsVisible = true;

            // Define a new net engine
            netEngine = new SdkNetEngine();

            // Define a new command base
            cmdBase = new cmdFrendTimeLine
            {
                acessToken = App.AccessToken,
                count = "20"
            };

            // Request server, the last parameter is set as default (".xml")
            netEngine.RequestCmd(SdkRequestType.FRIENDS_TIMELINE, cmdBase,
                // Requeset callback
                delegate(SdkRequestType requestType, SdkResponse response)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => ProgressIndicatorIsVisible = false);
                    if (response.errCode == SdkErrCode.SUCCESS)
                    {
                        // Parse the response content
                        XElement statusXml = XElement.Parse(response.content);
                        var statusCollection = from XElement statusElement in statusXml.Descendants("status")
                                               select new StatusItem(statusElement , statusElement.Element("user"));

                        // Use dispatcher invoke when you want to use UI thread to do things in callback delegate
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            //FriendTimelineList = new ObservableCollection<Status>(statusCollection);
                            FriendTimelineList = new ObservableCollection<StatusItem>(statusCollection);
                            _shareIconButton.IsEnabled = true;
                        });
                    }
                    else
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(response.content, response.errCode.ToString(), MessageBoxButton.OK));             
                    }
                });
        }

        private void shareIconButton_Click(object sender, EventArgs e)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(friendsTimeline, null);
            string tempJPEG = "TempJPEG.jpg";
            using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myStore.FileExists(tempJPEG))
                {
                    myStore.DeleteFile(tempJPEG);
                }
                using (IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG))
                {
                    System.Windows.Media.Imaging.Extensions.SaveJpeg(writeableBitmap, myFileStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 85);
                }
                MessageBox.Show("ListBox 截图成功保存到本地 \"TempJPEG.jpg\"");
            }
            if (FriendTimelineList != null && FriendTimelineList.Count > 0)
                this.NavigationService.Navigate(new Uri(string.Format("/PageViews/GrabInfoToShare.xaml?Text={0}", FriendTimelineList[0].Message), UriKind.Relative));
            else
                this.NavigationService.Navigate(new Uri("/PageViews/GrabInfoToShare.xaml", UriKind.Relative));
        }

        private void refreshIconButton_Click(object sender, EventArgs e)
        {
            RefreshTimeline();
        }

        private void sendIconButton_Click(object sender, EventArgs e)
        {
            //新建一个 SdkSend 实例
            SdkSend sdkSend = new SdkSend();
            //设置OAuth2.0的access_token
            sdkSend.AccessToken = App.AccessToken;
            //定义发送微博完毕的回调函数
            sdkSend.Completed = SendCompleted;

            //调用Show方法展现页面的跳转
            sdkSend.Show();
        }

        void SendCompleted(object sender, SendCompletedEventArgs e)
        {
            if (e.IsSendSuccess)
                MessageBox.Show("发送成功");
            else
                MessageBox.Show(e.Response, e.ErrorCode.ToString(), MessageBoxButton.OK);
        }
    }
}
