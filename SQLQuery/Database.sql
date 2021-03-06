USE [dam]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 02-Dec-19 22:31:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [bigint] NOT NULL,
	[Username] [nchar](250) NULL,
	[Password] [nchar](250) NULL,
	[Name] [nvarchar](250) NULL,
	[IdTeam] [bigint] NULL,
	[IdOrganization] [bigint] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 02-Dec-19 22:31:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](250) NULL,
 CONSTRAINT [PK_Yasuo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 02-Dec-19 22:31:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Id] [bigint] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name], [IdTeam], [IdOrganization]) VALUES (1, N'dinhhai                                                                                                                                                                                                                                                   ', N'1                                                                                                                                                                                                                                                         ', N'Team1', 1, 1)
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name], [IdTeam], [IdOrganization]) VALUES (2, N'aaa                                                                                                                                                                                                                                                       ', N'aaa                                                                                                                                                                                                                                                       ', N'Team1', 1, 1)
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name], [IdTeam], [IdOrganization]) VALUES (3, N'bbb                                                                                                                                                                                                                                                       ', N'bbb                                                                                                                                                                                                                                                       ', N'Team2', 2, 2)
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name], [IdTeam], [IdOrganization]) VALUES (4, N'ccc                                                                                                                                                                                                                                                       ', N'ccc                                                                                                                                                                                                                                                       ', N'Team1', 1, 2)
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name], [IdTeam], [IdOrganization]) VALUES (5, N'ddd                                                                                                                                                                                                                                                       ', N'ddd                                                                                                                                                                                                                                                       ', N'Team3', 3, 1)
INSERT [dbo].[Client] ([Id], [Username], [Password], [Name], [IdTeam], [IdOrganization]) VALUES (6, N'eee                                                                                                                                                                                                                                                       ', N'eee                                                                                                                                                                                                                                                       ', N'Team2', 2, NULL)
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (1, N'Meme1')
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (2, N'Meme2')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (1, N'Team1')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (2, N'Team2')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (3, N'Team3')
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_Organization] FOREIGN KEY([IdOrganization])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_Organization]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_Team] FOREIGN KEY([IdTeam], [Name])
REFERENCES [dbo].[Team] ([Id], [Name])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_Team]
GO
