# maloprodajnu platofrmu Demo (.NET + React)

Ovaj projekt predstavlja **maloprodajnu platofrmu**  implementiranu pomoću **.NET-a (backend)** i **React-a (frontend)**. Fokus je na dvije ključne funkcionalnosti servisa: 

* Auth  – autentifikacija i autorizacija korisnika (JWT)
* Cart (košarica) – upravljanje korisničkom košaricom
* Cart (košarica) – upravljanje korisničkom košaricom
* Layered arhitecture backend

Sustav koristi **Supabase** kao vanjsku bazu podataka, a aplikacija se može pokrenuti **lokalno** ili joj pristupiti preko linka: https://webshopabysalto.onrender.com


#### Implementirane funkcionalnosti
1. autentifikacija i autorizacija preko JWT tokena kao anonimni korisnik
2. display proizvoda
3. dodavanje proizvoda u košaricu
4. micanje proizvoda iz košarice
5. entiteti u bazi: Customer, Address, Category, Product, Inventory, Cart, ProductInCart, Order
6. promjene u bazi koristeći migrations
7. minimalni UI sa komponentama: Sidemenu za display kategorija, Category za display proizvoda, Home, Cart za display košarice
8. dodavanje poizvoda klikom na košaricu ispod svakog poizvoda, pregled košarice klikom na košaricu u gornjem desnom kutu; kreira se anonimni JWT token pri dodavanju prvog proizvoda u košaricu 
---

## Preduvjeti

Za lokalno pokretanje potrebno je imati:

* **.NET SDK** 
* **Node.js** 
* **npm** ili **yarn**

---

## Pokretanje aplikacije 

### .NET Backend

```bash
cd WebShop
 dotnet restore
 dotnet run
```

#### Preko Docker
```bash
cd WebShop
 docker build -t my-dotnet-app .   
 docker run -d -p 5001:80 my-dotnet-app
```

Backend aplikacija se pokreće na:

```
http://localhost:5001
```

### React Frontend

```bash
cd webshop-react
npm install
npm run dev
```

Frontend aplikacija se pokreće na:

```
http://localhost:3000
```

