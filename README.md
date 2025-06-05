# Courier Management

## Opis projektu
Courier Management to aplikacja przeznaczona do zarządzania procesami logistycznymi w firmach kurierskich. Umożliwia monitorowanie przesyłek, zarządzanie zadaniami kurierów oraz administrowanie danymi klientów, przesyłek i kurierów.

### Główne funkcjonalności:
1. **Panel klienta:**
   - Podgląd aktualnych paczek przypisanych do klienta.
   - Szczegółowe informacje o statusie przesyłki (np. w drodze, doręczona).

2. **Panel kuriera:**
   - Wyświetlanie listy przesyłek przypisanych do danego kuriera.

3. **Panel administratora:**
   - Podgląd wszystkich danych (klientów, kurierów, przesyłek).
   - Możliwość edycji niektórych danych.

## Technologie
  - .NET 8
  - Windows Forms
  - MySQL

### Wymagania:
- Zainstalowane środowisko .NET 8.
- Dostęp do bazy danych MySQL.
- Narzędzie do zarządzania bazą danych (np. MySQL Workbench).


Skonfiguruj bazę danych:
   - Utwórz nową bazę danych MySQL.
   - Uzupełnij plik `app.config` danymi dostępowymi do bazy danych:
 ```xml
<configuration>
	<connectionStrings>
		<add name="CourierDbConnection"
			 connectionString="Server=localhost;Database=database_name;Uid=root;Pwd=your_password;"
			 providerName="MySql.Data.MySqlClient" />
	</connectionStrings>
</configuration>
 ```

## Użycie
1. Zaloguj się na odpowiedni panel (klient, kurier, administrator).
2. Korzystaj z funkcji zarządzania paczkami, przeglądaj szczegóły i aktualizuj dane.
