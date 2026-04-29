-- A transaction groups multiple statements into one unit of work.
-- Either everything succeeds (COMMIT) or everything fails (ROLLBACK).

USE HotelDb;
GO

-- COMMIT example (safe)
BEGIN TRANSACTION;

DECLARE @Email1 NVARCHAR(150) = CONCAT('simple_', NEWID(), '@hotel.com');

INSERT INTO Guests (FullName, Email)
VALUES ('Simple Guest', @Email1);

COMMIT;
GO

-- ROLLBACK example (handled with TRY/CATCH): Forces an error and rolls back everything

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Email2 NVARCHAR(150) = CONCAT('test_', NEWID(), '@hotel.com');

    INSERT INTO Guests (FullName, Email)
    VALUES ('Test', @Email2);

    -- Force duplicate error
    INSERT INTO Guests (FullName, Email)
    VALUES ('Test 2', @Email2);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    SELECT 'Rolled back successfully' AS Result,
           ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO

-- TRY / CATCH example (successful transaction): Normal flow without errors
BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Email3 NVARCHAR(150) = CONCAT('safe_', NEWID(), '@hotel.com');

    INSERT INTO Guests (FullName, Email)
    VALUES ('Safe Guest', @Email3);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    SELECT ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO