using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoMVCProjectJquery.Models;
using System.IO;
using System.Data.Entity;

namespace DemoMVCProjectJquery.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        //GoogleMap
        public ActionResult GoogleMap()
        {
            return View();
        }

        //List
        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }

        //GetAllEmployee
        IEnumerable<EmployeeDB> GetAllEmployee()
        {
            using (EmployeeDBEntities db = new EmployeeDBEntities())
            {
                return db.EmployeeDBs.ToList<EmployeeDB>();
            }
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            EmployeeDB emp = new EmployeeDB();
            if (id != 0)
            {
                using (EmployeeDBEntities db = new EmployeeDBEntities())
                {
                    emp = db.EmployeeDBs.Where(x => x.EmployeeID == id).FirstOrDefault<EmployeeDB>();
                }
            }
            return View(emp);
        }
        [HttpPost]
        public ActionResult AddOrEdit(EmployeeDB emp)
        {
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/AppFiles/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (EmployeeDBEntities db = new EmployeeDBEntities())
                {
                    if (emp.EmployeeID == 0)
                    {
                        db.EmployeeDBs.Add(emp);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities db = new EmployeeDBEntities())
                {
                    EmployeeDB emp = db.EmployeeDBs.Where(x => x.EmployeeID == id).FirstOrDefault<EmployeeDB>();
                    db.EmployeeDBs.Remove(emp);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}