using ContactManagement.Models;

namespace ContactManagement.Data;
public interface IContactDataService
{
    public Task<bool> AddContact(Contact contact);

    public Task<(IEnumerable<Contact> contacts, string error)> SearchContactsByName(string searchString);

    public Task<(IEnumerable<Contact> contacts, string error)> GetContacts();

    Task<bool> DeleteContactById(string id);

    Task<bool> UpdateContact(Contact contact);

    Task<Contact?> GetContactById(string id);
}
