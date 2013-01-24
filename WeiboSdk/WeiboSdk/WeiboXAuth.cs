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
using SinaBase;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using NetDefine;
using WeiboService.Model.JObjectDefine;

namespace WeiboSdk
{
    /// <summary>
    /// 用于XAuth登录的静态类
    /// Author:linan4
    /// </summary>
    static public class WeiboXAuth
    {
        public delegate void LoginBack(bool isSucess, SdkAuthError err, SdkAuthRes response);

        static public void XAuthLogin(string userName, string passWord, bool ifSave = false, LoginBack callBack = null)
        {
            SdkAuthError error = new SdkAuthError();
            SdkAuthRes response = new SdkAuthRes();

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.ClientAuthentication,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = ServiceData.AppKey,
                ConsumerSecret = ServiceData.AppSecret,
                ClientUsername = userName,
                ClientPassword = passWord
            };

            var client = new RestClient
            {
                Authority = ConstDefine.ServerUrl,
                Credentials = credentials,
                HasElevatedPermissions = true,

            };

            var request = new RestRequest
            {
                Path = "/oauth/access_token",
                Method = WebMethod.Post
            };

            client.BeginRequest(request, (e1, e2, e3) =>
            {
                if (null != e2.UnKnowException)
                {
                    error.errCode = SdkErrCode.NET_UNUSUAL;
                    error.errMessage = "无网络";
                    if (null != callBack)
                        callBack(false, error, response);
                    return;
                }

                if (null != e2.InnerException)
                {
                    error.errCode = SdkErrCode.NET_UNUSUAL;
                    if (!string.IsNullOrEmpty(e2.Content))
                    {
                        error.errCode = SdkErrCode.SERVER_ERR;
                        //request=%2Foauth%2Faccess_token&error_code=403&error=40309%3AError%3A+
                        //password+error%21&error_CN=%E9%94%99%E8%AF%AF%3A%E5%AF%86%E7%A0%81%E4%B8%8D%E6%AD%A3%E7%A1%AE
                        string content = HttpUtility.UrlDecode(e2.Content);
                        content = BaseTool.GetQueryParameter(content, "error");
                        int pos = content.IndexOf(":");
                        if (-1 != pos)
                            content = content.Substring(0, pos);
                        //错误码
                        error.specificCode = content;
                    }
                    if (null != callBack)
                        callBack(false, error, response);
                    return;
                }
                //获取accessToken 和 accessTokenSecret
                string token = BaseTool.GetQueryParameter(e2.Content, "oauth_token");
                string tokenAccess = BaseTool.GetQueryParameter(e2.Content, "oauth_token_secret");
                string userId = BaseTool.GetQueryParameter(e2.Content, "user_id");
                //成功/失败
                if (0 == token.Length || 0 == tokenAccess.Length)
                {
                    string _content = HttpUtility.UrlDecode(e2.Content);
                    string errCode = BaseTool.GetQueryParameter(_content, "error");
                    int pos = errCode.IndexOf(":");
                    if (pos != -1)
                        errCode = errCode.Remove(pos);
                    string chMessage = "";
                    //if (errCode == "40309")
                    //    chMessage = "您输入的登录或密码错误，请重新输入.";
                    //else
                    chMessage = BaseTool.GetQueryParameter(_content, "error_CN");

                    error.errCode = SdkErrCode.AUTH_FAILED;
                    error.specificCode = errCode;
                    error.errMessage = chMessage;
                    
                    if(null != callBack)
                        callBack(false, error,response);
                }

                else
                {
                    if (true == ifSave)
                    {
                        //保存信息
                        JUser user = new JUser();
                        user.LID = userName;
                        user.AutoSave = true;
                        user.Id = userId;
                        user.accessToken = token;
                        user.accessTokenSecrect = tokenAccess;

                        //保存到账户配置文件
                        if (null != user)
                            DataMgr.getInstance().SaveJsonLoginUser(user);
                    }
                    if (null != callBack)
                    {
                        response.userId = userId;
                        response.acessToken = token;
                        response.acessTokenSecret = tokenAccess;
                        callBack(true, error, response);
                    }
                        
               }
            });
        }

    }
}
