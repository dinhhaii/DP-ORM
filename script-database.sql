IF  NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'dam')
    BEGIN
        CREATE DATABASE [dam]
    END;
GO
USE [dam]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 1/4/2020 8:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Client]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Client](
	[Id] [bigint] NOT NULL,
	[Username] [varchar](30) NULL,
	[Password] [varchar](30) NULL,
	[organizationId] [bigint] NULL,
	[teamId] [bigint] NULL,
	primary key(Id)
)
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ClientDetail]    Script Date: 1/4/2020 8:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClientDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ClientDetail](
	[Id] [bigint] NOT NULL,
	[address] [nvarchar](100) NULL
	primary key(Id)
) 
END
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 1/4/2020 8:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Organization]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Organization](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](30) NULL,
	primary key(Id)
)
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Organization_Team]    Script Date: 1/4/2020 8:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Organization_Team]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Organization_Team](
	[Organization_Id] [bigint] NOT NULL,
	[Team_Id] [bigint] NOT NULL
	primary key([Organization_Id],[Team_Id])
)
END
GO
/****** Object:  Table [dbo].[Team]    Script Date: 1/4/2020 8:57:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Team]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Team](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](30) NULL
	primary key(Id)
)
END
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (0, N'cao', N'123', NULL, NULL)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (1, N'thanh', N'132', NULL, NULL)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (2, N'hai', N'123', NULL, NULL)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (3, N'thanh hai', N'12', 1, 1)
INSERT [dbo].[Client] ([Id], [Username], [Password], [organizationId], [teamId]) VALUES (4, N'hai thanh', N'123', 1, 1)
INSERT [dbo].[ClientDetail] ([Id], [address]) VALUES (3, N'Dormitory')
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (1, N'Organization 1')
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (2, N'Organization 2')
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (3, N'Organization 3')
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (1, 1)
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (1, 2)
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (1, 3)
INSERT [dbo].[Organization_Team] ([Organization_Id], [Team_Id]) VALUES (3, 1)
INSERT [dbo].[Team] ([Id], [Name]) VALUES (1, N'Team 1')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (2, N'Team 2')
INSERT [dbo].[Team] ([Id], [Name]) VALUES (3, N'Team 3')

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CLIENT_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[Client]'))
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_CLIENT_Organization] FOREIGN KEY([organizationId])
REFERENCES [dbo].[Organization] ([Id])

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CLIENT_Organization]') AND parent_object_id = OBJECT_ID(N'[dbo].[Client]'))
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_CLIENT_Organization]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CLIENT_TEAM]') AND parent_object_id = OBJECT_ID(N'[dbo].[Client]'))
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_CLIENT_TEAM] FOREIGN KEY([teamId])
REFERENCES [dbo].[Team] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CLIENT_TEAM]') AND parent_object_id = OBJECT_ID(N'[dbo].[Client]'))
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_CLIENT_TEAM]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CLIENTDETAIL_CLIENT]') AND parent_object_id = OBJECT_ID(N'[dbo].[ClientDetail]'))
ALTER TABLE [dbo].[ClientDetail]  WITH CHECK ADD  CONSTRAINT [FK_CLIENTDETAIL_CLIENT] FOREIGN KEY([Id])
REFERENCES [dbo].[Client] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CLIENTDETAIL_CLIENT]') AND parent_object_id = OBJECT_ID(N'[dbo].[ClientDetail]'))
ALTER TABLE [dbo].[ClientDetail] CHECK CONSTRAINT [FK_CLIENTDETAIL_CLIENT]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ORGANIZATIONTEAM_ORGANIZATION]') AND parent_object_id = OBJECT_ID(N'[dbo].[Organization_Team]'))
ALTER TABLE [dbo].[Organization_Team]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONTEAM_ORGANIZATION] FOREIGN KEY([Organization_Id])
REFERENCES [dbo].[Organization] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ORGANIZATIONTEAM_ORGANIZATION]') AND parent_object_id = OBJECT_ID(N'[dbo].[Organization_Team]'))
ALTER TABLE [dbo].[Organization_Team] CHECK CONSTRAINT [FK_ORGANIZATIONTEAM_ORGANIZATION]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ORGANIZATIONTEAM_TEAM]') AND parent_object_id = OBJECT_ID(N'[dbo].[Organization_Team]'))
ALTER TABLE [dbo].[Organization_Team]  WITH CHECK ADD  CONSTRAINT [FK_ORGANIZATIONTEAM_TEAM] FOREIGN KEY([Team_Id])
REFERENCES [dbo].[Team] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ORGANIZATIONTEAM_TEAM]') AND parent_object_id = OBJECT_ID(N'[dbo].[Organization_Team]'))
ALTER TABLE [dbo].[Organization_Team] CHECK CONSTRAINT [FK_ORGANIZATIONTEAM_TEAM]
GO
