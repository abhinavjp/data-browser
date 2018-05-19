
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FieldMappingConfiguration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FieldMappingConfiguration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MapTableName] [varchar](50) NULL,
	[MapColumnName] [varchar](50) NULL,
	[FieldConfigurationId] [int] NULL,
 CONSTRAINT [PK_FieldMappingConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FieldMappingConfiguration_FieldConfiguration]') AND parent_object_id = OBJECT_ID(N'[dbo].[FieldMappingConfiguration]'))
ALTER TABLE [dbo].[FieldMappingConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_FieldMappingConfiguration_FieldConfiguration] FOREIGN KEY([FieldConfigurationId])
REFERENCES [dbo].[FieldConfiguration] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FieldMappingConfiguration_FieldConfiguration]') AND parent_object_id = OBJECT_ID(N'[dbo].[FieldMappingConfiguration]'))
ALTER TABLE [dbo].[FieldMappingConfiguration] CHECK CONSTRAINT [FK_FieldMappingConfiguration_FieldConfiguration]
GO


