using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace EmployeeManagement.Models
{
    public  interface IemployeeRepo
    {
        public employee getemployee(int id);
        public  IEnumerable<employee> GetAllEmployee();

        public employee Addemp(employee emp);

        public employee Update(employee emp);

        public employee Delete(int id);


    }
}