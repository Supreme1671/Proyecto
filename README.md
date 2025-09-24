# Netflix de Libros

Proyecto fullstack que simula un **Netflix de libros**: un front en React para mostrar libros y categorías, y un backend en C# (.NET 8) que se conecta a una base de datos MySQL para guardar la información de libros, categorías y usuarios.

---

## Estructura del proyecto
Proyecto/
├─ Backend/ # API en C#
│ ├─ Controllers/ # Controladores de API (Libros, Categorías, Usuarios)
│ ├─ Data/ # Contexto de Entity Framework
│ ├─ Modelos/ # Modelos de datos (Libro, Usuario, Categoria)
│ └─ Program.cs # Configuración del backend
├─ Frontend/ # Frontend en React
│ ├─ src/
│ │ ├─ components/ # Componentes React (Navbar, Categoria, BookCard)
│ │ ├─ pages/ # Páginas principales (PaginaPrincipal, LibroDetalle, etc.)
│ │ └─ styles/ # Archivos CSS de los componentes
│ └─ public/ # Archivos estáticos (portadas, PDFS)


---

## Cómo funciona

1. **Base de datos**  
   - La base de datos MySQL se llama `NetflixLibrosBD`.
   - Tablas principales:
     - `Libro` → almacena título, autor, descripción, portada, PDF y categoría.
     - `Categoria` → almacena las categorías de los libros.
     - `Usuario` → opcional, para registrar usuarios.
   - Las relaciones:
     - Cada libro tiene un `CategoriaId` que apunta a `Categoria.Id`.

2. **Backend (C#/.NET 8)**
   - Usa **Entity Framework Core** para conectarse a MySQL.
   - Controladores:
     - `LibrosController` → obtiene todos los libros o un libro por ID.
     - `CategoriasController` → obtiene todas las categorías.
     - `UsuariosController` → obtiene la lista de usuarios.
   - Configuración:
     - `Program.cs` establece la conexión a MySQL, agrega CORS y los controladores.

3. **Frontend (React)**
   - **Navbar** → visible en todas las páginas.
   - **PaginaPrincipal.js** → muestra todos los libros obtenidos de la API.
   - **Categoria.js** → muestra todas las categorías desde la base de datos.
   - **BookCard.js** → componente para mostrar libros individualmente.
   - Estilos CSS tipo Netflix y responsive.

4. **Carga de libros**
   - Los libros deben agregarse en la base de datos (`INSERT INTO Libro ...`).
   - Cada libro tiene una portada y un PDF, que se colocan en `public/portadas/` y `public/pdfs/` respectivamente.
   - El frontend obtiene los datos de la API y genera dinámicamente las tarjetas.

---

## Instalación y uso

### Backend

1. Ir a la carpeta `Backend`:
   ```bash
   cd Backend
2. Restaurar paquetes NuGet:
   ```bash
   dotnet restore
3. Crear la base de datos en MySQL Workbench:
   ```bash
   CREATE DATABASE NetflixLibrosBD;
