INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES (NEWID(), 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, 'HASHED_PASSWORD', NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0);

SELECT Id FROM AspNetRoles WHERE NormalizedName = 'ADMIN';

INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('USER_ID', 'ROLE_ID');
