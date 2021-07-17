using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TaskForms
{
    class DBHandler : IDisposable
    {
        public static readonly string ConnectionString = "server=localhost;Database=taskforms;Uid=root;Pwd=root";
        MySqlConnection conn;

        public DBHandler()
        {
            conn = new MySqlConnection(ConnectionString);
        }

        public void Dispose()
        {
            conn.Dispose();
        }

        public DataTable GetAllCategories()
        {
            if(conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(DBQueries.GET_ALL_CATEGORIES,conn);
            MySqlDataAdapter adt = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adt.Fill(dt);
            return dt;
        }

        public DataTable GetAllProducts()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(DBQueries.GET_ALL_PRODUCTS, conn);
            MySqlDataAdapter adt = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adt.Fill(dt);
            return dt;
        }

        public void AddCategory(string categoryName)
        {
            if(conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format(DBQueries.ADD_CATEGORY,categoryName), conn);
            var res = cmd.ExecuteNonQuery();

        }

        public void EditCategory(int catId, string categoryName)
        {
            if(conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format(DBQueries.UPDATE_CATEGORY, catId, categoryName), conn);
            var res = cmd.ExecuteNonQuery();
        }

        public void ValidateCategoryReference(int categoryId)
        {
            if(conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format(DBQueries.VALIDATE_CATEGORY_BEFORE_DELETE, categoryId), conn);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                throw new Exception("Category Reference Exist in Products");
        }

        public void DeleteCategory(int categoryId)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format(DBQueries.DELETE_CATEGORY, categoryId), conn);
            var res = cmd.ExecuteNonQuery();
        }

        public void AddProduct(string productName, int categoryId)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format(DBQueries.ADD_PRODUCT, productName, categoryId), conn);
            var res = cmd.ExecuteNonQuery();
        }

        public void EditProduct(int productId, string productName, int categoryId)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format(DBQueries.UPDATE_PRODUCT, productId, productName, categoryId), conn);
            var res = cmd.ExecuteNonQuery();
        }

        public void DeleteProduct(int productId)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(string.Format(DBQueries.DELETE_PRODUCT, productId), conn);
            var res = cmd.ExecuteNonQuery();
        }
    }

    static class DBQueries
    {
        public static readonly string GET_ALL_PRODUCTS = "SELECT PId, PName, CName FROM product JOIN category ON product.CId = category.CID ORDER BY PId";
        public static readonly string ADD_PRODUCT = "INSERT INTO product(PName,CId) VALUES ('{0}',{1})";
        public static readonly string UPDATE_PRODUCT = "UPDATE product SET PName = '{1}', CId = {2} WHERE PId = {0}";
        public static readonly string DELETE_PRODUCT = "DELETE FROM product WHERE PId = {0}";

        public static readonly string GET_ALL_CATEGORIES = "SELECT CID, CName FROM category ORDER BY CID";
        public static readonly string ADD_CATEGORY = "INSERT INTO category(CName) VALUES ('{0}')";
        public static readonly string UPDATE_CATEGORY = "UPDATE category SET CName = '{1}' WHERE CID = {0}";
        public static readonly string VALIDATE_CATEGORY_BEFORE_DELETE = "SELECT COUNT(*) FROM product WHERE CId = {0}";
        public static readonly string DELETE_CATEGORY = "DELETE FROM category WHERE CId = {0}";
    }
}
