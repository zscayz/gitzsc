using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WeiBoService;
using SinaBase;
using System.Windows.Navigation;
using Hammock;
using Hammock.Authentication;
using NetDefine;
using System.ComponentModel;
using Hammock.Authentication.OAuth;
using WeiboService.Model.JObjectDefine;

namespace WeiboSdk
{
    /// <summary>
    /// OAUTH 登录控件
    /// Author:linan4
    /// </summary>
    public partial class OAuthControl : UserControl
    {
        //注册依赖项
        public static readonly DependencyProperty ifSaveProperty =
        DependencyProperty.Register("ifSave", typeof(bool), typeof(OAuthControl), new PropertyMetadata(false));


        public OAuthControl()
        {
            InitializeComponent();
            if (DesignerProperties.IsInDesignTool)
                return;
            error = new SdkAuthError();
            error.errCode = SdkErrCode.SUCCESS;
            resInfo = new SdkAuthRes();
            GetOAuthToken();
        }

        #region
        private string oAuthToken = "";
        private string oAuthTokenSecret = "";
        private SdkAuthError error = null;
        private SdkAuthRes resInfo = null;
        //登录的回调
        public WeiboXAuth.LoginBack OAuthBack = null;

        //public bool ifSave = false;

        public bool ifSave
        {
            get
            {
                return (bool)GetValue(ifSaveProperty);
            }
            set
            {
                SetValue(ifSaveProperty, value);
            }
        }

        private EventHandler _OAuthBrowserCancelled;
        public EventHandler OAuthBrowserCancelled
        {
            get
            {
                return _OAuthBrowserCancelled;
            }
            set
            {
                _OAuthBrowserCancelled = value;
            }
        }

        private EventHandler _OAuthBrowserNavigated;
        public EventHandler OAuthBrowserNavigated 
        {
            get
            {
                return _OAuthBrowserNavigated;
            }
            set
            {
                _OAuthBrowserNavigated = value;
            }
        }

        private EventHandler _OAuthBrowserNavigating;
        public EventHandler OAuthBrowserNavigating
        {
            get
            {
                return _OAuthBrowserNavigating;
            }
            set
            {
                _OAuthBrowserNavigating = value;
            }
        }

        #endregion

        private void BrowserNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //if (null != OAuthBrowserNavigated)
            //    OAuthBrowserNavigated.Invoke(sender, e);
        }

        private void BrowserNavigating(object sender, NavigatingEventArgs e)
        {

            if (null != OAuthBrowserNavigating)
                OAuthBrowserNavigating.Invoke(sender, e);

            if (e.Uri.AbsoluteUri.Contains("closeWindow()"))
            {
                if (null != OAuthBrowserCancelled)
                    OAuthBrowserCancelled.Invoke(sender, e);
            }

            string url = SdkData.RedirectUri.ToLower();
            if (!e.Uri.AbsoluteUri.Contains(url))
                return;

            e.Cancel = true;

            var arguments = e.Uri.AbsoluteUri.Split('?');
            if (0 == arguments.Length)
            {
                error.errCode = SdkErrCode.AUTH_FAILED;
                if(null != OAuthBack)
                    OAuthBack(false, error, resInfo);
                return;
            }

            GetAccessToken(arguments[1]);
        }

        private void BrowserLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

            //test
            //string strHtml = OAuthBrowser.SaveToString();

            //if (-1 != strHtml.IndexOf("<span class=\"fb\">"))
            //{
            //    int x = 6;
            //    // GetAccessToken(arguments[1]);
            //}
            //test
            //处理Browser控件"_parent"标签不能点击
            try
            {
                OAuthBrowser.InvokeScript("eval",
                @"
                window.ScanTelTag=function(elem) {
                if (elem.getAttribute('target') != null && elem.getAttribute('target').indexOf('_parent') == 0) {
                    elem.setAttribute('target', '_self');
                    }
                }
            
                window.Initialize=function() {
                var elems = document.getElementsByTagName('a');
                for (var i = 0; i < elems.length; i++)
                ScanTelTag(elems[i]);
                }");
                OAuthBrowser.InvokeScript("Initialize");
            }
            catch (Exception)
            {
            }

            if (null != OAuthBrowserNavigated)
                OAuthBrowserNavigated.Invoke(sender, e);
        }

        private void GetOAuthToken()
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = SdkData.AppKey,
                ConsumerSecret = SdkData.AppSecret,
                Version = "1.0",
                CallbackUrl = SdkData.RedirectUri
            };

            var client = new RestClient
            {
                Authority = ConstDefine.OAuthTokenRequestUrl,
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/request_token"
            };
            client.BeginRequest(request, new RestCallback(RequestTokenCallBack));
        }

        private void RequestTokenCallBack(RestRequest request, RestResponse response, object userstate)
        {
            oAuthToken = BaseTool.GetQueryParameter(response.Content, "oauth_token");
            oAuthTokenSecret = BaseTool.GetQueryParameter(response.Content, "oauth_token_secret");
            var authorizeUrl = ConstDefine.AuthorizeUrl + "?oauth_token=" + oAuthToken;

            authorizeUrl += "&display=mobile&oauth_callback=" + System.Net.HttpUtility.UrlEncode(SdkData.RedirectUri);
            if (String.IsNullOrEmpty(oAuthToken) || String.IsNullOrEmpty(oAuthTokenSecret))
            {

                if (null != OAuthBack)
                {
                    error.errCode = SdkErrCode.AUTH_FAILED;
                    OAuthBack(false, error, resInfo);
                }
                return;
            }

            Dispatcher.BeginInvoke(() =>
            {
                OAuthBrowser.Navigate(new Uri(authorizeUrl));
            });
        }

        private void GetAccessToken(string uri)
        {
            var requestToken = WeiboTool.GetQueryParameter(uri, "oauth_token");
            if (requestToken != oAuthToken)
            {
                if (null != OAuthBack)
                {
                    error.errCode = SdkErrCode.AUTH_FAILED;
                    OAuthBack(false, error, resInfo);
                    return;
                }
            }

            var requestVerifier = WeiboTool.GetQueryParameter(uri, "oauth_verifier");

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = SdkData.AppKey,
                ConsumerSecret = SdkData.AppSecret,
                Token = oAuthToken,
                TokenSecret = oAuthTokenSecret,
                Verifier = requestVerifier
            };

            var client = new RestClient
            {
                Authority = ConstDefine.AcessTokenRequstUrl,
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/access_token"
            };

            client.BeginRequest(request, new RestCallback(RequestAccessTokenCallBack));
        }

        private void RequestAccessTokenCallBack(RestRequest request, RestResponse response, object userstate)
        {
            string accessToken = WeiboTool.GetQueryParameter(response.Content, "oauth_token");
            string accessTokenSecret = WeiboTool.GetQueryParameter(response.Content, "oauth_token_secret");
            string userId = WeiboTool.GetQueryParameter(response.Content, "user_id");
            //string screenName = GetQueryParameter(response.Content, "screen_name");

            if (String.IsNullOrEmpty(accessToken) || String.IsNullOrEmpty(accessTokenSecret))
            {
                //TODO:
                //通知
                if (null != OAuthBack)
                {
                    error.errCode = SdkErrCode.AUTH_FAILED;
                    OAuthBack(false, error, resInfo);
                }
                return;
            }


            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (true == ifSave)
                {
                    JUser user = new JUser();
                    //user.LID = userName;
                    user.AutoSave = true;
                    user.Id = userId;
                    user.accessToken = accessToken;
                    user.accessTokenSecrect = accessTokenSecret;
                    //保存
                    DataMgr.getInstance().SaveJsonLoginUser(user);
                }
                //通知
                if (null != OAuthBack)
                {
                    resInfo.userId = userId;
                    resInfo.acessToken = accessToken;
                    resInfo.acessTokenSecret = accessTokenSecret;
                    OAuthBack(true, error, resInfo);
                }
        });

      }

    }
}