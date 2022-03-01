# VStore_API


#### Resumo

- Comecei porque queria estudar ASP.NET, principalmente APIs e convergiu que um amigo que estava estudando front-end queria uma API pra consumir, unindo ambos os projetos, iniciei uma API para uma loja virtual genérica e a coisa foi escalando demais para um iniciante na tecnologia, sistema de autenticação, encriptação de senha, banco em cloud e chegando a finalmente hospedagem da API, foi mais difícil do que eu achei que seria mas me sinto orgulhoso de ter conseguido sozinho sem seguir tutorial, quebrando muito a cara, vendo muita coisa dando errado mas aprendendo bastante no caminho.



### Funcionalidades e desafios

- CRUD de usuário, produtos e pedidos.
- Todas as 3 tabelas com suas relações, tanto para consultas quanto para alterações.
- Autenticação e autorização utilizando JWT bearer e sistemas de “roles” com camadas de autorização.
- Ferramenta de encriptação AES com chave gerada com SHA256 para senhas e informações sensíveis.
- Banco PostgreSQL já hospedado em cloud (Heroku).
- Gerenciamento de versões de development e em produção.
- Informações sensíveis configuradas a partir da VPS, como connection strings e chaves de hash.



### Instruções de execução

- Após o clone do projeto você precisa criar um `appsettings.json` na raiz com as seguintes variáveis:

```json
"DATABASE_URL": "",
"JWT_HASH": "",
"AES_KEY": "",
"AES_IV": ""
```

- `DATABASE_URL` Sendo sua connection string com o banco.
- `JWT_HASH` Sua chave privada para gerar os JWTs, recomendo utilizar SHA256 para gerar sua chave.
- `AES_KEY` Sua chave privada para geração e recuperação de informações usadas com algoritmo AES, recomendo utilizar SHA256 para gerar sua chave.
- `AES_IV` Algoritmos utilizando AES precisando de um vetor de bytes iniciais, você o coloca aqui.
- Após as variáveis configuradas, basta navegar até a raiz do projeto e executar o seguinte comando para rodar localmente.

```ps1
dotnet run
```
