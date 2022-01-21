create database Helperland1

use [Helperland1]
go

Create Table Customer(
	Cust_Id int IDENTITY(1,1) PRIMARY KEY,
	FirstName varchar(30) NOT NULL,
	LastName varchar(30) NOT NULL,
	Email varchar(30) NOT NULL,
	[Password] varchar(20) NOT NULL,
	Mobile varchar(10) NOT NULL,
	Dob date,
	[language] varchar(10),
  	Role_Id int NOT NULL	
)

Create Table Service_Provider (
	Sp_Id int IDENTITY(1,1) PRIMARY KEY,
	FirstName varchar(30) NOT NULL,
	LastName varchar(30) NOT NULL,
	Email varchar(30) NOT NULL,
	[Password] varchar(20) NOT NULL,
	Mobile varchar(10) NOT NULL,
	Dob date,
	Nationality varchar(20),
	Gender varchar(10) NOT NULL,
	[Profile] image,
	[Address] varchar(30) NOT NULL,
	PostalCode int NOT NULL,
	City varchar(20) NOT NULL, 
  	Role_Id int NOT NULL	
)

Create Table Cust_Address(
	Address_Id int IDENTITY(1,1) PRIMARY KEY,
	[Address] varchar(30) NOT NULL,
	Cust_Id int,
	City varchar(30) NOT NULL,
	PostalCode int NOT NULL,
	Mobile varchar(10) NOT NULL,
)

Create Table [Service] (
	Serv_Id int IDENTITY(1,1) PRIMARY KEY,
	Cust_Id int,
	Sp_Id int,
	PostalCode int NOT NULL,
	ServiceDate date NOT NULL,
	ServiceTime time NOT NULL,
	[Hours] int NOT NULL,
	ExtraService varchar(10),
	Comments varchar(30),
	PatAtHome bit,
	[Address] varchar(30) NOT NULL, 
  	Payment int NOT NULL,
	[Status] varchar(10) NOT NULL 
)

Create Table Fav_SP(
	Fav_Id int IDENTITY(1,1) PRIMARY KEY,
	Cust_Id int,
	Sp_Id int,
	[Status] bit NOT NULL
)

Create Table Rate_SP(
	Rate_Id int IDENTITY(1,1) PRIMARY KEY,
	Cust_Id int,
	Sp_Id int,
	Serv_Id int,
	OnTimeRating int,
	FriendlyRating int,
	QualityRating int,
	Feedback varchar(50)
)

Create Table Block_Cust(
	Block_Id int IDENTITY(1,1) PRIMARY KEY,
	Cust_Id int,
	Sp_Id int,
	[Status] bit NOT NULL
)

Create Table Role_Detail(
	Role_Id int IDENTITY(1,1) PRIMARY KEY,
	[Role] varchar(10) NOT NULL
)

Create Table Admin_Detail(
	Admin_Id int IDENTITY(1,1) PRIMARY KEY,
	Username varchar(20) NOT NULL,
	[Password] varchar(20) NOT NULL
)

Create Table Info_Contact(
	Cont_Id int IDENTITY(1,1),
	FirstName varchar(30) NOT NULL,
	LastName varchar(30) NOT NULL,
	Email varchar(30) NOT NULL,
	Contact int NOT NULL,
	[Message] varchar(50),
	Subject varchar(30) NOT NULL,
)

Create Table Info_NewsLetter(
	News_Id int IDENTITY(1,1),
	Email varchar(30) NOT NULL,
)