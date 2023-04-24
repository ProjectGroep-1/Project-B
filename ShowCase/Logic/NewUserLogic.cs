/* public class NewUserLogic{
    public string Mail;
    public string Pass;
    public string Name;
    public string User;
  
    
    public NewUserLogic(string emailAddress, string password, string fullName){
       
        Mail = emailAddress;
        Pass = password;
        Name = fullName;
             
    }

    public void add_acc(){
        Random r = new Random();
        int n = r.Next(1,9999);
        User = "user";
        AccountModel Acc = new AccountModel(n, Mail, Pass, Name, User);
        List<AccountModel> accountlist = new List<AccountModel>();
        accountlist = AccountsAccess.LoadAll();
        accountlist.Add(Acc);
        AccountsAccess.WriteAll(accountlist);
    }
}
*/



 
 
 
 
 
 
