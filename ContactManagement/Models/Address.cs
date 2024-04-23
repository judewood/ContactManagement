namespace ContactManagement.Models;

public readonly record struct Address(string AddressLine1, string AddressLine2, string City, string PostCode, string Country, bool IsValid = false);
