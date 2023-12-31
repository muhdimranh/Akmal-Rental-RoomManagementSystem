USE [master]
GO
/****** Object:  Database [db_rental]    Script Date: 30/6/2023 9:25:29 PM ******/
CREATE DATABASE [db_rental]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'db_rental', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\db_rental.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'db_rental_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\db_rental_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [db_rental] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [db_rental].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [db_rental] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [db_rental] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [db_rental] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [db_rental] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [db_rental] SET ARITHABORT OFF 
GO
ALTER DATABASE [db_rental] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [db_rental] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [db_rental] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [db_rental] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [db_rental] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [db_rental] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [db_rental] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [db_rental] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [db_rental] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [db_rental] SET  DISABLE_BROKER 
GO
ALTER DATABASE [db_rental] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [db_rental] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [db_rental] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [db_rental] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [db_rental] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [db_rental] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [db_rental] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [db_rental] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [db_rental] SET  MULTI_USER 
GO
ALTER DATABASE [db_rental] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [db_rental] SET DB_CHAINING OFF 
GO
ALTER DATABASE [db_rental] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [db_rental] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [db_rental] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [db_rental] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [db_rental] SET QUERY_STORE = ON
GO
ALTER DATABASE [db_rental] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [db_rental]
GO
/****** Object:  Table [dbo].[tb_attendanceCleaner]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_attendanceCleaner](
	[att_id] [int] IDENTITY(1,1) NOT NULL,
	[att_lid] [int] NOT NULL,
	[att_count] [int] NULL,
 CONSTRAINT [PK_tb_attendance] PRIMARY KEY CLUSTERED 
(
	[att_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_attendancedate]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_attendancedate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[attid] [int] NOT NULL,
	[attdate] [date] NULL,
 CONSTRAINT [PK_tb_dateAttendance] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_inventory]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_inventory](
	[i_id] [int] IDENTITY(1,1) NOT NULL,
	[i_locationid] [int] NOT NULL,
	[i_item] [varchar](100) NOT NULL,
	[i_quantity] [int] NOT NULL,
 CONSTRAINT [PK_tb_inventory] PRIMARY KEY CLUSTERED 
(
	[i_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_investor]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_investor](
	[i_id] [int] NOT NULL,
	[i_investment] [float] NOT NULL,
	[i_percent] [float] NOT NULL,
	[i_lot] [int] NOT NULL,
 CONSTRAINT [PK_tb_investor] PRIMARY KEY CLUSTERED 
(
	[i_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_location]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_location](
	[l_id] [int] IDENTITY(1,1) NOT NULL,
	[l_address] [varchar](100) NOT NULL,
	[l_code] [varchar](100) NULL,
	[l_wifi] [varchar](100) NULL,
	[l_modemIP] [varchar](100) NULL,
	[l_cctv] [varchar](100) NULL,
	[l_imglayout1] [varchar](max) NULL,
	[l_reminderDate] [int] NULL,
	[isPaymentMade] [bit] NOT NULL,
 CONSTRAINT [PK_tb_location] PRIMARY KEY CLUSTERED 
(
	[l_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_lot]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_lot](
	[lot_id] [int] IDENTITY(1,1) NOT NULL,
	[lot_name] [varchar](50) NOT NULL,
	[lot_status] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tb_lot] PRIMARY KEY CLUSTERED 
(
	[lot_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_payment]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_payment](
	[p_id] [int] IDENTITY(1,1) NOT NULL,
	[p_tenantid] [int] NOT NULL,
	[p_date] [date] NOT NULL,
	[p_amount] [float] NOT NULL,
	[p_type] [varchar](50) NOT NULL,
	[p_roomNo] [int] NULL,
	[p_locationCode] [varchar](50) NULL,
	[p_receipt] [varchar](max) NULL,
 CONSTRAINT [PK_tb_payment] PRIMARY KEY CLUSTERED 
(
	[p_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_profit]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_profit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[TotalSales] [float] NOT NULL,
 CONSTRAINT [PK_tb_profit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_role]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_role](
	[rl_id] [int] IDENTITY(1,1) NOT NULL,
	[rl_desc] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tb_role] PRIMARY KEY CLUSTERED 
(
	[rl_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_room]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_room](
	[r_id] [int] IDENTITY(1,1) NOT NULL,
	[r_no] [int] NOT NULL,
	[r_tid] [int] NULL,
	[r_locationid] [int] NOT NULL,
	[r_type] [varchar](50) NOT NULL,
	[r_price] [float] NOT NULL,
	[r_depositAmount] [float] NOT NULL,
	[r_desc] [varchar](50) NOT NULL,
	[r_status] [varchar](50) NOT NULL,
	[r_img1] [varchar](max) NULL,
	[r_img2] [varchar](max) NULL,
 CONSTRAINT [PK_tb_room] PRIMARY KEY CLUSTERED 
(
	[r_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_sales]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_sales](
	[s_id] [int] IDENTITY(1,1) NOT NULL,
	[s_date] [date] NOT NULL,
	[s_paymentid] [int] NULL,
	[s_category] [varchar](50) NOT NULL,
	[s_transactionType] [varchar](50) NULL,
	[s_detail] [varchar](50) NOT NULL,
	[s_amountIN] [float] NULL,
	[s_amountOUT] [float] NULL,
	[s_receipt] [varchar](max) NULL,
 CONSTRAINT [PK_tb_sales] PRIMARY KEY CLUSTERED 
(
	[s_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_share]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_share](
	[share_id] [int] IDENTITY(1,1) NOT NULL,
	[share_amount] [float] NOT NULL,
	[share_date] [date] NOT NULL,
	[share_investor] [int] NOT NULL,
 CONSTRAINT [PK_tb_share] PRIMARY KEY CLUSTERED 
(
	[share_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_tenants]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_tenants](
	[t_id] [int] IDENTITY(1,1) NOT NULL,
	[t_name] [varchar](100) NOT NULL,
	[t_ic] [varchar](50) NOT NULL,
	[t_phone] [varchar](50) NOT NULL,
	[t_address] [text] NOT NULL,
	[t_uploadIC] [varchar](max) NULL,
	[t_emerContact] [varchar](50) NOT NULL,
	[t_roomid] [int] NULL,
	[t_entrydate] [datetime] NOT NULL,
	[t_exitdate] [datetime] NULL,
	[t_cardCode] [varchar](50) NOT NULL,
	[t_addOnDetail] [varchar](50) NOT NULL,
	[t_paymentStatus] [varchar](50) NOT NULL,
	[t_agreement] [varchar](max) NULL,
	[LastReminderDate] [datetime] NOT NULL,
	[IsPaymentCollected] [bit] NOT NULL,
	[IsCheckedOut] [bit] NOT NULL,
 CONSTRAINT [PK_tb_tenants] PRIMARY KEY CLUSTERED 
(
	[t_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_user]    Script Date: 30/6/2023 9:25:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_user](
	[u_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](50) NOT NULL,
	[u_pass] [varchar](100) NOT NULL,
	[u_phone] [varchar](50) NOT NULL,
	[u_email] [varchar](50) NOT NULL,
	[u_roleid] [int] NOT NULL,
 CONSTRAINT [PK_tb_user] PRIMARY KEY CLUSTERED 
(
	[u_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tb_attendanceCleaner] ON 

INSERT [dbo].[tb_attendanceCleaner] ([att_id], [att_lid], [att_count]) VALUES (2, 2, 2)
INSERT [dbo].[tb_attendanceCleaner] ([att_id], [att_lid], [att_count]) VALUES (1002, 3, 1)
INSERT [dbo].[tb_attendanceCleaner] ([att_id], [att_lid], [att_count]) VALUES (1003, 7, NULL)
SET IDENTITY_INSERT [dbo].[tb_attendanceCleaner] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_attendancedate] ON 

INSERT [dbo].[tb_attendancedate] ([id], [attid], [attdate]) VALUES (1, 2, CAST(N'2023-06-15' AS Date))
INSERT [dbo].[tb_attendancedate] ([id], [attid], [attdate]) VALUES (2, 2, CAST(N'2023-06-16' AS Date))
INSERT [dbo].[tb_attendancedate] ([id], [attid], [attdate]) VALUES (3, 2, CAST(N'2023-07-03' AS Date))
INSERT [dbo].[tb_attendancedate] ([id], [attid], [attdate]) VALUES (4, 2, CAST(N'2023-06-15' AS Date))
INSERT [dbo].[tb_attendancedate] ([id], [attid], [attdate]) VALUES (5, 2, CAST(N'2023-06-24' AS Date))
INSERT [dbo].[tb_attendancedate] ([id], [attid], [attdate]) VALUES (6, 2, NULL)
INSERT [dbo].[tb_attendancedate] ([id], [attid], [attdate]) VALUES (7, 1002, CAST(N'2023-06-17' AS Date))
SET IDENTITY_INSERT [dbo].[tb_attendancedate] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_inventory] ON 

INSERT [dbo].[tb_inventory] ([i_id], [i_locationid], [i_item], [i_quantity]) VALUES (1, 3, N'bantal', 5)
SET IDENTITY_INSERT [dbo].[tb_inventory] OFF
GO
INSERT [dbo].[tb_investor] ([i_id], [i_investment], [i_percent], [i_lot]) VALUES (1017, 5000, 10, 2)
INSERT [dbo].[tb_investor] ([i_id], [i_investment], [i_percent], [i_lot]) VALUES (1018, 25000, 50, 1)
INSERT [dbo].[tb_investor] ([i_id], [i_investment], [i_percent], [i_lot]) VALUES (1019, 25000, 50, 2)
GO
SET IDENTITY_INSERT [dbo].[tb_location] ON 

INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (1, N'11-1, JALAN DATARAN LARKIN 1, JB', N'DL1', N'1', N'1', N'123432142131', N'2c23c6d1-047a-45da-ae58-f4b13447366c_DL1 harga.jpg', 1, 1)
INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (2, N'18-2, JALAN DATARAN LARKIN 5, JB', N'DL5', N'2', N'2', N'293827132613', N'fccbba59-ad0f-4c55-a15d-e754726304cd_DL5 harga.jpg', 1, 0)
INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (3, N'03-1, JALAN GERODA 2/1, LARKIN', N'GE1', N'3', N'3', N'383723622812', N'44533617-4617-4713-aebc-7d4d04b6bd50_GE1 harga.jpg', 1, 0)
INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (4, N'27-2, JALAN GERODA 2/1, LARKIN', N'GE3', N'4', N'4', N'383743623282', N'7b7bb487-2146-4587-be72-bb5ab3b85833_GE3 harga.jpg', 1, 0)
INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (5, N'20-2, JALAN NUSARIA 11/7, GP', N'TN1', N'5', N'5', N'5', N'02a51b4d-9111-46a2-920c-2a8b05c83580_TN1 harga.jpg', 1, 0)
INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (6, N'20-3, JALAN NUSARIA 11/7, GP', N'TN2', N'6', N'6', N'6', N'debe8dd1-f35b-40dd-aa7e-629577a9a49b_TN2 harga.jpg', 15, 0)
INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (7, N'38-2, JALAN NUSARIA 11/4, GP', N'TN3', N'7', N'7', N'7', N'2c379407-fc4b-4380-a1c3-7d99e742d079_TN3 harga.jpg', 15, 0)
INSERT [dbo].[tb_location] ([l_id], [l_address], [l_code], [l_wifi], [l_modemIP], [l_cctv], [l_imglayout1], [l_reminderDate], [isPaymentMade]) VALUES (8, N'123', N'123', N'123', N'123', N'123', NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[tb_location] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_lot] ON 

INSERT [dbo].[tb_lot] ([lot_id], [lot_name], [lot_status]) VALUES (1, N'Lot 1', N'Free')
INSERT [dbo].[tb_lot] ([lot_id], [lot_name], [lot_status]) VALUES (2, N'Lot 2', N'Free')
INSERT [dbo].[tb_lot] ([lot_id], [lot_name], [lot_status]) VALUES (3, N'Lot Tepi', N'Free')
SET IDENTITY_INSERT [dbo].[tb_lot] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_payment] ON 

INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (39, 85, CAST(N'2023-06-19' AS Date), 1500, N'Credit Card', 1, N'DL1', NULL)
INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (40, 71, CAST(N'2023-07-06' AS Date), 1222, N'Credit Card', 3, N'GE3', NULL)
INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (41, 82, CAST(N'2023-06-15' AS Date), 1760, N'Cash', 4, N'TN2', NULL)
INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (43, 71, CAST(N'2023-06-15' AS Date), 1203, N'Cash', 3, N'GE3', NULL)
INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (44, 71, CAST(N'2023-06-28' AS Date), 503, N'Cash', 3, N'GE3', NULL)
INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (45, 71, CAST(N'2023-08-25' AS Date), 203, N'Cash', 3, N'GE3', NULL)
INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (48, 71, CAST(N'2023-09-21' AS Date), 1203, N'Credit Card', 3, N'GE3', NULL)
INSERT [dbo].[tb_payment] ([p_id], [p_tenantid], [p_date], [p_amount], [p_type], [p_roomNo], [p_locationCode], [p_receipt]) VALUES (50, 84, CAST(N'2023-06-17' AS Date), 1411, N'Cash', 1, N'DL1', NULL)
SET IDENTITY_INSERT [dbo].[tb_payment] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_profit] ON 

INSERT [dbo].[tb_profit] ([Id], [Year], [Month], [TotalSales]) VALUES (13, 2023, 5, 1400)
INSERT [dbo].[tb_profit] ([Id], [Year], [Month], [TotalSales]) VALUES (14, 2023, 6, 154123)
INSERT [dbo].[tb_profit] ([Id], [Year], [Month], [TotalSales]) VALUES (15, 2023, 7, 4822)
INSERT [dbo].[tb_profit] ([Id], [Year], [Month], [TotalSales]) VALUES (16, 2023, 4, 500)
INSERT [dbo].[tb_profit] ([Id], [Year], [Month], [TotalSales]) VALUES (17, 2023, 8, 2203)
INSERT [dbo].[tb_profit] ([Id], [Year], [Month], [TotalSales]) VALUES (18, 2023, 9, 1203)
SET IDENTITY_INSERT [dbo].[tb_profit] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_role] ON 

INSERT [dbo].[tb_role] ([rl_id], [rl_desc]) VALUES (3, N'Admin')
INSERT [dbo].[tb_role] ([rl_id], [rl_desc]) VALUES (4, N'Investor')
SET IDENTITY_INSERT [dbo].[tb_role] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_room] ON 

INSERT [dbo].[tb_room] ([r_id], [r_no], [r_tid], [r_locationid], [r_type], [r_price], [r_depositAmount], [r_desc], [r_status], [r_img1], [r_img2]) VALUES (1, 1, NULL, 1, N'1', 1111, 111, N'1111', N'Not Available', N'f46f809e-4a69-4f23-a4f5-f3545d860f23_Screenshot_2.jpg', NULL)
INSERT [dbo].[tb_room] ([r_id], [r_no], [r_tid], [r_locationid], [r_type], [r_price], [r_depositAmount], [r_desc], [r_status], [r_img1], [r_img2]) VALUES (2, 2, NULL, 3, N'2', 2, 2, N'2', N'Not Available', N'f29b170a-8d0d-4471-a048-6496cb4b921b_aesthetic-room-decor-ideas-3_1600x.jpg', NULL)
INSERT [dbo].[tb_room] ([r_id], [r_no], [r_tid], [r_locationid], [r_type], [r_price], [r_depositAmount], [r_desc], [r_status], [r_img1], [r_img2]) VALUES (3, 3, NULL, 4, N'3', 3, 3, N'3', N'Not Available', N'39ea90e6-3198-4e00-b521-7a8c9edb5756_PAinteriors-7-cafe9c2bd6be4823b9345e591e4f367f.jpg', NULL)
INSERT [dbo].[tb_room] ([r_id], [r_no], [r_tid], [r_locationid], [r_type], [r_price], [r_depositAmount], [r_desc], [r_status], [r_img1], [r_img2]) VALUES (4, 4, NULL, 6, N'Semi', 560, 100, N'aircond', N'Available', N'0627fd00-3081-47b0-b49d-85072e4198f8_modern-living-room-decor-1366x768.webp', NULL)
INSERT [dbo].[tb_room] ([r_id], [r_no], [r_tid], [r_locationid], [r_type], [r_price], [r_depositAmount], [r_desc], [r_status], [r_img1], [r_img2]) VALUES (7, 5, NULL, 8, N'123', 123, 123, N'123', N'Available', N'ef28442b-990c-488b-97b0-2a695bab0625_photo6100296549528613408.jpg', NULL)
INSERT [dbo].[tb_room] ([r_id], [r_no], [r_tid], [r_locationid], [r_type], [r_price], [r_depositAmount], [r_desc], [r_status], [r_img1], [r_img2]) VALUES (8, 876, NULL, 4, N'Large', 670, 80, N'SIngle Bed, Standing Fan', N'Available', N'97a5af23-5182-4588-8b33-6db9eb89533d_PAinteriors-7-cafe9c2bd6be4823b9345e591e4f367f.jpg', NULL)
SET IDENTITY_INSERT [dbo].[tb_room] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_sales] ON 

INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (38, CAST(N'2023-05-10' AS Date), NULL, N'TN4', N'Cash', N'Test', 1400, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (39, CAST(N'2023-07-13' AS Date), NULL, N'GE2', N'Credit Card', N'Test', NULL, 300, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (40, CAST(N'2023-06-23' AS Date), NULL, N'TN2', N'Bank Transfer', N'Test', 2500, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (41, CAST(N'2023-06-20' AS Date), NULL, N'TN3', N'Credit Card', N'Test', 1500, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (42, CAST(N'2023-06-13' AS Date), NULL, N'TN2', N'Cash', N'Test', 2000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (43, CAST(N'2023-07-19' AS Date), NULL, N'TN2', N'Cash', N'Test', NULL, 200, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (44, CAST(N'2023-07-04' AS Date), NULL, N'GE3', N'Credit Card', N'Test', NULL, 200, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (45, CAST(N'2023-06-27' AS Date), NULL, N'GE2', N'Credit Card', N'Test', 1200, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (46, CAST(N'2023-07-18' AS Date), NULL, N'GE2', N'Bank Transfer', N'Test', NULL, 100, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (47, CAST(N'2023-07-14' AS Date), NULL, N'TN2', N'Credit Card', N'Test', 1400, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (48, CAST(N'2023-06-18' AS Date), NULL, N'TN3', N'Credit Card', N'Test', 1236, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (49, CAST(N'2023-07-19' AS Date), NULL, N'TN1', N'Bank Transfer', N'Test', 3000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (51, CAST(N'2023-06-14' AS Date), NULL, N'TN3', N'Credit Card', N'Test', NULL, 700, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (52, CAST(N'2023-06-19' AS Date), 39, N'DL1', N'Credit Card', N'Nazran - Room 1', 1500, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (53, CAST(N'2023-07-06' AS Date), 40, N'GE3', N'Credit Card', N'Faizal - Room 3', 1222, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (54, CAST(N'2023-04-13' AS Date), NULL, N'TN1', N'Bank Transfer', N'Test', 700, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (55, CAST(N'2023-04-19' AS Date), NULL, N'TN4', N'Credit Card', N'Test', NULL, 200, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (56, CAST(N'2023-08-15' AS Date), NULL, N'GE1', N'Credit Card', N'Test', 2000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (57, CAST(N'2023-06-21' AS Date), NULL, N'TN4', N'Credit Card', N'Test', 1200, 190, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (58, CAST(N'2023-06-15' AS Date), 41, N'TN2', N'Cash', N'Hakimi - Room 4', 1760, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (60, CAST(N'2023-06-15' AS Date), 43, N'GE3', N'Cash', N'Faizal - Room 3', 1203, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (61, CAST(N'2023-06-28' AS Date), 44, N'GE3', N'Cash', N'Faizal - Room 3', 503, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (62, CAST(N'2023-08-25' AS Date), 45, N'GE3', N'Cash', N'Faizal - Room 3', 203, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (65, CAST(N'2023-09-21' AS Date), 48, N'GE3', N'Credit Card', N'Faizal - Room 3', 1203, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (67, CAST(N'2023-06-15' AS Date), NULL, N'General', N'Bank', N'Investment by Ali', 5000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (68, CAST(N'2023-06-15' AS Date), NULL, N'General', N'Bank', N'Investment by Ahmad Albab', 34000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (69, CAST(N'2023-06-15' AS Date), NULL, N'General', N'Bank', N'Investment by Ahmad Albab', 20000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (70, CAST(N'2023-06-15' AS Date), NULL, N'General', N'Bank', N'Investment by Ahmad Albab', 15000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (71, CAST(N'2023-06-15' AS Date), NULL, N'General', N'Bank', N'Investment by Ahmad Albab', 15000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (73, CAST(N'2023-06-15' AS Date), NULL, N'General', N'Bank Transfer', N'Investment by Ali - Lot 1', 25000, NULL, N'ae45f2c7-3aad-4e4f-9620-9f8b82adbea4_a3f263f266f176428ae59c2bae7b5e9c.jpg')
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (74, CAST(N'2023-06-16' AS Date), NULL, N'General', N'Cash', N'Investment by Ahmad Albab - Lot 2', 25000, NULL, NULL)
INSERT [dbo].[tb_sales] ([s_id], [s_date], [s_paymentid], [s_category], [s_transactionType], [s_detail], [s_amountIN], [s_amountOUT], [s_receipt]) VALUES (75, CAST(N'2023-06-17' AS Date), 50, N'DL1', N'Cash', N'Yusof - Room 1', 1411, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tb_sales] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_share] ON 

INSERT [dbo].[tb_share] ([share_id], [share_amount], [share_date], [share_investor]) VALUES (7, 178.94400000000002, CAST(N'2023-06-16' AS Date), 1017)
INSERT [dbo].[tb_share] ([share_id], [share_amount], [share_date], [share_investor]) VALUES (8, 894.72, CAST(N'2023-06-16' AS Date), 1018)
INSERT [dbo].[tb_share] ([share_id], [share_amount], [share_date], [share_investor]) VALUES (9, 894.72, CAST(N'2023-06-16' AS Date), 1019)
SET IDENTITY_INSERT [dbo].[tb_share] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_tenants] ON 

INSERT [dbo].[tb_tenants] ([t_id], [t_name], [t_ic], [t_phone], [t_address], [t_uploadIC], [t_emerContact], [t_roomid], [t_entrydate], [t_exitdate], [t_cardCode], [t_addOnDetail], [t_paymentStatus], [t_agreement], [LastReminderDate], [IsPaymentCollected], [IsCheckedOut]) VALUES (70, N'Imran Hakimi', N'010823020547', N'601136584107', N'Malaysia', NULL, N'01136584107', NULL, CAST(N'2023-06-06T05:33:00.000' AS DateTime), CAST(N'2023-06-09T05:28:00.000' AS DateTime), N'123', N'123', N'Unpaid', NULL, CAST(N'2023-06-10T05:33:00.000' AS DateTime), 0, 1)
INSERT [dbo].[tb_tenants] ([t_id], [t_name], [t_ic], [t_phone], [t_address], [t_uploadIC], [t_emerContact], [t_roomid], [t_entrydate], [t_exitdate], [t_cardCode], [t_addOnDetail], [t_paymentStatus], [t_agreement], [LastReminderDate], [IsPaymentCollected], [IsCheckedOut]) VALUES (71, N'Faizal', N'123456', N'6014521859', N'ahmad@123.com', NULL, N'6014521859', 3, CAST(N'2023-06-07T05:32:00.000' AS DateTime), CAST(N'2023-06-07T05:31:00.000' AS DateTime), N'123', N'123', N'Paid', NULL, CAST(N'2023-10-19T05:32:00.000' AS DateTime), 1, 0)
INSERT [dbo].[tb_tenants] ([t_id], [t_name], [t_ic], [t_phone], [t_address], [t_uploadIC], [t_emerContact], [t_roomid], [t_entrydate], [t_exitdate], [t_cardCode], [t_addOnDetail], [t_paymentStatus], [t_agreement], [LastReminderDate], [IsPaymentCollected], [IsCheckedOut]) VALUES (80, N'Maslan', N'123', N'123', N'123', NULL, N'123', NULL, CAST(N'2023-06-11T14:35:00.000' AS DateTime), CAST(N'2023-06-14T14:35:00.000' AS DateTime), N'123', N'123', N'Unpaid', NULL, CAST(N'2023-06-12T14:35:00.000' AS DateTime), 0, 1)
INSERT [dbo].[tb_tenants] ([t_id], [t_name], [t_ic], [t_phone], [t_address], [t_uploadIC], [t_emerContact], [t_roomid], [t_entrydate], [t_exitdate], [t_cardCode], [t_addOnDetail], [t_paymentStatus], [t_agreement], [LastReminderDate], [IsPaymentCollected], [IsCheckedOut]) VALUES (82, N'Hakimi', N'123', N'123', N'123', NULL, N'123', NULL, CAST(N'2023-06-11T22:51:00.000' AS DateTime), CAST(N'2023-06-13T22:51:00.000' AS DateTime), N'123', N'123', N'Unpaid', NULL, CAST(N'2023-07-02T22:51:00.000' AS DateTime), 0, 1)
INSERT [dbo].[tb_tenants] ([t_id], [t_name], [t_ic], [t_phone], [t_address], [t_uploadIC], [t_emerContact], [t_roomid], [t_entrydate], [t_exitdate], [t_cardCode], [t_addOnDetail], [t_paymentStatus], [t_agreement], [LastReminderDate], [IsPaymentCollected], [IsCheckedOut]) VALUES (84, N'Yusof', N'123', N'123', N'123', NULL, N'123', 1, CAST(N'2023-05-03T04:14:00.000' AS DateTime), CAST(N'2023-06-19T04:14:00.000' AS DateTime), N'123', N'123', N'Unpaid', NULL, CAST(N'2023-06-21T04:14:00.000' AS DateTime), 0, 0)
INSERT [dbo].[tb_tenants] ([t_id], [t_name], [t_ic], [t_phone], [t_address], [t_uploadIC], [t_emerContact], [t_roomid], [t_entrydate], [t_exitdate], [t_cardCode], [t_addOnDetail], [t_paymentStatus], [t_agreement], [LastReminderDate], [IsPaymentCollected], [IsCheckedOut]) VALUES (85, N'Nazran', N'123456789', N'60123456789', N'Selangor', NULL, N'60123456789', NULL, CAST(N'2023-05-24T04:01:00.000' AS DateTime), CAST(N'2023-06-16T03:56:00.000' AS DateTime), N'123', N'123', N'Paid', NULL, CAST(N'2023-07-05T04:01:00.000' AS DateTime), 1, 1)
INSERT [dbo].[tb_tenants] ([t_id], [t_name], [t_ic], [t_phone], [t_address], [t_uploadIC], [t_emerContact], [t_roomid], [t_entrydate], [t_exitdate], [t_cardCode], [t_addOnDetail], [t_paymentStatus], [t_agreement], [LastReminderDate], [IsPaymentCollected], [IsCheckedOut]) VALUES (86, N'Aaron', N'1231239524', N'601136584107', N'Ampang', NULL, N'601136584107', 2, CAST(N'2023-05-22T14:49:00.000' AS DateTime), CAST(N'2023-06-20T14:49:00.000' AS DateTime), N'123', N'123', N'Unpaid', NULL, CAST(N'2023-06-12T14:49:00.000' AS DateTime), 0, 0)
SET IDENTITY_INSERT [dbo].[tb_tenants] OFF
GO
SET IDENTITY_INSERT [dbo].[tb_user] ON 

INSERT [dbo].[tb_user] ([u_id], [u_username], [u_pass], [u_phone], [u_email], [u_roleid]) VALUES (1017, N'imran', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'123', N'imran@gmail.com', 3)
INSERT [dbo].[tb_user] ([u_id], [u_username], [u_pass], [u_phone], [u_email], [u_roleid]) VALUES (1018, N'Ali', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'112354668', N'ali@gmail.com', 4)
INSERT [dbo].[tb_user] ([u_id], [u_username], [u_pass], [u_phone], [u_email], [u_roleid]) VALUES (1019, N'Ahmad Albab', N'5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5', N'6013456788', N'albab@gmail.com', 4)
INSERT [dbo].[tb_user] ([u_id], [u_username], [u_pass], [u_phone], [u_email], [u_roleid]) VALUES (1020, N'Nayli Nabihah', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'0123456789', N'nayli@gmail.com', 4)
INSERT [dbo].[tb_user] ([u_id], [u_username], [u_pass], [u_phone], [u_email], [u_roleid]) VALUES (1021, N'Nur Syamalia Faiqah', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'0199998787', N'syama@gmail.com', 4)
SET IDENTITY_INSERT [dbo].[tb_user] OFF
GO
ALTER TABLE [dbo].[tb_location] ADD  CONSTRAINT [DF_tb_location_isPaymentMade]  DEFAULT ((0)) FOR [isPaymentMade]
GO
ALTER TABLE [dbo].[tb_tenants] ADD  CONSTRAINT [DF_tb_tenants_IsCheckedOut]  DEFAULT ((0)) FOR [IsCheckedOut]
GO
ALTER TABLE [dbo].[tb_attendanceCleaner]  WITH CHECK ADD  CONSTRAINT [FK_tb_attendanceCleaner_tb_location] FOREIGN KEY([att_lid])
REFERENCES [dbo].[tb_location] ([l_id])
GO
ALTER TABLE [dbo].[tb_attendanceCleaner] CHECK CONSTRAINT [FK_tb_attendanceCleaner_tb_location]
GO
ALTER TABLE [dbo].[tb_attendancedate]  WITH CHECK ADD  CONSTRAINT [FK_tb_attendancedate_tb_attendanceCleaner] FOREIGN KEY([attid])
REFERENCES [dbo].[tb_attendanceCleaner] ([att_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tb_attendancedate] CHECK CONSTRAINT [FK_tb_attendancedate_tb_attendanceCleaner]
GO
ALTER TABLE [dbo].[tb_inventory]  WITH CHECK ADD  CONSTRAINT [FK_tb_inventory_tb_location] FOREIGN KEY([i_locationid])
REFERENCES [dbo].[tb_location] ([l_id])
GO
ALTER TABLE [dbo].[tb_inventory] CHECK CONSTRAINT [FK_tb_inventory_tb_location]
GO
ALTER TABLE [dbo].[tb_investor]  WITH CHECK ADD  CONSTRAINT [FK_tb_investor_tb_lot] FOREIGN KEY([i_lot])
REFERENCES [dbo].[tb_lot] ([lot_id])
GO
ALTER TABLE [dbo].[tb_investor] CHECK CONSTRAINT [FK_tb_investor_tb_lot]
GO
ALTER TABLE [dbo].[tb_investor]  WITH CHECK ADD  CONSTRAINT [FK_tb_investor_tb_user] FOREIGN KEY([i_id])
REFERENCES [dbo].[tb_user] ([u_id])
GO
ALTER TABLE [dbo].[tb_investor] CHECK CONSTRAINT [FK_tb_investor_tb_user]
GO
ALTER TABLE [dbo].[tb_payment]  WITH CHECK ADD  CONSTRAINT [FK_tb_payment_tb_tenants] FOREIGN KEY([p_tenantid])
REFERENCES [dbo].[tb_tenants] ([t_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tb_payment] CHECK CONSTRAINT [FK_tb_payment_tb_tenants]
GO
ALTER TABLE [dbo].[tb_room]  WITH CHECK ADD  CONSTRAINT [FK_tb_room_tb_location] FOREIGN KEY([r_locationid])
REFERENCES [dbo].[tb_location] ([l_id])
GO
ALTER TABLE [dbo].[tb_room] CHECK CONSTRAINT [FK_tb_room_tb_location]
GO
ALTER TABLE [dbo].[tb_room]  WITH CHECK ADD  CONSTRAINT [FK_tb_room_tb_tenants] FOREIGN KEY([r_tid])
REFERENCES [dbo].[tb_tenants] ([t_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tb_room] CHECK CONSTRAINT [FK_tb_room_tb_tenants]
GO
ALTER TABLE [dbo].[tb_sales]  WITH CHECK ADD  CONSTRAINT [FK_tb_sales_tb_payment] FOREIGN KEY([s_paymentid])
REFERENCES [dbo].[tb_payment] ([p_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tb_sales] CHECK CONSTRAINT [FK_tb_sales_tb_payment]
GO
ALTER TABLE [dbo].[tb_share]  WITH CHECK ADD  CONSTRAINT [FK_tb_share_tb_investor] FOREIGN KEY([share_investor])
REFERENCES [dbo].[tb_investor] ([i_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tb_share] CHECK CONSTRAINT [FK_tb_share_tb_investor]
GO
ALTER TABLE [dbo].[tb_tenants]  WITH CHECK ADD  CONSTRAINT [FK_tb_tenants_tb_room] FOREIGN KEY([t_roomid])
REFERENCES [dbo].[tb_room] ([r_id])
GO
ALTER TABLE [dbo].[tb_tenants] CHECK CONSTRAINT [FK_tb_tenants_tb_room]
GO
ALTER TABLE [dbo].[tb_user]  WITH CHECK ADD  CONSTRAINT [FK_tb_user_tb_role] FOREIGN KEY([u_roleid])
REFERENCES [dbo].[tb_role] ([rl_id])
GO
ALTER TABLE [dbo].[tb_user] CHECK CONSTRAINT [FK_tb_user_tb_role]
GO
USE [master]
GO
ALTER DATABASE [db_rental] SET  READ_WRITE 
GO
