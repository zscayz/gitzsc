using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using NetDefine;

namespace WeiboSdk
{

    //自定义的ErrCode
    public enum SdkErrCode
    {

        XPARAM_ERR = -1,
        SUCCESS =0,
        //NET_VAILD,
        NET_UNUSUAL,
        AUTH_FAILED,
        SERVER_ERR,

    }
    public class SdkAuthError
    {
        public SdkErrCode errCode;
        public string specificCode = "";
        public string errMessage = "";
    }

    public class SdkAuthRes
    {
        public string userId = "";
        public string acessToken = "";
        public string acessTokenSecret = "";
    }

    public enum SdkRequestType
    {
        NULL_TYPE = -1,
        FRIENDS_TIMELINE = 0,        //获取下行数据集(timeline)接口(cmdFrendTimeLine)
        VERIFY_CREDENTIALS = 1,     //验证当前用户身份是否合法(cmdVerifyCredential)
        UPLOAD_MESSAGE,             //发送微博(cmdUploadMessage)
        UPLOAD_MESSAGE_PIC,         //发送带图片微博(cmdUploadPic)
        GET_MESSAGE_BYID,           //根据ID获取单条微博信息内容(cmdBlogID)
        COMMENT_MESSAGE,            //对一条微博信息进行评论(cmdReplyComment)
        REPLY_COMMENT,              //恢复评论(cmdReplyComment)
        SEND_DIRECT_MESSAGE,        //发送私信(cmdSendDirectMessage)
        COMMENT_FOWARD_COUNT,       //批量获取一组微博的评论数及转发数(cmdBatchCmd)
        GET_USER_INFO,              //获取用户信息(cmdFriendShip)
        ADD_FAVORITE,               //加入收藏(cmdBlogID)
        CANCLE_FAVORITE,            //取消收藏(cmdBlogID)
        GET_UNREAD_MESSAGE,         //获取当前用户未读消息数(cmdGetUnreadMessage)
        RESET_COUNT,                //未读消息数清零接口(cmdResetCount)
        REPOST_MESSAGE,             //转发一条微博信息(cmdRepostMessage)
        FRIENDSHIP_CREATE,          //关注某用户(cmdFriendShip)
        FRIENDSHIP_DESDROY,         //取消关注(cmdFriendShip)
        FRIENDSHIP_EXIST,           //是否关注某用户(推荐使用friendships/show)
        FRIENDSHIP_SHOW,            //获取两个用户关系的详细情况(cmdFriendExist)

        DELETE_BLOG,                //删除指定微博(cmdBlogID)
        DELETE_COMMENT,             //删除指定评论(DELETE_COMMENT)

        DIRECT_MESSAGES,            //获取私信列表(需升级)(cmdNormalMessages)
        MENTIONS,                   //获取@当前用户的微博列表(cmdNormalMessages)
        USER_TIMELINE,              //获取用户发布的微博消息列表(cdmUserTimeline)
        GET_FRIENDS,                //获取指定人的关注人的列表(cmdGetFriends_Fans)
        GET_FANS,                   //获取指定人的粉丝的列表(cmdGetFriends_Fans)
        GET_FAVORITES,              //获取当前用户收藏列表(cmdGetFavorite)
        COMMENTS_BYID,              //获取指定微博的评论列表(cmdCommentByID)
        COMMENTS_BYID_ORG,          //获取指定微博的原始评论列表(cmdCommentByID)

        FREE_LOOK_OPEN,             //获取随便看看列表(cdmCoutParam)


        HOURLYHOT_TRENDS_OPEN,      //按小时返回热门话题(SdkCmdBase)
        DAILYHOT_TRENDS_OPEN,       //返回当日热门话题(SdkCmdBase)
        WEEKLYHOT_TRENDS_OPEN,      //返回当周热门话题(SdkCmdBase)

        GET_MAP_IMAGE,              //获取地图图片(cmdGetMapImage)
        GET_TRENDS,                 //获取关注话题列表(cmdGetTrends)
        DESTORY_TRENDS,             //取消关注指定话题(cmdDestoryTrend)
        FOLLOW_TREND,                //关注指定话题(cmdFollowTrend)
        URL_L_TO_S                  //长url转换成短的

    }

    //鉴权方式
    public enum EumAuth
    {
        OAUTH1_0 = 0,
        OAUTH2_0
    }

    public class SdkCmdBase
    {
        public string acessToken = "";
        public string acessTokenSecret = "";

        public string requestId = "";

        public CmdBase Target_Data
        {
            get
            {
                CmdBase data = new CmdBase
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,
                };
                return data;
            }
        }
    }

    /// <summary>
    /// FRIEND_TIMELINE
    /// </summary>
    public class cmdFrendTimeLine : SdkCmdBase
        {

            public string sinceID = "";
            public string maxID = "";
            public string count = "";
            public string page = "";
            public string baseApp = "";
            public string feature = "";

            public CmdBase TargetData
            {
                get
                {
                    CmdBase data = new NormalMessages
                    {
                       
                        acessToken = this.acessToken,
                        accessTokenSecret = this.acessTokenSecret,

                        //
                        requestId = this.requestId,

                        //
                        _sinceID = this.sinceID,
                        _maxID = this.maxID,
                        _count = this.count,
                        _page = this.page,
                        _baseApp = this.baseApp,
                        _feature = this.feature

                    };
                    return data;
                }
            }
        }

    /// <summary>
    /// VERIFY_CREDENTIALS
    /// </summary>
    public class cmdVerifyCredential : SdkCmdBase
    {
        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new VerifyCredential
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,
                };
                return data;
            }
        }
    }

    /// <summary>
    /// UPLOAD_MESSAGE
    /// </summary>
    public class cmdUploadMessage : SdkCmdBase
    {
        public string status = "";
        public string ReplyId = "";
        public string lat = "";
        public string _long = "";
        public string annotations = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new UploadMessage
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _status = this.status,
                    _ReplyId = this.ReplyId,
                    _lat = this.lat,
                    _long = this._long,
                    _annotations = this.annotations

                };
                return data;
            }
        }
    }

    /// <summary>
    /// UPLOAD_MESSAGE_PIC
    /// </summary>
    public class cmdUploadPic : SdkCmdBase
    {
        public string messageText = "";
        public string lat = "";
        public string _long = "";
        public string picPath = "";

        public CmdBase TargetData
        {
            get
            {
                 CmdBase data = new UploadPic
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _messageText = this.messageText,
                    _lat = this.lat,
                    _long = this._long,
                    _picPath = picPath
                };
               return data;
            }
        }
    }

    /// <summary>
    /// GET_MESSAGE_BYID | ADD_FAVORITE | CANCLE_FAVORITE | DELETE_BLOG | DELETE_COMMENT
    /// </summary>
    public class cmdBlogID : SdkCmdBase
    {
        public string blogID = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new BlogID
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _blogID = this.blogID,
                };
                return data;
            }
        }
    }

    /// <summary>
    /// COMMENT_MESSAGE | REPLY_COMMENT
    /// </summary>
    public class cmdReplyComment : SdkCmdBase
    {

        public string blogID = "";
        public string commentText = "";
        public string cid = "";
        public string withoutMention = "";
        public bool ifToOrg = false;
        public bool isRepost = false;

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new Reply_Comment
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _blogID = this.blogID,
                    _commentText = this.commentText,
                    _cid = this.cid,
                    _withoutMention = this.withoutMention,
                    _ifToOrg = this.ifToOrg,
                    _ifRepost = isRepost
                };
                return data;
            }
        }
    }

    /// <summary>
    /// SEND_DIRECT_MESSAGE
    /// </summary>
    public class cmdSendDirectMessage : SdkCmdBase
    {
        public string messageText = "";
        public string userId = "";
        public string screenName = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new SendDirectMessage
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _messageText = this.messageText,
                    _userId = this.userId,
                    _screenName = this.screenName
                };
                return data;
            }
        }
    }
    
    /// <summary>
    /// COMMENT_FOWARD_COUNT
    /// </summary>
    public class cmdBatchCmd : SdkCmdBase
    {
        //登录状况下，非登录状况下
        public bool _isAuth = true;
        public string[] _idsArray = null;

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new BatchCmd
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    idsArray = this._idsArray,
                    isAuth = this._isAuth
                };
                return data;
            }
        }
    }

    
    /// <summary>
    /// GET_USER_INFO | FRIENDSHIP_CREATE | FRIENDSHIP_DESDROY | FRIENDSHIP_SHOW
    /// </summary>
    public class cmdFriendShip : SdkCmdBase
    {
        //源用户(如果不填，则默认取当前登录用户)
        public string sourceId = "";
        public string sourceScreenName = "";
        //目标用户
        public string userId = "";
        public string screenName = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new FriendShip
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _sourceId = this.sourceId,
                    _sourceScreenName = this.sourceScreenName,
                    _userId = this.userId,
                    _screenName = this.sourceScreenName
                };
                return data;
            }
        }
    }

    /// <summary>
    ///GET_UNREAD_MESSAGE 
    /// </summary>
    public class cmdGetUnreadMessage : SdkCmdBase
    {
        public string _withNewStatus = "";
        public string _sinceID = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new GetUnreadMessage
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    withNewStatus = this._withNewStatus,
                    sinceID = this._sinceID,
                };
                return data;
            }
        }
    }

    /// <summary>
    ///RESET_COUNT 
    /// </summary>
    public class cmdResetCount : SdkCmdBase
    {
        public string type = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new ResetCount
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _type = this.type
                };
                return data;
            }
        }
    }

    /// <summary>
    /// REPOST_MESSAGE
    /// </summary>
    public class cmdRepostMessage : SdkCmdBase
    {
        public string blogID = "";
        public string messageText = "";
        public string isComment = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new RepostMessage
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,
                    _blogID = this.blogID,
                    _messageText = this.messageText,
                    _isComment = this.isComment
                    
                };
                return data;
            }
        }
    }

    /// <summary>
    /// FRIENDSHIP_EXIST
    /// </summary>
    public class cmdFriendExist : SdkCmdBase
    {
        public string userA = "";
        public string userB = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new FriendExist
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _userA = this.userA,
                    _userB = this.userB

                };
                return data;
            }
        }
    }

    ///// <summary>
    ///// SEARCH_USER
    ///// </summary>
    //public class cmdSearchUser : SdkCmdBase
    //{
    //    public string keyWord = "";
    //    public string sNick = "";
    //    public string sDomain = "";
    //    public string sIntro = "";
    //    public string province = "";
    //    public string city = "";
    //    public string gender = "";
    //    public string comorsch = "";
    //    public string sort = "";
    //    public string page = "";
    //    public string count = "";
    //    public string callBack = "";
    //    public string base_app = "";

    //    public CmdBase TargetData
    //    {
    //        get
    //        {
    //            CmdBase data = new SearchUser
    //            {
    //                acessToken = this.acessToken,
    //                accessTokenSecret = this.acessTokenSecret,

    //                //
    //                requestId = this.requestId,
    //                _queueId = this.queueID,

    //                _keyWord = this.keyWord,
    //                _sNick = this.sNick,
    //                _sDomain = this.sDomain,
    //                _sIntro = this.sIntro,
    //                _province = this.province,
    //                _city = this.city,
    //                _gender = this.gender,
    //                _comorsch = this.comorsch,
    //                _sort = this.sort,
    //                _page = this.page,
    //                _count = this.count,
    //                _callBack = this.callBack,
    //                _base_app = this.base_app

    //            };
    //            return data;
    //        }
    //    }
    //}

    
    ///// <summary>
    ///// AT_USERS
    ///// </summary>
    //public class cmdAtUsers : SdkCmdBase
    //{
    //    /// <summary>
    //    /// 搜索的关键字。必须进行URL_encoding。UTF-8编码
    //    /// </summary>
    //    public string keyword = "";

    //    /// <summary>
    //    /// 每页返回结果数。默认10
    //    /// </summary>
    //    public string count = "";

    //    /// <summary>
    //    /// 1代表粉丝，0代表关注人
    //    /// </summary>
    //    public string type = "";

    //    /// <summary>
    //    /// 0代表只查关注人，1代表只搜索当前用户对关注人的备注，2表示都查. 默认为2.
    //    /// </summary>
    //    public string range = "";

    //    public CmdBase TargetData
    //    {
    //        get
    //        {
    //            CmdBase data = new AtUsers
    //            {
    //                acessToken = this.acessToken,
    //                accessTokenSecret = this.acessTokenSecret,

    //                //
    //                requestId = this.requestId,
    //                _queueId = this.queueID,

    //                _keyword = this.keyword,
    //                _count = this.count,
    //                _type = this.type,
    //                _range = this.range

    //            };
    //            return data;
    //        }
    //    }
    //}

    ///// <summary>
    ///// SEARCH__MESSAGE_STATUSES
    ///// </summary>
    //public class cmdSearchMessage : SdkCmdBase
    //{
    //    public string q = "";
    //    public string filter_ori = "";
    //    public string filter_pic = "";
    //    public string fuid = "";
    //    public string province = "";
    //    public string city = "";
    //    public string starttime = "";
    //    public string endtime = "";
    //    public string page = "";
    //    public string count = "";
    //    public string needcount = "";
    //    public string base_app = "";

    //    public CmdBase TargetData
    //    {
    //        get
    //        {
    //            CmdBase data = new SearchMessage
    //            {
    //                acessToken = this.acessToken,
    //                accessTokenSecret = this.acessTokenSecret,

    //                //
    //                requestId = this.requestId,
    //                _queueId = this.queueID,

    //                _q = this.q,
    //                _filter_ori = this.filter_ori,
    //                _filter_pic = this.filter_pic,
    //                _fuid = this.fuid,
    //                _province = this.province,
    //                _city = this.city,
    //                _starttime = this.starttime,
    //                _endtime = this.endtime,
    //                _page = this.page,
    //                _count = this.count,
    //                _needcount = this.needcount,
    //                _base_app = this.base_app

    //            };
    //            return data;
    //        }
    //    }
    //}

    ///// <summary>
    ///// MESSAGE_CONTACT_USERS
    ///// </summary>
    //public class cmdContactUsers : SdkCmdBase
    //{
    //    public string count = "";
    //    public string cursor = "";
    //    public string page = "";

    //    public CmdBase TargetData
    //    {
    //        get
    //        {
    //            CmdBase data = new ContactUsers
    //            {
    //                acessToken = this.acessToken,
    //                accessTokenSecret = this.acessTokenSecret,

    //                //
    //                requestId = this.requestId,
    //                _queueId = this.queueID,

    //                _count = this.count,
    //                _cursor = this.cursor,
    //                _page = this.page

    //            };
    //            return data;
    //        }
    //    }
    //}

    /// <summary>
    /// DIRECT_MESSAGES | DIRECT_MESSAGES_SENT | MENTIONS | COMMENTS_TIMELINE | COMMENTS_BYME | COMMENTS_TOME | REPOST_BY_ME | MESSAGES_WITH
    /// </summary>
    public class cmdNormalMessages : SdkCmdBase
    {
        public string id = "";
        public string sinceID = "";
        public string maxID = "";
        public string count = "";
        public string page = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new NormalMessages
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _id = this.id,
                    _sinceID = this.sinceID,
                    _maxID = this.maxID,
                    _count = this.count,
                    _page = this.page

                };
                return data;
            }
        }
    }

    ///// <summary>
    ///// FRIENDS_TIMELINE
    ///// </summary>
    //public class cmdFriendTimeline : SdkCmdBase
    //{
    //    public string sinceID = "";
    //    public string maxID = "";
    //    public string count = "";
    //    public string page = "";
    //    public string baseApp = "";
    //    public string feature = "";

    //    public CmdBase TargetData
    //    {
    //        get
    //        {
    //            CmdBase data = new FriendTimeline
    //            {
    //                acessToken = this.acessToken,
    //                accessTokenSecret = this.acessTokenSecret,

    //                //
    //                requestId = this.requestId,
    //                _queueId = this.queueID,
                    
    //                _sinceID = this.sinceID,
    //                _maxID = this.maxID,
    //                _count = this.count,
    //                _page = this.page,
    //                _baseApp = this.baseApp,
    //                _feature = this.feature

    //            };
    //            return data;
    //        }
    //    }
    //}

    ///// <summary>
    ///// GROUP_TIMElINE
    ///// </summary>
    //public class cmdGroupTimeline : SdkCmdBase
    //{
    //    public string groupId = "";
    //    public string sinceId = "";
    //    public string maxId = "";
    //    public string perPage = "";
    //    public string page = "";
    //    public int feature = 0;

    //    public CmdBase TargetData
    //    {
    //        get
    //        {
    //            CmdBase data = new GroupTimeline
    //            {
    //                acessToken = this.acessToken,
    //                accessTokenSecret = this.acessTokenSecret,

    //                //
    //                requestId = this.requestId,
    //                _queueId = this.queueID,

    //                _groupId = this.groupId,
    //                _sinceId = this.sinceId,
    //                _maxId = this.maxId,
    //                _perPage = this.perPage,
    //                _page = this.page,
    //                _feature = this.feature


    //            };
    //            return data;
    //        }
    //    }
    //}

    /// <summary>
    /// USER_TIMELINE
    /// </summary>
    public class cdmUserTimeline : SdkCmdBase
    {
        public string userId = "";
        public string screenName = "";
        public string sinceID = "";
        public string maxID = "";
        public string count = "";
        public string page = "";
        public string baseApp = "";
        public string feature = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new NormalMessages
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                   _id =this.userId,
                   _screenName = this.screenName,
                   _sinceID = this.sinceID,
                   _maxID = this.maxID,
                   _count = this.count,
                   _page = this.page,
                   _baseApp = this.baseApp,
                   _feature = this.feature


                };
                return data;
            }
        }
    }

    /// <summary>
    /// GET_FRIENDS | GET_FANS
    /// </summary>
    public class cmdGetFriends_Fans : SdkCmdBase
    {
        public string userId = "";
        public string screenName = "";
        public string cursor = "";
        public string count = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new NormalMessages
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _id = this.userId,
                    _screenName = this.screenName,
                    _cursor = this.cursor,
                    _count = this.count


                };
                return data;
            }
        }
    }

    /// <summary>
    /// GET_FAVORITES
    /// </summary>
    public class cmdGetFavorite : SdkCmdBase
    {
        public string page = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new NormalMessages
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _page = this.page

                };
                return data;
            }
        }

    }

    /// <summary>
    /// COMMENTS_BYID | COMMENTS_BYID_ORG
    /// </summary>
    public class cmdCommentByID : SdkCmdBase
    {
        public string blogID = "";
        public string count = "";
        public string page = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new NormalMessages
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _id = this.blogID,
                    _count = this.count,
                    _page = this.page

                };
                return data;
            }
        }

    }

    /// <summary>
    /// FREE_LOOK_OPEN | | DAILYHOT_REPOST_OPEN 
    /// | WEEKLYHOT_REPOST_OPEN | DAILYHOT_COMMENT_OPEN | WEEKLYHOT_COMMENT_OPEN | WEEKLYHOT_COMMENT_OPEN
    /// </summary>
    public class cdmCoutParam : SdkCmdBase
    {
        public string _count = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new CoutParam
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                 _count = this._count

                };
                return data;
            }
        }
    }

    /// <summary>
    /// GET_MAP_IMAGE
    /// </summary>
    public class cmdGetMapImage : SdkCmdBase
    {
        public string centerCoordinates = "";
        public string city = "";
        public string coordinates = "";
        public string names = "";
        public string size = "";
        public string imgFormat = "";
        public string zoom = "";
        public string px = "";
        public string py = "";
        public string icons = "";
        public string font = "";
        public string lines = "";
        public string scale = "";
        public string traffic = "";
        public string polygons = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new GetMapImage
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _centerCoordinates = this.centerCoordinates,

                    _city = this.city,
                    _coordinates = this.coordinates,
                    _names = this.names,
                    _size = this.size,
                    _imgFormat = this.imgFormat,
                    _zoom = this.zoom,
                    _px = this.px,
                    _py = this.py,
                    _icons = this.icons,
                    _font = this.font,
                    _lines = this.lines,
                    _scale = this.scale,
                    _traffic = this.traffic,
                    _polygons = this.polygons

                };
                return data;
            }
        }

    }

    
    ///// <summary>
    /////GROUP_LIST 
    ///// </summary>
    //public class cmdGroupList : SdkCmdBase
    //{
    //    //可选参数。将结果分页，每一页包含20个lists。由-1开始分页，定位一个id地址
    //    //，通过比较id大小实现next_cursor 和previous_cursor向前或向后翻页。
    //    public string cursor = "";
    //    //可选参数。控制返回的LIST。1：返回私有列表； 0：返回公开列表 ；默认0。
    //    //当要求返回私有LIST时，当前用户必须为私有LIST的创建者。
    //    public int listType = 1;

    //    public CmdBase TargetData
    //    {
    //        get
    //        {
    //            CmdBase data = new GroupList
    //            {
    //                acessToken = this.acessToken,
    //                accessTokenSecret = this.acessTokenSecret,

    //                //
    //                requestId = this.requestId,
    //                _queueId = this.queueID,

    //                _cursor =this.cursor,
    //                _listType = this.listType

    //            };
    //            return data;
    //        }
    //    }
    //}

    /// <summary>
    /// GET_TRENDS
    /// </summary>
    public class cmdGetTrends : SdkCmdBase
    {
        public string user_id = "";
        public string page = "";
        public string count = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new GetTrends
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _user_id = this.user_id,
                    _page = this.page,
                    _count = this.count

                };
                return data;
            }
        }

    }

    /// <summary>
    /// FOLLOW_TREND
    /// </summary>
    public class cmdFollowTrend : SdkCmdBase
    {
        public string trendName = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new FollowTrend
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _trendName = this.trendName

                };
                return data;
            }
        }
    }

    /// <summary>
    /// DESTORY_TRENDS
    /// </summary>
    public class cmdDestoryTrend : SdkCmdBase
    {
        public string trendId = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new DestoryTrend
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    _trendId = this.trendId

                };
                return data;
            }
        }
    }

    /// URL_L_TO_S
    /// </summary>
    public class cmdUrlConvert : SdkCmdBase
    {
        public string longUrl = "";

        public CmdBase TargetData
        {
            get
            {
                CmdBase data = new UrlConvert
                {
                    acessToken = this.acessToken,
                    accessTokenSecret = this.acessTokenSecret,

                    //
                    requestId = this.requestId,

                    //目前只支持一个url转换
                    longUrls = new string[] { longUrl },
                };
                return data;
            }
        }
    }
}
