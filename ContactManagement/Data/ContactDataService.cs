using ContactManagement.Models;
using ContactManagement.Repository;
using static System.String;


namespace ContactManagement.Data;

internal class ContactDataService : IContactDataService
{
    public async Task<bool> AddContact(Contact contact)
    {
        var storedContact = contact with { Id = Guid.NewGuid().ToString() };
        //force our dummy database to behave asynchronously 
        var task = Task.Run(() => ContactsStore.AddContact(storedContact));
        // Real Data store could return error but our in memory one doesn't. 
        // Allowing return of error allows us to test business logic handling of this scenario
        await task;
        return true;
    }

    public async Task<(IEnumerable<Contact> contacts, string error)> SearchContactsByName(string searchString)
    {
        //var task = Task.Run(ContactsStore.GetContacts);
        var contacts = await Task.Run(ContactsStore.GetContacts);
        var matches = contacts.Where(contact => contact.Profile.FullName.ToLowerInvariant().Contains(searchString))
            .OrderBy(contact => contact.Profile.LastName);

        return (matches, Empty);
    }

    public async Task<bool> DeleteContactById(string id)
    {
        var task = Task.Run(() => ContactsStore.GetContactById(id));
        var result = await task;
        if (result == null)
        {
            return false;
        }
        ContactsStore.DeleteContact(id);
        return true;
    }

    public async Task<bool> UpdateContact(Contact contact)
    {
        var task = Task.Run(() => ContactsStore.GetContactById(contact.Id));
        var result = await task;
        if (result == null)
        {
            return false;
        }
        await Task.Run(() => ContactsStore.DeleteContact(contact.Id));
        await Task.Run(() => ContactsStore.AddContact(contact));
        return true;
    }

    public async Task<Contact?> GetContactById(string id)
    {
        var task = Task.Run(() => ContactsStore.GetContactById(id));
        return await task.ConfigureAwait(false);
    }
}
