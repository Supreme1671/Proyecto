# Netflix Libros

## Descripción

Este proyecto es una **aplicación web tipo Netflix de libros**.  
- **Frontend:** React  
- **Backend:** .NET 8 Web API  
- **Base de datos:** MySQL  
- **ORM:** Entity Framework Core (EF Core)  

Funcionalidades principales:  
- Mostrar libros en el frontend.  
- Consultar y crear usuarios desde la API.  
- Agregar libros a la base de datos mediante la API.

---

## Estructura del proyecto

Proyecto/
│
├─ Backend/NetflixLibrosApi/
│ ├─ Controllers/
│ ├─ Data/
│ ├─ Modelos/
│ ├─ Program.cs
│ ├─ appsettings.json
│ └─ NetflixLibrosApi.csproj
│
├─ Frontend/netflix-libros/
│ ├─ src/
│ │ ├─ components/
│ │ ├─ PaginaPrincipal.jsx
│ │ └─ App.js
│ └─ package.json
│
└─ README.md


---

## Requisitos previos

- Node.js (v18 o superior) y npm  
- .NET SDK 8  
- MySQL Server + MySQL Workbench  

---

## Configuración de la base de datos

1. Abrir MySQL Workbench y conectarse al servidor.  
2. Crear la base de datos:

```sql
CREATE DATABASE NetlfixLibrosBD CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

--- Confirmar que se creó la Base de Datos
SHOW DATABASES;

--- Configurar la conexión a la BD
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=NetlfixLibrosBD;user=TU_USUARIO;password=TU_CONTRASEÑA;"
}
