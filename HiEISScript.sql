USE [HiEIS]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 8/2/2018 5:06:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](256) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](256) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](256) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](256) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaxNo] [nvarchar](18) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Tel] [varchar](50) NOT NULL,
	[Email] [varchar](max) NOT NULL,
	[Website] [varchar](max) NULL,
	[Fax] [varchar](50) NULL,
	[Bank] [nvarchar](max) NULL,
	[BankAccountNumber] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CompanyCustomer]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyCustomer](
	[CustomerId] [nvarchar](256) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Liabilities] [numeric](18, 0) NULL,
 CONSTRAINT [PK_CompanyCustomer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Enterprise] [nvarchar](max) NULL,
	[TaxNo] [nvarchar](18) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Tel] [varchar](16) NULL,
	[Fax] [varchar](16) NULL,
	[Bank] [nvarchar](max) NULL,
	[BankAccountNumber] [varchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerProduct]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerProduct](
	[CustomerId] [nvarchar](256) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
 CONSTRAINT [PK_CustomerProduct] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Invoice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LookupCode] [varchar](13) NULL,
	[Number] [varchar](7) NULL,
	[Type] [int] NOT NULL,
	[Date] [smalldatetime] NOT NULL,
	[DueDate] [smalldatetime] NULL,
	[PaymentMethod] [int] NOT NULL,
	[FileUrl] [nvarchar](max) NULL,
	[SubTotal] [numeric](18, 0) NOT NULL,
	[VATRate] [numeric](18, 2) NOT NULL,
	[VATAmount] [numeric](18, 0) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[Total] [numeric](18, 0) NOT NULL,
	[AmountInWords] [nvarchar](max) NOT NULL,
	[PaymentStatus] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Enterprise] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[TaxNo] [nvarchar](18) NULL,
	[Tel] [varchar](16) NULL,
	[Fax] [varchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[BankAccountNumber] [nvarchar](256) NULL,
	[Bank] [nvarchar](256) NULL,
	[TemplateId] [int] NOT NULL,
	[StaffId] [nvarchar](256) NOT NULL,
	[CustomerId] [nvarchar](256) NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InvoiceItem]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceItem](
	[InvoiceId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [numeric](18, 0) NOT NULL,
	[VATRate] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_InvoiceItem] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Unit] [nvarchar](16) NULL,
	[UnitPrice] [numeric](18, 0) NOT NULL,
	[VATRate] [numeric](18, 2) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[HasIndex] [bit] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProformaInvoice]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProformaInvoice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LookupCode] [varchar](13) NULL,
	[Date] [smalldatetime] NOT NULL,
	[FileUrl] [varchar](max) NOT NULL,
	[PaymentDeadline] [smalldatetime] NOT NULL,
	[Status] [int] NOT NULL,
	[SubTotal] [numeric](18, 0) NOT NULL,
	[VATAmount] [numeric](18, 0) NOT NULL,
	[TotalNoLiabilities] [numeric](18, 0) NOT NULL,
	[Total] [numeric](18, 0) NOT NULL,
	[StaffId] [nvarchar](256) NOT NULL,
	[CustomerId] [nvarchar](256) NOT NULL,
	[Liabilities] [numeric](18, 0) NULL,
 CONSTRAINT [PK_ProformaInvoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProformaInvoiceItem]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProformaInvoiceItem](
	[ProformaInvoiceId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[VATRate] [numeric](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [numeric](18, 0) NOT NULL,
	[OldNumber] [int] NULL,
	[NewNumber] [int] NULL,
	[DateFrom] [smalldatetime] NULL,
	[DateTo] [smalldatetime] NULL,
 CONSTRAINT [PK_ProformaInvoiceItem] PRIMARY KEY CLUSTERED 
(
	[ProformaInvoiceId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Staff]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Staff](
	[Id] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Tel] [varchar](16) NULL,
	[CompanyId] [int] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Template]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Template](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Form] [varchar](15) NOT NULL,
	[Serial] [varchar](15) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[FileUrl] [varchar](max) NOT NULL,
	[Amount] [int] NOT NULL,
	[BeginNo] [int] NOT NULL,
	[CurrentNo] [int] NOT NULL,
	[ReleaseDate] [smalldatetime] NOT NULL,
	[ReleaseAnnouncementUrl] [varchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 8/2/2018 5:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Amount] [numeric](18, 0) NOT NULL,
	[Date] [date] NOT NULL,
	[CustomerId] [nvarchar](256) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Admin')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'Manager')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'Accounting Manager')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4', N'Liability Accountant')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5', N'Payable Accountant')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'6', N'Customer')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbec', N'1')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbed', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbee', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'5')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', N'6')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', N'6')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbei', N'6')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbej', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejk', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejl', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejl', N'4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejl', N'5')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', N'6')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5655175f-20e7-43f9-b9c9-a09ab85f0c69', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5655175f-20e7-43f9-b9c9-a09ab85f0c69', N'3')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5655175f-20e7-43f9-b9c9-a09ab85f0c69', N'4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5655175f-20e7-43f9-b9c9-a09ab85f0c69', N'5')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'bf41f04e-8c11-4071-9267-63f21188775c', N'2')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbec', 1, N'admin@gmail.com', 0, N'AAycf0JcP2G11oQxoBi+SBLw8GPZ32Yaif797iSbUa5MFR1+0syl9huXopnoXNXHdA==', N'34c9d22a-3336-419e-ba88-621a19386ae0', NULL, 0, 0, NULL, 1, 0, N'admin')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbed', 1, N'anhdt@gmail.com', 0, N'AAM7QYtRmIe6CjqnjgqC8jkluL6JCqN+QzrzHUoXR3lHeak47HChQkKSduyscsDNDw==', N'f2d4271d-ebb3-4fb0-9450-ed8d5c6a1a08', NULL, 0, 0, NULL, 1, 0, N'anhdt')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbee', 1, N'galaxy28140@gmail.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'gianghnm')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', 1, N'duynq@yahoo.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'duynq')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 1, N'huongspring1204@gmail.com', 0, N'AHDgmyQv8s3JChiX4mxe2QV02wgTMBgN2siMH7HNORC8kHHhcR82LYE07Nw7dykOKA==', N'ee0c21fd-cc68-45a0-91a5-20ca2d7b0333', NULL, 0, 0, NULL, 1, 0, N'huongntx')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 1, N'phahd@gmail.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'phahd')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbei', 1, N'minhthy@gmail.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'thyhmn')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbej', 1, N'truongnln@gmail.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'truongnln')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejk', 0, N'nguyengiaquoc@gmail.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'nguyengiaquoc')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejl', 1, N'tamnguyen@gmail.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'tamnguyen')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', 1, N'sunny@gmail.com', 0, N'ACxt0KNV53gDAPgttBoQUfbEe4WlPPIZlI3uwUmNHVNFIUo2Pc7PCkOsGFjH9R92Dw==', N'3a3ab174-733b-4678-a667-6507b29d0094', NULL, 0, 0, NULL, 1, 0, N'sunnylee')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'5655175f-20e7-43f9-b9c9-a09ab85f0c69', 1, N'hahoangvinh@gmail.com', 0, N'ABFro9+eM8DuJ8PQVbllb7x57AzfWySl01eF3qPwwZ++fE91ezgW8S5nkTStT8O56g==', N'58b7fca3-9cfb-4427-ad7b-18285130f6a0', NULL, 0, 0, NULL, 1, 0, N'hhv_quanly')
INSERT [dbo].[AspNetUsers] ([Id], [IsActive], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'bf41f04e-8c11-4071-9267-63f21188775c', 0, N'hisoft@gmail.com', 0, N'AK1k407haoGqxBoByr9w/vWuM98Uo59GFNW09+CUvnRMBw3/E9IXYRsNAicp9G0uHA==', N'11cd7195-c405-4cd4-9bc2-c548431bbe32', NULL, 0, 0, NULL, 1, 0, N'hisoft_admin')
SET IDENTITY_INSERT [dbo].[Company] ON 

INSERT [dbo].[Company] ([Id], [TaxNo], [Name], [Address], [Tel], [Email], [Website], [Fax], [Bank], [BankAccountNumber], [IsActive]) VALUES (1, N'0300591882', N'QTSC Corporation', N'97-101 Nguyễn Công Trứ, P. Nguyễn Thái Bình, Q.1, TP.HCM', N'84 28 37155051', N'qtsc@quangtrungsoft.com.vn', N'www.quangtrungsoft.com.vn', N'84 28 37155985', N'Nông nghiệp & Phát triển Nông thôn Việt Nam - Chi nhánh Xuyên Á', N'6150211370017', 1)
INSERT [dbo].[Company] ([Id], [TaxNo], [Name], [Address], [Tel], [Email], [Website], [Fax], [Bank], [BankAccountNumber], [IsActive]) VALUES (2, N'5701934180', N'Công ty thương mại và vận tải Hà Hoàng Vinh', N'Ngõ 447, đường Lê Thanh Nghị, Phường Cẩm Thủy, Thành phố Cẩm Phả, Tỉnh Quảng Ninh, Việt Nam', N'01668762818', N'hahoangvinh@gmail.com', NULL, N'1111-2575', N'Techcombank', NULL, 1)
INSERT [dbo].[Company] ([Id], [TaxNo], [Name], [Address], [Tel], [Email], [Website], [Fax], [Bank], [BankAccountNumber], [IsActive]) VALUES (3, N'0313508761', N'Công ty US MART', N'329 TRẦN HƯNG ĐẠO, P. CÔ GIANG, Q1, TP HCM', N'02416289245', N'us.mart@gmail.com', N'www.usmart.com', NULL, NULL, NULL, 1)
INSERT [dbo].[Company] ([Id], [TaxNo], [Name], [Address], [Tel], [Email], [Website], [Fax], [Bank], [BankAccountNumber], [IsActive]) VALUES (5, N'0314508549', N'Công Ty Cổ Phần Giải Pháp Công Nghệ Hisoft', N'307/41 Thạch Lam, Phường Phú Thạnh, Quận Tân Phú, Thành phố Hồ Chí Minh', N'0906 782 333', N'hisoft@gmail.com', N'www.hisoft.com', NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[Company] OFF
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 1, CAST(1800000 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 2, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 1, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 2, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 3, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbei', 2, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbei', 3, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', 1, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[CompanyCustomer] ([CustomerId], [CompanyId], [Liabilities]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', 3, CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[Customer] ([Id], [Name], [Enterprise], [TaxNo], [Address], [Tel], [Fax], [Bank], [BankAccountNumber]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', N'Nguyễn Thị Xuân Hương', N'Công ty TNHH Công nghệ và Thương mại VCOM', N'0101791855', N'Q. Bình Thạnh - Tp. HCM', N'0908850869', N'+1 212 999 8888', N'Vietinbank', NULL)
INSERT [dbo].[Customer] ([Id], [Name], [Enterprise], [TaxNo], [Address], [Tel], [Fax], [Bank], [BankAccountNumber]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', N'Hoàng Đăng Pha', N'Công ty TNHH Bình Minh', N'0234218052', N'143A/31 Ung Văn Khiêm - Q. Bình Thạnh', N'0908073848', NULL, NULL, NULL)
INSERT [dbo].[Customer] ([Id], [Name], [Enterprise], [TaxNo], [Address], [Tel], [Fax], [Bank], [BankAccountNumber]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbei', N'Hoàng Nguyễn Minh Thy', N'Công ty TNHH TM - DV Bảo Quyên', N'3603043915', N'93/76 KP8 - P. Tân Hòa - Biên Hòa - ĐN', N'0937596120', N'2222 8888', N'TP Bank', N'123456789')
INSERT [dbo].[Customer] ([Id], [Name], [Enterprise], [TaxNo], [Address], [Tel], [Fax], [Bank], [BankAccountNumber]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', N'Lee Sunny', N'Sunny Corp.', N'0524129992', N'281 Hàm Nghi - Q.1', N'0123456789', N'3434-99999', N'Techombank', N'042245002481')
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 28, 2)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 45, 100)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 46, 200)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 2, 1)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 45, 500)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', 10, 10)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', 11, 1)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', 38, 5)
INSERT [dbo].[CustomerProduct] ([CustomerId], [ProductId], [Amount]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejn', 39, 5)
SET IDENTITY_INSERT [dbo].[Invoice] ON 

INSERT [dbo].[Invoice] ([Id], [LookupCode], [Number], [Type], [Date], [DueDate], [PaymentMethod], [FileUrl], [SubTotal], [VATRate], [VATAmount], [Note], [Total], [AmountInWords], [PaymentStatus], [Status], [Name], [Enterprise], [Address], [TaxNo], [Tel], [Fax], [Email], [BankAccountNumber], [Bank], [TemplateId], [StaffId], [CustomerId]) VALUES (9, N'Bp52MzJuK8B93', N'0000001', 2, CAST(N'2018-07-10 15:08:00' AS SmallDateTime), NULL, 2, N'/Files/Invoices/20180629_Bp52MzJuK8B93.pdf', CAST(1402000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), CAST(0 AS Numeric(18, 0)), NULL, CAST(1402000 AS Numeric(18, 0)), N'Một triệu bốn trăm lẻ hai ngàn đồng', 1, 2, N'Yan Hoàng', N'Yan Inc.', N'192 Nguyễn Du - Q.1', NULL, N'0937596120', NULL, N'chicrabpc@gmail.com', N'0123 456 7890', N'Agribank', 2, N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', NULL)
INSERT [dbo].[Invoice] ([Id], [LookupCode], [Number], [Type], [Date], [DueDate], [PaymentMethod], [FileUrl], [SubTotal], [VATRate], [VATAmount], [Note], [Total], [AmountInWords], [PaymentStatus], [Status], [Name], [Enterprise], [Address], [TaxNo], [Tel], [Fax], [Email], [BankAccountNumber], [Bank], [TemplateId], [StaffId], [CustomerId]) VALUES (1070, N'jeiMEPmzu096P', N'0000001', 2, CAST(N'2017-12-05 00:00:00' AS SmallDateTime), CAST(N'2017-12-05 00:00:00' AS SmallDateTime), 1, N'/Files/Invoices/20171205_jeiMEPmzu096P.pdf', CAST(2200000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), CAST(0 AS Numeric(18, 0)), NULL, CAST(2200000 AS Numeric(18, 0)), N'Hai triệu hai trăm ngàn đồng', 0, 3, N'Nguyễn Thị Xuân Hương', N'Công ty TNHH Công nghệ và Thương mại VCOM', N'Q. Bình Thạnh - Tp. HCM', N'0101791855', N'0908850869', N'+1 212 999 8888', N'chicrabpc@gmail.com', NULL, N'Vietinbank', 1, N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg')
INSERT [dbo].[Invoice] ([Id], [LookupCode], [Number], [Type], [Date], [DueDate], [PaymentMethod], [FileUrl], [SubTotal], [VATRate], [VATAmount], [Note], [Total], [AmountInWords], [PaymentStatus], [Status], [Name], [Enterprise], [Address], [TaxNo], [Tel], [Fax], [Email], [BankAccountNumber], [Bank], [TemplateId], [StaffId], [CustomerId]) VALUES (1071, N'hDUC7QF42ku0r', N'0000002', 2, CAST(N'2018-01-05 00:00:00' AS SmallDateTime), CAST(N'2018-01-05 00:00:00' AS SmallDateTime), 1, N'/Files/Invoices/20171226_hDUC7QF42ku0r.pdf', CAST(1100000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), CAST(0 AS Numeric(18, 0)), NULL, CAST(1100000 AS Numeric(18, 0)), N'Một triệu một trăm ngàn đồng', 0, 3, N'Nguyễn Thị Xuân Hương', N'Công ty TNHH Công nghệ và Thương mại VCOM', N'Q. Bình Thạnh - Tp. HCM', N'0101791855', N'0908850869', N'+1 212 999 8888', N'huongspring1204@gmail.com', NULL, N'Vietinbank', 1, N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg')
INSERT [dbo].[Invoice] ([Id], [LookupCode], [Number], [Type], [Date], [DueDate], [PaymentMethod], [FileUrl], [SubTotal], [VATRate], [VATAmount], [Note], [Total], [AmountInWords], [PaymentStatus], [Status], [Name], [Enterprise], [Address], [TaxNo], [Tel], [Fax], [Email], [BankAccountNumber], [Bank], [TemplateId], [StaffId], [CustomerId]) VALUES (1072, N'lCKWxmGHx3nqR', N'0000003', 2, CAST(N'2018-02-01 00:00:00' AS SmallDateTime), CAST(N'2018-02-05 00:00:00' AS SmallDateTime), 1, N'/Files/Invoices/20180125_lCKWxmGHx3nqR.pdf', CAST(850000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), CAST(0 AS Numeric(18, 0)), NULL, CAST(850000 AS Numeric(18, 0)), N'Tám trăm năm mươi ngàn đồng', 1, 3, N'Nguyễn Thị Xuân Hương', N'Công ty TNHH Công nghệ và Thương mại VCOM', N'Q. Bình Thạnh - Tp. HCM', N'0101791855', N'0908850869', N'+1 212 999 8888', N'huongspring1204@gmail.com', NULL, N'Vietinbank', 1, N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg')
INSERT [dbo].[Invoice] ([Id], [LookupCode], [Number], [Type], [Date], [DueDate], [PaymentMethod], [FileUrl], [SubTotal], [VATRate], [VATAmount], [Note], [Total], [AmountInWords], [PaymentStatus], [Status], [Name], [Enterprise], [Address], [TaxNo], [Tel], [Fax], [Email], [BankAccountNumber], [Bank], [TemplateId], [StaffId], [CustomerId]) VALUES (1074, N'VQ7PfxmfWclQb', N'0000002', 2, CAST(N'2018-06-05 00:00:00' AS SmallDateTime), CAST(N'2018-06-05 00:00:00' AS SmallDateTime), 1, N'/Files/Invoices/20180530_VQ7PfxmfWclQb.pdf', CAST(1500000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), CAST(0 AS Numeric(18, 0)), NULL, CAST(1500000 AS Numeric(18, 0)), N'Một triệu năm trăm ngàn đồng', 0, 3, N'Hoàng Đăng Pha', N'Công ty TNHH Bình Minh', N'143A/31 Ung Văn Khiêm - Q. Bình Thạnh', N'0234218052', N'0908073848', NULL, N'phahd@gmail.com', NULL, NULL, 2, N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh')
INSERT [dbo].[Invoice] ([Id], [LookupCode], [Number], [Type], [Date], [DueDate], [PaymentMethod], [FileUrl], [SubTotal], [VATRate], [VATAmount], [Note], [Total], [AmountInWords], [PaymentStatus], [Status], [Name], [Enterprise], [Address], [TaxNo], [Tel], [Fax], [Email], [BankAccountNumber], [Bank], [TemplateId], [StaffId], [CustomerId]) VALUES (1075, N'sptYW7m9yaLSl', N'0000003', 2, CAST(N'2018-07-05 00:00:00' AS SmallDateTime), CAST(N'2018-07-05 00:00:00' AS SmallDateTime), 1, N'/Files/Invoices/20180629_sptYW7m9yaLSl.pdf', CAST(2000000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), CAST(0 AS Numeric(18, 0)), NULL, CAST(2000000 AS Numeric(18, 0)), N'Hai triệu đồng', 0, 3, N'Hoàng Đăng Pha', N'Công ty TNHH Bình Minh', N'143A/31 Ung Văn Khiêm - Q. Bình Thạnh', N'0234218052', N'0908073848', NULL, N'phahd@gmail.com', NULL, NULL, 2, N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh')
INSERT [dbo].[Invoice] ([Id], [LookupCode], [Number], [Type], [Date], [DueDate], [PaymentMethod], [FileUrl], [SubTotal], [VATRate], [VATAmount], [Note], [Total], [AmountInWords], [PaymentStatus], [Status], [Name], [Enterprise], [Address], [TaxNo], [Tel], [Fax], [Email], [BankAccountNumber], [Bank], [TemplateId], [StaffId], [CustomerId]) VALUES (1076, N'DKF30MUmijg0G', N'0000001', 2, CAST(N'2018-07-06 00:00:00' AS SmallDateTime), CAST(N'2018-07-06 00:00:00' AS SmallDateTime), 1, N'/Files/Invoices/20180629_DKF30MUmijg0G.pdf', CAST(14000000 AS Numeric(18, 0)), CAST(0.05 AS Numeric(18, 2)), CAST(700000 AS Numeric(18, 0)), NULL, CAST(14700000 AS Numeric(18, 0)), N'Mười tư triệu bảy trăm ngàn đồng', 0, 3, N'Hoàng Đăng Pha', N'Công ty TNHH Bình Minh', N'143A/31 Ung Văn Khiêm - Q. Bình Thạnh', N'0234218052', N'0908073848', NULL, N'phahd@gmail.com', NULL, NULL, 3, N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejl', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh')
SET IDENTITY_INSERT [dbo].[Invoice] OFF
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (9, 8, 1, CAST(55000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (9, 38, 3, CAST(449000 AS Numeric(18, 0)), CAST(0.05 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1070, 45, 200, CAST(5000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1070, 46, 300, CAST(4000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1071, 45, 100, CAST(5000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1071, 46, 150, CAST(4000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1072, 45, 50, CAST(5000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1072, 46, 150, CAST(4000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1074, 45, 300, CAST(5000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1075, 45, 400, CAST(5000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)))
INSERT [dbo].[InvoiceItem] ([InvoiceId], [ProductId], [Quantity], [UnitPrice], [VATRate]) VALUES (1076, 2, 2, CAST(7000000 AS Numeric(18, 0)), CAST(0.05 AS Numeric(18, 2)))
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (1, N'Mực nướng cay', N'MNC', N'Gói', CAST(20000 AS Numeric(18, 0)), CAST(0.10 AS Numeric(18, 2)), 1, 0, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (2, N'Máy lạnh', N'ML', N'Máy', CAST(7000000 AS Numeric(18, 0)), CAST(0.05 AS Numeric(18, 2)), 3, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (3, N'Điều hòa', N'DH', N'Máy', CAST(10000000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (4, N'Kem Chống Nắng La Roche-Posay Anthelios Ultra Fluid Spf 50+ UVB & UVA', N'', N'Chai 50ml', CAST(369000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 3, 0, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (5, N'Sữa tắm Bath Baby', N'BB', N'Chai', CAST(71000 AS Numeric(18, 0)), CAST(0.10 AS Numeric(18, 2)), 3, 0, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (7, N'Đế HDMI Âm Tường Ugreen 20315', N'U20315', N'', CAST(169000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (8, N'Ly Sứ Cao Cấp Dong Hwa Hình Ngôi Làng', N'MC001', N'Ly 300ml', CAST(50000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (9, N'Sữa Tắm pH 5.5 Hạnh Nhân Johnson’s', N'JHN', N'Chai 750ml', CAST(109000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 3, 0, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (10, N'Bàn Rec-T Đen IBIE', N'TORETR12B', N'Bàn', CAST(1196000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (11, N'Gel Rửa Mặt La Roche Posay Effaclar Purifying Foaming Gel', N'GRMLRPEPFG', N'Chai 50ml', CAST(140000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 3, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (14, N'Mặt Nạ Ngủ Vichy Aqualia Masque Nuit', N'', N'', CAST(200000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 3, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (19, N'Son Lì Maybelline Intimatte Nude', N'', N'Thỏi 3.9g', CAST(175000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 3, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (20, N'Bột Trà Xanh Nguyên Chất Milaganics', N'XRN', N'Gói 100g', CAST(49000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 3, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (23, N'Kem Dưỡng Vùng Da Mắt Hunca Care Delight Therapy Pomegranate Eye Gel', N'', N'Tuýp 15ml', CAST(99000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 3, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (24, N'Harry Potter And The Goblet Of Fire', N'HP&TGOF', N'Quyển', CAST(206000 AS Numeric(18, 0)), CAST(0.10 AS Numeric(18, 2)), 2, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (25, N'Harry Potter And The Philosopher''s Stone – Gryffindor Edition - TR', N'', N'', CAST(184800 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 2, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (26, N'Oxford Learner''s Pocket Dictionary: A Pocket-sized Reference to English Vocabulary', N'OLPD', N'', CAST(57000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 2, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (27, N'$10,000,000 Marriage Proposal', N'MP', N'', CAST(57000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 2, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (28, N'BaZi The Destiny Code', N'', N'Quyển', CAST(825000 AS Numeric(18, 0)), CAST(0.10 AS Numeric(18, 2)), 2, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (29, N'Mặt Nạ Ngủ Vichy Aqualia Masque Nuit', N'XRN4', N'Chai 50ml', CAST(3890000 AS Numeric(18, 0)), CAST(0.05 AS Numeric(18, 2)), 2, 0, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (31, N'Loa Bluetooth Harman Kardon Onyx Studio 3 60W', N'HKOS3', N'', CAST(2599000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 2, 0, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (33, N'Nàng Và Con Mèo Của Nàng', N'', N'', CAST(22500 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 2, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (36, N'Thẻ Nhớ Micro SD Samsung Evo Plus 32GB Class 10', N'', N'', CAST(196500 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (37, N'Ghế CZN-Eames Chân Gỗ', N'CZNE', N'Chiếc', CAST(530000 AS Numeric(18, 0)), CAST(0.10 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (38, N'Ghế Orchid', N'OC', N'Chiếc', CAST(449000 AS Numeric(18, 0)), CAST(0.05 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (39, N'Ghế Lười Hình Giọt Nước Phú Mỹ', N'GH-GINU-XA', N'Chiếc', CAST(600000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (40, N'Quạt Trần Panasonic', N'F-60UFN', N'', CAST(9760000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (41, N'Quạt Đứng KDK', N'M40K', N'Chiếc', CAST(2999000 AS Numeric(18, 0)), CAST(0.10 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (42, N'Đèn Soi Tolsen', N'60011', N'', CAST(85000 AS Numeric(18, 0)), CAST(-1.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (43, N'Laptop Lenovo IdeaPad 120S-11IAP', N'', N'Máy', CAST(4779000 AS Numeric(18, 0)), CAST(0.10 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (44, N'Đèn Diệt Muỗi Comet', N'CM048', N'Đèn', CAST(180000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 1, 1, 0)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (45, N'Điện', N'', N'kW/h', CAST(5000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 1, 1, 1)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (46, N'Nước', N'', N'm3', CAST(4000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 1, 1, 1)
INSERT [dbo].[Product] ([Id], [Name], [Code], [Unit], [UnitPrice], [VATRate], [CompanyId], [IsActive], [HasIndex]) VALUES (48, N'Adapter Sạc Xe Hơi Ugreen 2 Cổng 30W', N'QC2.0', N'', CAST(316000 AS Numeric(18, 0)), CAST(0.00 AS Numeric(18, 2)), 1, 1, 0)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[ProformaInvoice] ON 

INSERT [dbo].[ProformaInvoice] ([Id], [LookupCode], [Date], [FileUrl], [PaymentDeadline], [Status], [SubTotal], [VATAmount], [TotalNoLiabilities], [Total], [StaffId], [CustomerId], [Liabilities]) VALUES (13, N'1LgcAxONXqiWi', CAST(N'2017-11-25 00:00:00' AS SmallDateTime), N'/Files/ProformaInvoices/20171125_1LgcAxONXqiWi.pdf', CAST(N'2017-12-05 00:00:00' AS SmallDateTime), 2, CAST(2200000 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(2200000 AS Numeric(18, 0)), CAST(2200000 AS Numeric(18, 0)), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[ProformaInvoice] ([Id], [LookupCode], [Date], [FileUrl], [PaymentDeadline], [Status], [SubTotal], [VATAmount], [TotalNoLiabilities], [Total], [StaffId], [CustomerId], [Liabilities]) VALUES (15, N'rqVh3PbhBIVJz', CAST(N'2017-12-25 00:00:00' AS SmallDateTime), N'/Files/ProformaInvoices/20180727_rqVh3PbhBIVJz.pdf', CAST(N'2018-01-05 00:00:00' AS SmallDateTime), 2, CAST(1100000 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(1100000 AS Numeric(18, 0)), CAST(3300000 AS Numeric(18, 0)), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', CAST(2200000 AS Numeric(18, 0)))
INSERT [dbo].[ProformaInvoice] ([Id], [LookupCode], [Date], [FileUrl], [PaymentDeadline], [Status], [SubTotal], [VATAmount], [TotalNoLiabilities], [Total], [StaffId], [CustomerId], [Liabilities]) VALUES (16, N'OmPGcvKORSbWv', CAST(N'2018-01-25 00:00:00' AS SmallDateTime), N'/Files/ProformaInvoices/20180727_OmPGcvKORSbWv.pdf', CAST(N'2018-02-05 00:00:00' AS SmallDateTime), 2, CAST(850000 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(850000 AS Numeric(18, 0)), CAST(2650000 AS Numeric(18, 0)), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', CAST(1800000 AS Numeric(18, 0)))
INSERT [dbo].[ProformaInvoice] ([Id], [LookupCode], [Date], [FileUrl], [PaymentDeadline], [Status], [SubTotal], [VATAmount], [TotalNoLiabilities], [Total], [StaffId], [CustomerId], [Liabilities]) VALUES (18, N'CS5IYaRNAkj0s', CAST(N'2018-05-30 00:00:00' AS SmallDateTime), N'/Files/ProformaInvoices/20180530_CS5IYaRNAkj0s.pdf', CAST(N'2018-06-05 00:00:00' AS SmallDateTime), 2, CAST(1500000 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(1500000 AS Numeric(18, 0)), CAST(1500000 AS Numeric(18, 0)), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[ProformaInvoice] ([Id], [LookupCode], [Date], [FileUrl], [PaymentDeadline], [Status], [SubTotal], [VATAmount], [TotalNoLiabilities], [Total], [StaffId], [CustomerId], [Liabilities]) VALUES (19, N'9ujuXzlX4UFVX', CAST(N'2018-06-29 00:00:00' AS SmallDateTime), N'/Files/ProformaInvoices/20180629_9ujuXzlX4UFVX.pdf', CAST(N'2018-07-05 00:00:00' AS SmallDateTime), 2, CAST(2000000 AS Numeric(18, 0)), CAST(0 AS Numeric(18, 0)), CAST(2000000 AS Numeric(18, 0)), CAST(2000000 AS Numeric(18, 0)), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', CAST(0 AS Numeric(18, 0)))
INSERT [dbo].[ProformaInvoice] ([Id], [LookupCode], [Date], [FileUrl], [PaymentDeadline], [Status], [SubTotal], [VATAmount], [TotalNoLiabilities], [Total], [StaffId], [CustomerId], [Liabilities]) VALUES (20, N'qfl83XFLDZypR', CAST(N'2018-06-29 00:00:00' AS SmallDateTime), N'/Files/ProformaInvoices/20180629_qfl83XFLDZypR.pdf', CAST(N'2018-07-06 00:00:00' AS SmallDateTime), 2, CAST(14000000 AS Numeric(18, 0)), CAST(700000 AS Numeric(18, 0)), CAST(14700000 AS Numeric(18, 0)), CAST(14700000 AS Numeric(18, 0)), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejl', N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', CAST(0 AS Numeric(18, 0)))
SET IDENTITY_INSERT [dbo].[ProformaInvoice] OFF
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (13, 45, CAST(0.00 AS Numeric(18, 2)), 200, CAST(5000 AS Numeric(18, 0)), 0, 200, CAST(N'2017-10-25 00:00:00' AS SmallDateTime), CAST(N'2017-11-24 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (13, 46, CAST(0.00 AS Numeric(18, 2)), 300, CAST(4000 AS Numeric(18, 0)), 0, 300, CAST(N'2017-10-25 00:00:00' AS SmallDateTime), CAST(N'2017-11-24 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (15, 45, CAST(0.00 AS Numeric(18, 2)), 100, CAST(5000 AS Numeric(18, 0)), 200, 300, CAST(N'2017-11-25 00:00:00' AS SmallDateTime), CAST(N'2017-12-24 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (15, 46, CAST(0.00 AS Numeric(18, 2)), 150, CAST(4000 AS Numeric(18, 0)), 300, 450, CAST(N'2017-11-25 00:00:00' AS SmallDateTime), CAST(N'2017-12-24 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (16, 45, CAST(0.00 AS Numeric(18, 2)), 50, CAST(5000 AS Numeric(18, 0)), 300, 350, CAST(N'2017-12-25 00:00:00' AS SmallDateTime), CAST(N'2018-01-24 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (16, 46, CAST(0.00 AS Numeric(18, 2)), 150, CAST(4000 AS Numeric(18, 0)), 450, 600, CAST(N'2017-12-25 00:00:00' AS SmallDateTime), CAST(N'2018-01-24 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (18, 45, CAST(0.00 AS Numeric(18, 2)), 300, CAST(5000 AS Numeric(18, 0)), 0, 300, CAST(N'2018-04-29 00:00:00' AS SmallDateTime), CAST(N'2018-05-28 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (19, 45, CAST(0.00 AS Numeric(18, 2)), 400, CAST(5000 AS Numeric(18, 0)), 300, 700, CAST(N'2018-05-29 00:00:00' AS SmallDateTime), CAST(N'2018-06-28 00:00:00' AS SmallDateTime))
INSERT [dbo].[ProformaInvoiceItem] ([ProformaInvoiceId], [ProductId], [VATRate], [Quantity], [UnitPrice], [OldNumber], [NewNumber], [DateFrom], [DateTo]) VALUES (20, 2, CAST(0.05 AS Numeric(18, 2)), 2, CAST(7000000 AS Numeric(18, 0)), -1, -1, CAST(N'2018-05-29 00:00:00' AS SmallDateTime), CAST(N'2018-06-28 00:00:00' AS SmallDateTime))
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbed', N'Triều Anh', N'QL_0001', N'Quang Trung - Q. Gò Vấp', N'0969342904', 1)
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbee', N'Yan Hoàng', NULL, N'Ung Văn Khiêm - Q. Bình Thạnh', N'0937596120', 1)
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbef', N'Duy Ngô Quang', N'KT_0002', N'Q.12', NULL, 1)
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbej', N'Nguyễn Lê Nhật Trường', N'TruongNLN', N'Phan Huy Ích - Q. Gò Vấp', NULL, 3)
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejk', N'Nguyễn Gia Quốc', NULL, NULL, NULL, 3)
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbejl', N'Tâm Nguyễn', N'TamNTM', N'Cộng Hòa - Q. Tân Bình - Tp. HCM', N'0969964930', 3)
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'5655175f-20e7-43f9-b9c9-a09ab85f0c69', N'Hà Hoàng Vinh', NULL, N'HN', NULL, 2)
INSERT [dbo].[Staff] ([Id], [Name], [Code], [Address], [Tel], [CompanyId]) VALUES (N'bf41f04e-8c11-4071-9267-63f21188775c', N'Ngô Đăng Hà An', NULL, NULL, NULL, 5)
SET IDENTITY_INSERT [dbo].[Template] ON 

INSERT [dbo].[Template] ([Id], [Name], [Form], [Serial], [CompanyId], [FileUrl], [Amount], [BeginNo], [CurrentNo], [ReleaseDate], [ReleaseAnnouncementUrl], [IsActive]) VALUES (1, NULL, N'01GTKT0/003', N'MQ/17E', 1, N'/Files/TemplateInvoice/Invoice_0629V3.pdf', 100, 1, 3, CAST(N'2017-01-01 00:00:00' AS SmallDateTime), N'google.com', 1)
INSERT [dbo].[Template] ([Id], [Name], [Form], [Serial], [CompanyId], [FileUrl], [Amount], [BeginNo], [CurrentNo], [ReleaseDate], [ReleaseAnnouncementUrl], [IsActive]) VALUES (2, NULL, N'01GTKT0/001', N'AA/18E', 1, N'/Files/TemplateInvoice/Invoice_0629V3.pdf', 100, 1, 3, CAST(N'2018-05-05 00:00:00' AS SmallDateTime), N'google.com', 1)
INSERT [dbo].[Template] ([Id], [Name], [Form], [Serial], [CompanyId], [FileUrl], [Amount], [BeginNo], [CurrentNo], [ReleaseDate], [ReleaseAnnouncementUrl], [IsActive]) VALUES (3, NULL, N'01GTKT0/001', N'AA/18E', 3, N'/Files/TemplateInvoice/Invoice_0629V3.pdf', 200, 1, 1, CAST(N'2018-05-05 00:00:00' AS SmallDateTime), N'google.com', 1)
INSERT [dbo].[Template] ([Id], [Name], [Form], [Serial], [CompanyId], [FileUrl], [Amount], [BeginNo], [CurrentNo], [ReleaseDate], [ReleaseAnnouncementUrl], [IsActive]) VALUES (4, NULL, N'01GTKT0/001', N'AA/18E', 1, N'/Files/TemplateInvoice/Invoice_0629V3.pdf', 200, 101, 100, CAST(N'2018-06-09 00:00:00' AS SmallDateTime), N'google.com', 0)
INSERT [dbo].[Template] ([Id], [Name], [Form], [Serial], [CompanyId], [FileUrl], [Amount], [BeginNo], [CurrentNo], [ReleaseDate], [ReleaseAnnouncementUrl], [IsActive]) VALUES (5, NULL, N'01GTKT0/003', N'MQ/17E', 1, N'/Files/TemplateInvoice/Invoice_0629V3.pdf', 200, 101, 100, CAST(N'2018-06-26 00:00:00' AS SmallDateTime), N'google.com', 0)
INSERT [dbo].[Template] ([Id], [Name], [Form], [Serial], [CompanyId], [FileUrl], [Amount], [BeginNo], [CurrentNo], [ReleaseDate], [ReleaseAnnouncementUrl], [IsActive]) VALUES (7, NULL, N'01GTKT0/003', N'MQ/17E', 1, N'/Files/TemplateInvoice/Invoice_0629V3.pdf', 100, 301, 300, CAST(N'2018-06-26 00:00:00' AS SmallDateTime), N'google.com', 0)
SET IDENTITY_INSERT [dbo].[Template] OFF
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (7, 1, CAST(2200000 AS Numeric(18, 0)), CAST(N'2017-12-05' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 1, NULL)
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (8, 1, CAST(1100000 AS Numeric(18, 0)), CAST(N'2018-01-05' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 1, NULL)
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (10, 2, CAST(1500000 AS Numeric(18, 0)), CAST(N'2018-01-14' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeg', 1, N'Trả trước 1,100,000 cho T10-11')
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (11, 1, CAST(1500000 AS Numeric(18, 0)), CAST(N'2018-06-05' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 1, NULL)
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (12, 2, CAST(1500000 AS Numeric(18, 0)), CAST(N'2018-06-07' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 1, N'Trả đủ tiền T5')
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (13, 1, CAST(2000000 AS Numeric(18, 0)), CAST(N'2018-07-05' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 1, NULL)
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (14, 2, CAST(1300000 AS Numeric(18, 0)), CAST(N'2018-07-10' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 1, N'Trả trước 1,300,000 cho T6')
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (15, 2, CAST(700000 AS Numeric(18, 0)), CAST(N'2018-07-15' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 1, N'Trả đủ cho T6')
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (16, 1, CAST(14700000 AS Numeric(18, 0)), CAST(N'2018-07-06' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 3, NULL)
INSERT [dbo].[Transaction] ([Id], [Type], [Amount], [Date], [CustomerId], [CompanyId], [Note]) VALUES (17, 2, CAST(10000000 AS Numeric(18, 0)), CAST(N'2018-07-10' AS Date), N'0e0c4b27-ca8f-4a88-8ae4-6a8c63e7fbeh', 3, N'Trả trước 10,000,000')
SET IDENTITY_INSERT [dbo].[Transaction] OFF
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[CompanyCustomer]  WITH CHECK ADD  CONSTRAINT [FK_CompanyCustomer_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CompanyCustomer] CHECK CONSTRAINT [FK_CompanyCustomer_Company]
GO
ALTER TABLE [dbo].[CompanyCustomer]  WITH CHECK ADD  CONSTRAINT [FK_CompanyCustomer_Customer1] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[CompanyCustomer] CHECK CONSTRAINT [FK_CompanyCustomer_Customer1]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_AspNetUsers] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_AspNetUsers]
GO
ALTER TABLE [dbo].[CustomerProduct]  WITH CHECK ADD  CONSTRAINT [FK_CustomerProduct_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[CustomerProduct] CHECK CONSTRAINT [FK_CustomerProduct_Customer]
GO
ALTER TABLE [dbo].[CustomerProduct]  WITH CHECK ADD  CONSTRAINT [FK_CustomerProduct_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[CustomerProduct] CHECK CONSTRAINT [FK_CustomerProduct_Product]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Customer]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Staff]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Template] ([Id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Template]
GO
ALTER TABLE [dbo].[InvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceItem_Invoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([Id])
GO
ALTER TABLE [dbo].[InvoiceItem] CHECK CONSTRAINT [FK_InvoiceItem_Invoice]
GO
ALTER TABLE [dbo].[InvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[InvoiceItem] CHECK CONSTRAINT [FK_InvoiceItem_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Company]
GO
ALTER TABLE [dbo].[ProformaInvoice]  WITH CHECK ADD  CONSTRAINT [FK_ProformaInvoice_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[ProformaInvoice] CHECK CONSTRAINT [FK_ProformaInvoice_Customer]
GO
ALTER TABLE [dbo].[ProformaInvoice]  WITH CHECK ADD  CONSTRAINT [FK_ProformaInvoice_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([Id])
GO
ALTER TABLE [dbo].[ProformaInvoice] CHECK CONSTRAINT [FK_ProformaInvoice_Staff]
GO
ALTER TABLE [dbo].[ProformaInvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_ProformaInvoiceItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[ProformaInvoiceItem] CHECK CONSTRAINT [FK_ProformaInvoiceItem_Product]
GO
ALTER TABLE [dbo].[ProformaInvoiceItem]  WITH CHECK ADD  CONSTRAINT [FK_ProformaInvoiceItem_ProformaInvoice] FOREIGN KEY([ProformaInvoiceId])
REFERENCES [dbo].[ProformaInvoice] ([Id])
GO
ALTER TABLE [dbo].[ProformaInvoiceItem] CHECK CONSTRAINT [FK_ProformaInvoiceItem_ProformaInvoice]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_AspNetUsers] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_AspNetUsers]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Company]
GO
ALTER TABLE [dbo].[Template]  WITH CHECK ADD  CONSTRAINT [FK_Template_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Template] CHECK CONSTRAINT [FK_Template_Company]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Company]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Customer]
GO
