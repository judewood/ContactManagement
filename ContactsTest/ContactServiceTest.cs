using ContactManagement.Data;
using ContactManagement.Extensions;
using ContactManagement.Models;
using ContactManagement.Service;
using Moq;
using static System.String;

namespace ContactsTest;


//This is to demonstrate that I can create unit tests and is a small subset of the unit tests that would be created in a real project

[TestClass]
public class ContactServiceTest
{

    [TestMethod]
    public void AddContact_AddsContactToStore()
    {
        var mockContactDataService = new Mock<IContactDataService>();
        var validContact = GetValidContact();
        mockContactDataService.Setup(x => x.AddContact(It.IsAny<Contact>())).ReturnsAsync(true);
        var contactService = new ContactService(mockContactDataService.Object);

        var result = contactService.AddContact(validContact).GetAwaiter().GetResult();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CheckContact_WithMissingContactName_ReturnsError()
    {
        const string expectedFeedback = "Contact must have first or last name";
        var mockContactDataService = new Mock<IContactDataService>();
        var validContact = GetValidContact();
        Profile invalidProfile = new(Empty, Empty, validContact.Profile.Phone, validContact.Profile.Email);
        var invalidContact = validContact with { Profile = invalidProfile };
        var contactService = new ContactService(mockContactDataService.Object);

        var (_, isValidContact, feedback) = contactService.CheckContact(invalidContact);

        Assert.IsFalse(isValidContact);
        Assert.AreEqual(expectedFeedback, feedback);
    }

    [TestMethod]
    public void CheckContact_WithInvalidData_ReturnsExpectedFeedback()
    {
        const string invalidPhone = "invalidPhone";
        const string invalidEmail = "invalidEmail";
        var expectedFeedback = $"Please check email is correct.{Environment.NewLine}Please check phone number is correct.{Environment.NewLine}Please check address is complete and correct.";
        var mockContactDataService = new Mock<IContactDataService>();
        var validContact = GetValidContact();
        var validProfile = validContact.Profile;
        var invalidProfile = validProfile with { Phone = invalidPhone, Email = invalidEmail };
        var validAddress = validContact.Address;
        var invalidAddress = validAddress with { City = "" };
        var invalidContact = validContact with { Profile = invalidProfile, Address = invalidAddress };

        var contactService = new ContactService(mockContactDataService.Object);
        var (actualContact, isValidContact, feedback) = contactService.CheckContact(invalidContact);

        Assert.IsTrue(isValidContact);
        Assert.AreEqual(expectedFeedback, feedback);
        Assert.IsFalse(actualContact.Profile.IsValidEmail);
        Assert.IsFalse(actualContact.Profile.IsValidPhone);
        Assert.IsFalse(actualContact.Address.IsValid);
    }


    [TestMethod]
    public void CheckContact_FormatsAsExpected()
    {
        var mockContactDataService = new Mock<IContactDataService>();
        var validContact = GetValidContact();
        var contactService = new ContactService(mockContactDataService.Object);
        var (actualContact, isValidContact, feedback) = contactService.CheckContact(validContact);

        Assert.AreEqual(Empty, feedback);
        Assert.IsTrue(isValidContact);
        Assert.AreEqual(actualContact.Profile.FirstName, validContact.Profile.FirstName.ToLeadingCapital());
        Assert.AreEqual(actualContact.Profile.LastName, validContact.Profile.LastName.ToLeadingCapital());
        Assert.AreEqual(actualContact.Profile.Phone, validContact.Profile.Phone.ToWhitespaceRemoved());
        Assert.AreEqual(actualContact.Profile.Email, validContact.Profile.Email.ToWhitespaceRemoved().ToLowerInvariant());
        Assert.AreEqual(actualContact.Address.AddressLine1, validContact.Address.AddressLine1.Trim());
        Assert.AreEqual(actualContact.Address.AddressLine2, validContact.Address.AddressLine2.Trim());
        Assert.AreEqual(actualContact.Address.PostCode, validContact.Address.PostCode.ToPostcode());
        Assert.AreEqual(actualContact.Address.City, validContact.Address.City.Trim().ToLeadingCapital());
        Assert.IsTrue(actualContact.Address.IsValid);
    }
    private Contact GetValidContact()
    {
        Profile profile = new("john", "smith", "07 123 456 789", "John.Smith@gmail.com");
        Address address = new(" Ivy Cottage ", " The Village ", " london ", "WE1 7TY ", "UK");
        return new Contact(profile, address, Guid.NewGuid().ToString());
    }
}