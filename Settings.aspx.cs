using prjLibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjLibrarySystem
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null) { Response.Redirect("Login.aspx"); return; }
            string role = Session["Role"]?.ToString();
            if (role != "Admin") { Response.Redirect("MemberDashboard.aspx"); return; }

            litSidebar.Text = SidebarHelper.GetSidebar(role, "settings");

            if (!IsPostBack)
            {
                LoadCategories();
                LoadYearLevel();
                LoadCourses();
            }
        }

        // ================= CATEGORY =================

        void LoadCategories()
        {
            gvCategories.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM tblCategories", null);
            gvCategories.DataBind();
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO tblCategories(CategoryName, IsActive) VALUES(@name,1)",
                new SqlParameter[] { new SqlParameter("@name", txtCategory.Text) });

            txtCategory.Text = "";
            LoadCategories();
        }

        protected void btnToggleCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((System.Web.UI.WebControls.Button)sender).CommandArgument);

            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE tblCategories
                SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                WHERE CategoryID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            LoadCategories();
        }

        protected void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((System.Web.UI.WebControls.Button)sender).CommandArgument);

            DataTable dt = DatabaseHelper.ExecuteQuery(
                "SELECT COUNT(*) FROM tblBooks WHERE CategoryID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                Response.Write("<script>alert('Category in use!');</script>");
                return;
            }

            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM tblCategories WHERE CategoryID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            LoadCategories();
        }

        // ================= YEAR LEVEL =================

        void LoadYearLevel()
        {
            gvYearLevel.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM tblYearLevels", null);
            gvYearLevel.DataBind();
        }

        protected void btnAddYear_Click(object sender, EventArgs e)
        {
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO tblYearLevels(YearLevelName, IsActive) VALUES(@name,1)",
                new SqlParameter[] { new SqlParameter("@name", txtYearLevel.Text) });

            txtYearLevel.Text = "";
            LoadYearLevel();
        }

        protected void btnToggleYear_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((System.Web.UI.WebControls.Button)sender).CommandArgument);

            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE tblYearLevels
                SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                WHERE YearLevelID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            LoadYearLevel();
        }

        protected void btnDeleteYear_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((System.Web.UI.WebControls.Button)sender).CommandArgument);

            DataTable dt = DatabaseHelper.ExecuteQuery(
                "SELECT COUNT(*) FROM tblMembers WHERE YearLevelID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                Response.Write("<script>alert('Year Level in use!');</script>");
                return;
            }

            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM tblYearLevels WHERE YearLevelID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            LoadYearLevel();
        }

        // ================= COURSE =================

        void LoadCourses()
        {
            gvCourse.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM tblCourses", null);
            gvCourse.DataBind();
        }

        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            DatabaseHelper.ExecuteNonQuery(
                "INSERT INTO tblCourses(CourseName, IsActive) VALUES(@name,1)",
                new SqlParameter[] { new SqlParameter("@name", txtCourse.Text) });

            txtCourse.Text = "";
            LoadCourses();
        }

        protected void btnToggleCourse_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((System.Web.UI.WebControls.Button)sender).CommandArgument);

            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE tblCourses
                SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
                WHERE CourseID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            LoadCourses();
        }

        protected void btnDeleteCourse_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(((System.Web.UI.WebControls.Button)sender).CommandArgument);

            DataTable dt = DatabaseHelper.ExecuteQuery(
                "SELECT COUNT(*) FROM tblMembers WHERE CourseID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                Response.Write("<script>alert('Course in use!');</script>");
                return;
            }

            DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM tblCourses WHERE CourseID=@id",
                new SqlParameter[] { new SqlParameter("@id", id) });

            LoadCourses();
        }
    }
}