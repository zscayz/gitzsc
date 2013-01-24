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
using WeiBoService;
using NetDefine;
using System.IO;

namespace WeiboSdk
{
    public class SdkResponse
    {
        public string requestID = "";
        public string queueID = "";
        public SdkErrCode errCode;
        public string specificCode;
        public string content = "";
        public Stream stream = null;
    }

    /// <summary>
    /// SDK中的数据请求类
    /// Author:linan4
    /// </summary>
    public class SdkNetEngine
    {
        //设定鉴权方式
        public static EumAuth AuthOption
        {
            set
            {
                if (value == EumAuth.OAUTH2_0)
                    NetEngine.ifOAuth2 = true;
                else
                    NetEngine.ifOAuth2 = false;
            }
        }

        public delegate void SdkCallBack(SdkRequestType type, SdkResponse response);
        public void RequestCmd(SdkRequestType type, SdkCmdBase data, SdkCallBack callBack, DataType dataType = DataType.XML)
        {

            NetEngine net = new NetEngine();
            net.otherCallBack = (e1, e2) =>
                {
                    SdkErrCode _errcode = SdkErrCode.SUCCESS;
                    switch (e2._errCode)
                    {
                        case EErrCode.XPARAM_ERR:
                            {
                                _errcode = SdkErrCode.XPARAM_ERR;
                            }
                            break;

                        case EErrCode.NET_UNUSUAL:
                            {
                                _errcode = SdkErrCode.NET_UNUSUAL;
                            }
                            break;
                        case EErrCode.SERVER_ERR:
                            {
                                _errcode = SdkErrCode.SERVER_ERR;
                            }
                            break;
                            
                    }


                    SdkResponse response = new SdkResponse
                    {
                        requestID = e2._requestID,
                        queueID = e2._queueID,
                        errCode = _errcode,
                        specificCode = e2._specificCode,
                        content = e2._content,
                        stream = e2._stream
                    };

                    //回调
                    if (null != callBack)
                        callBack(type, response);

                };

            Action<string> errAction = (e1) =>
            {
                if (null != callBack)
                {
                    SdkErrCode _errcode = SdkErrCode.XPARAM_ERR;

                    SdkResponse response = new SdkResponse
                    {
                        requestID = null != data?data.requestId : "",
                        errCode = _errcode,
                        specificCode = "",
                        content = e1,
                        stream = null
                    };
                    callBack(type, response);
                }

            };
            #region 分类组参数
            switch (type)
            {   
                case SdkRequestType.FRIENDS_TIMELINE:
                    {
                        cmdFrendTimeLine _data = data as cmdFrendTimeLine;

                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.FRIENDS_TIMELINE, _data.TargetData, dataType);
                    }
                    break;

                case SdkRequestType.VERIFY_CREDENTIALS:
                    {
                        cmdVerifyCredential _data = data as cmdVerifyCredential;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.VERIFY_CREDENTIALS, _data.TargetData, dataType);
                    }
                    break;

                case SdkRequestType.UPLOAD_MESSAGE:
                    {
                        cmdUploadMessage _data = data as cmdUploadMessage;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.UPLOAD_MESSAGE, _data.TargetData, dataType);

                    }
                    break;

                case SdkRequestType.UPLOAD_MESSAGE_PIC:
                    {
                        cmdUploadPic _data = data as cmdUploadPic;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.UPLOAD_MESSAGE_PIC, _data.TargetData, dataType);
                    }
                    break;

                case SdkRequestType.GET_MESSAGE_BYID:
                    {
                        cmdBlogID _data = data as cmdBlogID;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.GET_MESSAGE_BYID, _data.TargetData, dataType);

                    }
                    break;
                case SdkRequestType.COMMENT_MESSAGE:
                    {
                        cmdReplyComment _data = data as cmdReplyComment;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.COMMENT_MESSAGE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.REPLY_COMMENT:
                    {
                        cmdReplyComment _data = data as cmdReplyComment;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.REPLY_COMMENT, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.SEND_DIRECT_MESSAGE:
                    {
                        cmdSendDirectMessage _data = data as cmdSendDirectMessage;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.SEND_DIRECT_MESSAGE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.COMMENT_FOWARD_COUNT:
                    {
                        cmdBatchCmd _data = data as cmdBatchCmd;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.COMMENT_FOWARD_COUNT, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.GET_USER_INFO:
                    {
                        cmdFriendShip _data = data as cmdFriendShip;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.GET_USER_INFO, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.ADD_FAVORITE:
                    {
                        cmdBlogID _data = data as cmdBlogID;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.ADD_FAVORITE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.CANCLE_FAVORITE:
                    {
                        cmdBlogID _data = data as cmdBlogID;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.CANCLE_FAVORITE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.GET_UNREAD_MESSAGE:
                    {
                        cmdGetUnreadMessage _data = data as cmdGetUnreadMessage;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.GET_UNREAD_MESSAGE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.RESET_COUNT:
                    {
                        cmdResetCount _data = data as cmdResetCount;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.RESET_COUNT, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.REPOST_MESSAGE:
                    {
                        cmdRepostMessage _data = data as cmdRepostMessage;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }

                        net.NetRequest(ServiceType.REPOST_MESSAGE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.FRIENDSHIP_CREATE:
                    {
                        cmdFriendShip _data = data as cmdFriendShip;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.FRIENDSHIP_CREATE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.FRIENDSHIP_DESDROY:
                    {
                        cmdFriendShip _data = data as cmdFriendShip;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.FRIENDSHIP_DESDROY, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.FRIENDSHIP_EXIST:
                    {
                        cmdFriendExist _data = data as cmdFriendExist;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.FRIENDSHIP_EXIST, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.FRIENDSHIP_SHOW:
                    {
                        cmdFriendShip _data = data as cmdFriendShip;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.FRIENDSHIP_SHOW, _data.TargetData, dataType);
                    }
                    break;

                case SdkRequestType.DELETE_BLOG:
                    {
                        cmdBlogID _data = data as cmdBlogID;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.DELETE_BLOG, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.DELETE_COMMENT:
                    {
                        cmdBlogID _data = data as cmdBlogID;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.DELETE_COMMENT, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.DIRECT_MESSAGES:
                    {
                        cmdNormalMessages _data = data as cmdNormalMessages;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.DIRECT_MESSAGES, _data.TargetData, dataType);
                    }
                    break;

                //case SdkRequestType.FRIEND_TIMELINE:
                //    {
                //        cmdFriendTimeline _data = data as cmdFriendTimeline;
                //        if (null == _data)
                //            return;
                //        net.NetRequest(ServiceType.FRIENDS_TIMELINE, _data.TargetData, dataType);
                //    }
                //    break;
                case SdkRequestType.MENTIONS:
                    {
                        cmdNormalMessages _data = data as cmdNormalMessages;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.MENTIONS, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.USER_TIMELINE:
                    {
                        cdmUserTimeline _data = data as cdmUserTimeline;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.USER_TIMELINE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.GET_FRIENDS:
                    {
                        cmdGetFriends_Fans _data = data as cmdGetFriends_Fans;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.GET_FRIENDS, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.GET_FANS:
                    {
                        cmdGetFriends_Fans _data = data as cmdGetFriends_Fans;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.GET_FANS, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.GET_FAVORITES:
                    {
                        cmdGetFavorite _data = data as cmdGetFavorite;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.GET_FAVORITES, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.COMMENTS_BYID:
                    {
                        cmdCommentByID _data = data as cmdCommentByID;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.COMMENTS_BYID, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.COMMENTS_BYID_ORG:
                    {
                        cmdCommentByID _data = data as cmdCommentByID;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.COMMENTS_BYID_ORG, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.FREE_LOOK_OPEN:
                    {
                        cdmCoutParam _data = data as cdmCoutParam;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.FREE_LOOK_OPEN, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.HOURLYHOT_TRENDS_OPEN:
                    {
                        if (null == data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.HOURLYHOT_TRENDS_OPEN, data.Target_Data, dataType);
                    }
                    break;
                case SdkRequestType.DAILYHOT_TRENDS_OPEN:
                    {
                        if (null == data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.DAILYHOT_TRENDS_OPEN, data.Target_Data, dataType);
                    }
                    break;
                case SdkRequestType.WEEKLYHOT_TRENDS_OPEN:
                    {
                        if (null == data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.WEEKLYHOT_TRENDS_OPEN, data.Target_Data, dataType);
                    }
                    break;

                case SdkRequestType.GET_MAP_IMAGE:
                    {
                        cmdGetMapImage _data = data as cmdGetMapImage;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }

                        net.NetRequest(ServiceType.GET_MAP_IMAGE, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.GET_TRENDS:
                    {
                        cmdGetTrends _data = data as cmdGetTrends;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }

                        net.NetRequest(ServiceType.GET_TRENDS, _data.TargetData, dataType);
                    }
                    break;

                case SdkRequestType.DESTORY_TRENDS:
                    {
                        cmdDestoryTrend _data = data as cmdDestoryTrend;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }

                        net.NetRequest(ServiceType.DESTORY_TRENDS, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.FOLLOW_TREND:
                    {
                        cmdFollowTrend _data = data as cmdFollowTrend;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }

                        net.NetRequest(ServiceType.FOLLOW_TREND, _data.TargetData, dataType);
                    }
                    break;
                case SdkRequestType.URL_L_TO_S:
                    {
                        cmdUrlConvert _data = data as cmdUrlConvert;
                        if (null == _data)
                        {
                            errAction("参数类型错误.");
                            return;
                        }
                        net.NetRequest(ServiceType.URL_L_TO_S, _data.TargetData, dataType);
                    }
                    break;

                default:
                    break;

            }
            #endregion
        }

    }
}
