O sistema Gestão de Pedidos é uma aplicação desenvolvida em .NET 8, utilizando o padrão arquitetural CQRS (Command Query Responsibility Segregation) e o mediador MediatR para desacoplamento da lógica de negócios. O objetivo principal é gerenciar pedidos, produtos, clientes e itens de pedido, permitindo operações como cadastro, atualização, consulta e listagem.
---
Principais Componentes
•	Domain: Contém as entidades de negócio, como Order, OrderItem, Product e Customer, além das regras de validação.
•	Application: Implementa os comandos (para escrita) e queries (para leitura), além dos respectivos handlers que processam cada operação.
•	Infrastructure: Responsável pelo acesso a dados, implementando repositórios e contexto do banco de dados.
•	UI: Camada de apresentação, composta por controllers (API) e DTOs para comunicação com o frontend.

Fluxo Geral
1.	Usuário faz uma requisição (ex: criar ou atualizar um pedido) via API.
2.	Controller recebe a requisição e converte para um comando ou query.
3.	MediatR encaminha o comando/query para o handler correspondente.
4.	Handler executa a lógica de negócio, interage com o repositório e retorna o resultado.
5.	Repositório acessa o banco de dados para persistir ou recuperar informações.
6.	Resposta é enviada de volta ao usuário.

![image](https://github.com/user-attachments/assets/905ee4d0-8fa7-4739-8592-27caadc5987a)



Resumo das Responsabilidades
•	Domain: Modela as regras e entidades do negócio.
•	Application: Orquestra comandos, queries e validações.
•	Infrastructure: Gerencia persistência e acesso a dados.
•	UI: Expõe endpoints para interação externa.
