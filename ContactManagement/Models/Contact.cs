namespace ContactManagement.Models;

public readonly record struct Contact(Profile Profile, Address Address, string Id = "");
