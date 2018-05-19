IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DatabaseConnection]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DatabaseConnection](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[ServerInstanceName] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[DatabaseName] [nvarchar](50) NULL,
 CONSTRAINT [PK_DataBaseConnection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


