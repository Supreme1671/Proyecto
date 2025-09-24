DROP DATABASE  IF EXISTS NetflixLibrosBD;

CREATE DATABASE NetflixLibrosBD;
USE NetflixLibrosBD;

-- Tabla Usuarios
CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NombreUsuario VARCHAR(100) NOT NULL UNIQUE,
    Email VARCHAR(200) NOT NULL UNIQUE,
    PasswordHash VARCHAR(200) NOT NULL,
    FechaRegistro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabla Categorias
CREATE TABLE Categorias (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL UNIQUE
);

-- Tabla Libros
CREATE TABLE Libros (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(200) NOT NULL,
    Autor VARCHAR(200) NOT NULL,
    UrlPortada VARCHAR(500),
    Descripcion VARCHAR(1000),
    UrlPdf VARCHAR(500),
    CategoriaId INT,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);

-- Tabla Reseñas
CREATE TABLE Reseñas (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT NOT NULL,
    LibroId INT NOT NULL,
    Comentario VARCHAR(1000),
    Puntaje INT CHECK (Puntaje BETWEEN 1 AND 5),
    Fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (LibroId) REFERENCES Libros(Id)
);

INSERT INTO Categorias (Nombre) VALUES
('Infantil'),
('Aventura'),
('Literatura Clásica'),
('Fantasía'),
('Épica');


INSERT INTO Libros (Titulo, Autor, UrlPortada, Descripcion, UrlPdf, CategoriaId) VALUES
('El Principito', 'Antoine de Saint-Exupéry', '/portadas/el-principito.jpeg', 'Un cuento maravilloso sobre la amistad y la imaginación.', '/pdfs/el-principito.pdf', 1),
('20 Mil Leguas de Viaje Submarino', 'Julio Verne', '/portadas/20mil.jpeg', 'Aventura submarina a bordo del Nautilus.', '/pdfs/20mil.pdf', 2),
('Cien Años de Soledad', 'Gabriel García Márquez', '/portadas/cien-anos.jpeg', 'Saga de la familia Buendía en Macondo.', '/pdfs/cien-anos.pdf', 3),
('Orgullo y Prejuicio', 'Jane Austen', '/portadas/orgullo-prejuicio.jpeg', 'Historia de amor y sociedad en Inglaterra.', '/pdfs/orgullo-prejuicio.pdf', 1),
('Don Quijote de la Mancha', 'Miguel de Cervantes', '/portadas/don-quijote.jpeg', 'Las aventuras del ingenioso hidalgo Don Quijote.', '/pdfs/don-quijote.pdf', 2),
('El Señor de los Anillos', 'J.R.R. Tolkien', '/portadas/lotr.jpeg', 'Épica fantasía en la Tierra Media.', '/pdfs/lotr.pdf', 4),
('La Odisea', 'Homero', '/portadas/odisea.jpeg', 'El regreso de Ulises a Ítaca.', '/pdfs/odisea.pdf', 5),
('Crimen y Castigo', 'Fiódor Dostoyevski', '/portadas/crimen-castigo.jpeg', 'La historia de Raskólnikov y su crimen.', '/pdfs/crimen-castigo.pdf', 3),
('El Extraño Caso del Dr. Jekyll y Mr. Hyde', 'Robert Louis Stevenson', '/portadas/dr-jekyll.jpeg', 'Dualidad de la naturaleza humana.', '/pdfs/dr-jekyll.pdf', 1),
('Tres Portugueses Bajo un Paraguas', 'Rodolfo Walsh', '/portadas/tres-portugueses.jpeg', 'Relato policial y de suspenso.', '/pdfs/tres-portugueses.pdf', 2);
