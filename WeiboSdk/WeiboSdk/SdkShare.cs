using System;
using System.Windows;
using Microsoft.Phone.Controls;


namespace WeiboSdk
{
    public class SdkShare : SdkSendBase
    {
        private const string FILE_NOT_EXIST = "The picture file is not exist";

        public override void Show()
        {
            if (IsPicStatus)
            {
                if(string.IsNullOrEmpty(PicturePath))
                {
                    errBack();
                    return;
                }

                if (PicturePath.StartsWith("project://", StringComparison.OrdinalIgnoreCase))
                {
                    PicturePath = PicturePath.Substring(10);
                    ISHelper.CopyFromContentToStorage(PicturePath);
                }

                if (!ISHelper.FileExist(PicturePath))
                {
                    errBack();
                    return;
                }
            }
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/WeiboSdk;component/PageViews/SharePageView.xaml", UriKind.Relative));
            SharePageView.sdkSendBase = this;
        }

        private void errBack()
        {
            if (this.Completed != null)
            {
                SendCompletedEventArgs e = new SendCompletedEventArgs()
                {
                    IsSendSuccess = false,
                    ErrorCode = SdkErrCode.XPARAM_ERR,
                    Response = FILE_NOT_EXIST
                };
                this.Completed.Invoke(this, e);
                return;
            }
        }
    }
}
