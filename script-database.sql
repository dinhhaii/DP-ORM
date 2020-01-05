create database dam
GO
USE [dam]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 1/5/2020 10:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](30) NULL,
	[Password] [varchar](30) NULL,
	[organizationId] [bigint] NULL,
	[teamId] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClientDetail]    Script Date: 1/5/2020 10:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[address] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Organization]    Script Date: 1/5/2020 10:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Organization_Team]    Script Date: 1/5/2020 10:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization_Team](
	[Organization_Id] [bigint] NOT NULL,
	[Team_Id] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Organization_Id] ASC,
	[Team_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Team]    Script Date: 1/5/2020 10:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (1, N'cao', N'123', 1, 2)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (2, N'thanh', N'132', 2, 2)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (3, N'hai', N'123', 3, 1)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (4, N'thanh hai', N'12', 1, 1)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (5, N'hai thanh', N'123', 1, 1)
SET IDENTITY_INSERT [dbo].[Client] OFF
SET IDENTITY_INSERT [dbo].[ClientDetail] ON 

INSERT [dbo].[ClientDetail] ([Id], [address]) VALUES (1, N'Dormitory')
SET IDENTITY_INSERT [dbo].[ClientDetail] OFF
SET IDENTITY_INSERT [dbo].[Organization] ON 

INSERT [dbo].[Organization] ([Id], [Name]) VALUES (1, N'Organization 1')
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (2, N'Organization 2')
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (3, N'Organization 3')
SET IDENTITY_INSERT [dbo].[Organization] OFF
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (1, 1)
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (1, 2)
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (1, 3)
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (3, 1)
SET IDENTITY_INSERT [dbo].[Team] ON 

INSERT [dbo].[Team] ([Id], [Name]) VALUES (1, N'Team 1')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (2, N'Team 2')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (3, N'Team 3')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (4, N'123')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (5, N'123')
SET IDENTITY_INSERT [dbo].[Team] OFF
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_CLIENT_Organization] FOREIGN KEY([organizationId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_CLIENT_Organization]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_CLIENT_TEAM] FOREIGN KEY([teamId])
REFERENCES [dbo].[Team] ([Id])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_CLIENT_TEAM]
GO
ALTER TABLE [dbo].[ClientDetail]  WITH CHECK ADD  CONSTRAINT [FK_CLIENTDETAIL_CLIENT] FOREIGN KEY([Id])
REFERENCES [dbo].[Client] ([Id])
GO
ALTER TABLE [dbo].[ClientDetail] CHECK CONSTRAINT [FK_CLIENTDETAIL_CLIENT]
GO
ALTER TABLE [dbo].[Organization_Team]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONTEAM_ORGANIZATION] FOREIGN KEY([Organization_Id])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Organization_Team] CHECK CONSTRAINT [FK_ORGANIZATIONTEAM_ORGANIZATION]
GO
ALTER TABLE [dbo].[Organization_Team]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONTEAM_TEAM] FOREIGN KEY([Team_Id])
REFERENCES [dbo].[Team] ([Id])
GO
ALTER TABLE [dbo].[Organization_Team] CHECK CONSTRAINT [FK_ORGANIZATIONTEAM_TEAM]
GO
