using AptechShoseShop.Models.Entites;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace AptechShoseShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CategoriesController : Controller
    {
        private readonly AptechShoseShopDbContext db = new AptechShoseShopDbContext();

        // GET: Admin/Categories
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Categories.OrderBy(x => x.Position).ToList());
        }

        [HttpGet]
        public ActionResult CreateCate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCate(Category cate)
        {
            //if (cate.Position >= 0)
            //{

            //};

            //Category newCate = new Category()
            //{
            //    CategoryName = cate.CategoryName,
            //    Position = cate.Position
            //};
            //db.Categories.Add(newCate);
            //db.SaveChanges();

            string connect = System.Configuration.ConfigurationManager.ConnectionStrings["AptechShoseShop"].ConnectionString;

            using (var connection = new SqlConnection(connect))
            {
                connection.Open();

                int Id = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $@"INSERT INTO Categories {Id}
                                    (
                                    CategoryName,
                                    Position
                                    )
                                    VALUES
                                    (
                                    @CategoryName, 
                                    @Position
                                    )";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@CategoryName", cate.CategoryName);
                    cmd.Parameters.AddWithValue("@Position", cate.Position);
                    Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult _EditCate(int id)
        {
            var cate = db.Categories.Find(id);
            return PartialView(cate);
        }

        [HttpPost]
        public ActionResult EditCate(int id, string categoryName, int position)
        {
            var cate = db.Categories.Find(id);
            cate.CategoryName = categoryName;
            cate.Position = position;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCate(int id)
        {
            Category t = db.Categories.Find(id);
            db.Categories.Remove(t);
            db.SaveChanges();
            return Content("OK");
        }
    }
}
