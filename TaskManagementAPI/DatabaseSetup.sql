-- Create database
CREATE DATABASE IF NOT EXISTS TaskManagementDB;

USE TaskManagementDB;

-- Create Users table
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT NOW()
);

-- Create Tasks table
CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NULL,
    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT NOW(),
    UpdatedAt DATETIME NOT NULL DEFAULT NOW(),
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Insert initial users
INSERT IGNORE INTO Users (Username, Password, CreatedAt)
VALUES ('admin', 'password123', NOW());

INSERT IGNORE INTO Users (Username, Password, CreatedAt)
VALUES ('user', 'password123', NOW());

-- Insert sample tasks (assign to admin user, assuming Id=1)
INSERT IGNORE INTO Tasks (Title, Description, IsCompleted, CreatedAt, UpdatedAt, UserId)
VALUES ('Complete Project Documentation', 'Write comprehensive documentation for the task management application', FALSE, NOW(), NOW(), 1);

INSERT IGNORE INTO Tasks (Title, Description, IsCompleted, CreatedAt, UpdatedAt, UserId)
VALUES ('Implement User Authentication', 'Add user registration and login functionality', TRUE, NOW(), NOW(), 1);

INSERT IGNORE INTO Tasks (Title, Description, IsCompleted, CreatedAt, UpdatedAt, UserId)
VALUES ('Design User Interface', 'Create responsive UI for desktop and mobile devices', FALSE, NOW(), NOW(), 1);

SELECT 'Database setup completed successfully!' AS Message;
SELECT 'Default users:' AS Message;
SELECT '  - Username: admin, Password: password123' AS Message;
SELECT '  - Username: user, Password: password123' AS Message; 