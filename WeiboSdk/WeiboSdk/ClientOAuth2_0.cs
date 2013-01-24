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
using Hammock;
using Hammock.Web;
using Hammock.Silverlight.Compat;
using System.Text;
using System.Diagnostics;

namespace WeiboSdk
{
    static public class ClientOAuth2_0 
    {
        /// <summary>
        /// 客户端方式(需要授权)获取AccessToken
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passWord"></param>

        public delegate void LoginBack(SdkErrCode err, string response);
        static public void GetAccessToken(string name,string passWord,LoginBack callback)
        {
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
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", name);
            request.AddParameter("password", passWord);

            SdkAuthError err = new SdkAuthError();
            SdkAuthRes response = new SdkAuthRes();

            client.BeginRequest(request, (e1, e2, e3) =>
            {
                if (null != e2.UnKnowException || null != e2.InnerException || e2.StatusCode == HttpStatusCode.NotFound)
                {
                    err.errCode = SdkErrCode.NET_UNUSUAL;
                    if (null != callback)
                        callback(SdkErrCode.NET_UNUSUAL, "");
                    return;
                }
                else
                {
                    if (null != callback)
                        callback(SdkErrCode.SUCCESS, e2.Content);
                }


            });
        }

        /// <summary>
        /// 用relfshCode刷新AccessToken
        /// </summary>
        /// <param name="refleshCode"></param>
        static public void RefleshAccessToken(string refleshCode,LoginBack callBack)
        {
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
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refleshCode);

            client.BeginRequest(request, (e1, e2, e3) =>
            {
                if (null != e2.UnKnowException || null != e2.InnerException || e2.StatusCode == HttpStatusCode.NotFound)
                {
                    if (null != callBack)
                        callBack(SdkErrCode.NET_UNUSUAL, "");
                    return;
                }

                if (null != callBack)
                    callBack(SdkErrCode.SUCCESS, e2.Content);
            });
        }
    }
}
