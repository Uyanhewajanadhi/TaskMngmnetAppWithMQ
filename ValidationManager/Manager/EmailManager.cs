using System.Text.RegularExpressions;

namespace ProjectForAssignment.Manager
{
    public class EmailManager
    {
        public static bool EmailValidation(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@mens((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }

            return false;
        }
    }
}
