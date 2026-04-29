-- Pagination returns a subset of rows instead of the full result.

USE HotelDb;
GO

-- Parameters
DECLARE @Page INT = 1;
DECLARE @PageSize INT = 5;

-- Basic pagination
-- Page 1 = first 5 rows
-- Page 2 = next 5 rows
SELECT Id, CheckIn, TotalPrice
FROM Reservations
ORDER BY CheckIn
OFFSET (@Page - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;
GO

-- Change page
DECLARE @Page INT = 2;
DECLARE @PageSize INT = 5;

SELECT Id, CheckIn, TotalPrice
FROM Reservations
ORDER BY CheckIn
OFFSET (@Page - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;
GO

-- Total records and pages
DECLARE @PageSize INT = 5;
SELECT
    COUNT(*) AS TotalRecords,
    CEILING(COUNT(*) * 1.0 / @PageSize) AS TotalPages
FROM Reservations;
GO