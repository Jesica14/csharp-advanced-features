USE HotelDb;
GO

-- VIEW: Is a saved SELECT query
IF OBJECT_ID('vw_Reservations', 'V') IS NOT NULL DROP VIEW vw_Reservations;
GO

CREATE VIEW vw_Reservations AS
SELECT Id, GuestId, RoomId, CheckIn,TotalPrice
FROM Reservations;
GO

-- Use the view
SELECT * FROM vw_Reservations;
GO

-- SCALAR FUNCTION: Returns a single value
IF OBJECT_ID('fn_TotalPrice', 'FN') IS NOT NULL DROP FUNCTION fn_TotalPrice;
GO

CREATE FUNCTION fn_TotalPrice (
    @PricePerNight DECIMAL(10,2),
    @Nights INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @PricePerNight * @Nights;
END;
GO

-- Use the function
SELECT Number, dbo.fn_TotalPrice(PricePerNight, 3) AS PriceFor3Nights
FROM Rooms;
GO

-- STORED PROCEDURE: Can contain logic and modify data
IF OBJECT_ID('sp_AddGuest', 'P') IS NOT NULL DROP PROCEDURE sp_AddGuest;
GO

CREATE PROCEDURE sp_AddGuest
    @FullName NVARCHAR(100),
    @Email NVARCHAR(150)
AS
BEGIN
    INSERT INTO Guests (FullName, Email)
    VALUES (@FullName, @Email);
END;
GO

-- Use the stored procedure
DECLARE @Email NVARCHAR(150) = CONCAT('user_', NEWID(), '@hotel.com');

EXEC sp_AddGuest
    @FullName = 'New Guest',
    @Email = @Email;
GO