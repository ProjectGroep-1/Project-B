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

[TestClass]
public class JSONTest
{
    [TestMethod]
    public void TestJSON()
    {
        var expected = "ID: 0 | Name: Carpaccio | Category: Soup | Course: Voor | Price: 2.30 EUR";
        Assert.AreEqual(FoodMenuFunctions.FindItem(0).ToString(), expected);
    }

    
}