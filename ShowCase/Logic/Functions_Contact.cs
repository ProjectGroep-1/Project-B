public static class Functions_Contact
{   
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"DataSources/ContactFile.txt"));
    static Logic_Contact contactlogic = new(path);

    public static void EditContactInformation()
    {
        Console.WriteLine("1: View contact information"+ "\n" + "2: Edit contact information");
        string ContactInput = Console.ReadLine();
        int variable = 0;
        int.TryParse(ContactInput, out variable);
        switch (variable){
            case 1:
                Console.Clear();
                Console.WriteLine(contactlogic.Read());
                Console.ReadKey(true);
                break;
            case 2: 
                Console.Clear();
                Console.WriteLine("1: Edit Phone number"+ "\n" + "2: Edit Adress" + "\n" + "3: Edit E-mail");
                string contactinput_2 = Console.ReadLine();
                if(contactinput_2 == "1"){
                    try{
                    Console.WriteLine("What is your new phone number?");
                    string new_number = $"Phone number: {Console.ReadLine()}";
                    contactlogic.ChangeValueById(0, new_number);}
                    catch (IOException e) {
                    Console.WriteLine("Error occurred while editing file: " + e.Message);
                    }}

                else if(contactinput_2 == "2"){
                    try{
                    Console.WriteLine("What is your new Adress?");
                    string new_adress = $"Adress: {Console.ReadLine()}";
                    contactlogic.ChangeValueById(1, new_adress);}
                    catch (IOException e) {
                    Console.WriteLine("Error occurred while editing file: " + e.Message);
                    }}

                else if(contactinput_2 == "3"){
                    try{
                    Console.WriteLine("What is your new Email adress?");
                    string new_email = $"Email : {Console.ReadLine()}";
                    contactlogic.ChangeValueById(2, new_email);}
                    catch (IOException e) {
                    Console.WriteLine("Error occurred while editing file: " + e.Message);
                    }}
                else{
                    Console.WriteLine("Invalid input");
                    Console.ReadKey(true);
                   }
                break;
            default:
                Console.WriteLine("You've entered an invalid input, try again");
                break;
            } 
    }

    public static void DisplayContactInformation()
    {
        Console.WriteLine(contactlogic.Read());
    }
}