# Banco Arboleda — PC2 SI806

Sistema web bancario — Módulo de Recarga Celular  
**Curso:** Desarrollo Adaptativo e Integración de Software — SI806  
**Universidad:** Universidad Nacional de Ingeniería — 2026-1

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org) (para servir el frontend)

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
npm start
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
│   │   ├── AuthController.cs
│   │   └── RecargaController.cs
│   ├── Models/Models.cs
│   └── Data/DataStore.cs
└── frontend/                   HTML + CSS + JS — Puerto 3000
    ├── package.json
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
