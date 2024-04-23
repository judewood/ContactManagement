using ContactManagement.Extensions;
using ContactManagement.Models;
using static System.Console;
using static System.String;

namespace ContactManagement.Service;
internal class UserInterfaceService
{
    public string GetSelection()
    {
        WriteLine();
        WriteLine("Enter 'A' to add contact, 'S' to search contacts, 'E' to Edit contact, 'D' to delete contact, 'H' for help or 'Q' to quit");
        return ReadLine().NullToEmpty().Trim().ToUpperInvariant();
    }

    public Contact GetEnteredContact()
    {
        return new Contact(GetEnteredProfile(), GetEnteredAddress());
    }

    public Contact GetUpdatedContact(Contact contact)
    {
        var profile = GetEnteredProfile();
        var existingProfile = contact.Profile;
        Profile updatedProfile = new(GetPreferred(profile.FirstName, existingProfile.FirstName),
            GetPreferred(profile.LastName, existingProfile.LastName),
            GetPreferred(profile.Phone, existingProfile.Phone),
            GetPreferred(profile.Email, existingProfile.Email)
        );
        var address = GetEnteredAddress();
        var existingAddress = contact.Address;
        Address updatedAddress = new(GetPreferred(address.AddressLine1, existingAddress.AddressLine1),
            GetPreferred(address.AddressLine2, existingAddress.AddressLine2),
            GetPreferred(address.City, existingAddress.City),
            GetPreferred(address.PostCode, existingAddress.PostCode),
            GetPreferred(address.Country, existingAddress.Country)
        );
        return new Contact(updatedProfile, updatedAddress, contact.Id);
    }


    public void DisplayContact(Contact contact)
    {
        if (!IsNullOrWhiteSpace(contact.Id))
        {
            WriteLine($"Id:{contact.Id}");
        }
        WriteLine("Profile:");
        WriteLine($"Name:{contact.Profile.FirstName} {contact.Profile.LastName}");
        WriteLine($"Phone number:{contact.Profile.Phone}");
        WriteLine($"Email address:{contact.Profile.Email}");
        WriteLine();

        WriteLine("Address:");
        WriteLine($"Line 1: {contact.Address.AddressLine1}");
        WriteLine($"Line 2:{contact.Address.AddressLine2}");
        WriteLine($"Town/City: {contact.Address.City}");
        WriteLine($"PostCode: {contact.Address.PostCode}");
        WriteLine($"Country: {contact.Address.Country}");
    }

    public string GetId()
    {
        WriteLine("Enter the Id of the contact");
        return ReadLine().NullToEmpty().Trim().ToLowerInvariant();
    }

    public void DisplaySearchResults(IEnumerable<Contact> contacts)
    {
        WriteLine($"{Environment.NewLine}Matching Results...{Environment.NewLine}");
        foreach (var contact in contacts)
        {
            DisplayContact(contact);
            WriteLine($"{Environment.NewLine}{Environment.NewLine}");
        }
    }


    public string GetSearchString()
    {
        WriteLine("Enter part of contact name to search for");
        return ReadLine().NullToEmpty().Trim().ToLowerInvariant();
    }


    public bool ConfirmAddContact(Contact contact, string feedback)
    {
        WriteLine($"{Environment.NewLine}{Environment.NewLine}You want to add this contact: ");
        DisplayContact(contact);
        WriteLine();
        WriteLine($"{Environment.NewLine}{feedback}");
        WriteLine($"{Environment.NewLine}Enter 'y' to Add contact or 'Esc' to cancel");
        var confirmOrCancel = ReadLine().NullToEmpty().ToLowerInvariant();
        return confirmOrCancel == "y";
    }

    private Profile GetEnteredProfile()
    {
        var firstName = GetEntry("first name");
        var lastName = GetEntry("last name");
        var phone = GetEntry("phone");
        var email = GetEntry("email");
        return new Profile(firstName, lastName, phone, email);
    }

    private static string GetPreferred(string entered, string theDefault)
    {
        return IsNullOrWhiteSpace(entered) ? theDefault : entered;
    }
    private Address GetEnteredAddress()
    {
        var addressLine1 = GetEntry("first line of address");
        var addressLine2 = GetEntry("second line of address");
        var city = GetEntry("town or city");
        var postCode = GetEntry("postCode");
        const string country = "UK";
        return new Address(addressLine1, addressLine2, city, postCode, country);
    }

    private string GetEntry(string entryType)
    {
        WriteLine($"Enter {entryType} or just press Enter to omit");
        return ReadLine().NullToEmpty();
    }
}
