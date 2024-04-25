using ContactManagement.Models;
using static System.String;

namespace ContactManagement.Repository;

public static class ContactsStore
{
    public static List<Contact> Contacts { get; set; } = [];

    public static void AddContact(Contact contact)
    {
        Contacts = Contacts.Append(contact).ToList();
    }

    public static IEnumerable<Contact> GetContacts()
    {
        return Contacts.ToList();
    }

    public static Contact? GetContactById(string id)
    {
        var result = Contacts.FirstOrDefault(p => p.Id == id);
        return IsNullOrEmpty(result.Id) ? null : result;
    }

    public static void DeleteContact(string id)
    {
        Contacts = Contacts.Where(p => p.Id != id).ToList();
    }
}