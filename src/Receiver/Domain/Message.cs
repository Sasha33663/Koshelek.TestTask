using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;
public class Message
{
    public int Number { get; set; }
    public string Text {  get; set; }
    public DateTime Date { get; set; }

    public Message(int number, string text )
    {
        Number = number;
        Text = text;
        Date = DateTime.UnixEpoch;
    }

}
