namespace TataAppMac.ViewModels
{
    using TataAppMac.Models;

	public class EmployeeDetailViewModel : Employee
    {
        Employee employee;

        public EmployeeDetailViewModel(Employee employee)
        {
            this.employee = employee;

            FirstName = employee.FirstName;
            LastName = employee.LastName;
            EmployeeCode = employee.EmployeeCode;
            DocumentTypeId = employee.DocumentTypeId;
			LoginTypeId = employee.LoginTypeId;
            Document = employee.Document;
			Picture = employee.Picture;
            Email = employee.Email;
            Phone = employee.Phone;
            Address = employee.Address;
        }
    }
}