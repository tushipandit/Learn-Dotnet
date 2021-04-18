USE [master]
GO
/****** Object:  Database [LogoFieldDb]    Script Date: 18-04-2021 12:11:49 ******/
CREATE DATABASE [LogoFieldDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LogoFieldDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\LogoFieldDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LogoFieldDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\LogoFieldDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [LogoFieldDb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LogoFieldDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LogoFieldDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LogoFieldDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LogoFieldDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LogoFieldDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LogoFieldDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [LogoFieldDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [LogoFieldDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LogoFieldDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LogoFieldDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LogoFieldDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LogoFieldDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LogoFieldDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LogoFieldDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LogoFieldDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LogoFieldDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LogoFieldDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LogoFieldDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LogoFieldDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LogoFieldDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LogoFieldDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LogoFieldDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LogoFieldDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LogoFieldDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LogoFieldDb] SET  MULTI_USER 
GO
ALTER DATABASE [LogoFieldDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LogoFieldDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LogoFieldDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LogoFieldDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LogoFieldDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LogoFieldDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [LogoFieldDb] SET QUERY_STORE = OFF
GO
USE [LogoFieldDb]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 18-04-2021 12:11:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[SEOKeyword] [nvarchar](max) NULL,
	[SEOUrl] [nvarchar](max) NULL,
	[IsAlive] [bit] NULL,
	[showinmenu] [bit] NULL,
	[createdon] [datetime] NULL,
	[modifiedon] [datetime] NULL,
	[ImageId] [bigint] NULL,
	[SEOTitle] [nvarchar](max) NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupon]    Script Date: 18-04-2021 12:11:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupon](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsAlive] [bit] NULL,
	[showinmenu] [bit] NULL,
	[SEOKeyword] [nvarchar](max) NULL,
	[SEOUrl] [nvarchar](50) NULL,
	[createdon] [datetime] NULL,
	[modifiedon] [datetime] NULL,
	[SEOTiltle] [nvarchar](50) NULL,
	[Couponid] [nvarchar](50) NULL,
 CONSTRAINT [PK_Coupon] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Imagestore]    Script Date: 18-04-2021 12:11:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Imagestore](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Isalive] [bit] NULL,
	[createdon] [datetime] NULL,
	[modifiedon] [datetime] NULL,
	[photourl] [nvarchar](max) NULL,
	[CategoryId] [bigint] NULL,
	[SubcateogoryId] [bigint] NULL,
	[ProductId] [bigint] NULL,
	[serviceId] [bigint] NULL,
 CONSTRAINT [PK_Imagestore] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 18-04-2021 12:11:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[SEOKeyword] [nvarchar](max) NULL,
	[SEOUrl] [nvarchar](max) NULL,
	[IsAlive] [bit] NULL,
	[createon] [datetime] NULL,
	[modifiedon] [datetime] NULL,
	[skucode] [nvarchar](max) NULL,
	[itemcode] [nvarchar](max) NULL,
	[brandname] [nvarchar](max) NULL,
	[features] [nvarchar](max) NULL,
	[stockstaus] [bit] NULL,
	[stockquantity] [bigint] NULL,
	[minquantity] [bigint] NULL,
	[price_mrp] [decimal](18, 2) NULL,
	[price_mrp_display] [bit] NULL,
	[showonhome] [bit] NULL,
	[tax_percentage] [decimal](18, 2) NULL,
	[ImageId] [bigint] NULL,
	[SEOTitle] [nvarchar](max) NULL,
	[serviceId] [bigint] NULL,
	[couponid] [nvarchar](max) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[services]    Script Date: 18-04-2021 12:11:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[services](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](50) NULL,
	[SEOKeyword] [nvarchar](50) NULL,
	[SEOUrl] [nvarchar](50) NULL,
	[IsAlive] [bit] NULL,
	[showinmenu] [nvarchar](50) NULL,
	[createdon] [datetime] NULL,
	[modifiedon] [datetime] NULL,
	[SEOTitle] [nvarchar](50) NULL,
	[ImageId] [bigint] NULL,
	[SubCatId] [bigint] NULL,
 CONSTRAINT [PK_services] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sub_Category]    Script Date: 18-04-2021 12:11:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sub_Category](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[SEOKeyword] [nvarchar](max) NULL,
	[SEOUrl] [nvarchar](max) NULL,
	[IsAlive] [bit] NULL,
	[showinmenu] [bit] NULL,
	[createdon] [datetime] NULL,
	[modifiedon] [datetime] NULL,
	[ImageId] [bigint] NULL,
	[CategoryId] [bigint] NULL,
	[SEOTitle] [nvarchar](max) NULL,
 CONSTRAINT [PK_Sub_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category_createdon]  DEFAULT (getdate()) FOR [createdon]
GO
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category_modifiedon]  DEFAULT (getdate()) FOR [modifiedon]
GO
ALTER TABLE [dbo].[Coupon] ADD  CONSTRAINT [DF_Coupon_createdon]  DEFAULT (getdate()) FOR [createdon]
GO
ALTER TABLE [dbo].[Coupon] ADD  CONSTRAINT [DF_Coupon_modifiedon]  DEFAULT (getdate()) FOR [modifiedon]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_createon]  DEFAULT (getdate()) FOR [createon]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_modifiedon]  DEFAULT (getdate()) FOR [modifiedon]
GO
ALTER TABLE [dbo].[Sub_Category] ADD  CONSTRAINT [DF_Sub_Category_createdon]  DEFAULT (getdate()) FOR [createdon]
GO
ALTER TABLE [dbo].[Sub_Category] ADD  CONSTRAINT [DF_Sub_Category_modifiedon]  DEFAULT (getdate()) FOR [modifiedon]
GO
ALTER TABLE [dbo].[Imagestore]  WITH CHECK ADD  CONSTRAINT [FK_Imagestore_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Imagestore] CHECK CONSTRAINT [FK_Imagestore_Category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Imagestore] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Imagestore] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Imagestore]
GO
ALTER TABLE [dbo].[services]  WITH CHECK ADD  CONSTRAINT [FK_services_Imagestore] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Imagestore] ([Id])
GO
ALTER TABLE [dbo].[services] CHECK CONSTRAINT [FK_services_Imagestore]
GO
ALTER TABLE [dbo].[services]  WITH CHECK ADD  CONSTRAINT [FK_services_Sub_Category] FOREIGN KEY([SubCatId])
REFERENCES [dbo].[Sub_Category] ([Id])
GO
ALTER TABLE [dbo].[services] CHECK CONSTRAINT [FK_services_Sub_Category]
GO
ALTER TABLE [dbo].[Sub_Category]  WITH CHECK ADD  CONSTRAINT [FK_Sub_Category_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Sub_Category] CHECK CONSTRAINT [FK_Sub_Category_Category]
GO
ALTER TABLE [dbo].[Sub_Category]  WITH CHECK ADD  CONSTRAINT [FK_Sub_Category_Imagestore] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Imagestore] ([Id])
GO
ALTER TABLE [dbo].[Sub_Category] CHECK CONSTRAINT [FK_Sub_Category_Imagestore]
GO
USE [master]
GO
ALTER DATABASE [LogoFieldDb] SET  READ_WRITE 
GO
