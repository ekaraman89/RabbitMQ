using RabbitMQ.Client;

namespace RabbitMQ.Common
{
    public class Connection
    {
        private ConnectionFactory? factory;

        public IConnection GetConnection()
        {

            if (factory == null)
            {
                factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };
            }
            return factory.CreateConnection();
        }
        public IModel GetModel()=> GetConnection().CreateModel();

    }
}
