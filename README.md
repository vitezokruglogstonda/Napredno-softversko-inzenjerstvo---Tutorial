# ASP.NET Core 

ASP.NET Core ima mogućnosti rada na različitim platformama, vrhunskih performansi, jednostavne integracije sa modernim alatima i bibliotekama, kao i dugoročne stabilnosti koju garantuje podrška kompanije Microsoft.
Ovo resenje omogucava razvoj savremenih veb aplikacija koje su brze, fleksibilne i pouzdane.
Za razliku od npr. Spring Boot-a, nudi superiornije performanse, i laksu integraciju sa Microsoft-ovim servisima poput Azure-a, i mnogih drugih.

## Kljucne karakteristike ASP.NET Core-a

- Kros-platformska podrška: Omogućava razvoj i pokretanje aplikacija na Windows, Linux i macOS platformama.

- Visoke performanse: Jedan od najbržih web frejmvorka zahvaljujući optimizacijama poput Kestrel servera.

- Modularni dizajn: Korisnici biraju samo potrebne komponente, što omogućava lakšu konfiguraciju i manji otisak aplikacije.

- Ugrađena podrška za Dependency Injection: Olakšava implementaciju i upravljanje zavisnostima u aplikaciji.

- Integracija sa modernim alatima: Podrška za alate poput Swagger-a za API dokumentaciju, Entity Framework Core-a za rad sa bazom i SignalR-a za real-time komunikaciju.

- Bezbednost: Ugrađene funkcionalnosti za autentifikaciju, autorizaciju i zaštitu podataka putem HTTPS-a i JWT tokene.

- Jednostavno testiranje: Prilagođen za unit i integration testove zahvaljujući modularnosti i odvojenoj poslovnoj logici.

## Kada izabrati ASP.NET Core

ASP.NET Core je idealan izbor za razvoj modernih web aplikacija koje zahtevaju visoke performanse, skalabilnost i fleksibilnost. Sa mogućnošću pokretanja na različitim platformama i širokom podrškom za integraciju sa Microsoft servisima, ovaj frejmvork je posebno pogodan za timove koji traže dugoročnu stabilnost i podršku. Pogodan je za različite vrste aplikacija, uključujući web sajtove, RESTful API-jeve, mikroservise i cloud rešenja.

- Kros-platformska kompatibilnost: Mogućnost rada na Windows-u, Linux-u i macOS-u omogućava fleksibilnost u izboru infrastrukture.

- Visoke performanse: Optimizovan za brzinu i efikasnost, zahvaljujući Kestrel serveru i podršci za napredne tehnologije poput gRPC-a.

- Modularnost i prilagodljivost: Korisnici mogu birati samo one komponente koje su im potrebne, smanjujući kompleksnost aplikacije.

- Ugrađena podrška za moderne web tehnologije: Real-time komunikacija sa SignalR-om, podrška za API dokumentaciju sa Swagger-om i jednostavna integracija sa front-end alatima poput Angular-a i React-a.

- Jednostavna integracija sa Microsoft servisima: Kompatibilan sa Azure platformom, Application Insights-om, Active Directory-jem i drugim Microsoft alatima.

- Bezbednost: Ugrađeni alati za zaštitu podataka, kao što su HTTPS, autentifikacija pomoću JWT tokena i zaštita od CSRF napada.

- Podrška za mikroservisnu arhitekturu: Omogućava razvoj i skaliranje nezavisnih servisa sa visokom pouzdanošću.

ASP.NET Core je pogodan kada je prioritet kreiranje pouzdanih, skalabilnih i fleksibilnih aplikacija koje mogu da iskoriste moderne tehnologije, visoke performanse i podršku moćnog ekosistema. Sa jasnom strukturom i bogatom podrškom, ASP.NET Core pruža sve što je potrebno za razvoj savremenih softverskih rešenja.

---

# Tutorial aplikacija

Ovaj repozitorijum sadrži primer aplikacije razvijene u ASP.NET Core-u za demonstraciju RESTful API-ja, korišćenje EF Core-a za implementaciju biznis logike i rad sa bazama, kao i Redis kes za privremeno cuvanje podataka.
Aplikacija je dizajnirana da bude skalabilna i bezbedna uz primenu JWT autentifikacije, i pokriva osnovne kocepte razvoja jednog servera.

## Instalacija i setup

- Kloniranje repozitorijuma

- Pre pokretanja potrebno je intalirati:

	- .NET SDK (pozeljno 8.0.0)

    	- https://dotnet.microsoft.com/en-us/download/dotnet/8.0

	- EntityFramework Core

	```bash
	dotnet tool install --global dotnet-ef
	```

	- Odgovarajuci IDE

    	- https://visualstudio.microsoft.com/downloads/

	- Docker

    	- https://www.docker.com/products/docker-desktop/

	- Postman (ili slicna resenja za slanje zahteva ka serveru)
	
    	- https://www.postman.com/downloads/

- Pokrenuti sledece komande u terminalu:

    - Za Redis:
    
	```bash
	docker pull redis
	```
	```bash
	docker run --name my-redis -d -p 6379:6379 redis
	```

	- Za PostgreSQL bazu i PgAdmin:

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
	
    - Ukoliko nisu automatski pokrenuti kontejneri, komande za pokretanje su:

    ```bash
	docker start <ime_kontejnera>
	```

	- Za migraciju baze

	```bash
	dotnet ef migrations add v1
	```
	```bash
	dotnet ef database update
	```

    - Nakon setup-a docker container-a neophodnih za pokretanje i rad aplikacije, opciono se mozete povezati na PgAdmin (IDE za Postgres baze) kroz browser:

    	- URL: localhost:80

		- U Object Explorer-u sa leve strane **desni klik na *Servers*** => **Register** => **Server..**

		- U novom prozoru u kartici *General* ukucajte ime vaseg servera (proizvoljno), a potom u kartici *Connection* unesite sledece podatke:
	
			- Host name/address: postgres-db
			- Port: 5432
			- Maintenance database: exampleDb
			- Username: postgres
			- Password: test

## Pokretanje aplikacije

- Pokrenite aplikaciju koristeći sledeću komandu:

	[![Run](https://img.shields.io/badge/dotnet-run-blue?style=flat-square&logo=.net)](data:text/plain;charset=utf-8;base64,LmRvdG5ldCBydW4K)
	```bash
	dotnet run
	```

- Ili kroz vas IDE

Server je podesen da osluskuje na sledecim portovima:

    -HTTP: 5031

    -HTTPS: 7168

- Nakon uspesnog pokretanja, mozete slati zahteve kroz Postman

# Tehnologije

- Backend: **ASP.NET Core**
- Kes: **Redis**
- Baza: **PostgreSQL**
- **Docker**

# Funkcionalnosti i API pozivi

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

# Zasto ASP.NET Core?

- API pozivi: Kreiranje RESTful API-ja za komunikaciju sa klijentima i pružanje podataka u strukturisanom formatu.

- Obrada zahteva: Efikasno rukovanje HTTP zahtevima i odgovorima, uz podršku za različite HTTP metode (GET, POST, PUT, DELETE).

- Autentifikacija i autorizacija: Implementacija sigurnosnih mehanizama pomoću JWT tokena za autentifikaciju i pravila pristupa za autorizaciju korisnika.

- Biznis logika: Razdvajanje poslovne logike od kontrolera, radi bolje organizacije i skalabilnosti aplikacije.

- Komunikacija sa bazama podataka: Korišćenje Entity Framework Core-a za upravljanje podacima i izvršavanje SQL upita.

- Komunikacija sa klijentom: Olakšavanje interakcije sa front-end aplikacijama, uključujući serijalizaciju i deserializaciju podataka u JSON formatu.

# Zasto EF Core?

Entity Framework Core je korišćen kao ORM alat za rad sa bazom podataka, omogućavajući lakšu i intuitivniju manipulaciju podacima.

- LINQ upiti: Omogućava kreiranje upita prema bazi koristeći C# sintaksu, bez potrebe za pisanjem SQL koda.

- Code First pristup: Podrška za definisanje baze podataka na osnovu C# modela, što olakšava početak razvoja i iterativno dodavanje novih funkcionalnosti.

- Preslikavanje modela na entitete: Automatski kreira tabele i strukturu baze podataka na osnovu definisanih klasa u C#.

- Podesavanje relacija sa Fluent API-jem: Relacije između entiteta mogu se precizno definisati koristeći C# sintaksu, pružajući fleksibilnost i kontrolu nad strukturom baze.

- Podrška za migracije: Omogućava lako praćenje promena u bazi podataka kroz verzionisanje migracija.

# Zasto Redis?

- Brzina i efikasnost: Redis je in-memory baza podataka koja omogućava izuzetno brz pristup podacima, što je ključno za keširanje korisnika.

- Smanjenje opterećenja baze podataka: Keširanjem korisničkih podataka u Redis-u smanjuje se broj zahteva prema glavnoj bazi, čime se povećavaju performanse aplikacije.

- Jednostavna implementacija: Redis nudi jednostavne funkcionalnosti za keširanje, što ga čini odličnim izborom za ovu namenu.

- Kros-platformska podrška: Pokretanjem Redis-a kao Docker image-a osigurava se konzistentnost okruženja na različitim platformama.

- Prilagođenost za autentifikaciju i autorizaciju: Keširanje korisničkih podataka ubrzava procese autentifikacije i autorizacije, jer se informacije brzo čitaju iz Redis-a umesto iz baze.

Korišćenje Redis-a omogućava efikasan i pouzdan način upravljanja keširanim podacima, što direktno doprinosi bržem i skalabilnijem radu aplikacije. Docker image dodatno olakšava podešavanje i pokretanje Redis instance na različitim sistemima.

# Zasto PostgreSQL?

Koristio sam PostgreSQL za čuvanje entiteta i relacija između njih, zahvaljujući njegovim naprednim funkcionalnostima koje omogućavaju efikasno upravljanje podacima.

- Podrška za kompleksne relacije: PostgreSQL nudi napredne mogućnosti rada sa relacijama, što ga čini idealnim za aplikacije sa složenom baznom strukturom.

- ACID kompatibilnost: Garantuje pouzdanost i tačnost podataka čak i u slučaju grešaka ili prekida rada.

- Proširivost: Podrška za ekstenzije, kao što su PostGIS za rad sa prostornim podacima, omogućava prilagodbu specifičnim potrebama aplikacije.

- Performanse: Optimizovan za rukovanje velikim količinama podataka i kompleksnim upitima.

- Podrška za JSON i NoSQL funkcionalnosti: Omogućava skladištenje i obradu polustrukturisanih podataka uz relacione modele.

# Zasto Docker?

Docker sam koristio kako bih izbegao manuelno instaliranje i podešavanje okruženja, omogućavajući jednostavno preuzimanje i pokretanje unapred pripremljenih Docker image-a.

- Jednostavna upotreba: Umesto manuelne instalacije, Docker omogućava brzo preuzimanje i pokretanje image-a sa svim potrebnim zavisnostima.

- Konzistentnost okruženja: Osigurava da aplikacija radi isto na svim sistemima, bez obzira na lokalnu konfiguraciju.

- Izolacija aplikacija: Svaka aplikacija radi u sopstvenom kontejneru, bez konflikta sa drugim procesima na sistemu.

- Podrška za skaliranje: Docker omogućava lako skaliranje aplikacija pokretanjem dodatnih instanci kontejnera.

- Kros-platformska kompatibilnost: Omogućava rad na različitim platformama uz identično okruženje.