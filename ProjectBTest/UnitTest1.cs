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

    [TestMethod]
    public void TestChangeAdress()
    {
        string New_Adress = "Rotterdam 2177 BN";
        ContactInformation.ChangeAdress(New_Adress);
        Assert.AreEqual(ContactInformation.Address, New_Adress);
    }

    [TestMethod]
    public void TestChangeEmail()
    {
        string New_Email = "JeffDarcy@example.com";
        ContactInformation.ChangeEmail(New_Email);
        Assert.AreEqual(ContactInformation.Email, New_Email);
    }

}

[TestClass]
public class AccountTest
{
    [TestMethod]
    public void TestAccountCreation()
    {   
        int id = 100;
        string email = "TestEmail@example.nl";
        string password = "Test127";
        string name = "John Doe";
        string usertype = "user";
        Model_Account mytestaccount = Functions_Account.MethodToTestAccount(id, email, password, name, usertype);
        Assert.IsNotNull(mytestaccount);
        Assert.AreEqual(mytestaccount.Id, id);
        Assert.AreEqual(mytestaccount.EmailAddress, email);
        Assert.AreEqual(mytestaccount.Password, password);
        Assert.AreEqual(mytestaccount.FullName, name);
        Assert.AreEqual(mytestaccount.UserType, usertype);
        Assert.IsNotNull(mytestaccount.ReservationIDs);
    }
}

[TestClass]
public class ReservationTest
{

}

[TestClass]
public class CapacityTest
{  

}