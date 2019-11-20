USE [dam]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 20-Nov-19 19:07:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [bigint] NOT NULL,
	[Username] [nchar](250) NULL,
	[Password] [nchar](250) NULL,
	[Name] [nvarchar](250) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name]) VALUES (1, N'abc                                                                                                                                                                                                                                                       ', N'xyz                                                                                                                                                                                                                                                       ', N'Hau')
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name]) VALUES (2, N'tsd xxxc                                                                                                                                                                                                                                                  ', N'xxx                                                                                                                                                                                                                                                       ', N'Gat')
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name]) VALUES (3, N'eee                                                                                                                                                                                                                                                       ', N'ttt                                                                                                                                                                                                                                                       ', N'Gevde')
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name]) VALUES (4, N'ccc                                                                                                                                                                                                                                                       ', N'vvv                                                                                                                                                                                                                                                       ', N'Eyfd')
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name]) VALUES (5, N'username5                                                                                                                                                                                                                                                 ', N'password5                                                                                                                                                                                                                                                 ', N'HAIDINH')
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name]) VALUES (6, N'username6                                                                                                                                                                                                                                                 ', N'password6                                                                                                                                                                                                                                                 ', N'DINHHAI')
