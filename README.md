# Napredno-softversko-inzenjerstvo---Tutorial

# ASP.NET Core API

Ovaj repozitorijum sadrži primer API-ja razvijenog u ASP.NET Core-u za demonstraciju RESTful API-ja, korišćenje EF Core-a i JWT autentifikacije.

---

## **API pozivi**

Koristite sledeće API pozive za testiranje u alatu kao što je [Postman](https://www.postman.com/):

| HTTP Metod | Endpoint                  | Opis                       | Request Body               | HTTP headers               |
|------------|---------------------------|----------------------------|----------------------------|----------------------------|
| POST       | `/Account/register`       | Registrovanje naloga       |{"email":<string>, "password":<string>}|
| PUT        | `/Account/log-in`         | Logovanje korisnika        |{"email":<string>, "password":<string>}|
| GET        | `/Account/log-out`        | Odjavljivanje sa naloga    ||JWT: <string>|
| GET        | `/Account/create-admin`   | Kreiranje Admin naloga     |||
| DELETE     | `/api/products/{id}`      | Brisanje proizvoda         |

---

## **Pokretanje aplikacije**

Pokrenite aplikaciju koristeći sledeću komandu:

[![Run](https://img.shields.io/badge/dotnet-run-blue?style=flat-square&logo=.net)](data:text/plain;charset=utf-8;base64,LmRvdG5ldCBydW4K)
```bash
dotnet run
