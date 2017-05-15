using ajax_分页.sqlhelp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ajax_分页.DAL
{
    public class Aticel
    {

        public static List<Location> GetPageListByPageIndex(int pageSize, int currentpage, out int rowCount, out int pageCount)
        {
            DataTable dt = SqlHelper.ExecuteProcPageList(pageSize, currentpage, out rowCount, out pageCount);

            var list = new List<Location>();// 声明一个泛型对象list  
            if (dt != null && dt.Rows.Count > 0)
            {
                //将DataTable转换成一个list  
                list = (from p in dt.AsEnumerable()  //（遍历DataTable）  
                        select new Location
                        {
                            id = p.Field<int>("id"),   //将DateTable里的字段赋值给Location类中的属性  
                            ProductCodde = p.Field<string>("ProductCodde"),
                            ProdcutName = p.Field<string>("ProdcutName"),
                            //Rates = p.Field<short>("Rates")
                           // LocType = p.Field<short>("locType"),                          
                        }).ToList();
            }
            return list;   

        }  
    }

    public class Location
    {
        public int id { get; set; }
        public string ProdcutName { get; set; }
        public string ProductCodde { get; set; }
        public double Rates { get; set; }
    }
}