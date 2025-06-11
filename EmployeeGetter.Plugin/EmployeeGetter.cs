using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using System.Net.Http;

namespace EmployeeGetter.Plugin
{
    [Author(Name = "Mikhail Marasanov")]
    public class EmployeeGetter : IPluggable
    {
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var client = new HttpClient();
            
            var json = client.GetAsync("https://jsonplaceholder.typicode.com/users").GetAwaiter().GetResult();
            var content = json.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var users = JsonConvert.DeserializeObject<List<UserSchema>>(content);
            var employees = new List<EmployeesDTO>();
            
            foreach (var user in users!)
            {
                var employee = new EmployeesDTO {Name = user.Name};
                employee.AddPhone(user.Phone);
                employees.Add(employee);
                Console.WriteLine(employee.ToJson());
            }

            return args.Concat(employees);
        }
    }
}
