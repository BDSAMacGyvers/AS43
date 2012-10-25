
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/22/2012 16:11:34
-- Generated from EDMX file: C:\Users\Ellen\Documents\Visual Studio 2012\Projects\BDSA\AS41\AS41\AS41\Benchmarking\SchedulingBenchmarking\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BenchmarkDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DbLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DbLogs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DbLogs'
CREATE TABLE [dbo].[DbLogs] (
    [jobId] int  NOT NULL,
    [timeStamp] datetime  NOT NULL,
    [jobState] nvarchar(max)  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [user] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'DbLogs'
ALTER TABLE [dbo].[DbLogs]
ADD CONSTRAINT [PK_DbLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------