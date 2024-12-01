# Napredno-softversko-inzenjerstvo---Tutorial

# ASP.NET Core API

Ovaj repozitorijum sadrži primer API-ja razvijenog u ASP.NET Core-u za demonstraciju RESTful API-ja, korišćenje EF Core-a i JWT autentifikacije.

---

## **API pozivi**

Koristite sledeće API pozive za testiranje u alatu kao što je [Postman](https://www.postman.com/):

| HTTP Metod | Endpoint                         | Opis										| Request Body										| HTTP headers |
|------------|----------------------------------|-------------------------------------------|---------------------------------------------------|--------------|
| POST       | `/account/register`              | Registrovanje naloga						|{"email":string, "password":string}				|			   |
| PUT        | `/account/log-in`                | Logovanje korisnika						|{"email":string, "password":string}				|			   |
| GET        | `/account/log-out`               | Odjavljivanje sa naloga					|													|JWT: string   |
| GET        | `/account/create-admin`          | Kreiranje Admin naloga					|													|			   |
| GET        | `/item/get-all-items`            | Pribavljanje svih item-a iz baze          |													|			   |
| GET        | `/item/get-users-items/{userId}` | Pribavljanje svih item-a jednog korisnika |													|			   |
| GET        | `/item/get-item/{itemId}`        | Pribavljanje item-a						|													|			   |
| POST       | `/item/publish-item`             | Postavljanje novog item-a					|{"title":string, "description":string}				|JWT: string   |
| PUT        | `/item/change-item`				          | Promena atributa jednog item-a			|{"id":number, "title":string, "description":string}|JWT: string   |
| DELETE     | `/item/delete-item/{itemId}`     | Brisanje item-a							|													|JWT: string   |
| DELETE     | `/item/delete-all-items`			      | Brisanje svih item-a						|													|JWT: string   |

Server je podesen da osluskuje na sledecim portovima:

    -HTTP: 5031

    -HTTPS: 7168

---
## **Docker komande**

Pre pokretanja aplikacije, pokrenite sledece komande u terminalu:

### **Redis**
```bash
docker pull redis
```
```bash
docker run --name my-redis -d -p 6379:6379 redis
```

### **Postgres baza i PgAdmin**
```bash
docker network create pg-network
```
```bash
docker pull postgres
```
```bash
docker run --name postgres-db --network pg-network -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=test -e POSTGRES_DB=exampleDb -p 5432:5432 -d postgres
```
```bash
docker pull dpage/pgadmin4
```
```bash
docker run --name pg-admin --network pg-network -p 80:80 -e 'PGADMIN_DEFAULT_EMAIL=user@domain.com' -e 'PGADMIN_DEFAULT_PASSWORD=password' -d dpage/pgadmin4
```
#### **Povezivanje na bazu iz PgAdmin-a:**
-Otvorite PgAdmin u browser-u: *ukucajte **localhost** u URL-u*.

-U Object Explorer-u sa leve strane **desni klik na *Servers*** => **Register** => **Server..**

-U novom prozoru u kartici *General* ukucajte ime vaseg servera (proizvoljno), a potom u kartici *Connection* unesite sledece podatke:
	
	- Host name/address: postgres-db
	- Port: 5432
	- Maintenance database: exampleDb
	- Username: postgres
	- Password: test


### **Migracija baze**
```bash
dotnet ef migrations add v1
```
```bash
dotnet ef database update
```
Ukoliko nemate instaliran Entity Framework, pokrenite sledecu komandu:
```bash
dotnet tool install --global dotnet-ef
```

---

## **Pokretanje aplikacije**

Pokrenite aplikaciju koristeći sledeću komandu:

[![Run](https://img.shields.io/badge/dotnet-run-blue?style=flat-square&logo=.net)](data:text/plain;charset=utf-8;base64,LmRvdG5ldCBydW4K)
```bash
dotnet run
