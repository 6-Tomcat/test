using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
namespace ykx
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AdminLogin : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string userName = context.Request["UserName"];
            string password = context.Request["Password"];
            string action = context.Request["Action"];//标记是通过异步进入后台的还是通过提交按钮进入后台的
            //==============异步实现的区域==========================
            if (!string.IsNullOrEmpty(action))
            {
                if (string.IsNullOrEmpty(userName))
                {
                    context.Response.Write("noUser");//该用户名不能为空
                    return;
                }
                else
                {
                    int b = (int)SqlHelper.ExecuteScalar("select Count(*) from Admin where Account=@userName"
                                   , new SqlParameter("@userName", userName));//异步查找该用户名是否存在
                    if (b < 1)
                    {
                        context.Response.Write("no");//该用户不存在
                        return;
                    }
                    if (b >= 1)
                    {
                        context.Response.Write("yes");//该用户存在
                        return;

                    }
                }

            }
            // ======================================================
            //===============通过按钮的提交实现的区域=================
            else
            {


                if (string.IsNullOrEmpty(password))
                {
                    context.Response.Write("passwordnull");//密码是否为空
                    return;
                }
                else
                {
                    if (!context.Request["YanZheng"].Equals(context.Session["check"]))
                    {
                        //先检查验证码          
                        context.Response.Write("yanzhengma");
                        return;
                    }
                    else
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from Admin where PSW=@password and Account=@userName"
                        , new SqlParameter("@password", password)
                        , new SqlParameter("@userName", userName));
                        if (dt.Rows.Count < 1)
                        {
                            context.Response.Write("password");//说明密码错误
                            return;
                        }
                        else
                        {
                            //这里面没有对Count>1做处理是因为没有必要
                            //这只是一个搜索的功能用户名和密码都可以重复
                            context.Response.Write("succeed");
                            context.Session["Admin"] = userName;

                            return;
                        }
                    }
                }
            }
            //===============================================================
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