-- ================================================
-- Insert Specific User into Users Table (Recommended)
-- ================================================
INSERT INTO Users (
    Username, 
    FirstName, 
    LastName, 
    Email, 
    PasswordHash, 
    PhoneNumber, 
    Role, 
    IsApproved, 
    IsActive, 
    IsEmailVerified, 
    EmailVerificationToken, 
    PasswordResetToken, 
    PasswordResetTokenExpiry
)
VALUES 
    (
        'sainadhvatturi', -- Username
        'Sainadh',        -- FirstName
        'Vatturi',        -- LastName
        'sainadhvatturi@gmail.com', -- Email
        '$2a$11$gSxpmTrwPSvT1zFcfJT7Z.5HaVSZ44q9DWu8JBeYh92oGzCUxyl7m', -- PasswordHash
        '+917799383099', -- PhoneNumber
        'Admin',          -- Role
        1,                -- IsApproved
        1,                -- IsActive
        1,                -- IsEmailVerified
        NULL,             -- EmailVerificationToken
        NULL,             -- PasswordResetToken
        NULL              -- PasswordResetTokenExpiry
    );
SELECT * FROM Users;