FROM bitnami/aspnet-core:5

WORKDIR /app

LABEL version="1.0" maintainer="berg"

COPY ./dist .

RUN useradd -m myappuser
USER myappuser

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet VStoreAPI.dll
