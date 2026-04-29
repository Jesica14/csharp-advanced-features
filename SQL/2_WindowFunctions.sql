-- Window functions calculate values across rows without collapsing them.

USE HotelDb;
GO

-- ROW_NUMBER(): Assigns a sequential number to each row
SELECT Id,CheckIn, ROW_NUMBER() OVER (
        ORDER BY CheckIn
    ) AS RowNum
FROM Reservations;
GO

-- PARTITION BY: Resets numbering per group (per GuestId)

SELECT GuestId, CheckIn, ROW_NUMBER() OVER (
        PARTITION BY GuestId
        ORDER BY CheckIn
    ) AS ReservationNumber
FROM Reservations;
GO

-- Running total: Accumulates values across rows
SELECT CheckIn, TotalPrice, SUM(TotalPrice) OVER (
        ORDER BY CheckIn
    ) AS RunningTotal
FROM Reservations;
GO

-- LAG(): Accesses previous row's value
SELECT GuestId, CheckIn, TotalPrice, LAG(TotalPrice) OVER (
        PARTITION BY GuestId
        ORDER BY CheckIn
    ) AS Previous
FROM Reservations;
GO