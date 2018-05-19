IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TableConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TableConfiguration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[DataKey] [varchar](50) NULL,
	[IsView] [bit] NULL,
	[MasterTableName] [varchar](50) NULL,
	[ConnectionId] [int] NULL,
 CONSTRAINT [PK_TableConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TableConfiguration_DataBaseConnection]') AND parent_object_id = OBJECT_ID(N'[dbo].[TableConfiguration]'))
ALTER TABLE [dbo].[TableConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_TableConfiguration_DataBaseConnection] FOREIGN KEY([ConnectionId])
REFERENCES [dbo].[DatabaseConnection] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TableConfiguration_DataBaseConnection]') AND parent_object_id = OBJECT_ID(N'[dbo].[TableConfiguration]'))
ALTER TABLE [dbo].[TableConfiguration] CHECK CONSTRAINT [FK_TableConfiguration_DataBaseConnection]
GO


