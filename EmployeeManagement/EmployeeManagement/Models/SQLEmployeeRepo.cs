using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepo : IemployeeRepo
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepo(AppDbContext context)
        {
            this.context = context;
        }
        public employee Addemp(employee emp)
        {
            context.Employees.Add(emp);
            context.SaveChanges();
            return emp;
        }

        public employee Delete(int Id)
        {
            employee emp = context.Employees.Find(Id);
            if(emp !=null)
            {
                context.Employees.Remove(emp);
                context.SaveChanges();
            }
            return emp;

        }

        public IEnumerable<employee> GetAllEmployee()
        {
           return context.Employees;
        }

        public employee getemployee(int id)
        {
            throw new NotImplementedException();
        }

        public employee Update(employee emp)
        {
            throw new NotImplementedException();
        }
    }
}
