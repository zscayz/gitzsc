using System;

namespace WeiboSdk
{
    public abstract class SdkSendBase
    {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public virtual bool IsPicStatus { get; set; }
        public virtual bool IsShowChoosePhotoButton { get; set; }

        public virtual string Message { get; set; }
        public virtual string PicturePath { get; set; }

        public EventHandler<SendCompletedEventArgs> Completed;

        public abstract void Show();
    }


}
