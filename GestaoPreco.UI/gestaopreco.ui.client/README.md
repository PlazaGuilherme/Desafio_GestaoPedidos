# GestaoPreco.UI

## Descrição

Este projeto é composto por uma aplicação ASP.NET Core (.NET 8) que expõe uma API para consulta de preços de produtos, além de um front-end (sugestão: React.js) para interação com o usuário.

## Estrutura

- **Back-end:** ASP.NET Core Web API
- **Front-end:** React.js 
- **Controller principal:** `PriceController` para consulta de preços

## Funcionamento

A API possui um endpoint para consultar o preço de um produto a partir do seu ID. O controller `PriceController` faz uma requisição HTTP para uma API de preços externa e retorna o resultado para o front-end.

### Exemplo de uso do endpoint

- **GET** `/Price/{productId}`  
  Retorna o preço do produto informado.

