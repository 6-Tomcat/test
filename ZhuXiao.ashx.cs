using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
namespace ykx
{
    /// <summary>
    /// ZhuXiao 的摘要说明
    /// </summary>
    public class ZhuXiao : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Session.Clear();  //从会话状态集合中移除所有的键和值
            context.Session.Abandon(); //取消当前会话
            context.Response.Redirect("/html/login.html");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}