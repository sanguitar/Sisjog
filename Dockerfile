# Usa a imagem oficial do SQL Server 2022
FROM mcr.microsoft.com/mssql/server:2022-lts

# Define variáveis de ambiente necessárias
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=nightrain

# Expondo a porta padrão do SQL Server
EXPOSE 1433
