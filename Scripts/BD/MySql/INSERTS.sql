USE EventosBD;
INSERT INTO Local (Nombre, Direccion, Capacidad, Telefono) VALUES
('Cancha Monumental', 'Av. Figueroa Alcorta 7597, CABA', 83000, '011-12345678'),
('Estadio Libertadores de América', 'Av. Pres. Figueroa Alcorta 7597, CABA', 66000, '011-87654321'),
('Estadio Ciudad de La Plata', 'Av. 25 y 32, La Plata, Buenos Aires', 53000, '0221-5555555'),
('Luna Park', 'Av. Eduardo Madero 470, C1106 CABA', 8400, '011-44445555'),
('Estadio Único Diego Armando Maradona', 'Av. Pres. Juan Domingo Perón 3500, La Plata, Buenos Aires', 53000, '0221-66667777');

-- 2. INSERTAR SECTORES
INSERT INTO Sector (Nombre, Descripcion, Capacidad, Precio, idLocal) VALUES
('Platea Baja', 'Platea cerca del escenario', 5000, 2000, 1),
('Platea Alta', 'Platea elevada', 8000, 1500, 1),
('VIP', 'Zona VIP', 500, 5000, 2),
('General', 'Asientos generales', 30000, 1000, 4),
('Tribuna Norte', 'Tribuna norte del estadio', 10000, 1200, 4),
('Palco', 'Palco privado', 200, 6000, 2);
-- 3. INSERTAR CLIENTES

INSERT INTO Cliente (DNI, Nombre, Apellido, Email, Telefono) VALUES
('00000001', 'Fulano', 'Gutierrez', 'fulanogutierrez@gmail.com', '1122334455'),
('00000002', 'Cesar', 'Torres', 'cesar@gmail.com', '2233445566'),
('00000003', 'Alpaka', 'Delvalle', 'alpaka@gmail.com', '3344556677'),
('00000004', 'Hernan', 'Lopez', 'hernan@gmail.com', '4455667788'),
('00000005', 'Tiago', 'Videira', 'tiago@gmail.com', '5566778899');

-- 4. INSERTAR EVENTOS

INSERT INTO Evento (Nombre, Fecha, Tipo, Descripcion, idLocal) VALUES
('Partido de Fútbol', '2025-11-01', 'Deporte', 'Partido oficial de liga', 1),
('Concierto de Pop', '2025-10-20', 'Concierto', 'Presentación en vivo', 1),
('Evento de Box', '2025-09-30', 'Deporte', 'Pelea profesional', 3),
('Festival de Música', '2025-12-25', 'Festival', 'Festival con múltiples artistas', 4);

-- 5. INSERTAR FUNCIONES

INSERT INTO Funcion (Descripcion, FechaHora, idEvento, IdLocal) VALUES
('Concierto Coldplay - Día 1', '2025-11-20 20:00:00', 2, 1),
('Concierto Coldplay - Día 2', '2025-11-21 21:00:00', 2, 1),
('Partido Argentina vs Brasil', '2025-11-30 18:00:00', 1, 2),
('Concierto Taylor Swift', '2025-12-01 20:30:00', 2, 3),
('Evento de Boxeo - Campeonato Mundial', '2025-12-03 21:00:00', 3, 4),
('Obra de teatro - Romeo y Julieta', '2025-12-05 19:30:00', 4, 4),
('Cine - Estreno Avengers 6', '2025-12-10 22:00:00', 4, 4),
('Partido Boca vs River', '2025-12-15 17:00:00', 1, 2);

-- 6. INSERTAR TARIFAS

INSERT INTO Tarifa (Precio, Descripcion, Stock, idSector, IdFuncion, idEvento) VALUES
(25000, 'Platea Baja - Coldplay', 100, 1, 1, 2),
(18000, 'Platea Alta - Coldplay', 150, 2, 2, 2),
(50000, 'Palco VIP - Argentina vs Brasil', 50, 3, 3, 1),
(15000, 'Campo General - Boxeo', 200, 4, 5, 3);

-- 7. INSERTAR ÓRDENES

INSERT INTO Orden (Fecha, Total, Estado, idCliente, NumeroOrden) VALUES
('2025-10-01 15:30:00', 10500.00, 'Pendiente', 1, 1),
('2025-10-02 18:45:00', 10000.00, 'Pagada', 2, 2),
('2025-10-03 20:15:00', 14000.00, 'Anulada', 3, 3),
('2025-10-05 11:00:00', 13000.00, 'Pendiente', 1, 4),
('2025-10-06 19:00:00', 15000.00, 'Pagada', 4, 5);

-- 8. INSERTAR DETALLE DE ÓRDENES

INSERT INTO DetalleOrden (Cantidad, PrecioUnitario, IdOrden, IdEvento, IdTarifa) VALUES
(2, 25000.00, 1, 2, 1),
(1, 18000.00, 2, 2, 2),
(3, 50000.00, 3, 1, 3),
(2, 15000.00, 4, 3, 4);

-- 9. INSERTAR ENTRADAS
INSERT INTO Entrada (Precio, idTarifa, idFuncion, Estado, Usada, Anulada, Numero, idSector, IdDetalleOrden, idCliente) VALUES
(25000, 1, 1, 'Disponible', TRUE, FALSE, '001', 1, 1, 1),
(18000, 2, 2, 'Disponible', TRUE, FALSE, '002', 2, 2, 2),
(50000, 3, 3, 'Disponible', TRUE, FALSE, '003', 3, 3, 3),
(15000, 4, 5, 'Disponible', TRUE, FALSE, '004', 4, 4, 4);

-- 10. EJEMPLO DE CÓDIGO QR
INSERT INTO QR (IdEntrada, Codigo, FechaCreacion, FechaUso) VALUES
(1, 'QR_0001', NOW(), NOW()),
(2, 'QR_0002', NOW(), NOW()),
(3, 'QR_0003', NOW(), NOW()),
(4, 'QR_0004', NOW(), NOW()),
(5, 'QR_0005', NOW(), NOW());

SELECT * FROM `Cliente`

-- 11. CREAR USUARIO ADMIN
INSERT INTO Usuario (NombreUsuario, Email, Contrasena, Roles)
VALUES ('AdminMaster', 'admin@gmail.com', '1234', 'Admin');
