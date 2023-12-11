# Some materials about Kafka

## Запус Kafka через docker-compose

Links:

* [Guide to Setting Up Apache Kafka Using Docker](https://www.baeldung.com/ops/kafka-docker-setup)
* [Команды Docker Compose Up и Start, а также Down и Stop: в чем разница?](https://habr.com/ru/companies/first/articles/592321/)
* [Файл docker-compose: docker-compose_0.yml](docker-compose_0.yml)

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

## Kafka в действии (Kafka in action)

Links:

* [github.com](https://github.com/Kafka-In-Action-Book/Kafka-In-Action-Source-Code.git)
* [docker-compose.yml](docker-compose.yml) - файл из книги для локального запуска kafka в контейнере.

Запуск docker-compose:

```text
docker-compose up -d
```

Проверка запуска всех компонентов:

```text
docker ps --format "{{.Names}}: {{.State}}"
```

### Запуск kafka cli из docker

Links:

* [How to operate Kafka, mostly using Docker](https://gist.github.com/DevoKun/01b6c9963d5508579f4cbd75d52640a9#file-kafka-md)

Run Kafka Commands inside the container:

Usage. `docker exec` runs a new command in a running container:

```text
docker exec [OPTIONS] CONTAINER COMMAND [ARG...]
```

Примеры команд kafka cli:

``` text
## List Brokers
docker exec -ti kafka /usr/bin/broker-list.sh

## List Topics
docker exec -ti kafka /opt/kafka/bin/kafka-topics.sh --list --zookeeper zookeeper:2181

## Create a Topic
docker exec -ti kafka /opt/kafka/bin/kafka-topics.sh --create --zookeeper zookeeper:2181 --replication-factor 1 --partitions 1 --topic test2

## List Topics
docker exec -ti kafka /opt/kafka/bin/kafka-topics.sh --list --zookeeper zookeeper:2181
```

Где:

* `-i` or `--interactive` - Keep STDIN open even if not attached
* `-t` or `--tty` - Allocate a pseudo-TTY

### Создание темы `kinaction_helloworld`

Команда в книге:

```text
bin/kafka-topics.sh --create --bootstrap-server localhost:9094 --topic kinaction_helloworld --partitions 3 --replication-factor 3
```

* `--partitions` - на сколько частей будет делиться тема.
Например, мы предполагаем использовать три брокера, поэтому, разбив тему на три раздела,
дадим по одному разделу каждому брокеру.

* `--replication-factor` - в каждом разделе мы хотим иметь по три реплики (копии).

* `--bootstrap-server` - сетевой адрес брокера Kafka.

Команда в Docker:

```text
docker-compose exec broker1 kafka-topics --create --bootstrap-server localhost:29092 --topic kinaction_helloworld --partitions 3 --replication-factor 3
```

* `broker1` - один из контейнеров с бокером kafka, `localhost:29092` - его адрес.

### Проверка наличия созданной темы

Выводит все темы, которые есть у брокера.

Команда в книге:

```text
bin/kafka-topics.sh --list --bootstrap-server localhost:9094
```

Команда в Docker:

```text
docker-compose exec broker1 kafka-topics --list --bootstrap-server localhost:29092
```

### Вывод (описание) темы `kinaction_helloworld`

Команда в книге:

```text
bin/kafka-topics.sh --bootstrap-server localhost:9094 --describe --topic kinaction_helloworld
```

Команда в Docker:

```text
docker-compose exec broker1 kafka-topics --bootstrap-server localhost:29092 --describe --topic kinaction_helloworld
```

### Запуск производителя и потребителя kafka

Два окна терминала. На одном запускается производитель, на втором - потребитель.
В производителе все набираемые строки будут посланы в потребитель.

Из книги:

```text
# Команда запуска производителя Kafka
bin/kafka-console-producer.sh --bootstrap-server localhost:9094 --topic kinaction_helloworld

# Команда запуска потребителя Kafka
bin/kafka-console-consumer.sh --bootstrap-server localhost:9094 --topic kinaction_helloworld --from-beginning
```

`--from-beginning` - потребитель читает все сообщения по теме, которые есть в брокере (даже те,
которые уже посылались).

Из книги: при отправке последующих сообщений и подтверждении доставки в приложении-потребителе
можно опустить параметр `--from-beginning`.

Соответствующие команды в Docker:

```text
docker-compose exec broker1 kafka-console-producer --bootstrap-server localhost:29092 --topic kinaction_helloworld
docker-compose exec broker1 kafka-console-consumer --bootstrap-server localhost:29092 --topic kinaction_helloworld --from-beginning
```
