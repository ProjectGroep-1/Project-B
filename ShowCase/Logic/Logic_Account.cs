﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
public class Logic_Account
{
    public List<Model_Account> _accounts;

    public Logic_Account()
    {
        _accounts = Access_Account.LoadAll();
    }


    public void UpdateList(Model_Account acc)
    {   
        //Find if there is already an model with the same id
        int index = _accounts.FindIndex(s => s.Id == acc.Id);
        
        if (index != -1)
        {
            _accounts[index] = acc;
        }
        else
        {
            //add new model
            _accounts.Add(acc);
        }

        Access_Account.WriteAll(_accounts);
    }

    public bool checkDuplicateEmail(string mail)
    {
        int index = _accounts.FindIndex(s => s.EmailAddress == mail);

        if (index == 0)
        {
            return true;
        }
        return false;
    }

    public string EnteringPassword()
    {
        string password = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
        
            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } 
        return password;
    }

    public Model_Account GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    public int GetNewID()
    {
        if (_accounts.Count == 0) { return 1; }
        return _accounts[_accounts.Count -1].Id +1;
    }

}