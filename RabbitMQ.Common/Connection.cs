using RabbitMQ.Client;
using System.Collections.Generic;

namespace RabbitMQ.Common
{
    /*

docker run -d --net rabbits --hostname rabbit-1 --name rabbit-1 -p 8081:15672 -p 5672:5672 -e RABBITMQ_ERLANG_COOKIE=DSHEVCXBBETJJVJWTOWT rabbitmq:3.9.12-management-alpine
docker run -d --net rabbits --hostname rabbit-2 --name rabbit-2 -p 8082:15672 -p 5673:5672 -e RABBITMQ_ERLANG_COOKIE=DSHEVCXBBETJJVJWTOWT rabbitmq:3.9.12-management-alpine
docker run -d --net rabbits --hostname rabbit-3 --name rabbit-3 -p 8083:15672 -p 5674:5672 -e RABBITMQ_ERLANG_COOKIE=DSHEVCXBBETJJVJWTOWT rabbitmq:3.9.12-management-alpine

docker exec -it rabbit-2 rabbitmqctl stop_app
docker exec -it rabbit-2 rabbitmqctl reset
docker exec -it rabbit-2 rabbitmqctl join_cluster rabbit@rabbit-1
docker exec -it rabbit-2 rabbitmqctl start_app
docker exec -it rabbit-2 rabbitmqctl cluster_status

docker exec -it rabbit-3 rabbitmqctl stop_app
docker exec -it rabbit-3 rabbitmqctl reset
docker exec -it rabbit-3 rabbitmqctl join_cluster rabbit@rabbit-1
docker exec -it rabbit-3 rabbitmqctl start_app
docker exec -it rabbit-3 rabbitmqctl cluster_status

rabbit-1 rabbitmq-plugins enable rabbitmq_management

rabbitmqctl set_policy ha-all "^" '{"ha-mode":"all"}' --priority 0 --apply-to queues

     */

    public class Connection
    {
        private ConnectionFactory? factory;

        public IConnection GetConnection()
        {
            if (factory == null)
            {
                factory = new ConnectionFactory
                {
                    UserName = "guest",
                    Password = "guest",
                };
            }

            List<AmqpTcpEndpoint> lst = new() {
                new AmqpTcpEndpoint("localhost", 5672),
                new AmqpTcpEndpoint(new System.Uri("amqp://guest:guest@localhost:5673")),
                new AmqpTcpEndpoint(new System.Uri("amqp://guest:guest@localhost:5674")),
            };
            return factory.CreateConnection(lst);
        }

        public IModel GetModel() => GetConnection().CreateModel();
    }
}