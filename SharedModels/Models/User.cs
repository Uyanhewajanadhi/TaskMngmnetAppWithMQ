using System.ComponentModel.DataAnnotations;

namespace UserService.Database
{
    public class User
    {
        [Key]
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
    }
}
