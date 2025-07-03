# GestaoPreco.UI

## Descri��o

Este projeto � composto por uma aplica��o ASP.NET Core (.NET 8) que exp�e uma API para consulta de pre�os de produtos, al�m de um front-end (sugest�o: React.js) para intera��o com o usu�rio.

## Estrutura

- **Back-end:** ASP.NET Core Web API
- **Front-end:** React.js 
- **Controller principal:** `PriceController` para consulta de pre�os

## Funcionamento

A API possui um endpoint para consultar o pre�o de um produto a partir do seu ID. O controller `PriceController` faz uma requisi��o HTTP para uma API de pre�os externa e retorna o resultado para o front-end.

### Exemplo de uso do endpoint

- **GET** `/Price/{productId}`  
  Retorna o pre�o do produto informado.

