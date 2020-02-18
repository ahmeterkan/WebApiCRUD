using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        EmployeeDBEntities db = new EmployeeDBEntities();
        public IEnumerable<Employee> Get()
        {
            return db.Employees.ToList();
        }

        public HttpResponseMessage Get(int id)
        {
            Employee employee = db.Employees.FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Id'si {id} olan çalışan bulunamadı. ");
            }
            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        public HttpResponseMessage Post(Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                if (db.SaveChanges() > 0)
                {
                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + employee.Id);
                    return message;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Veri ekleme işlemi yapılamadı.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }


        public HttpResponseMessage Put(Employee employee)
        {
            try
            {
                Employee emp = db.Employees.FirstOrDefault(e => e.Id == employee.Id);

                if (emp == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee ID: " + employee.Id);
                }
                else
                {
                    emp.Name = employee.Name;
                    emp.Surname = employee.Surname;
                    emp.Salary = employee.Salary;
                    emp.Gender = employee.Gender;

                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, employee);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Güncelleme yapılamadı.");
                    }
                }


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }


        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Employee emp = db.Employees.FirstOrDefault(e => e.Id == id);
                if (emp == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Employee Id: " + id);
                }
                else
                {
                    db.Employees.Remove(emp);
                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Silme işlemi başarısız.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
