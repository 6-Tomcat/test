using System.Web;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.SessionState;
using System.IO;
using System;

using System.Linq;

using System.Text.RegularExpressions;
using System.Configuration;
namespace ykx
{
    /// <summary>
    /// LeiBie 的摘要说明
    /// </summary>
    public class LeiBie : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string Category_ID = context.Request["Category_ID"];
            //类别
            DataTable Category = SqlHelper.ExecuteDataTable("select *  from Category where IS_DELETE = 0");
            DataTable Product = SqlHelper.ExecuteDataTable("select *  from Product where IS_DELETE = 0");
            //////product的分页
            string requestNum = context.Request["PageNumber"];
            string ashxName = "LeiBie.ashx";
            string tableName = "Product";
            string id = "Product_ID";
            int yeShu = int.Parse(ConfigurationManager.AppSettings["pagecount"].ToString());
            int PageNumber = 1;
            if (requestNum != null)
            {
                //前后端用正则表达式的形式有点不一样，道理是一样的。
                Regex r = new Regex(@"^\d*$");
                if (r.IsMatch(requestNum))
                    PageNumber = int.Parse(requestNum);
            }
            DataTable dt = SqlHelper.ExecuteDataTable("select * from (select *,ROW_NUMBER() over( order by " + id + " asc) as num from " + tableName + " p where IS_DELETE = 0 and Category_ID = "+ Category_ID + " ) s where s.num>@Start and s.num<@End ",
                new SqlParameter("@Start", (PageNumber - 1) * yeShu),
                new SqlParameter("@End", PageNumber * yeShu + 1));

            int totalCount = (int)SqlHelper.ExecuteScalar("select count(*) from " + tableName + " where IS_DELETE = 0 and Category_ID = "+ Category_ID);
            int pageCount = (int)Math.Ceiling(totalCount / (float)yeShu);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "" + ashxName + "?Category_ID="+ Category_ID + "&PageNumber=" + (i + 1), Title = i + 1 };
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////
            var Data = new { Products = Product.Rows, Product = dt.Rows, Category = Category.Rows, PageData = pageData, TotalCount = totalCount, PageNumber = PageNumber, PageCount = pageCount ,flag = 1, Category_ID = Category_ID };
            string html = CommonHelper.RenderHtml("index.html", Data);
            context.Response.Write(html);
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