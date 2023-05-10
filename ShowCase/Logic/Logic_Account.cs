using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class Logic_Account
{
    private List<Model_Account> _accounts;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public Model_Account? CurrentAccount { get; private set; }

    public Logic_Account()
    {
        _accounts = Access_Account.LoadAll();
    }


    public bool UpdateList(Model_Account acc)
    {   
        int index = _accounts.FindIndex(s => s.EmailAddress == acc.EmailAddress);
        //Find if there is already an model with the same id
        int index2 = _accounts.FindIndex(s => s.Id == acc.Id);
        
        if (index2 != -1)
            {
                _accounts[index2] = acc;
            }
            else
            {
                //add new model
                _accounts.Add(acc);
            }

            Access_Account.WriteAll(_accounts);
            return true;
        }


    public Model_Account GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    public Model_Account CheckLogin(string email, string password)
    {
        Access_Account.LoadAll();
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = _accounts.Find(i => i.EmailAddress == email && i.Password == password);
        return CurrentAccount;
    }

    public void SetCurrentAccount(Model_Account account)
    {
        CurrentAccount = account;
    }

    public int GetNewID()
    {
        if (_accounts.Count == 0) { return 1; }
        return _accounts[_accounts.Count -1].Id +1;
    }
}




