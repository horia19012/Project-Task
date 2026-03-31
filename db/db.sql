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
        name NVARCHAR(255) NOT NULL UNIQUE,
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