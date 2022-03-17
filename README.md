# VStore_API

## Resumo

- Comecei porque queria estudar ASP.NET, principalmente APIs e convergiu que um amigo que estava estudando front-end queria uma API pra consumir, unindo ambos os projetos, iniciei uma API para uma loja virtual genérica e a coisa foi escalando demais para um iniciante na tecnologia, sistema de autenticação, encriptação de senha, banco em cloud e chegando a finalmente hospedagem da API, foi mais difícil do que eu achei que seria mas me sinto orgulhoso de ter conseguido sozinho sem seguir tutorial, quebrando muito a cara, vendo muita coisa dando errado mas aprendendo bastante no caminho.


## Funcionalidades e desafios

- CRUD de usuário, produtos e pedidos.
- Todas as 3 tabelas com suas relações, tanto para consultas quanto para alterações.
- Autenticação e autorização utilizando JWT bearer e sistemas de “roles” com camadas de autorização.
- Ferramenta de encriptação AES com chave gerada com SHA256 para senhas e informações sensíveis.
- Banco PostgreSQL já hospedado em cloud (Heroku).
- Gerenciamento de versões de development e em produção.
- Informações sensíveis configuradas a partir da VPS, como connection strings e chaves de hash.
- Conteinerização com docker pois queria usar o heroku como hospedagem para API tambem porém ele não aceita C# nativamente.
- Docker composer para gerenciar o conteiner e algumas variaveis de ambiente como as portas que os heroku exporta dinamicamente via load balancer, acredito ser NGINX.

## Dependências
- #### Execução local
	- `ASP.NET-Core 5`
	- `PostgreSQL`
	- Todas as libs já estão inclusas no projeto.
- #### Publicação
	- Heroku CLI.
	- Docker e Docker Compose.
	- Uma distro linux é recomendada, pode ser WSL2.

## Instruções de execução
- Após o clone do projeto você precisa criar um arquivo chamado `.env` na raiz com as seguintes variáveis:

```.env
ENV=DEV
DATABASE_URL_DEV=
DATABASE_URL=
JWT_HASH=
AES_KEY=
AES_IV=
```
> Poderá seguir o exemplo do arquivo .env-example

- `DATABASE_URL` Sendo sua connection string com o banco.
- `JWT_HASH` Sua chave privada para gerar os JWTs, recomendo utilizar SHA256 para gerar sua chave.
- `AES_KEY` Sua chave privada para geração e recuperação de informações usadas com algoritmo AES, recomendo utilizar SHA256 novamente.
- `AES_IV` Algoritmos utilizando AES precisando de um vetor de bytes iniciais, você o coloca aqui.

Após as variáveis configuradas, navegue até a raiz do projeto e atualize as tabelas do seu banco.

```bash
dotnet ef database update
```

Agora podemos executar o projeto localmente

```bash
dotnet run
```

## Publicando
__IMPORTANTE__. Para utilizar `PostgreeSQL` com a aplicação publicada, você irá precisar um banco hospedado, o heroku tem uma ferramente muito fácil para isso, é um addon, procure sobre. Em seguida configure as variaveis de ambiente no seu `.env` com a connection string do seu banco hospedado antes de criar sua imagem docker

### ASP.NET core 🚀
Vamos compilar o projeto com o comando.

```bash
dotnet publish -o ./dist
```

> __IMPORTANTE__. Mova o .env da pasta raiz para a past /dist, agora pode gerar sua imagem docker.

### Docker 🐳

Opcionalmente baixe a imagem base na versão correta com o comando: 

> O heroku por padrão não aceita projetos C#, porém ele aceita containers 

```bash
docker pull bitnami/aspnet-core:5
```

__IMPORTANTE__ antes de criar sua imagem você deve mover o arquivo  `.env` preenchido com suas variaveis dentro da pasta `/dist` ou você deve configura-las dentro do heroku posteriormente, as variaveis configuradas no heroku tem prioridade sobre as do `.env`

- __Exemplo:__

![ Heroku env variables ](URL da imagem)

Em seguida gere um build da sua imagem

```bash
docker-compose build
```

### Heroku 

Faça login no heroku

```bash 
heroku login && heroku container:login
```

> Caso não tenha criado sua aplicação no heroku ainda

```bash 
heroku apps:create [app]
```

Finalmente, faça o deploy

```bash 
heroku container:push web -a [your app name]
```

E então o release

```bash
heroku container:release web -a [your app name]
```

Agora se tudo correu bem, você pode utilizar o comando 

```bash
heroku open
```

Se aparecer uma tela como essa, parabéns, sua API está publicada 😎😁

![ HelloWorldPage ](URL da imagem)
