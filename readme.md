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
