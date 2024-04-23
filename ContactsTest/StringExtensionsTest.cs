using ContactManagement.Extensions;

namespace ContactsTest;

//This is to demonstrate that I can create unit tests and is a small subset of the unit tests that would be created in a real project

[TestClass]
public class StringExtensionsTest
{
    [DataRow(null, "")]
    [DataRow("", "")]
    [DataRow("unchanged", "unchanged")]
    [DataTestMethod]
    public void NullToEmpty_Returns_Expected_Results(string input, string expectedOutput)
    {
        var actualOutput = input.NullToEmpty();

        Assert.AreEqual(expectedOutput, actualOutput);
    }
}
