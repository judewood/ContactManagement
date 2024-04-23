namespace ContactManagement.Models;
public readonly record struct Profile(string FirstName, string LastName, string Phone, string Email, bool IsValidEmail = false, bool IsValidPhone = false)
{
    public string FullName => FirstName + " " + LastName;
}
