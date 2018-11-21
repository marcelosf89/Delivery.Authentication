# Delivery Authentication

## Stack

* **.NET Core 2.1**
* **C# Version 7.2**
* **Cassandra DB 3.11.2**

---

## Preview run

Before starting the solution please create the DB to run it.

---

### Cassandra

#### If you don't have cassandra Db you can pull the image in docker and run the container 

```
docker run -d -p 7000:7000 -p 9042:9042 -e CASSANDRA_USER=local -e CASSANDRA_PASSWORD=local --name local-cassandra cassandra:3.11.2
```

#### For access the cqlsh run this command in your command
```
docker exec -it local-cassandra cqlsh
```


#### After install the Cassandra db run this script to create the tables

1. Keyspace
```
CREATE KEYSPACE auth WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1};
```

2. Enter in keyspace
```
USE auth;
```

3. Crate Tables
```
CREATE TABLE users (
  Id uuid,
  Email text,
  Username text,
  FirstName text,
  LastName text,
  Creation timestamp,
  LastAccess timestamp,
  DeletionDate timestamp,
  Phone text,
  Password text,
  Claims list<text>,
  PRIMARY KEY (Id)
);

CREATE TABLE claims (
  Code text,
  Description text,
  IsObsolete boolean,
  PRIMARY KEY (Code)
);

CREATE TYPE identityclaim (
  Type text,
  Value text
);

CREATE TABLE identityClient (
  ClientId text,
  ClientDescription text,
  TimeLife int,
  GrantTypes list<text>,
  RequireClientSecret boolean,
  ClientSecret text,
  AllowAccessInBrowser boolean,
  Scopes list<text>,
  RedirectUris list<text>,
  PostLogoutRedirectUris list<text>,
  Authority list<text>,
  AllowOfflineAccess boolean,
  Claims list<FROZEN<identityclaim>>,
  PRIMARY KEY (ClientId)
);

CREATE TABLE identityapi (
  Code text,
  Description text,
  Claims list<text>,
  PRIMARY KEY (Code)
);
```

4. Insert data
```
INSERT INTO identityClient (ClientId,ClientDescription,TimeLife,GrantTypes,RequireClientSecret,ClientSecret,AllowAccessInBrowser,Scopes,RedirectUris,PostLogoutRedirectUris,Authority,AllowOfflineAccess,Claims) VALUES('delivery','Delivery Client', 60, ['implicit'], false, 'secret', true, ['delivery'], ['https://localhost:5001/signin-oidc', 'https://localhost:5001/swagger/oauth2-redirect.html' ], ['https://localhost:5001/signout-callback-oidc'],['https://localhost:5001'], true, [{Type: 'role', Value: 'Role'}]  );
INSERT INTO identityapi (Code,Description,Claims) VALUES ('delivery', 'Delivery API', ['role']);
INSERT INTO claims (Code, Description, IsObsolete) VALUES ('admin', 'Admin', false);
INSERT INTO users (id, claims, creation, deletiondate, email, firstname, lastaccess, lastname, password, phone, username) VALUES (e7bb08b0-cdbe-4a59-b437-56c1277f1a58, ['admin'] , '2018-11-20', null , 'admin@admin.com' , 'admin', '2018-11-20', 'admin', 'D033E22AE348AEB5660FC2140AEC35850C4DA997', '0211111111' , 'admin');
```

---

### Appsettings

* The Appsettings.json is configured with the default ports, if you want to change these configuration please open the Appsettings.json and change it.
 
```JSON
  "CassandraConnection": {
    "Hosts": [ "localhost" ],
    "Keyspace": "auth",
    "Port": 9042,
    "User": "local",
    "Password": "local"
  }
```

---

## Run Aplication

* The application is configured to show swagger only in `Dedug` configuration
** You can access the swagger with the URL `https://localhost:{port by default is 5001}/swagger`

* The application is configured to show api docs only in `Dedug` configuration
** You can access the api docs with the URL `https://localhost:{port by default is 5001}/api-docs`

---

## Benchmark Report

### UsersController
|                             Method |     Mean |    Error |    StdDev | Allocated |
|----------------------------------- |---------:|---------:|----------:|----------:|
|                      CreateUser_Ok | 3.701 ms | 17.30 ms | 11.446 ms |       0 B |
|          CreateUserUserEmailExists | 3.985 ms | 18.57 ms | 12.283 ms |       0 B |
|           CreateUserUsernameExists | 3.825 ms | 17.96 ms | 11.877 ms |       0 B |
|            UpdateUserUserNotExists | 2.443 ms | 10.93 ms |  7.228 ms |       0 B |
|               GetUserUserNotExists | 2.527 ms | 11.28 ms |  7.459 ms |       0 B |
|              AddClaimUserNotExists | 2.863 ms | 12.83 ms |  8.489 ms |       0 B |
|                DeleteUserNotExists | 2.795 ms | 12.55 ms |  8.298 ms |       0 B |
|                          Delete_Ok | 2.692 ms | 12.51 ms |  8.272 ms |       0 B |
| UpdateUserWithoutChangePassword_Ok | 2.535 ms | 11.80 ms |  7.808 ms |       0 B |
|    UpdateUserWithChangePassword_Ok | 3.605 ms | 16.77 ms | 11.091 ms |       0 B |
|                         GetUser_Ok | 2.383 ms | 11.19 ms |  7.399 ms |       0 B |

### ClaimsController
|                     Method |     Mean |     Error |   StdDev | Allocated |
|--------------------------- |---------:|----------:|---------:|----------:|
|             CreateClaim_Ok | 2.355 ms | 11.087 ms | 7.333 ms |       0 B |
|    CreateClaim_ClaimExists | 2.580 ms | 11.482 ms | 7.595 ms |       0 B |
|    GetClaim_ClaimNotExists | 2.653 ms | 11.915 ms | 7.881 ms |       0 B |
|                GetClaim_Ok | 2.734 ms | 12.785 ms | 8.457 ms |       0 B |
|              SetObsoleteOk | 2.474 ms | 11.740 ms | 7.765 ms |       0 B |
|       SetObsoleteNotExists | 2.026 ms |  9.596 ms | 6.347 ms |       0 B |
| UpdateClaim_ClaimNotExists | 2.483 ms | 11.231 ms | 7.429 ms |       0 B |
|             UpdateClaim_Ok | 2.654 ms | 12.385 ms | 8.192 ms |       0 B |
|

