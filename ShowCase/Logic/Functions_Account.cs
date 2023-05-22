public static class Functions_Account
{
    public static Logic_Account accountLogic = new();

    public static Model_Account? CurrentAccount { get; private set; }
    public static Model_Account CheckLogin(string email, string password)
    {
        Access_Account.LoadAll();
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = accountLogic._accounts.Find(i => i.EmailAddress == email && i.Password == password);
        return CurrentAccount;
    }

    public static void AddReservationToAccount(int resID)
    {
        CurrentAccount.ReservationIDs.Add(resID);
        accountLogic.UpdateList(CurrentAccount);
    }
    public static void SetCurrentAccount(Model_Account account)
    {
        CurrentAccount = account;
    }
}