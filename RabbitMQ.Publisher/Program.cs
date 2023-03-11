using RabbitMQ.Client;
using RabbitMQ.Common;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

Connection con = new Connection();
Random random = new Random();

var channel = con.GetModel();

while (true)
{
    Console.Write("Write line count : ");
    var line = Console.ReadLine();
    int lineCount;
    if (int.TryParse(line, out lineCount))
    {
        DoWork(lineCount);
    }

    Console.WriteLine("Continue ? Y/N");

    string isContinue = Console.ReadLine() ?? "N";
    if (isContinue.ToUpper().Equals("N")) break;
}

Console.WriteLine("done");

string RandomString(int length)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    return new string(Enumerable.Repeat(chars, length)
      .Select(s => s[random.Next(s.Length)]).ToArray());
}

void DoWork(int lineCount)
{
    channel.QueueDeclare("TestQueue", true, false, false);
    //var queueName = channel.QueueDeclare().QueueName;
    for (int i = 1; i <= lineCount; i++)
    {
        var userr = new { index = i, name = RandomString(5), surname = RandomString(8) };
        string model = JsonSerializer.Serialize(userr);
        byte[] data = Encoding.UTF8.GetBytes(model);
        //channel.QueueBind("TestQueue", "", "");
        channel.BasicPublish("", "TestQueue", body: data);
        Console.WriteLine($"{i}. line => {userr}");
    }
}