# Poc.Order
Este projeto tem o objetivo de resolver um desafio de desenvolvimento utilizando a tecnologia .NET e boas práticas de programação.

## Escopo
É uma solução para gerenciar e calcular os produtos de um pedido. No caso, os pedidos serão enviados por sistema externo chamado A, a solução recebe esse pedido, efetua o calculo de imposto e envia o mesmo para o Sistema externo B.

A volumetria levantada é que essa solução receberá aproximadamente 200 mil pedidos por dia.

A solução deve ser de fácil manutenção no calculo de imposto, possibilidade de ativar/desativar os calculos de imposto.

## Tecnologias
- .NET 8
- MongoDB
- RabbitMQ

## Diagrama de Container
![Diagrama de Container](docs/Poc.Order.Diagram.Container.png)

### Decisões
No desenvolvimento, foi escolhido a arquitetura de microserviços junto com arquitetura limpa, para isolar e focar melhor no dominio da solução e atender a volumetria atual do requisito, além de ser uma arquitetura escalável quando a volumetria aumentar.

Idealmente, nessa arquitetura, seria melhor a criação de um repositório para cada microserviço, porém como é um conceito, os mesmos ficaram no mesmo local.

Teremos o microserviço *Poc.Order.Api* responsável por receber o pedido, validar campos, salvar no banco de dados (MongoDB) e disponibilizar os dados para consulta. O microserviço *Poc.Order.Processor* terá a única responsabilidade de realizar o calculo de imposto, e disponibilizar o resultado.

### Como executar
*em construção*

### Próximos passos
- Implementar resiliência
- Implementar **rediness probe**
- Implementar fila para publicação dos resultados processados (Comunicação assíncrona com sistemas externos)