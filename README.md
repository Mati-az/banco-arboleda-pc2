# Banco Arboleda — PC2 SI806

Sistema web bancario — Módulo de Recarga Celular  
**Curso:** Desarrollo Adaptativo e Integración de Software — SI806  
**Universidad:** Universidad Nacional de Ingeniería — 2026-1

---

## Requisito único

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## Ejecutar el proyecto

```bash
cd banco-arboleda-pc2
dotnet run
```

Abrir el navegador en: **http://localhost:5000**

> La primera vez descarga los paquetes automáticamente (~30 seg).  
> No se requiere instalar base de datos ni ninguna configuración adicional.

---

## Credenciales de prueba

| Campo | Valor |
|-------|-------|
| Usuario | `cliente01` |
| Contraseña | `clave123` |
| Clave online | `online456` |

---

## Historia de Usuario implementada

**HU-01 — Recarga de Celular**  
*Como cliente bancario autenticado, quiero recargar saldo a un número de celular ingresando el número, seleccionando el operador, el monto y confirmando con mi clave online, para completar la operación de forma segura desde el portal web del banco.*

### Flujo (5 pasos):
1. Ingresar número de celular (9 dígitos, inicia con 9)
2. Seleccionar operador (Movistar, Claro, Entel, Bitel)
3. Seleccionar o ingresar monto (S/5 a S/500)
4. Confirmar con clave online
5. Pantalla de éxito: **"Recarga realizada con éxito."**

---

## Estructura del proyecto

```
BancoArboleda/
├── Program.cs                  Configuración y arranque del servidor
├── BancoArboleda.csproj        Dependencias NuGet
├── appsettings.json            Configuración JWT
├── Models/
│   └── Models.cs               Entidades y DTOs
├── Data/
│   └── DataStore.cs            Almacenamiento en memoria + archivo JSON
├── Controllers/
│   ├── AuthController.cs       POST /api/auth/login
│   └── RecargaController.cs    POST /api/recarga/procesar
└── wwwroot/
    └── index.html              Frontend completo (HTML + CSS + JS)
```

---

## Base de datos

No requiere configuración externa. Las recargas se persisten automáticamente en `Data/recargas.json`. El usuario de prueba se crea en memoria al iniciar.

---

## Seguridad

- Contraseñas y clave online hasheadas con **BCrypt**
- Autenticación mediante **JWT** (válido 8 horas)
- Validaciones en frontend **y** backend
- Prevención de doble envío
- Sin SQL → sin posibilidad de SQL Injection

---

## Arquitectura

Proyecto único **ASP.NET Core** que actúa como backend (API REST) y sirve el frontend (HTML/CSS/JS estático). Las capas están separadas en el código:

- **Backend** → `Controllers/`, `Models/`, `Data/`
- **Frontend** → `wwwroot/index.html`

La comunicación entre capas se realiza vía HTTP (`fetch` al API), igual que si estuvieran en servidores distintos.