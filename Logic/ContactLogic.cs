
public class TextFile
{
    private string filePath;

    public TextFile(string filePath)
    {
        this.filePath = filePath;
    }

    public string Read()
    {
        return File.ReadAllText(filePath);
    }

    public void Write(string text)
    {
        File.WriteAllText(filePath, text);
    }

    public void ChangeValueById(int id, string newValue)
    {
        string contents = Read();
        string[] lines = contents.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (i == id)
            {
                lines[i] = newValue;
                break;
            }
        }

        contents = string.Join('\n', lines);
        Write(contents);

        /* Default values .txt file:
         PhoneNumber = 0104962016
        adress: Wijnhaven 107, 3011 WN in Rotterdam;
        Email = JakeDarcy@email.com
        */
    }
}