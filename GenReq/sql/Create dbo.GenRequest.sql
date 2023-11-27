USE [trywiz]
GO

/****** Object: Table [dbo].[GenRequest] Script Date: 11/27/2023 3:05:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GenRequest] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [OwningUserId]     NVARCHAR (MAX)  NOT NULL,
    [Title]            NVARCHAR (1024) NOT NULL,
    [CreatedDate]      DATETIME2 (7)   NOT NULL,
    [GeneratedDate]    DATETIME2 (7)   NULL,
    [Actor]            NVARCHAR (1024) NOT NULL,
    [Status]           NVARCHAR (MAX)  NOT NULL,
    [GeneratedTitle]   NVARCHAR (MAX)  NULL,
    [GeneratedContent] NVARCHAR (MAX)  NULL,
    [ContentTemplate]  INT             NOT NULL
);


