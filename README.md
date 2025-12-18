﻿# Proyecto Boletería
# 🏫 E.T. N°12 D.E. 1° “Libertador Gral. José de San Martín”
## 💻 Computación 2025

### 📚 Asignaturas:
- Base de datos  
- Laboratorio de programación orientada a objetos  
- Proyecto Informático II  
- Análisis de sistemas  

---

### 👥 Integrantes del grupo:
- Enzo Casimiro
- Leonel Fernández
-  Algañaras Diego

---

📅 **Año:** 2025  
🏫 **Escuela Técnica N°12 D.E. 1° “Libertador Gral. José de San Martín”**  
🔧 **Especialidad:** Computación
🧑‍🏫 **Asignaturas involucradas:** Base de Datos, Laboratorio de Programación Orientada a Objetos, Proyecto Informático II, Análisis de Sistemas

---

### 🎯 Objetivo del Proyecto

Este proyecto consiste en el desarrollo de una **Web API en .NET** para la gestión completa de un sistema de eventos: locales, sectores, eventos, funciones, tarifas, clientes, órdenes de compra y emisión/validación de **entradas con código QR**.

El trabajo integra conceptos de:
- Diseño y normalización de base de datos
- Modelado de entidades y relaciones
- Programación orientada a objetos
- Implementación de servicios y repositorios
- Endpoints RESTful
- Autenticación/Login
- Generación y validación de códigos QR

---

### 🧾 Funcionalidades principales

**📍 Gestión de Eventos**
- Creación, listado, detalle y actualización de Locales, Sectores, Eventos, Funciones y Tarifas.
- Publicación y cancelación de eventos y funciones.

**🎟️ Entradas y Órdenes**
- Creación de órdenes de compra y reserva de stock.
- Pago de órdenes y emisión automática de entradas.
- Anulación de entradas.

**🔐 Autenticación**
- Registro y login de usuarios (con token JWT).
- Gestión de roles.

**📷 Códigos QR**
- Generación del QR de cada entrada.
- Validación en puerta (estado: Ok, YaUsada, Anulada, FirmaInvalida, NoExiste).
- Cambio automático del estado de la entrada al primer escaneo válido.

--- 

### 🚀 Tecnologías utilizadas
- C# / .NET 8
- ASP.NET Web API
- Dapper / MySQL
- Swagger
- QRCoder
- JWT Authentication

--- 

### 🚀 Cómo ejecutar este proyecto en otra computadora
A continuación se detallan los pasos necesarios para **clonar, configurar y ejecutar** esta Web API en una computadora nueva que tenga **VS Code, Git, extensiones de C#, y .NET 9 SDK** instalados.

--- 

### 1. Requisitos previos
Asegurate de tener instalado:
**📌 .NET SDK 9**
Descargar desde:
- https://dotnet.microsoft.com/

Comprobar instalación:
```json
dotnet --version
```

Debe mostrar algo como:
```json
9.x.x
```

### 📌 VS Code + Extensiones
- C# Dev Kit
- C# (oficial de Microsoft)
- NuGet Package Manager
- SQL y MySQL (si usás base de datos)
--- 
### 🧩 2. Clonar el proyecto
En la carpeta donde quieras guardarlo:

```json
git clone https://github.com/Supreme1671/Proyecto.git
```

Luego entrar a la carpeta:
```json
cd ProyectoAPI
```
---

### 📦 3. Restaurar paquetes NuGet
En la raíz del proyecto:
```json 
dotnet restore
```
Esto descarga todas las dependencias necesarias.

---

### ⚙️ 4. Configurar la base de datos (si corresponde)
Si el proyecto usa MySQL, SQL Server u otro, asegurate de:
1. Tener la base de datos instalada.
2. Crear la base con tu script SQL.
3. Configurar la cadena de conexión en **appsettings.json**.
Ejemplo:


```json
"ConnectionStrings": {
  "MySqlConnection": "Server=localhost;Database=EventosBD;User=root;Password=1234;"
}
```
---

### 🔑 5. Configurar variables del QR
En appsettings.json:   
```json
"Qr": {
  "Key": "Trigge3rs!"
}
```
---
### ▶️ 6. Ejecutar el proyecto
En la carpeta donde está el **.csproj** de la API:
```bash
dotnet run
```

---

### 🌐 7. Abrir Swagger
Cuando la API arranque, te mostrará algo así:
```json
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5240
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\Lab20-PC02\Videos\Proyecto\src\Csharp\ProyectoAPI
```

Entrar a tu navegador:
```json
http://localhost:5240/swagger
```
Desde ahí podes:
- Crear locales, funciones, clientes, etc.
- Pagar órdenes → genera entradas
- Generar QR por entrada → /entradas/{id}/qr
- Validar QR escaneado → /qr/validar

---

### 📷 8. ¿Cómo probar el QR?

1. Consumir el endpoint:

```bash
GET /entradas/{idEntrada}/qr
```
2. Guardar la imagen PNG.
3. Escanear con el celular o computadora.
4. El valor del QR se envía al endpoint:
```bash
POST /qr/validar?qrContent=...
```
El sistema responde:
- **Ok** → QR válido, entrada marcada como Usada
- **YaUsada** → QR escaneado de nuevo
- **FirmaInvalida** → QR modificado
- **NoExiste** → No coincide

---

### 🧹 9. Limpieza y rebuild (si algo falla)
```bash
dotnet clean
dotnet build
```

---

### 🎉 10. Proyecto listo para usarse
Con estos pasos, cualquier persona puede:
- clonar el repo
- ejecutar la API
- usar Swagger
- generar y validar QR
- integrar con la base de datos

---

### 📩 Contacto
Cualquier consulta puede realizarse a través del correo institucional o por los canales indicados por los docentes.