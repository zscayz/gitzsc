using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net;
using System.Windows.Data;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;


namespace WeiboSdk
{
    public partial class SharePageView : PhoneApplicationPage
    {
        public static SdkSendBase sdkSendBase;

        private const string UNEXPECTED_ERROR = "SDK send function is down due to some undefined reasons, if this message is showing ,please tell us.";
        private const string SHARE_WEIBO_TEXT = "分享到新浪微博";
        private const string SEND_WEIBO_TEXT = "发送微博";

        private bool _isPicStatus;
        private string _imageFileName;
        private SdkCmdBase cmdBase;
        private SdkNetEngine netEngine;
        
        #region Definie Dependency Property
        /// <summary>
        /// Define all this page's public proterties for data binding.
        /// </summary>

        #region IsProgressIndicatorVisibleProperty
        public static readonly DependencyProperty IsProgressIndicatorVisibleProperty =
            DependencyProperty.Register("IsProgressIndicatorVisible",
            typeof(bool),
            typeof(SharePageView),
            new PropertyMetadata(false));

        public bool IsProgressIndicatorVisible
        {
            get { return (bool)GetValue(IsProgressIndicatorVisibleProperty); }
            set { SetValue(IsProgressIndicatorVisibleProperty, value); }
        }
        #endregion

        #region WordCountProperty
        public static readonly DependencyProperty WordCountProperty =
            DependencyProperty.Register("WordCount", typeof(string), typeof(SharePageView), new PropertyMetadata((string)"140"));

        public string WordCount
        {
            get { return (string)GetValue(WordCountProperty); }
            set { SetValue(WordCountProperty, value); }
        }
        #endregion

        #endregion

        public SharePageView()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "网络通讯中";

            Binding bindingData;
            bindingData = new Binding("IsProgressIndicatorVisible");
            bindingData.Source = this;
            bindingData.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsVisibleProperty, bindingData);
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsIndeterminateProperty, bindingData);

            if (sdkSendBase.GetType() == typeof(SdkSend))
            {
                TitleBlock.Text = SEND_WEIBO_TEXT;
            }
            else if (sdkSendBase.GetType() == typeof(SdkShare))
            {
                TitleBlock.Text = SHARE_WEIBO_TEXT;

                if (!sdkSendBase.IsShowChoosePhotoButton)
                {
                    this.addPhotoButton.Visibility = Visibility.Collapsed;
                    this.CancelButton.Visibility = Visibility.Collapsed;
                }

                if (!string.IsNullOrEmpty(sdkSendBase.Message))
                {
                    this.StatusMessageBox.Text = sdkSendBase.Message;
                    this.StatusMessageBox.SelectionStart = this.StatusMessageBox.Text.Length;
                    this.sendButton.Focus();
                }

                if (sdkSendBase.IsPicStatus && !string.IsNullOrEmpty(sdkSendBase.PicturePath))
                {
                    _isPicStatus = true;
                    _imageFileName = sdkSendBase.PicturePath;

                    BitmapImage bmp = new BitmapImage();
                    bmp.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(_imageFileName, FileMode.Open, FileAccess.Read))
                    {
                        bmp.SetSource(fileStream);
                    }

                    this.ChosenPic.Source = bmp;
                    this.ChosenPicPanel.Visibility = Visibility.Visible;
                }
            }
            else 
            {
                if (sdkSendBase.Completed != null)
                {
                    SendCompletedEventArgs err = new SendCompletedEventArgs()
                    {
                        IsSendSuccess = false,
                        ErrorCode = SdkErrCode.XPARAM_ERR,
                        Response = UNEXPECTED_ERROR,
                    };
                    sdkSendBase.Completed.Invoke(sdkSendBase, err);
                }
            }
        }

        private void StatusMessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            WordCount = (140 - StatusMessageBox.Text.Length).ToString();
        }

        private void SendTextStatus()
        {
            IsProgressIndicatorVisible = true;
            this.StatusMessageBox.IsReadOnly = true;
            this.sendButton.IsEnabled = false;

            netEngine = new SdkNetEngine();
            cmdBase = new cmdUploadMessage
            {
                status =  StatusMessageBox.Text,
                acessToken = sdkSendBase.AccessToken,
                acessTokenSecret = sdkSendBase.AccessTokenSecret
            };

            netEngine.RequestCmd(SdkRequestType.UPLOAD_MESSAGE, cmdBase, requestCompleted);
        }

        private void SendPicStatus()
        {
            IsProgressIndicatorVisible = true;
            this.StatusMessageBox.IsReadOnly = true;
            this.CancelButton.IsEnabled = false;
            this.sendButton.IsEnabled = false;

            netEngine = new SdkNetEngine();
            cmdBase = new cmdUploadPic
            {
                messageText = StatusMessageBox.Text,
                picPath = _imageFileName,
                acessToken = sdkSendBase.AccessToken,
                acessTokenSecret = sdkSendBase.AccessTokenSecret
            };
            netEngine.RequestCmd(SdkRequestType.UPLOAD_MESSAGE_PIC, cmdBase, requestCompleted);
        }

        void requestCompleted(SdkRequestType requestType, SdkResponse response)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                IsProgressIndicatorVisible = false;
                this.sendButton.IsEnabled = true;
                this.CancelButton.IsEnabled = true;
                this.StatusMessageBox.IsReadOnly = false;

                if (sdkSendBase.Completed != null)
                {
                    SendCompletedEventArgs e = new SendCompletedEventArgs()
                    {
                        IsSendSuccess = (response.errCode == SdkErrCode.SUCCESS),
                        ErrorCode = response.errCode,
                        Response = response.content
                    };
                    sdkSendBase.Completed.Invoke(sdkSendBase, e);
                }

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            });
        }

        #region Func Part PicturePickers

        private void LibPickerButton_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask photoPicker = new PhotoChooserTask();
            photoPicker.ShowCamera = false;
            photoPicker.Completed+= new EventHandler<PhotoResult>(photoPicker_Completed);
            photoPicker.Show();
        }

        private void CameraPickerButton_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureTask cameraPicker = new CameraCaptureTask();
            cameraPicker.Completed += new EventHandler<PhotoResult>(cameraPicker_Completed);
            cameraPicker.Show();
        }

        void cameraPicker_Completed(object sender, PhotoResult e)
        {
            (sender as CameraCaptureTask).Completed -= new EventHandler<PhotoResult>(cameraPicker_Completed);
            PhotoFix(e);
        }

        void photoPicker_Completed(object sender, PhotoResult e)
        {
            (sender as PhotoChooserTask).Completed -= new EventHandler<PhotoResult>(photoPicker_Completed);
            PhotoFix(e);
        }

        private void PhotoFix(PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var dirs = e.OriginalFileName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                _imageFileName = (dirs[5] != null) ? dirs[5] : "TempJPEG";

                BitmapImage bmp = new BitmapImage();
                bmp.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                bmp.SetSource(e.ChosenPhoto);
                bmp.ImageOpened +=new EventHandler<RoutedEventArgs>(bmp_ImageOpened);
            }
            this.PhotoPicker.Visibility = Visibility.Collapsed;
        }

        void bmp_ImageOpened(object sender, RoutedEventArgs e)
        {
            BitmapImage bmp = sender as BitmapImage;
            bmp.ImageOpened -= new EventHandler<RoutedEventArgs>(bmp_ImageOpened);
            this.ChosenPic.Source = bmp;
            this.ChosenPicPanel.Visibility = Visibility.Visible;
            using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string dirPath = "SdkImage";
                _imageFileName = Path.Combine(dirPath, _imageFileName);

                if (!myStore.DirectoryExists(dirPath))
                    myStore.CreateDirectory(dirPath);
                if (myStore.FileExists(_imageFileName))
                    myStore.DeleteFile(_imageFileName);

                using (var myFileStream = myStore.CreateFile(_imageFileName))
                {
                    WriteableBitmap wb = new WriteableBitmap(bmp);
                    Extensions.SaveJpeg(wb, myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 90);
                }
            }
            _isPicStatus = true;
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this._isPicStatus = false;
            this.ChosenPic.Source = null;
            this.ChosenPicPanel.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Func Part Fake AppBar

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            int remainLenth = Convert.ToInt32(WordCount);
            if (remainLenth < 0)
                MessageBox.Show("微博信息量超出字符长度限制。", "错误", MessageBoxButton.OK);
            else if (remainLenth >= 140)
                MessageBox.Show("请保证您的微博包含有效信息。", "错误", MessageBoxButton.OK);
            else
            {
                if (!_isPicStatus)
                    SendTextStatus();
                else
                    SendPicStatus();
            }
        }

        private void addPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.PhotoPicker.Visibility == Visibility.Visible)
                this.PhotoPicker.Visibility = Visibility.Collapsed;
            else
            {
                this.PhotoPicker.Visibility = Visibility.Visible;
                this.sendButton.Focus();
            }
        }

        private void addTrendButton_Click(object sender, RoutedEventArgs e)
        {
            int selStart = StatusMessageBox.Text.Length + 1;
            int selLength = 7;
            this.StatusMessageBox.Text = StatusMessageBox.Text + "#在此处输入话题#";
            this.StatusMessageBox.Select(selStart, selLength);
            this.StatusMessageBox.Focus();
        }

        #endregion
    }
}