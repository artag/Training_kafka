# Some materials about Kafka

## Запус Kafka через docker-compose

Links:

* [Guide to Setting Up Apache Kafka Using Docker](https://www.baeldung.com/ops/kafka-docker-setup)
* [Команды Docker Compose Up и Start, а также Down и Stop: в чем разница?](https://habr.com/ru/companies/first/articles/592321/)

To start an Apache Kafka server, we’d first need to start a Zookeeper server.

We can configure this dependency in a `docker-compose.yml` file,
which will ensure that the Zookeeper server always starts before the Kafka server and stops after it.

* Create and start containers: `docker-compose up -d`

* Start containers: `docker-compose start -d`

* Stop containers (without deleting): `docker-compose stop`

* Stop and delete containers: `docker-compose down`

* List containers: `docker ps -a` или `docker-compose ps`. `-a` - вывод остановленных и
работающих контейнеров.

`-d` - detach. Запуск исполнения в фоне. Можно запустить без `-d`, но потом нажать `CTRL+Z`.
Программа продолжит свое выполнение в фоновом режиме.

`docker-compose down` удаляет контейнеры изо всех томов.
Команда удаляет все контейнеры и внутренние сети, связанные с этими сервисами -
но НЕ указанные внутри тома. Чтобы очистить и их, надо дополнить команду down флагом `-v`.

## Проверка работы

Use the `nc` command to verify that both the servers are listening on the respective ports
(почему-то не сработало):

```bash
nc -z localhost 22181
nc -z localhost 29092
```

А вот это сработало:

```bash
docker-compose logs kafka | grep -i started
```

## Kafka With .Net Core

Link:

* [Kafka With .Net Core](https://www.c-sharpcorner.com/article/kafka-with-net-core/)

Пример простого Producer, Consumer. В Producer печатаем сообщение, в Consumer принимаем
(передача string).

Код тут: [src1](src1)

## Getting Started with Apache Kafka and .NET

Link:

* [Getting Started with Apache Kafka and .NET](https://developer.confluent.io/get-started/dotnet/)

Еще один пример Producer, Consumer. В Producer пускаем пачку (10) из пар (key-value) строк,
в Consumer их принимаем в цикле.

При запуске producer и consumer используется файл конфигурации `getting-started.properties`.

Код тут: [src2](src2)
