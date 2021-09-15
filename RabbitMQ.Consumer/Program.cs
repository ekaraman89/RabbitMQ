using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common;
using System.Text;

Connection con = new Connection();
var channel = con.GetModel();

while (true)
{
    var b = channel.BasicGet(queue: "Test", autoAck: true);
    if (b != null)
    {
        var body = b.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message);
    }
}
