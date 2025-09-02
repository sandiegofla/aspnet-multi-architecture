# ASP.NET Multi-Project Solution (WebForms + MVC + Web API + WCF)
**Repository name:** `aspnet-multi-architecture`

> Monorepo contendo quatro projetos integrados para demonstrar diferentes arquiteturas ASP.NET:  
> **WCFServiceHost**, **WebAPI**, **WebForm**, **WebMVC**.  


---

## Sumário
- [Arquitetura e projetos](#arquitetura-e-projetos)
- [Requisitos](#requisitos)
- [Como rodar](#como-rodar)
  - [1) WCFServiceHost](#1-wcfservicehost)
  - [2) WebAPI](#2-webapi)
  - [3) WebForm](#3-webform)
  - [4) WebMVC](#4-webmvc)
- [Swagger/OpenAPI](#swaggeropenapi)
- [Estrutura da solução](#estrutura-da-solução)
- [Configurações (appSettings, connection strings)](#configurações-appsettings-connection-strings)
- [Fluxo de integração sugerido](#fluxo-de-integração-sugerido)
- [Scripts úteis (Git, .gitignore)](#scripts-úteis-git-gitignore)
- [Problemas comuns](#problemas-comuns)
- [Licença](#licença)

---

## Arquitetura e projetos

### WCFServiceHost
- **Função**: expor operações SOAP/HTTP via serviços WCF (ex.: `ClienteService.svc`).
- **Consumo**: utilizado pelo **WebForm** (via `Service Reference`) e pode ser usado por outros clientes.

### WebAPI
- **Função**: endpoints REST (JSON) para CRUD e integrações.
- **Swagger**: pode ser habilitado para documentação e teste dos endpoints.

### WebForm (ASP.NET Web Forms)
- **Função**: UI clássica (aspx) consumindo **WCF** (e opcionalmente **WebAPI**).
- **Exemplos**: GridView, Repeater, WebMethods + AJAX, validações.

### WebMVC (ASP.NET MVC)
- **Função**: UI MVC consumindo **WebAPI** (HttpClient) e/ou diretamente o **WCF**.
- **Exemplos**: Controllers, Views (Razor), ViewModels.

---

## Requisitos
- **Windows + Visual Studio 2019/2022** (com workloads .NET).
- **.NET Framework 4.8** para **WebForms**, **WCF** e **MVC**.
- **ASP.NET Web API (Framework)** *ou* **ASP.NET Core Web API** (conforme o template usado).
- **SQLite/SQL Server** (se aplicável).

---

## Como rodar

### 1) WCFServiceHost
1. Defina **WCFServiceHost** como **Startup Project**.  
2. Verifique `web.config` do WCF: bindings, endpoints e base addresses.  
3. Start (IIS Express ou IIS Local).  

### 2) WebAPI
1. Ajuste `appSettings`/`appsettings.json`.  
2. Start o projeto **WebAPI**.  
3. Teste endpoint `GET /api/health` (ou equivalente).  
4. **Swagger**: veja a seção abaixo.  

### 3) WebForm
1. Adicione referência ao WCF: `Add Service Reference` → URL do `.svc`.  
2. Configure páginas (`Default.aspx`) para consumir o service client.  
3. Rode e valide grids/listagens.  

### 4) WebMVC
1. Configure `appSettings` com a URL da **WebAPI**.  
2. No controlador, use `HttpClient` apontando para a API.  
3. Rode e navegue nas Views.  

---

## Swagger/OpenAPI

### ASP.NET Web API (.NET Framework)
```powershell
Install-Package Swashbuckle.Core -Version 5.6.0

Estrutura da solução

WebApplication.Solution/
├─ WCFServiceHost/
├─ WebAPI/
├─ WebForm/
├─ WebMVC/
└─ WebApplication.Solution.sln


Fluxo de integração sugerido

WebForm → consome WCF.

WebMVC → consome WebAPI via HttpClient.

WebAPI → expõe endpoints REST.

WCF → mantém compatibilidade com clientes legados.
