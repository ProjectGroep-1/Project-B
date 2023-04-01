namespace ProjectBTest;

[TestClass]
public class ContactInformationTest
{
    [TestMethod]
    public void TestChangePhoneNumber()
    {
        string New_PhoneNumber = "0673095176";
        ContactInformation.ChangePhoneNumber(New_PhoneNumber);
        Assert.AreEqual(ContactInformation.PhoneNumber, New_PhoneNumber);
    }

    public void TestChangeAdress()
    {
        string New_Adress = "Rotterdam 2177 BN";
        ContactInformation.ChangeAdress(New_Adress);
        Assert.AreEqual(ContactInformation.Adress, New_Adress);
    }

    public void TestChangeEmail()
    {
        string New_Email = "JeffDarcy@example.com";
        ContactInformation.ChangeAdress(New_Email);
        Assert.AreEqual(ContactInformation.Email, New_Email);
    }
}