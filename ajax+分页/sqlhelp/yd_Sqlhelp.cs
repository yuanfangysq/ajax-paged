using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace ajax_分页.sqlhelp
{
    public class yd_Sqlhelp
    {
        public class SqlHelp
        {
            /// <summary>  
            /// 获取连接数据库字符串  
            /// </summary>  
            //   private static string connStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString; 

            private static string connStr = "Data Source=.;Initial Catalog=text;Integrated Security=True";
            /// <summary>  
            /// 对连接执行 Transact-SQL 语句并返回受影响的行数  
            /// </summary>  
            /// <param name="sql">对数据库表进行Insert，Update和Delete语句</param>  
            /// <param name="parameters">sql语句参数</param>  
            /// <returns>返回值为该命令所影响的行数</returns>  
            public static int ExecuteNonQuery(CommandType cmdTye, string sql, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = cmdTye;

                        cmd.Parameters.AddRange(parameters);

                        int i = cmd.ExecuteNonQuery();

                        return i;


                    }
                }
            }

            /// <summary>  
            /// 执行查询，并返回查询所返回的结果集中第一行的第一列。一般用于SQL聚合函数，如：count,Max，min，avg。。  
            /// </summary>  
            /// <param name="sql">要执行的sql语句</param>  
            /// <param name="parameters">sql语句的参数</param>  
            /// <returns>返回查询所返回的结果集中第一行的第一列。忽略其他列或行。返回的最大字符数为 2033 个字符。</returns>  
            public static object ExecuteScalar(CommandType cmdTye, string sql, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = cmdTye;
                        cmd.Parameters.AddRange(parameters);

                        //ExcuteScalar()的返回值是object类型的（如果想返回int类型的数据，可先将cmd.ExecuteSalar()转化成int类型再进行返回）    
                        return cmd.ExecuteScalar();

                    }

                }
            }

            /// <summary>  
            /// DataSet是将查询结果填充到本地内存中，这样即使与连接断开，服务器的连接断开，都不会影响数据的读取。但是它也有一个坏处，就是只有小数据量才能往里面存放，数据量大了就给你的本地内存冲爆了。电脑会卡死去。大数据量的话还得用SqlDataReader    
            /// </summary>  
            /// <param name="sql">要执行的sql语句</param>  
            /// <param name="parameters">sql语句的参数</param>  
            /// <returns>返回值是一个DataSet</returns>  
            public static DataSet ExecuteDataSet(CommandType cmdTye, string sql, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = cmdTye;
                        cmd.Parameters.AddRange(parameters);

                        DataSet ds = new DataSet();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            //将cmd的执行结果填充到ds里面  
                            adapter.Fill(ds);
                            return ds;
                        }

                    }
                }
            }

            /// <summary>  
            /// 执行查询，并将查询的结果以DataTable的形式返回  
            /// </summary>  
            /// <param name="sql">要执行的sql语句</param>  
            /// <param name="parameters">sql语句的参数</param>  
            /// <returns>返回值是一个DataTable</returns>  
            public static DataTable ExecuteDataTable(CommandType cmdTye, string sql, params SqlParameter[] parameters)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    //换一种写法，与上面的几种写法稍稍有点不同  
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = cmdTye;
                        cmd.Parameters.AddRange(parameters);

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                            return dt;
                        }

                    }
                }
            }

            /// <summary>  
            /// 将查询的结果转换成一个T类型的对象  
            /// </summary>  
            /// <typeparam name="T">泛型类T</typeparam>  
            /// <param name="sql">要执行的sql语句</param>  
            /// <param name="parameters">sql语句的参数</param>  
            /// <returns>T类型的对象集合</returns>  
            public static List<T> ExecuteClass<T>(CommandType cmdTye, string sql, params SqlParameter[] parameters)
            {
                List<T> list = new List<T>();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = cmdTye;
                        cmd.Parameters.AddRange(parameters);

                        DataSet ds = new DataSet();
                        
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds);
                        }

                        //获取ds里面的第0个表（索引从0开始）  
                        DataTable dt = ds.Tables[0];

                        //获取T类型的所有公有属性  
                        PropertyInfo[] tMembers = typeof(T).GetProperties();

                        //遍历dt表的所有行                      
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //创建一个泛型T类型的对象t。  
                            T t = Activator.CreateInstance<T>();

                            //遍历dt表的所有列  
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                //遍历T类型的所有公有属性  
                                foreach (PropertyInfo tMember in tMembers)
                                {
                                    //如果发现dt表中的列名与遍历出来的T类型属性名相同  
                                    if (dt.Columns[j].ColumnName.ToUpper().Equals(tMember.Name.ToUpper()))
                                    {
                                        //将dt.Rows[i][j]赋值给t对象的tMember成员  
                                        if (dt.Rows[i][j] != DBNull.Value)
                                        {
                                            tMember.SetValue(t, dt.Rows[i][j], null);
                                        }
                                        else
                                        {
                                            tMember.SetValue(t, null, null);
                                        }
                                        break;//;//注意这里的break是写在if语句里面的，意思就是说如果列名和属性名称相同并且已经赋值了，那么我就跳出foreach循环，进行j+1的下次循环    
                                    }

                                }
                            }
                            list.Add(t);
                        }
                        return list;

                    }
                }
            }

            /// <summary>  
            /// 直接将查询到的数据（表）转换成一个泛型类  
            /// </summary>  
            /// <typeparam name="T">泛型类T</typeparam>  
            /// <param name="cmdText">要执行的sql语句</param>  
            /// <param name="parameters">sql语句的参数</param>  
            /// <returns>T类型的对象的集合</returns>  
            /// 

            /*
             public static List<T> SelectDataToList<T>(string cmdText, params SqlParameter[] parameters)  
             {  
                 DataTable dt = new DataTable();  
                 using (SqlConnection conn = new SqlConnection(connStr))  
                 {  
                     conn.Open();  
                     using (SqlCommand cmd = conn.CreateCommand())  
                     {  
                         cmd.CommandText = cmdText;  
                         cmd.Parameters.AddRange(parameters);  

                         using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))  
                         {  

                             adapter.Fill(dt);  
                         }  
                     }  
                 }  

                 IList<T> list = new List<T>(); //实例化一个泛型类  

                 var tMembers = typeof(T).GetProperties(); //获取T类的所有公共属性  

                 for (int i = 0; i < dt.Rows.Count; i++) //遍历dt表的行  
                 {  
                     T t = Activator.CreateInstance<T>();//创建T类型的实例  

                     for (int j = 0; j < dt.Columns.Count; j++) //遍历dt表的列  
                     {  
                         foreach (var tMember in tMembers) //遍历T类的公共方法  
                         {  
                             if (tMember.Name.ToUpper() == dt.Columns[j].ColumnName.ToUpper()) //如果属性的名字与列的名字一致，则赋值  
                             {  
                                 if (dt.Rows[i][j] != null)  
                                 {  
                                     tMember.SetValue(t, dt.Rows[i][j], null);  
                                 }  
                                 else  
                                 {  
                                     tMember.SetValue(t, null, null);  
                                 }  
                                 break;  
                             }  
                         }  
                     }  
                     list.Add(t);  
                 }  
                 return list.ToList();  

             }  
        */
            /* 
            public static object ExecuteDataReader(string sql, params SqlParameter[] paramters) 
            { 
                using (SqlConnection conn = new SqlConnection(connStr)) 
                { 
                    conn.Open(); 
                    using(SqlCommand cmd=new SqlCommand(sql,conn)) 
                    { 
                        cmd.Parameters.AddRange(paramters); 
                        using (SqlDataReader dr = cmd.ExecuteReader()) 
                        { 
                            if (dr.Read()==true) 
                            {  

                            } 
                        } 
                    } 
                } 
            } 
            */
            /// <summary>  
            /// 从数据库取值赋给.net类的对象属性，如果值为DBNull.Value,则将值转换成.Net认的null  
            /// </summary>  
            /// <param name="value"></param>  
            /// <returns></returns>  
            public static object FromDbValue(object value)
            {
                if (value == DBNull.Value)
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }

            /// <summary>  
            /// 将.net类对象的属性值插入到数据库，如果类的属性值为null，则将值转换成数据库认的DBNull.value  
            /// </summary>  
            /// <param name="value"></param>  
            /// <returns></returns>  
            public static object ToDbValue(object value)
            {
                if (value == null)
                {
                    return DBNull.Value;
                }
                else
                {
                    return value;
                }

            }
        }

    }
}