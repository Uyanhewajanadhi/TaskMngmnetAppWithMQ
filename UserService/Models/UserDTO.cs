namespace UserService.Models
{
    public class UserDTO
    {
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }

        public string Token { get; set; }
    }
}
