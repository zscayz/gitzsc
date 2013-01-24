using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Controls;


namespace WeiboSdk
{
    public class SdkSend : SdkSendBase
    {
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool IsPicStatus
        {
            get { return base.IsPicStatus; }
            set { base.IsPicStatus = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool IsShowChoosePhotoButton
        {
            get { return base.IsShowChoosePhotoButton; }
            set { base.IsShowChoosePhotoButton = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Message
        {
            get { return base.Message; }
            set { base.Message = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string PicturePath
        {
            get { return base.PicturePath; }
            set { base.PicturePath = value; }
        }

        public override void Show()
        {
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/WeiboSdk;component/PageViews/SharePageView.xaml", UriKind.Relative));
            SharePageView.sdkSendBase = this;
        }
    }
}
