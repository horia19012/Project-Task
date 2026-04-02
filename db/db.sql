IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'device_management_db')
BEGIN
    CREATE DATABASE device_management_db;
END
GO

USE device_management_db;
GO

IF OBJECT_ID('devices', 'U') IS NULL
BEGIN
    CREATE TABLE devices (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(255) NOT NULL,
	    manufacturer NVARCHAR(255) NOT NULL,
        type NVARCHAR(255) NOT NULL,
        operating_system NVARCHAR(255) NOT NULL,
        os_version NVARCHAR(255) NOT NULL,
        processor NVARCHAR(255) NOT NULL,
        ram NVARCHAR(50) NOT NULL,
        description NVARCHAR(255) NOT NULL
    );
END
GO

IF OBJECT_ID('users', 'U') IS NULL
BEGIN
    CREATE TABLE users (
        id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(255) NOT NULL,
        role NVARCHAR(255) NOT NULL,
        location NVARCHAR(255) NOT NULL
    );
END
GO

IF COL_LENGTH('devices', 'userId') IS NULL
BEGIN
    ALTER TABLE devices
    ADD userId INT NULL;

    ALTER TABLE devices
    ADD CONSTRAINT FK_devices_users
    FOREIGN KEY (userId) REFERENCES users(id);
END
GO


IF COL_LENGTH('users', 'email') IS NULL
BEGIN
    ALTER TABLE users
    ADD email NVARCHAR(255) NULL;
END
GO

IF COL_LENGTH('users', 'password_hash') IS NULL
BEGIN  
    ALTER TABLE users
    ADD password_hash NVARCHAR(255) NULL;
END

