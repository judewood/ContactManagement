using ContactManagement.Data;
using ContactManagement.Extensions;
using ContactManagement.Models;
using System.Text;
using static System.String;

namespace ContactManagement.Service;
internal class ContactService(IContactDataService dataService)
{

    public (Contact, bool, string) CheckContact(Contact rawContact)
    {
        // The validation is deliberately lenient. In my experience is it commercially better to gather data and mark it as valid or otherwise.
        // This allows our commercial people to try to get in touch with a contact and add/correct missing and invalid details
        // The IsValid markers can be used to prevent e.g. sending an SMS to an invalid or  non-existent mobile number
        // This also helps when gathering metrics
        var rawProfile = rawContact.Profile;
        var rawAddress = rawContact.Address;
        if (!IsValidName(rawProfile.FirstName, rawProfile.LastName))
        {
            return (new Contact(rawProfile, rawAddress), false, "Contact must have first or last name");
        }

        var contact = new Contact(GetValidatedProfile(rawProfile), GetValidatedAddress(rawAddress));
        return (contact, true, GetFeedback(contact));
    }

    public async Task<bool> AddContact(Contact contact)
    {
        return await dataService.AddContact(contact);
    }

    public async Task<(IEnumerable<Contact>, string error)> SearchContacts(string searchString)
    {
        return await dataService.SearchContactsByName(searchString);
    }

    public async Task<bool> DeleteContactById(string id)
    {
        var contact = await dataService.GetContactById(id);
        if (contact is null)
        {
            return false;
        }
        return await dataService.DeleteContactById(id);
    }

    public async Task<bool> UpdateContact(Contact contact)
    {
        if (IsNullOrWhiteSpace(contact.Id))
        {
            return false;
        }
        return await dataService.UpdateContact(contact);
    }

    public async Task<Contact?> GetContactById(string id)
    {
        return await dataService.GetContactById(id);
    }

    private static bool IsValidAddress(Address address)
    {
        // A UK address requires first line of address, town/city and postcode to be able to post mail to it
        return !IsNullOrWhiteSpace(address.AddressLine1) && !IsNullOrWhiteSpace(address.City) && !IsNullOrWhiteSpace(address.PostCode);
    }

    private static bool IsValidName(string? firstname, string? lastName)
    {
        return !IsNullOrWhiteSpace(firstname) && !IsNullOrWhiteSpace(lastName);
    }

    private Profile GetValidatedProfile(Profile rawProfile)
    {
        var formattedPhone = rawProfile.Phone.ToWhitespaceRemoved();
        var formattedEmail = rawProfile.Email.ToWhitespaceRemoved().ToLowerInvariant();
        var formattedFirstName = rawProfile.FirstName.ToLeadingCapital();
        var formattedLastName = rawProfile.LastName.ToLeadingCapital();
        return new Profile(formattedFirstName, formattedLastName, formattedPhone, formattedEmail, rawProfile.Email.IsValidEmail(), formattedPhone.IsValidPhoneNumber());

    }

    private Address GetValidatedAddress(Address rawAddress)
    {
        var formattedAddressLine1 = rawAddress.AddressLine1.Trim();
        var formattedAddressLine2 = rawAddress.AddressLine2.Trim();
        var formattedCity = rawAddress.City.Trim().ToLeadingCapital();
        var formattedCountry = rawAddress.Country.Trim();
        var formattedPostCode = rawAddress.PostCode.ToPostcode();

        return new Address(formattedAddressLine1, formattedAddressLine2, formattedCity, formattedPostCode, formattedCountry, IsValidAddress(rawAddress));
    }

    private static string GetFeedback(Contact contact)
    {
        if (contact.Profile is { IsValidEmail: true, IsValidPhone: true } && contact.Address.IsValid)
        {
            return Empty;
        }

        StringBuilder stringBuilder = new(100);
        if (!contact.Profile.IsValidEmail)
        {
            stringBuilder = stringBuilder.Append("Please check email is correct.");
        }
        if (!contact.Profile.IsValidPhone)
        {
            stringBuilder = stringBuilder.Append($"{Environment.NewLine}Please check phone number is correct.");
        }
        if (!contact.Address.IsValid)
        {
            stringBuilder = stringBuilder.Append($"{Environment.NewLine}Please check address is complete and correct.");
        }

        return stringBuilder.ToString();

    }

}
