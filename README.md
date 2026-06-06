# Banco Arboleda — PC2 SI806

Sistema web bancario — Módulo de Recarga Celular  

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Python 3 (ya viene instalado en Windows 10/11)

---

## Levantar el Backend (Terminal 1)

```bash
cd backend
dotnet run
```

API disponible en: **http://localhost:5000**

---

## Levantar el Frontend (Terminal 2)

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

## Estructura

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

---

## Seguridad

- Contraseñas hasheadas con BCrypt
- Autenticación JWT (8 horas)
- Validaciones en frontend y backend
- Prevención de doble envío
- Sin SQL → sin SQL Injection