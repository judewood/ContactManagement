// See https://aka.ms/new-console-template for more information

using ContactManagement.Data;
using ContactManagement.Service;
using static System.Console;



UserInterfaceService userInterface = new();
ContactService contactService = new(new ContactDataService());

WriteLine("Welcome to Contact Management");

var exitRequested = false;
for (; ; )
{
    var input = userInterface.GetSelection();
    switch (input)
    {
        case "A": //Add contact
            {
                var rawContact = userInterface.GetEnteredContact();
                var (formattedContact, isValid, feedback) = contactService.CheckContact(rawContact);

                if (isValid)
                {
                    if (userInterface.ConfirmAddContact(formattedContact, feedback))
                    {
                        var result = await contactService.AddContact(formattedContact);
                        if (result is false)
                        {
                            WriteLine("failed to add contact");
                            break;
                        }
                        WriteLine("Contact stored Ok:");
                        break;
                    }
                    WriteLine("Cancelling request");
                    break;
                }
                WriteLine($"Contact cannot be added: {feedback}");
                break;
            }

        case "Q":  //Quit
            {
                exitRequested = true;
                WriteLine("Quitting Contact Management");
                break;
            }

        case "S": //Search Contacts
            {
                var namePart = userInterface.GetSearchString();
                var (contacts, error) = (await contactService.SearchContacts(namePart)).ToTuple();
                if (error.Length > 0)
                {
                    WriteLine(error);
                    break;
                }
                userInterface.DisplaySearchResults(contacts);
                break;
            }

        case "D":  //Delete Contact
            {
                var id = userInterface.GetId();
                var result = await contactService.DeleteContactById(id);
                if (result)
                {
                    WriteLine($"Deleted contact with Id: {id}");
                    break;
                }
                WriteLine($"Failed to delete contact with Id: {id}");
            }

            break;

        case "E": //Edit contact
            {
                var id = userInterface.GetId();
                var contact = await contactService.GetContactById(id);
                if (contact == null)
                {
                    WriteLine($"Unable to find contact with Id {id}");
                    break;
                }

                var updatedContact = userInterface.GetUpdatedContact(contact.Value);
                var result = await contactService.UpdateContact(updatedContact);
                if (result)
                {
                    WriteLine($"Updated contact with Id: {updatedContact.Id}");
                    userInterface.DisplayContact(updatedContact);
                    break;
                }
                WriteLine("Failed to update contact with Id: {id}");
                break;
            }
    }

    if (exitRequested)
    {
        return;
    }
}



