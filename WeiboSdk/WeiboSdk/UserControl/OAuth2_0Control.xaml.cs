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
using Hammock.Web;
using Hammock.Silverlight.Compat;
using System.Text;
namespace WeiboSdk
{
    /// <summary>
    /// OAUTH 登录控件
    /// Author:linan4
    /// </summary>
    public partial class OAuth2_0Control : UserControl
    {
        public OAuth2_0Control()
        {
            InitializeComponent();
            if (DesignerProperties.IsInDesignTool)
                return;

            string accredit = string.Format("{0}/oauth2/authorize?client_id={1}&response_type=code&redirect_uri={2}&display=mobile"
                ,ConstDefine.ServerUrl2_0, SdkData.AppKey, SdkData.RedirectUri);
            Dispatcher.BeginInvoke(() =>
            {
                OAuthBrowser.Navigate(new Uri(accredit));
            });
        }

        #region
        //登录的回调
        public ClientOAuth2_0.LoginBack OAuthBack = null;


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
            if (!e.Uri.AbsoluteUri.Contains("code=")&&!e.Uri.AbsoluteUri.Contains("code ="))
                return;

            e.Cancel = true;


            var arguments = e.Uri.AbsoluteUri.Split('?');
            if (0 == arguments.Length)
            {
                //error.errCode = SdkErrCode.AUTH_FAILED;
                if(null != OAuthBack)
                    OAuthBack(SdkErrCode.AUTH_FAILED, "");
                return;
            }

            GetAccessToken(arguments[1]);
        }

        private void GetAccessToken(string uri)
        {

            string requestVerifier = BaseTool.GetQueryParameter(uri, "code");
            if (string.IsNullOrEmpty(requestVerifier))
            {
                if (null != OAuthBack)
                    OAuthBack(SdkErrCode.NET_UNUSUAL, "");
                return;
            }

            RestClient client = new RestClient();
            client.Authority = ConstDefine.ServerUrl2_0;
            client.HasElevatedPermissions = true;

            RestRequest request = new RestRequest();
            request.Path = "/oauth2/access_token";
            request.Method = WebMethod.Post;

            request.DecompressionMethods = DecompressionMethods.GZip;
            request.Encoding = Encoding.UTF8;

            request.AddParameter("client_id", SdkData.AppKey);
            request.AddParameter("client_secret", SdkData.AppSecret);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("redirect_uri", SdkData.RedirectUri);
            request.AddParameter("code", requestVerifier);


            client.BeginRequest(request, (e1, e2, e3) =>
            {
                if (null != e2.UnKnowException || null != e2.InnerException || e2.StatusCode == HttpStatusCode.NotFound)
                {
                    if (null != OAuthBack)
                        OAuthBack(SdkErrCode.NET_UNUSUAL, "");
                    return;
                }

                if (null != OAuthBack)
                    OAuthBack(SdkErrCode.SUCCESS, e2.Content);
            });


        }
        private void BrowserLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

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
    }
}