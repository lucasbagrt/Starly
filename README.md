# Starly
Plataforma de avaliação de estabelecimentos, desenvolvida em .NET 8 com arquitetura de microsserviços e SQL Server.

### 📋 Pré-requisitos

* .NET 8 ([Download .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
* Docker ([Download Docker Desktop](https://www.docker.com/products/docker-desktop/))

## Integrantes

- [Lucas Hanke](https://github.com/lucasbagrt)

## Build 

Para rodar este projeto, siga estes passos:

* Inicie os microsserviços conforme abaixo:
  * Crie o banco de dados no Docker utilizando o comando abaixo.
  * Execute o serviço correspondente a cada microsserviço.
  * Utilize o Gateway para acessar os endpoints dos microsserviços.

### Microsserviços e Banco de Dados

Os microsserviços foram desenvolvidos utilizando a tecnologia .NET 8 e integram-se a um banco de dados SQL Server. Abaixo estão os passos para configurar o banco de dados utilizando Docker:

```docker
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=1q2w3e4r@#$' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

### Gateway

O Gateway foi implementado utilizando Ocelot para permitir o acesso aos endpoints dos microsserviços.

## Comunicação entre Microsserviços

Os serviços também se comunicam através dos endpoints e do gateway para acessar informações de outros microsserviços. Isso permite realizar validações e obter dados necessários para aplicar a lógica de negócio de forma distribuída, ele pode fazer uma requisição HTTP para o endpoint correspondente no microsserviço alvo, utilizando o gateway como intermediário.

* Detalhes dos Microserviços:

## Customer 

Microsserviço responsável pelo gerenciamento dos clientes.

Endpoints

![image](https://github.com/lucasbagrt/Starly/assets/75868307/f021a4c9-4b70-4649-ac7d-99bbea3121fc)

## Business 

Microsserviço responsável pelo gerenciamento dos estabelecimentos.

Endpoints

![image](https://github.com/lucasbagrt/Starly/assets/75868307/65598969-a267-4f11-a54b-9edcfe249ec4)
![image](https://github.com/lucasbagrt/Starly/assets/75868307/c35f2dab-a7f7-49eb-91ca-22d4db87eea1)

## Review

Microsserviço responsável pelo gerenciamento das avaliações dos estabelecimentos.

Endpoints

![image](https://github.com/lucasbagrt/Starly/assets/75868307/a82f1ece-802a-4a96-8802-b8fefd96ecee)


## Fluxo basico
  * O usuário admin cria os estabelecimentos.
  * O usuário normal consegue se registrar.
  * O usuário normal, consegue visualizar os estabelecimentos.
  * O usuário normal, consegue enviar uma avaliação para um estabelecimento, com fotos, nota, e um comentario.
 
## Frontend 
    
  ![image](https://github.com/lucasbagrt/Starly/assets/75868307/ebb35cbc-5f11-4452-8e23-088eb2de8a0b)    
  ![image](https://github.com/lucasbagrt/Starly/assets/75868307/68d851cb-68eb-4ccd-862c-0dc96ab7689f)
