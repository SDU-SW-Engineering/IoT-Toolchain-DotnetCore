# Kafka Kerberos integration

In order to use kerberos with the .NET confluent kafka library a series of steps has to be taken. It is not an easy process.

If you already use docker we recommend to use our image (TODO: Image with librdkafka installed and krb5-user installed)

If you don't use docker the process depends on your platform (OSX, Windows, Linux)

## Windows
On Windows you are quite limited and will have to rely on the user you use to log into Windows. 

So if you have multiple applications who needs to use different kafka users and you use Windows to develop you will probably need to use docker images instead.

TODO: documentation

## Linux
This guide assumes you use a Debian based linux distro with a bash shell.

In order to make confluent-kafka-dotnet use kerberos as authentication/authorization we will need to manually compile librdkafka. 
Confluent-kafka-dotnet is based on librdkafka which is a library written in C. But the official Nuget sources is based librdkafka.redist.
This dependance does not have support for Kerberos authentication, therefore we need to first compile another version of librdkafka which confluent-kafka-dotnet then can use.

Run the following command in your terminal

1. `sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF \
    && echo "deb http://download.mono-project.com/repo/debian stretch main" | tee /etc/apt/sources.list.d/mono-official.list \
    && sudo apt-get update && sudo apt-get install -y mono-devel default-jre build-essential libssl-dev libsasl2-2 libsasl2-dev libsasl2-modules-gssapi-mit wget unzip`
2. `sudo wget https://github.com/edenhill/librdkafka/archive/v1.5.0.zip -O /tmp/librdkafka.zip && cd /tmp/`
3. `sudo unzip librdkafka.zip && cd librdkafka && ./configure && make && make install`
4. You now need to replace the redist based version of librdkafka inside your application. 
   1. You first have to find the folder inside your application code called 'runtimes'. 
      a. If you haven't published your application at this stage, it can be found here: `bin -> Debug -> netcoreapp -> runtimes`. 
      b. If you have published your application it can be found in the main folder
   2. Inside 'runtimes' go to `linux-x64 -> native`
   3. Run the following command `cp /usr/local/lib/librdkafka*.so* .` This will copy over the build librdkafka files which will make confluent-kafka-dotnet use it instead of librdkafka.redist.
5. Now we need to install krb5-user and other dependencies and get our keytab.
   1. Run the following command `apt-get update && export DEBIAN_FRONTEND=noninteractive && apt-get install krb5-user libsasl2-2 libsasl2-modules-gssapi-mit libsasl2-modules`
   2. Download the keytab you require. If you use our kerberos server you can curl it.
      1. `export KEYTAB_USERNAME=testUser`. Replace testUser with your user
      2. `export KEYTAB_PASSWORD=testUserPassword`. Replace testUserPassword with your password.
      3. `export KERBEROS_IP=kerberosIP:6000`. Replace kerberosIP with your kerberos server IP and port. 
      4. `curl --fail --max-time 5 -X POST -H "Content-Type: application/json" -d "{\"username\":\""$KEYTAB_USERNAME"\", \"password\":\""$KEYTAB_PASSWORD"\"}" http://"$KERBEROS_IP":6000/get-keytab -o /tmp/keytab`
6. The last step is to configure confluent-kafka-dotnet

```
var consumerConfig = new ConsumerConfig {
    GroupId = "testApplication",
    BootstrapServers = KafkaIP,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    SecurityProtocol = SecurityProtocol.SaslPlaintext,
    SaslKerberosKeytab = /tmp/keytab,
    SaslKerberosPrincipal = testUser
};

```

It is identical for a producerConfig.

7. You are now ready for a kerberos authenticated setup

## OSX
If you develop on OSX you are fortunate as the process is almost identical to Linux, a few package names are different.

TODO: documentation
