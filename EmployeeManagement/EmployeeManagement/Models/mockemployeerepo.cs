using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class mockemployeerepo : IemployeeRepo
    {

        private List<employee> _employeelist;

        public mockemployeerepo()
        {
            _employeelist = new List<employee>()
            {
                new employee() {Id = 1, Name = "marry", Department = Dept.HR , Email = "pandit@gmail"},
                new employee() {Id = 2, Name = "sam", Department = Dept.IT , Email = "idnddd@gmail"}
            };
        }

        public employee getemployee(int id)
        {
            return _employeelist.FirstOrDefault(x => x.Id == id);
        }

        public  IEnumerable<employee> GetAllEmployee()
        {
            return _employeelist;
        }

        public employee Addemp(employee emp)
        {
            emp.Id = _employeelist.Max(e => e.Id) + 1;
            _employeelist.Add(emp);
            return emp;
        }

        public employee Update(employee employeeChanges)
        {
            employee emp = _employeelist.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (emp != null)
            {
                emp.Name = employeeChanges.Name;
                emp.Email = employeeChanges.Email;
                emp.Department = employeeChanges.Department;



            }
            return emp;
        }

        public employee Delete(int Id)
        {
            employee emp = _employeelist.FirstOrDefault(e => e.Id == Id);
            if(emp !=  null)
            {
                _employeelist.Remove(emp);

            }
            return emp;
        }

        // IEnumerable<employee> GetAllemployee()
        // {
        //     return _employeelist;
        // }
        
        
        
    }
}


