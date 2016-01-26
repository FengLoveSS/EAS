using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace WebApplication1.asp.net
{
    /// <summary>
    /// Student_List 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://JEFFERY.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Student_List : System.Web.Services.WebService
    {
        #region 学生
        /*
         *********************************************************************************************************
         *********************************************************************************************************
         Student
         *********************************************************************************************************
         *********************************************************************************************************
         */

        [WebMethod]
        //get学生列表
        public void GetStudentList()
        {

            DataSet ds = new DataSet();
            int page;


            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }

            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("Student")).ToString());//得到总共的数量
            
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "Student", "SNO");

            int index = sql.IndexOf("*");
            sql = sql.Insert(index + "*".Length, " SNO,SName,SSex,convert(varchar(10),SBrithday,120) SBrithday,SGrade,SYear,CLName");
            sql = sql.Remove(index, 1);

            index = sql.IndexOf("FROM ");
            sql = sql.Insert(index + "FROM ".Length, " Class,");

            index = sql.LastIndexOf("AS T");
            sql = sql.Insert(index + "AS T".Length, " where Class.CLNO=T.CLNO and SNO like '%" + HttpContext.Current.Request["SNO"] + "%' and SName like '%" + HttpContext.Current.Request["SName"] + "%'");
            if (HttpContext.Current.Request["DNO"] != null && HttpContext.Current.Request["DNO"] != "全部")
            {
                index = sql.IndexOf("FROM ");
                sql = sql.Insert(index + "FROM ".Length, " Department,");
                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " Department.DNO='" + HttpContext.Current.Request["DNO"] + "' and Class.DNO=Department.DNO and");
            }
            if (HttpContext.Current.Request["CLNO"] != null&&HttpContext.Current.Request["CLNO"] !="undefined" && HttpContext.Current.Request["CLNO"] != "全部")
            {

                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " T.CLNO='" + HttpContext.Current.Request["CLNO"] + "'and ");
            }
            if (HttpContext.Current.Request["Syear"] != "undefined" && HttpContext.Current.Request["Syear"] != "全部")
            {

                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " T.Syear='" + HttpContext.Current.Request["Syear"] + "'and ");
            }
            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            int i = ds.Tables[0].Rows.Count;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetStudentMaxPage()//取最大的学生页码
        {
            string sql = "select count(*) FROM Student where SNO like '%" + HttpContext.Current.Request["SNO"] + "%' and SName like '%" + HttpContext.Current.Request["SName"] + "%'";
            if (HttpContext.Current.Request["CLNO"] != "全部" && HttpContext.Current.Request["CLNO"] != null)
            {
                sql = sql + " and CLNO='" + HttpContext.Current.Request["CLNO"] + "'";
            }
            if (HttpContext.Current.Request["DNO"] != "全部" && HttpContext.Current.Request["DNO"] != null)
            {
                int index = sql.IndexOf("FROM ");
                sql = sql.Insert(index + "FROM ".Length, " Department,Class,");
                sql = sql + " and Department.DNO='" + HttpContext.Current.Request["DNO"] + "' and Department.DNO=Class.DNO and Class.CLNO=Student.CLNO";
            }
            if (HttpContext.Current.Request["SYear"] != "全部" && HttpContext.Current.Request["SYear"] != "undefined")
            {
                sql = sql + " and SYear='" + HttpContext.Current.Request["SYear"] + "'";
            }
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(sql).ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void NewStudent()//添加学生信息
        {
            string error = "成功";
            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("insert into Student (SNO,SName,SSex,SBrithday,SGrade,SYear,CLNO) values('" + "S" + HttpContext.Current.Request["SNO"] + "','" + HttpContext.Current.Request["SName"] + "','" + HttpContext.Current.Request["SSex"] + "','" + HttpContext.Current.Request["SBrithday"] + "','" + HttpContext.Current.Request["SGrade"] + "','" + HttpContext.Current.Request["SYear"] + "','" + HttpContext.Current.Request["CLNO"] + "')");


            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        [WebMethod]
        public void GetSingle_Student()//获得某一个学生的数据
        {

            DataSet ds = (DataSet)DTcms.DBUtility.DbHelperSQL.Query("select * from Student where SNO='" + HttpContext.Current.Request["SNO"] + "'");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetAllSYearList()//获取所有的入学年份
        {
            DataSet ds = (DataSet)DTcms.DBUtility.DbHelperSQL.Query("select distinct SYear from Student");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void UpdateStudent()//更新Student
        {
            string error = "成功";


            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("update Student set SName='" + HttpContext.Current.Request["SName"] + "',SSex='" + HttpContext.Current.Request["SSex"] + "',SBrithday='" + HttpContext.Current.Request["SBrithday"] + "',SGrade='" + HttpContext.Current.Request["SGrade"] + "',SYear='" + HttpContext.Current.Request["SYear"] + "',CLNO='" + HttpContext.Current.Request["CLNO"] + "' where SNO='" + HttpContext.Current.Request["SNO"] + "'");
                
            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }

        }
        [WebMethod]
        public void FindStudent_SNO()//查找是否存在该学生号的数据
        {
            if (int.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from Student where SNO='S" + HttpContext.Current.Request["SNO"] + "'").ToString()) >= 1)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("true");
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("false");
                HttpContext.Current.Response.End();
            }
        }
        [WebMethod]
        public void DeleteStudent()//删除某一条学生信息
        {
            string error = "成功";
            try
            {
                DTcms.DBUtility.DbHelperSQL.ExecuteSql("delete Student where SNO='" + HttpContext.Current.Request["SNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        
        #endregion
        #region 系
        /*
         *********************************************************************************************************
         *********************************************************************************************************
         Department
         *********************************************************************************************************
         *********************************************************************************************************
         */
        [WebMethod]
        //get系表
        public void GetDepartmentList()
        {
            DataSet ds = new DataSet();
            int page;

            
            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }
            
            
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("Department")).ToString());//得到总共的数量
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "Department", "DNO");
            int index = sql.LastIndexOf("AS T");

            sql = sql.Insert(index + "AS T".Length, " where T.DNO Like '%" + HttpContext.Current.Request["DNO"] + "%' and T.DName Like '%" + HttpContext.Current.Request["DName"] + "%'");//构造SQL语言
            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetDepartmentMaxPage()//取最大的系页码
        {
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from Department where DNO like '%" + HttpContext.Current.Request["DNO"] + "%' and DName like '%" + HttpContext.Current.Request["DName"]+"%'").ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        //get所有系表
        public void GetAllDepartmentList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DTcms.DBUtility.DbHelperSQL.Query("select * from Department");
                
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
                HttpContext.Current.Response.End();
            }
            catch
            {
                
            }

            
        }
        [WebMethod]
        public void NewDepartment()//新建Department系
        {
            string error = "成功";


            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("insert into Department (DNO,DName,DIntroduction) values('" + "D" + HttpContext.Current.Request["DNO"] + "','" + HttpContext.Current.Request["DName"] + "','" + HttpContext.Current.Request["DIntroduction"] + "')");
                
            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
            
        }
        [WebMethod]
        public void UpdateDepartment()//更新Department系
        {
            string error = "成功";


            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("update Department set DName='" + HttpContext.Current.Request["DName"] + "',DIntroduction='" + HttpContext.Current.Request["DIntroduction"] + "' where DNO='" + HttpContext.Current.Request["DNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }

        }
        [WebMethod]
        public void DeleteDepartment()//删除某一条系的数据
        {
            string error="成功";
            try
            {
                DTcms.DBUtility.DbHelperSQL.ExecuteSql("delete Department where DNO='" + HttpContext.Current.Request["DNO"] + "'");
                
            }
            catch(Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
            
        }
        [WebMethod]
        public void FindDepartment_DNO()//查找是否存在系的信息
        {
            if (int.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from Department where DNO='D" + HttpContext.Current.Request["DNO"] + "'").ToString())>=1)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("true");
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("false");
                HttpContext.Current.Response.End();
            }
        }
        [WebMethod]
        public void GetSingle_Department()//获得某一个系的数据
        {

            DataSet ds = (DataSet)DTcms.DBUtility.DbHelperSQL.Query("select * from Department where DNO='" + HttpContext.Current.Request["DNO"]+"'");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
#endregion
        #region 班级
        /*
         *********************************************************************************************************
         *********************************************************************************************************
         Class
         *********************************************************************************************************
         *********************************************************************************************************
         */
        [WebMethod]
        //get班级表
        public void GetClassList()
        {
            DataSet ds = new DataSet();
            int page;

            
            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }
            
            
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("Class")).ToString());//得到总共的数量
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "Class", "CLNO");
           
            int index = sql.IndexOf("*");
            sql = sql.Insert(index + "*".Length, " CLNO,CLName,DName");
            sql=sql.Remove(index, 1);
            
            index = sql.IndexOf("FROM ");
            sql = sql.Insert(index + "FROM ".Length, " Department,");
            
            index = sql.LastIndexOf("AS T");
            sql = sql.Insert(index + "AS T".Length, " where Department.DNO=T.DNO and CLNO like '%" + HttpContext.Current.Request["CLNO"] + "%' and CLName like '%" + HttpContext.Current.Request["CLName"]+"%'");
            if (HttpContext.Current.Request["DNO"] != null && HttpContext.Current.Request["DNO"] != "全部")
            {
                
                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " Department.DNO='" + HttpContext.Current.Request["DNO"] + "'and ");
            }
            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            int i=ds.Tables[0].Rows.Count;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetClassMaxPage()//取最大的班级页码
        {
            string sql = "select count(*) from Class where CLNO like '%" + HttpContext.Current.Request["CLNO"] + "%' and CLName like '%" + HttpContext.Current.Request["CLName"] + "%'";
            if (HttpContext.Current.Request["DNO"] != "全部")
            {
                sql = sql + " and DNO='" + HttpContext.Current.Request["DNO"]+"'";
            }
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(sql).ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void NewClass()//添加班级信息
        {
            string error = "成功";
            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("insert into Class (CLNO,CLName,DNO) values('" + "CL" + HttpContext.Current.Request["CLNO"] + "','" + HttpContext.Current.Request["CLName"] + "','" + HttpContext.Current.Request["DNO"] + "')");

                
            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        [WebMethod]
        public void UpdateClass()//更新Class
        {
            string error = "成功";


            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("update Class set CLName='" + HttpContext.Current.Request["CLName"] + "',DNO='" + HttpContext.Current.Request["DNO"] + "' where CLNO='" + HttpContext.Current.Request["CLNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }

        }
        [WebMethod]
        public void DeleteClass()//删除某一条班级信息
        {
            string error = "成功";
            try
            {
                DTcms.DBUtility.DbHelperSQL.ExecuteSql("delete Class where CLNO='" + HttpContext.Current.Request["CLNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        [WebMethod]
        public void FindClass_CLNO()//查找是否存在该班级号的数据
        {
            if (int.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from Class where CLNO='CL" + HttpContext.Current.Request["CLNO"] + "'").ToString()) >= 1)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("true");
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("false");
                HttpContext.Current.Response.End();
            }
        }
        [WebMethod]
        public void GetSingle_Class()//获得某一个班的数据
        {

            DataSet ds = (DataSet)DTcms.DBUtility.DbHelperSQL.Query("select * from Class where CLNO='" + HttpContext.Current.Request["CLNO"] + "'");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        //get所有班级表
        public void GetAllClassList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DTcms.DBUtility.DbHelperSQL.Query("select * from Class");

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
                HttpContext.Current.Response.End();
            }
            catch
            {

            }


        }
#endregion
        #region 教师
        [WebMethod]
        //get教师列表
        public void GetTeacherList()
        {

            DataSet ds = new DataSet();
            int page;


            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }


            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("Teacher")).ToString());//得到总共的数量
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "Teacher", "TNO");

            int index = sql.IndexOf("*");
            sql = sql.Insert(index + "*".Length, " TNO,TName,TSex,convert(varchar(10),TBrithday,120) TBrithday,DName,TTitle,TIntroduction");
            sql = sql.Remove(index, 1);

            index = sql.IndexOf("FROM ");
            sql = sql.Insert(index + "FROM ".Length, " Department,");

            index = sql.LastIndexOf("AS T");
            sql = sql.Insert(index + "AS T".Length, " where Department.DNO=T.DNO and TNO like '%" + HttpContext.Current.Request["TNO"] + "%' and TName like '%" + HttpContext.Current.Request["TName"]+"%'");
            if (HttpContext.Current.Request["DNO"] != null && HttpContext.Current.Request["DNO"] != "全部")
            {

                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " Department.DNO='" + HttpContext.Current.Request["DNO"] + "'and ");
            }
            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            int i = ds.Tables[0].Rows.Count;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetTeacherMaxPage()//取最大的教师页码
        {
            string sql = "select count(*) from Teacher where TNO like '%" + HttpContext.Current.Request["TNO"] + "%' and TName like '%" + HttpContext.Current.Request["TName"] + "%'";
            if (HttpContext.Current.Request["DNO"] != "全部")
            {
                sql = sql + " and DNO='" + HttpContext.Current.Request["DNO"] + "'";
            }
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(sql).ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetSingle_Teacher()//获得某一个教师的数据
        {

            DataSet ds = (DataSet)DTcms.DBUtility.DbHelperSQL.Query("select * from Teacher where TNO='" + HttpContext.Current.Request["TNO"] + "'");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void UpdateTeacher()//更新Teacher
        {
            string error = "成功";


            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("update Teacher set TName='" + HttpContext.Current.Request["TName"] + "',TSex='" + HttpContext.Current.Request["TSex"] + "',TBrithday='" + HttpContext.Current.Request["TBrithday"] + "',DNO='" + HttpContext.Current.Request["DNO"] + "',TTitle='" + HttpContext.Current.Request["TTitle"] + "',TIntroduction='" + HttpContext.Current.Request["TIntroduction"] + "' where TNO='" + HttpContext.Current.Request["TNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }

        }
        [WebMethod]
        public void FindTeacher_TNO()//查找是否存在该教师号的数据
        {
            if (int.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from Teacher where TNO='T" + HttpContext.Current.Request["TNO"] + "'").ToString()) >= 1)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("true");
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("false");
                HttpContext.Current.Response.End();
            }
        }
        [WebMethod]
        public void DeleteTeacher()//删除某一条教师信息
        {
            string error = "成功";
            try
            {
                DTcms.DBUtility.DbHelperSQL.ExecuteSql("delete Teacher where TNO='" + HttpContext.Current.Request["TNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        [WebMethod]
        public void NewTeacher()//添加教师信息
        {
            string error = "成功";
            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("insert into Teacher (TNO,TName,TSex,TBrithday,DNO,TTitle,TIntroduction) values('" + "T" + HttpContext.Current.Request["TNO"] + "','" + HttpContext.Current.Request["TName"] + "','" + HttpContext.Current.Request["TSex"] + "','" + HttpContext.Current.Request["TBrithday"] + "','" + HttpContext.Current.Request["DNO"] + "','" + HttpContext.Current.Request["TTitle"] + "','" + HttpContext.Current.Request["TIntroduction"] + "')");


            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        [WebMethod]
        //get所有班级表
        public void GetAllTeacherList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DTcms.DBUtility.DbHelperSQL.Query("select TNO,TName,DName from Teacher,Department where Teacher.DNO=Department.DNO");

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
                HttpContext.Current.Response.End();
            }
            catch
            {

            }


        }
#endregion
        #region 课程
        /*
         *********************************************************************************************************
         *********************************************************************************************************
         Course
         *********************************************************************************************************
         *********************************************************************************************************
         */

        [WebMethod]
        //get课程列表
        public void GetCourseList()
        {

            DataSet ds = new DataSet();
            int page;


            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }


            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("Course")).ToString());//得到总共的数量
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "Course", "CNO");

            int index = sql.IndexOf("*");
            sql = sql.Insert(index + "*".Length, " CNO,CName,TName,CHours,CCredit,CTime,CSemester,CPlace,CExamTime");
            sql = sql.Remove(index, 1);

            index = sql.IndexOf("FROM ");
            sql = sql.Insert(index + "FROM ".Length, " Teacher,");

            index = sql.LastIndexOf("AS T");
            sql = sql.Insert(index + "AS T".Length, " where Teacher.TNO=T.TNO and Teacher.TName like '%" + HttpContext.Current.Request["TName"] + "%' and T.CNO like '%" + HttpContext.Current.Request["CNO"] + "%' and T.CName like '%" + HttpContext.Current.Request["CName"] + "%'");
            if (HttpContext.Current.Request["DNO"] != null && HttpContext.Current.Request["DNO"] != "全部")
            {
                index = sql.IndexOf("FROM ");
                sql = sql.Insert(index + "FROM ".Length, " Department,");
                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " Department.DNO='" + HttpContext.Current.Request["DNO"] + "' and Teacher.DNO=Department.DNO and");
            }
            
            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            int i = ds.Tables[0].Rows.Count;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetCourseMaxPage()//取最大的课程页码
        {
            string sql = "select count(*) from Course,Teacher where Course.TNO=Teacher.TNO and CNO like '%" + HttpContext.Current.Request["CNO"] + "%' and CName like '%" + HttpContext.Current.Request["CName"] + "%' and TName like '%" + HttpContext.Current.Request["TName"] + "%'";
            if (HttpContext.Current.Request["DNO"] != "全部")
            {
                sql = sql + " and DNO='" + HttpContext.Current.Request["DNO"] + "'";
            }
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(sql).ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void NewCourse()//添加课程信息
        {
            string error = "成功";
            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("insert into Course (CNO,CName,TNO,CHours,CCredit,CTime,CSemester,CPlace,CExamTime) values('" + "C" + HttpContext.Current.Request["CNO"] + "','" + HttpContext.Current.Request["CName"] + "','" + HttpContext.Current.Request["TNO"] + "'," + HttpContext.Current.Request["CHours"] + "," + HttpContext.Current.Request["CCredit"] + ",'" + HttpContext.Current.Request["CTime"] + "'," + HttpContext.Current.Request["CSemester"] + ",'" + HttpContext.Current.Request["CPlace"] + "','" + HttpContext.Current.Request["CExamTime"] + "')");


            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        [WebMethod]
        public void GetSingle_Course()//获得某一个课程的数据
        {

            DataSet ds = (DataSet)DTcms.DBUtility.DbHelperSQL.Query("select * from Course where CNO='" + HttpContext.Current.Request["CNO"] + "'");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void UpdateCourse()//更新Course
        {
            string error = "成功";


            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("update Course set CName='" + HttpContext.Current.Request["CName"] + "',TNO='" + HttpContext.Current.Request["TNO"] + "',CHours='" + HttpContext.Current.Request["CHours"] + "',CCredit='" + HttpContext.Current.Request["CCredit"] + "',CTime='" + HttpContext.Current.Request["CTime"] + "',CSemester='" + HttpContext.Current.Request["CSemester"] + "',CPlace='" + HttpContext.Current.Request["CPlace"] + "',CExamTime='" + HttpContext.Current.Request["CExamTime"] + "' where CNO='" + HttpContext.Current.Request["CNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }

        }
        [WebMethod]
        public void FindCourse_CNO()//查找是否存在该课程号的数据
        {
            if (int.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from Course where CNO='C" + HttpContext.Current.Request["CNO"] + "'").ToString()) >= 1)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("true");
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("false");
                HttpContext.Current.Response.End();
            }
        }
        [WebMethod]
        public void DeleteCourse()//删除某一条课程信息
        {
            string error = "成功";
            try
            {
                DTcms.DBUtility.DbHelperSQL.ExecuteSql("delete Course where CNO='" + HttpContext.Current.Request["CNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
       
#endregion
        #region 选课
        [WebMethod]
       public void SignUp()
        {
            
            string[] students = HttpContext.Current.Request["students"].Split(',');
            string[] courses = HttpContext.Current.Request["courses"].Split(',');
            List<string> error=new List<string>();
            int result = 0;
            for (int i = 0; i < students.Length; i++)//检测是否符合每个学期学分不超过15的要求
            {
                for (int z = 1; z <= 8; z++)
                {
                    result = int.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select isnull(sum(CCredit),0) from SC,Course where SC.CNO=Course.CNO and SC.SNO='" + students[i] + "' and Course.CSemester="+z).ToString());
                    for (int j = 0; j < courses.Length; j++)
                    {
                        object a=DTcms.DBUtility.DbHelperSQL.GetSingle("select CCredit from Course where CNO='" + courses[j] + "' and CSemester="+z);
                        if(a==null)continue;
                        else
                        result += int.Parse(a.ToString());
                    }
                    if (result > 15)
                    {
                        error.Add(students[i]+"第"+z.ToString()+"学期超出15学分");
                    }

                    result = 0;
                }

                for (int j = 0; j < courses.Length; j++)
                {
                    if (int.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from SC where SNO='" + students[i] + "' and CNO='" + courses[j] + "'").ToString())!=0)
                    {
                        error.Add(students[i] + "已选修" + courses[j]);
                    }
                }
            }
            if (error.Count != 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(String.Join(",",error));
                HttpContext.Current.Response.End();
            }
            else
            {
                for (int i = 0; i < students.Length; i++)
                {
                    for (int j = 0; j < courses.Length; j++)
                    {
                        DTcms.DBUtility.DbHelperSQL.ExecuteSql("insert into SC(SNO,CNO) values('"+students[i]+"','"+courses[j]+"')");
                    }
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("成功");
                HttpContext.Current.Response.End();
            }
        }
        [WebMethod]
        public void GetSCList()//取出选修的数据
        {

            DataSet ds = new DataSet();
            int page;


            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }


            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("SC")).ToString());//得到总共的数量
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "SC", "CNO ASC,SCAScore DESC");

            int index = sql.IndexOf("*");
            sql = sql.Insert(index + "*".Length, " Student.SNO,Course.CNO,SName,TName,CName,SCUScore,SCEScore,SCAScore");
            sql = sql.Remove(index, 1);

            index = sql.IndexOf("FROM ");
            sql = sql.Insert(index + "FROM ".Length, " Teacher,Course,Student,");

            index = sql.LastIndexOf("AS T");
            sql = sql.Insert(index + "AS T".Length, " where Teacher.TNO=Course.TNO and Course.CNO=T.CNO and Student.SNO=T.SNO and T.SNO Like '%" + HttpContext.Current.Request["SNO"] + "%' and T.CNO Like '%" + HttpContext.Current.Request["CNO"]+"%'");
            
            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            int i = ds.Tables[0].Rows.Count;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetSCMaxPage()//取最大的选修页码
        {
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle("select count(*) from SC where SNO Like'%" + HttpContext.Current.Request["SNO"] + "%' and CNO like '%" + HttpContext.Current.Request["CNO"] + "%'").ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        
        [WebMethod]
        public void GetSingle_SC()//获得某一个选修的数据
        {

            DataSet ds = (DataSet)DTcms.DBUtility.DbHelperSQL.Query("select * from SC where SNO='" + HttpContext.Current.Request["SNO"] + "' and CNO='"+HttpContext.Current.Request["CNO"]+"'");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void UpdateSC()//更新SC
        {
            string error = "成功";


            try
            {

                DTcms.DBUtility.DbHelperSQL.ExecuteSql("update SC set SCUScore='" + HttpContext.Current.Request["SCUScore"] + "',SCEScore='" + HttpContext.Current.Request["SCEScore"] + "',SCAScore='" + HttpContext.Current.Request["SCAScore"] + "' where SNO='" + HttpContext.Current.Request["SNO"] + "' and CNO='" + HttpContext.Current.Request["CNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }

        }
        
        [WebMethod]
        public void DeleteSC()//删除某一条选修信息
        {
            string error = "成功";
            try
            {
                DTcms.DBUtility.DbHelperSQL.ExecuteSql("delete SC where SNO='" + HttpContext.Current.Request["SNO"] + "' and CNO='" + HttpContext.Current.Request["CNO"] + "'");

            }
            catch (Exception e)
            {
                error = "失败";
            }
            finally
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(error);
                HttpContext.Current.Response.End();

            }
        }
        #endregion
        # region 登记表
        [WebMethod]
        public void GetRegisterMaxPage()
        {
            string sql = "select count(*) from Course,Teacher,SC where Course.TNO=Teacher.TNO and Course.CNO=SC.CNO and Course.CNO like '%" + HttpContext.Current.Request["CNO"] + "%' and CName like '%" + HttpContext.Current.Request["CName"] + "%' and TName like '%" + HttpContext.Current.Request["TName"] + "%'";
            if (HttpContext.Current.Request["DNO"] != "全部")
            {
                sql = sql + " and DNO='" + HttpContext.Current.Request["DNO"] + "'";
            }
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(sql).ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetRegisterList()
        {
            DataSet ds = new DataSet();
            int page;


            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }


            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("SC")).ToString());//得到总共的数量
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "SC", "CNO ASC,SNO ASC");

            int index = sql.IndexOf("*");
            sql = sql.Insert(index + "*".Length, " T.CNO,CName,TName,CHours,CCredit,CTime,CSemester,CPlace,CExamTime,T.SNO,SName,SSex,SCUScore,SCEScore,SCAScore");
            sql = sql.Remove(index, 1);

            index = sql.IndexOf("FROM ");
            sql = sql.Insert(index + "FROM ".Length, " Teacher,Course,Student,");

            index = sql.LastIndexOf("AS T");
            sql = sql.Insert(index + "AS T".Length, " where T.SNO=Student.SNO and T.CNO=Course.CNO and Teacher.TNO=Course.TNO and Teacher.TName like '%" + HttpContext.Current.Request["TName"] + "%' and T.CNO like '%" + HttpContext.Current.Request["CNO"] + "%' and Course.CName like '%" + HttpContext.Current.Request["CName"] + "%'");
            if (HttpContext.Current.Request["DNO"] != null && HttpContext.Current.Request["DNO"] != "全部")
            {
                index = sql.IndexOf("FROM ");
                sql = sql.Insert(index + "FROM ".Length, " Department,");
                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " Department.DNO='" + HttpContext.Current.Request["DNO"] + "' and Teacher.DNO=Department.DNO and");
            }

            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            int i = ds.Tables[0].Rows.Count;
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        #endregion
        #region 报表
        [WebMethod]
        public void GetReportMaxPage()
        {
            string sql = "select count(*) from Course,Teacher where Course.TNO=Teacher.TNO  and Course.CNO like '%" + HttpContext.Current.Request["CNO"] + "%' and CName like '%" + HttpContext.Current.Request["CName"] + "%' and TName like '%" + HttpContext.Current.Request["TName"] + "%'";
            if (HttpContext.Current.Request["DNO"] != "全部")
            {
                sql = sql + " and DNO='" + HttpContext.Current.Request["DNO"] + "'";
            }
            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(sql).ToString());//得到总共的数量
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(all);
            HttpContext.Current.Response.End();
        }
        [WebMethod]
        public void GetReportList()
        {
            DataSet ds = new DataSet();
            int page;


            if (HttpContext.Current.Request["page"] == null)
            {
                page = 0;
            }
            else
            {
                page = Int16.Parse(HttpContext.Current.Request["page"]);
            }


            int all = Int16.Parse(DTcms.DBUtility.DbHelperSQL.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql("Course")).ToString());//得到总共的数量
            string sql = DTcms.Common.PagingHelper.CreatePagingSql(all, 10, page, "Course", "T.CNO ASC");//组装SQL语句

            int index = sql.IndexOf("*");
            sql = sql.Insert(index + "*".Length, " T.CNO,CName,TName,Num_90,Num_80,Num70,Num_60,Num_Fail");
            sql = sql.Remove(index, 1);

            index = sql.IndexOf("FROM ");
            sql = sql.Insert(index + "FROM ".Length, " Teacher,tongji,");

            index = sql.LastIndexOf("AS T");
            sql = sql.Insert(index + "AS T".Length, " where  Teacher.TNO=T.TNO and Teacher.TName like '%" + HttpContext.Current.Request["TName"] + "%' and T.CNO like '%" + HttpContext.Current.Request["CNO"] + "%' and T.CName like '%" + HttpContext.Current.Request["CName"] + "%' and tongji.CNO=T.CNO");
            if (HttpContext.Current.Request["DNO"] != "undefined" && HttpContext.Current.Request["DNO"] != "全部")
            {
                index = sql.IndexOf("FROM ");
                sql = sql.Insert(index + "FROM ".Length, " Department,");
                index = sql.LastIndexOf("where");
                sql = sql.Insert(index + "where".Length, " Department.DNO='" + HttpContext.Current.Request["DNO"] + "' and Teacher.DNO=Department.DNO and");
            }

            ds = DTcms.DBUtility.DbHelperSQL.Query(sql);//获得分页列表
            int i = ds.Tables[0].Rows.Count;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(To_Json.ToJson(ds.Tables[0]));
            HttpContext.Current.Response.End();
        }
        #endregion
    }
}
