SELECT DISTINCT UserId FROM Projects
WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);


SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Projects', 'AspNetUsers') AND COLUMN_NAME IN ('UserId', 'Id');

UPDATE Projects
SET UserId = NULL
WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);



DECLARE @ValidUserId NVARCHAR(450) = 'DIN-GILTIGA-ANVÄNDARE-ID';

UPDATE Projects
SET UserId = @ValidUserId
WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);


SELECT * FROM Projects WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);

SELECT TOP 1 Id, UserName FROM AspNetUsers;

UPDATE Projects
SET UserId = '1'
WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);


-- Kontrollera duplicat
SELECT ClientName, COUNT(*) 
FROM Clients 
GROUP BY ClientName 
HAVING COUNT(*) > 1;
-- Ta bort duplicat manuellt
DELETE FROM Clients 
WHERE Id NOT IN (
    SELECT MIN(Id)
    FROM Clients 
    GROUP BY ClientName
);

SELECT DISTINCT UserId 
FROM Projects 
WHERE UserId IS NOT NULL 
  AND UserId NOT IN (SELECT Id FROM AspNetUsers);

  -- Först, hitta en giltig användare
SELECT TOP 1 Id FROM AspNetUsers;

-- Byt ut 'VALID-USER-ID-HERE' nedan mot en riktig ID
UPDATE Projects
SET UserId = 'VALID-USER-ID-HERE'
WHERE UserId NOT IN (SELECT Id FROM AspNetUsers);
