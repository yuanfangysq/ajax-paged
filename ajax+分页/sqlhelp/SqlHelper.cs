using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ajax_分页.sqlhelp
{
    public class SqlHelper
    {
        private static string connStr = "Data Source=.;Initial Catalog=text;Integrated Security=True";

        public static DataTable ExecuteProcPageList(int pageSize, int currentPage, out int rowCount, out int pageCount)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "proc_location_paging"; //存储过程的名字  
                    cmd.CommandType = CommandType.StoredProcedure; //设置命令为存储过程类型(即：指明我们执行的是一个存储过程)  



                    rowCount = 0;
                    pageCount = 0;//这里随便给rowCount，pageCount赋个值，因为使用out传递参数的时候，在方法内部一定要给out参数赋值才能用它，但是虽然这里给它赋初值了，但是在执行存储过程中，存储过程又会给这两个参数赋值，并返还回来给我们，那个才是我们要值  


                    SqlParameter[] parameters ={  
                             new SqlParameter("@pageSize",pageSize),  
                             new SqlParameter("@currentpage",currentPage),  
                             new SqlParameter("@rowCount",rowCount),  
                             new SqlParameter("@pageCount",pageCount)  
                                                 
                                               };

                    //因为在存储过程中@rowCount 与@pageCount 是一个输出参数(output), 而parameters这个数组里，第三，和第四个参数就是要用来替换掉这两个输出参数的，所以这里要将parameters这个数组里的这两个参数设为输出参数。  
                    parameters[2].Direction = ParameterDirection.Output;
                    parameters[3].Direction = ParameterDirection.Output;

                    cmd.Parameters.AddRange(parameters); //将参数传递给我们的cmd命令对象  



                    DataTable dt = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);//到数据库去执行存储过程，并将结果填充到dt表中  
                    }

                    //等存储过程执行完毕后，存储过程会把这两个输出参数传递出来。那么我们在这里来取得这两个返回参数。  
                    rowCount = Convert.ToInt32(parameters[2].Value);
                    pageCount = Convert.ToInt32(parameters[3].Value);

                    return dt;

                }
            }

        }
    }


}