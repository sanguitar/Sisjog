# Sisjog Backend

API ASP.NET Core 8 organizada nos projetos Domain, Application, Infrastructure e Api.

## Configuração local

A connection string não é versionada. Configure-a no projeto da API com .NET User Secrets:

```powershell
cd Sisjog.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=SisjogDb;User Id=sa;Password=<SENHA_LOCAL>;TrustServerCertificate=True;"
cd ..
```

Também é possível usar a variável de ambiente `ConnectionStrings__DefaultConnection`.

Azure AD é ativado somente quando `TenantId` e `ClientId` estão configurados, portanto não é obrigatório para o CRUD anônimo de consoles. Sem essa configuração, o esquema JwtBearer básico mantém endpoints protegidos inacessíveis. Para habilitar Microsoft Identity corretamente, configure `AzureAd__Domain`, `AzureAd__TenantId` e `AzureAd__ClientId` por User Secrets ou variáveis de ambiente. Não versione esses valores.

## Banco e migrations

O contexto usa SQL Server e a migration existente é `20251107202242_InitialCreate`. Ela cria o banco lógico `SisjogDb`, as tabelas `Consoles` e `Jogos`, a chave estrangeira e o índice de `ConsoleId`.

```powershell
dotnet tool install --global dotnet-ef --version 9.0.10
dotnet ef database update --project Sisjog.Infrastructure --startup-project Sisjog.Api
```

Não foi adicionado seed. O contrato de consoles aceita os estados `Ativo` e `Inativo`, persistidos como enum na coluna `Estado`.

## Execução local

```powershell
dotnet restore Sisjog.sln
dotnet build Sisjog.sln
dotnet run --project Sisjog.Api
```

O perfil principal expõe `https://localhost:7058` e `http://localhost:5287`. O Swagger está disponível em `/swagger` no ambiente Development.

## Docker opcional

Copie `.env.example` para `.env`, defina uma senha SQL Server forte e execute:

```powershell
docker compose config
docker compose up --build
```

No Compose, a API usa `sqlserver` como host do banco. Antes de recriar um contêiner SQL já existente, faça backup dos dados; o volume nomeado protege apenas as execuções criadas com a configuração atual.

## Limitações conhecidas

- O frontend possui telas para Jogos, mas não existe `JogosController` no backend.
- O snapshot da migration ainda contém o nome CLR anterior `Sisjog.Domain.Entities.Console`; a estrutura relacional da migration permanece válida.
