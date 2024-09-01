namespace Domain;

public class Message
{
    public int Number { get; set; }
    public string Text { get; set; }
    public long Date { get; set; }

    public Message(int number, string text, long date)
    {
        Number = number;
        Text = text;
        Date = date;
    }
}