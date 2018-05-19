
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FieldConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FieldConfiguration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SourceColumnName] [varchar](50) NULL,
	[ReferenceTableName] [varchar](50) NULL,
	[ReferenceColumnName] [varchar](50) NULL,
	[TableConfigId] [int] NULL,
	[IsDisplay] [bit] NULL,
 CONSTRAINT [PK_FieldConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FieldConfiguration_TableConfiguration]') AND parent_object_id = OBJECT_ID(N'[dbo].[FieldConfiguration]'))
ALTER TABLE [dbo].[FieldConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_FieldConfiguration_TableConfiguration] FOREIGN KEY([TableConfigId])
REFERENCES [dbo].[TableConfiguration] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FieldConfiguration_TableConfiguration]') AND parent_object_id = OBJECT_ID(N'[dbo].[FieldConfiguration]'))
ALTER TABLE [dbo].[FieldConfiguration] CHECK CONSTRAINT [FK_FieldConfiguration_TableConfiguration]
GO


