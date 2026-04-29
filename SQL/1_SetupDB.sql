USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HotelDb')
    CREATE DATABASE HotelDb;
GO

USE HotelDb;
GO

-- Drop tables (if they exist)
IF OBJECT_ID('Reservations','U') IS NOT NULL DROP TABLE Reservations;
IF OBJECT_ID('Rooms','U') IS NOT NULL DROP TABLE Rooms;
IF OBJECT_ID('Guests','U') IS NOT NULL DROP TABLE Guests;
GO

-- Tables
CREATE TABLE Guests (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    CreatedAt DATE NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Rooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Number NVARCHAR(10) NOT NULL UNIQUE,
    Type NVARCHAR(20) NOT NULL, -- Standard, Deluxe, Suite
    PricePerNight DECIMAL(10,2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1
);

CREATE TABLE Reservations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    GuestId INT NOT NULL REFERENCES Guests(Id),
    RoomId INT NOT NULL REFERENCES Rooms(Id),
    CheckIn DATE NOT NULL,
    CheckOut DATE NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Notes NVARCHAR(500)
);
GO

-- Seed data
INSERT INTO Guests (FullName,Email,Phone) VALUES
('Jesica Villalpando','jesica@hotel.com','5512345678'),
('Ana Torres','ana@hotel.com','5587654321'),
('Luis Mendoza','luis@hotel.com','5599887766'),
('Carlos Ruiz','carlos@hotel.com','5511223344'),
('Maria Lopez','maria@hotel.com','5544332211'),
('Pedro Sanchez','pedro@hotel.com',NULL),
('Sofia Reyes','sofia@hotel.com','5566778899'),
('Diego Herrera','diego@hotel.com','5577889900');

INSERT INTO Rooms (Number,Type,PricePerNight) VALUES
('101','Standard',800.00),
('102','Standard',850.00),
('103','Standard',800.00),
('201','Deluxe',1500.00),
('202','Deluxe',1600.00),
('203','Deluxe',1550.00),
('301','Suite',3000.00),
('302','Suite',3200.00);

INSERT INTO Reservations (GuestId,RoomId,CheckIn,CheckOut,TotalPrice,Notes) VALUES
(1,1,'2025-01-05','2025-01-08',2400.00,'Early check-in requested'),
(1,4,'2025-03-10','2025-03-12',3000.00,NULL),
(1,7,'2025-06-01','2025-06-05',12000.00,'Anniversary trip'),
(2,2,'2025-02-14','2025-02-16',1700.00,'Valentine weekend'),
(2,5,'2025-04-20','2025-04-23',4800.00,NULL),
(3,3,'2025-01-15','2025-01-17',1600.00,NULL),
(3,6,'2025-05-10','2025-05-12',3100.00,NULL),
(4,1,'2025-02-01','2025-02-03',1600.00,NULL),
(4,8,'2025-07-04','2025-07-07',9600.00,'Independence Day'),
(5,2,'2025-03-20','2025-03-22',1700.00,NULL),
(5,4,'2025-08-15','2025-08-18',4500.00,NULL),
(6,3,'2025-04-10','2025-04-11',800.00,NULL),
(7,5,'2025-09-01','2025-09-05',6400.00,NULL),
(7,7,'2025-11-20','2025-11-23',9000.00,NULL),
(8,6,'2025-10-10','2025-10-12',3100.00,NULL);
GO

SELECT
'Setup complete' AS Status,
(SELECT COUNT(*) FROM Guests) AS Guests,
(SELECT COUNT(*) FROM Rooms) AS Rooms,
(SELECT COUNT(*) FROM Reservations) AS Reservations;
GO