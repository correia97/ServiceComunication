# Comunicação entre serviços utilizando RabbitMQ

Projeto de estudo de filas utilizando rabbitmq e .Net Core 3.1

**Objetivos**

Criar dois ou mais serviços que publicam mensagens em um fila do rabbit e trabalha como worker de outra fila

## Dependências

**Pacotes utilizados nos projetos:**

|Pacote          |Versão                          |
|----------------|-------------------------------|
|RabbitMQ.Client |`5.1.2`|
|Newtonsoft.Json |`12.0.3`|
|Polly			 |`7.2.0`|

**Imagem docker utilizadas:**

|Imagem          |Versão                          |
|----------------|-------------------------------|
|rabbitmq |`3.7-management-alpine`|
|mcr.microsoft.com/dotnet/core/sdk |`3.1-alpine3.11` |
|mcr.microsoft.com/dotnet/core/aspnet|`3.1-alpine3.11`|


### Executando projeto com Docker

Via terminal navegue até a pasta onde está o arquivo Service.sln e execute o comando
```bash
docker-compose up --build
```

Este comando vai baixar as imagens necessárias, compilar o projeto e criar as imagens com as aplicações .net executar os container na mesma rede para que eles possam se comunicar. Obs.: As Duas Api's estão com uma tempo de inicialização de 2min para garantir que o rabbit já está em execução

A API1 vai ser executada na porta 3001 

A API2 vai ser executada na porta 3002

O manager do RabbitMQ vai ser executado na porta 6672
