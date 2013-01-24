using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.ComponentModel;

namespace WeiboSdkSample
{
    public class StatusItem
    {
        public StatusItem()
        { }

        public StatusItem(XElement status, XElement user)
        {
            CreateTime = ExtHelpers.GetTime(status.Element("created_at").Value);
            Message = status.Element("text").Value;
            Source = status.Element("source").Value;
            StatusID = status.Element("id").Value;
            Favorited = Convert.ToBoolean(status.Element("favorited").Value);
            User = new StatusUser(user);
            IsRetweetedStatus = "Collapsed";
            IsThumbnail = "Collapsed";
            if (status.Element("retweeted_status") != null)
            {
                XElement retweetedStatus = status.Element("retweeted_status");
                XElement retweetedUser = retweetedStatus.Element("user");
                RetweetedStatus = new StatusItem(retweetedStatus, retweetedUser);
                IsRetweetedStatus = "Visible";
            }
            if (status.Element("thumbnail_pic") != null)
            {
                BmiddlePic = status.Element("bmiddle_pic").Value;
                OriginalPic = status.Element("original_pic").Value;
                ThumbnailPic = status.Element("thumbnail_pic").Value;
                IsThumbnail = "Visible";
            }
        }

        public StatusItem(XElement status)
        {
            CreateTime = ExtHelpers.GetTime(status.Element("created_at").Value);
            Message = status.Element("text").Value;
            Source = status.Element("source").Value;
            StatusID = status.Element("id").Value;
            Favorited = Convert.ToBoolean(status.Element("favorited").Value);
            IsRetweetedStatus = "Collapsed";
            IsThumbnail = "Collapsed";
            if (status.Element("retweeted_status") != null)
            {
                XElement retweetedStatus = status.Element("retweeted_status");
                XElement retweetedUser = retweetedStatus.Element("user");
                RetweetedStatus = new StatusItem(retweetedStatus, retweetedUser);
                IsRetweetedStatus = "Visible";
            }
            if (status.Element("thumbnail_pic") != null)
            {
                BmiddlePic = status.Element("bmiddle_pic").Value;
                OriginalPic = status.Element("original_pic").Value;
                ThumbnailPic = status.Element("thumbnail_pic").Value;
                IsThumbnail = "Visible";
            }
        }

        public override bool Equals(object obj)
        {
            StatusItem other = obj as StatusItem;
            if (other == null) return false;
            return StatusID.Equals(other.StatusID);
        }

        public override int GetHashCode()
        {
            return StatusID.GetHashCode();
        }

        public string OriginalPic { get; set; }

        public string StatusID { get; set; }

        public string BmiddlePic { get; set; }

        public string ThumbnailPic { get; set; }

        public string Message { get; set; }

        public bool Favorited { get; set; }

        public string CreateTime { get; set; }

        public StatusItem RetweetedStatus { get; set; }

        public string IsRetweetedStatus { get; set; }

        public string IsThumbnail { get; set; }

        public string Source { get; set; }

        public StatusUser User { get; set; }
    }

    public class StatusUser
    {
        public StatusUser()
        { }

        public StatusUser(XElement user)
        {
            ID = user.Element("id").Value;
            Name = user.Element("name").Value;
            FollowersCount = user.Element("followers_count").Value;
            FriendsCount = user.Element("friends_count").Value;
            StatusesCount = user.Element("statuses_count").Value;
            IsVerified = (Convert.ToBoolean(user.Element("verified").Value)) ? "Visible" : "Collapsed";
            URL = user.Element("url").Value;

            CreatedAt = ExtHelpers.GetTimeFull(user.Element("created_at").Value);

            Description = user.Element("description").Value;
            if (string.IsNullOrEmpty(Description))
                Description = "介个人很懒，神马都木有留下";

            Location = user.Element("location").Value;
            if (string.IsNullOrEmpty(Location))
                Location = "未知";

            Gender = user.Element("gender").Value;
            switch (Gender)
            {
                case "m":
                    Gender = "男";
                    break;
                case "f":
                    Gender = "女";
                    break;
                default:
                    Gender = "保密";
                    break;
            }

            ProfileImage = user.Element("profile_image_url").Value;
            if (ProfileImage.Length < 40)
                ProfileImage = "/Resources/Images/SinaResource.Profile.png";

            if (user.Element("status") != null)
                Status = new StatusItem(user.Element("status"));
           
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string FollowersCount { get; set; }

        public string FriendsCount { get; set; }

        public string StatusesCount { get; set; }

        public string CreatedAt { get; set; }

        public string IsVerified { get; set; }
  
        public string IsFollowing { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string ProfileImage { get; set; }

        public string URL { get; set; }

        public StatusItem Status { get; set; }
    }
}
