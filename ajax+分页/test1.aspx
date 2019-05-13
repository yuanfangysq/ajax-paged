<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test1.aspx.cs" Inherits="ajax_分页.test1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">  
<head runat="server">  
    <title>使用AJAX分页</title>  
    
    
    <script src="Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <style type="text/css">  
      table{ margin:80px 500px; }  
      td{ width:50px; height:auto}  
          
    </style>  
   
    <script type="text/javascript">  
     
        
        $(function () {  
            $.get("API/GetPageData.ashx?currentPage=3", function (obj) { //假设当前页是第二页currentPage=2  
                debugger;  
                String a="123";
                var JsonData = $.parseJSON(obj);  
                alert(JsonData[1].id);
                //debugger;  
                for (var i = 0; i < JsonData.length; i++) {  
                    var data = "<tr><td >" + JsonData[i].id + "</td><td >" + JsonData[i].ProdcutName + "</td><td >" + JsonData[i].ProductCodde + "</td><td >" + JsonData[i].Rates + "</td></tr>";
                    $("#t1").append(data);  
                    

                 
  
                }  
  
            })  
        })  
    </script> 
    
     
</head> 
    
     
<body>  
 <table border="1" cellpadding="5" cellspacing="0" style="margin-top:100px;width:600px;" id="t1">  
    <tr><td>编号</td><td >名字</td><td >编码</td><td >价格</td>>></tr>  
 </table>  
  
 </body>  
</html>  
