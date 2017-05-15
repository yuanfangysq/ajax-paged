using ajax_分页.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ajax_分页.API
{
    /// <summary>
    /// GetPageData 的摘要说明
    /// </summary>
    public class GetPageData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            context.Response.ContentType = "text/plain";
            int pageSize = 2; //设定页大小,每页显示10条数据  
            int currentPage = Convert.ToInt32(context.Request.QueryString["currentPage"]); //设定当前页  

            int rowCount = 0;  //作为out参数传递给方法,在方法里给rowCount赋值  
            int pageCount = 0; //作为out参数传递给方法,在方法里给rowCount赋值  

            string jsonData = null;
             
            List<Location> list = DAL.Aticel.GetPageListByPageIndex(pageSize, currentPage, out rowCount, out pageCount);
            if (list != null && list.Count > 0)
            {

                //创建Json序列化器，将对象转换成一个Json格式的字符串  
                JavaScriptSerializer jsz = new JavaScriptSerializer();

                jsonData = jsz.Serialize(list); //将一个list对象转换成json格式的字符串  

                context.Response.Write(jsonData);

            }
            else
            {
                context.Response.Write("no");
            }
           
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