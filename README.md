# 🌳 Banco Arboleda — PC2 SI806 (C# / ASP.NET Core)

## ⚙️ Requisito único
- **.NET 8 SDK** — [descargar aquí](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## 🚀 Ejecutar el proyecto (1 solo comando)

```bash
cd BancoArboleda
dotnet run
```

Abre el navegador en: **http://localhost:5000**

> Primera vez: `dotnet run` descarga los paquetes automáticamente (~30 seg).

---

## 🔑 Credenciales de prueba

| Campo | Valor |
|-------|-------|
| **Usuario** | `cliente01` |
| **Contraseña** | `clave123` |
| **Clave online** | `online456` |

---

## 🗄️ Base de datos

**No requiere configuración.** Los datos se guardan en un archivo JSON local:
`BancoArboleda/Data/recargas.json` (se crea automáticamente).

---

## 📁 Estructura

```
BancoArboleda/
├── Program.cs              Configuración y arranque
├── BancoArboleda.csproj    Dependencias
├── appsettings.json        JWT secret
├── Models/Models.cs        Entidades y DTOs
├── Data/DataStore.cs       Almacenamiento en memoria + JSON
├── Controllers/
│   ├── AuthController.cs   POST /api/auth/login
│   └── RecargaController.cs POST /api/recarga/procesar
└── wwwroot/
    └── index.html          Frontend completo (HTML+CSS+JS)
```

---

## 🔗 Subir al repositorio GitHub

```bash
cd BancoArboleda
git init
git add -A
git commit -m "feat: Banco Arboleda PC2 - C# ASP.NET Core"
git branch -M main
git remote add origin https://github.com/Mati-az/banco-arboleda-pc2.git
git push -u origin main
```

---

## 🛡️ Seguridad implementada
- Contraseñas y clave online con BCrypt
- Autenticación JWT (8 horas)
- Validaciones en frontend Y backend
- Sin SQL → sin posibilidad de SQL Injection
- Prevención de doble envío
- Errores internos no expuestos al cliente
