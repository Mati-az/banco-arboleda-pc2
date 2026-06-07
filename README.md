# Banco Arboleda — PC2

Sistema web bancario — Módulo de Recarga Celular  

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Python 3

---

## Levantar el Backend

```bash
cd backend
dotnet run
```

API disponible en: **http://localhost:5000**

---

## Levantar el Frontend

```bash
cd frontend
python -m http.server 3000
```

App disponible en: **http://localhost:3000**

---

## Credenciales de prueba

| Campo | Valor |
|-------|-------|
| Usuario | `cliente01` |
| Contraseña | `clave123` |
| Clave online | `online456` |

---


## Estructura del Proyecto en GitHub

Link del proyecto en GitHub: **https://github.com/Mati-az/banco-arboleda-pc2**

```
banco-arboleda-pc2/
├── backend/                    C# ASP.NET Core — Puerto 5000
│   ├── BancoArboleda.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Controllers/
│   │   ├── AuthController.cs       POST /api/auth/login
│   │   └── RecargaController.cs    POST /api/recarga/procesar
│   ├── Models/Models.cs
│   └── Data/DataStore.cs
└── frontend/                   HTML + CSS + JS — Puerto 3000
    └── index.html
```

---

## Base de datos

No requiere configuración. Las recargas se guardan en `backend/Data/recargas.json` automáticamente.
