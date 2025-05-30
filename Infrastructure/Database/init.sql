-- MySQL dump 10.13  Distrib 8.0.41, for Linux (x86_64)
--
-- Host: localhost    Database: CoworkingSpace
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Amenity`
--

DROP TABLE IF EXISTS `Amenity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Amenity` (
  `AmenityId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`AmenityId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Amenity`
--

LOCK TABLES `Amenity` WRITE;
/*!40000 ALTER TABLE `Amenity` DISABLE KEYS */;
INSERT INTO `Amenity` VALUES (1,'Amenity1','123');
/*!40000 ALTER TABLE `Amenity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetRoleClaims`
--

DROP TABLE IF EXISTS `AspNetRoleClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetRoleClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetRoleClaims`
--

LOCK TABLES `AspNetRoleClaims` WRITE;
/*!40000 ALTER TABLE `AspNetRoleClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetRoleClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetRoles`
--

DROP TABLE IF EXISTS `AspNetRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetRoles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetRoles`
--

LOCK TABLES `AspNetRoles` WRITE;
/*!40000 ALTER TABLE `AspNetRoles` DISABLE KEYS */;
INSERT INTO `AspNetRoles` VALUES (1,'Customer','CUSTOMER',NULL),(2,'Host','HOST',NULL);
/*!40000 ALTER TABLE `AspNetRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserClaims`
--

DROP TABLE IF EXISTS `AspNetUserClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserClaims`
--

LOCK TABLES `AspNetUserClaims` WRITE;
/*!40000 ALTER TABLE `AspNetUserClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserLogins`
--

DROP TABLE IF EXISTS `AspNetUserLogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserLogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` int NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserLogins`
--

LOCK TABLES `AspNetUserLogins` WRITE;
/*!40000 ALTER TABLE `AspNetUserLogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserLogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserRoles`
--

DROP TABLE IF EXISTS `AspNetUserRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserRoles` (
  `UserId` int NOT NULL,
  `RoleId` int NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserRoles`
--

LOCK TABLES `AspNetUserRoles` WRITE;
/*!40000 ALTER TABLE `AspNetUserRoles` DISABLE KEYS */;
INSERT INTO `AspNetUserRoles` VALUES (1,1),(2,1),(3,1),(4,1),(3,2);
/*!40000 ALTER TABLE `AspNetUserRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserTokens`
--

DROP TABLE IF EXISTS `AspNetUserTokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserTokens` (
  `UserId` int NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserTokens`
--

LOCK TABLES `AspNetUserTokens` WRITE;
/*!40000 ALTER TABLE `AspNetUserTokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserTokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUsers`
--

DROP TABLE IF EXISTS `AspNetUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUsers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FullName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AvatarUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUsers`
--

LOCK TABLES `AspNetUsers` WRITE;
/*!40000 ALTER TABLE `AspNetUsers` DISABLE KEYS */;
INSERT INTO `AspNetUsers` VALUES (1,'Trịnh Đức Thưởng',NULL,'DuckThuong','DUCKTHUONG','duckthuong@gmail.com','DUCKTHUONG@GMAIL.COM',0,'AQAAAAIAAYagAAAAEJ+I2b3isHbW8/qrffK2NsHtsJhBcdFFBbJBIa+QMfbj4K41P2MUEP2o28VvBkLfMQ==','JBBQNTV3M2ZJKX4WGX4U22V4DNCOL5JE','474600c9-5600-4093-96d6-33dd821ae02e',NULL,0,0,NULL,1,0),(2,'Hoàng Trung','https://lh3.googleusercontent.com/a/ACg8ocKAVfGAFmXDsx2IMO4WYg8pAqAoc3JtcsjDq3qcK9USIvcJ9Q=s96-c','trunghoang220703@gmail.com','TRUNGHOANG220703@GMAIL.COM','trunghoang220703@gmail.com','TRUNGHOANG220703@GMAIL.COM',1,NULL,'6VCMVZBCCN3TN7RBNAUXV7GP2KGZ6WVR','68702427-fce1-4228-b2c3-9126866ac9b0',NULL,0,0,NULL,1,0),(3,'Hoàng Việt Trung',NULL,'trung2207','TRUNG2207','trunghoang220703@gmail.com','TRUNGHOANG220703@GMAIL.COM',0,'AQAAAAIAAYagAAAAEJco7sL6deU4sZan/mwlfOhKhkht+rr4xYSthW1f4QaT0jAvjEKmXq98ek/VO8a07w==','VHSJRQST7LBJN2QRBHNHZAUKRBPDZBOZ','48592cc0-5eee-4149-a6d3-a44bce8b4ea1','0327822096',0,0,NULL,1,0),(4,'Trịnh Đức Thưởng',NULL,'ducthuong','DUCTHUONG','trinhthuong26022003@gmail.com','TRINHTHUONG26022003@GMAIL.COM',0,'AQAAAAIAAYagAAAAECAVV3Em4nx3jI31X6wYB6Oe1p0CtPC87nHy3u2d8jnnXo7pPCi+aMgtKVDrWB1Ivg==','DYI2FN7PRCO4QRX7FKFJXVOFFHQIYCSI','449c233c-4f7d-48e4-a5a1-2203f5707f05',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `AspNetUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `BookingWindow`
--

DROP TABLE IF EXISTS `BookingWindow`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `BookingWindow` (
  `BookingWindowId` int NOT NULL AUTO_INCREMENT,
  `MinNotice` int NOT NULL,
  `MaxNoticeDays` int DEFAULT NULL,
  `DisplayOnCalendar` tinyint(1) DEFAULT NULL,
  `Unit` int NOT NULL,
  PRIMARY KEY (`BookingWindowId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `BookingWindow`
--

LOCK TABLES `BookingWindow` WRITE;
/*!40000 ALTER TABLE `BookingWindow` DISABLE KEYS */;
INSERT INTO `BookingWindow` VALUES (1,3,3,NULL,1);
/*!40000 ALTER TABLE `BookingWindow` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Collection`
--

DROP TABLE IF EXISTS `Collection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Collection` (
  `CollectionId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`CollectionId`),
  KEY `IX_Collection_UserId` (`UserId`),
  CONSTRAINT `FK_Collection_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Collection`
--

LOCK TABLES `Collection` WRITE;
/*!40000 ALTER TABLE `Collection` DISABLE KEYS */;
/*!40000 ALTER TABLE `Collection` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ExceptionRule`
--

DROP TABLE IF EXISTS `ExceptionRule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ExceptionRule` (
  `ExceptionId` int NOT NULL AUTO_INCREMENT,
  `Unit` int NOT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  `StartTime` time(6) DEFAULT NULL,
  `EndTime` time(6) DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`ExceptionId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ExceptionRule`
--

LOCK TABLES `ExceptionRule` WRITE;
/*!40000 ALTER TABLE `ExceptionRule` DISABLE KEYS */;
INSERT INTO `ExceptionRule` VALUES (1,2,NULL,NULL,NULL,NULL,'Test');
/*!40000 ALTER TABLE `ExceptionRule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ExceptionRuleDay`
--

DROP TABLE IF EXISTS `ExceptionRuleDay`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ExceptionRuleDay` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ExceptionRuleId` int NOT NULL,
  `DayOfWeek` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ExceptionRuleDay_ExceptionRuleId` (`ExceptionRuleId`),
  CONSTRAINT `FK_ExceptionRuleDay_ExceptionRule_ExceptionRuleId` FOREIGN KEY (`ExceptionRuleId`) REFERENCES `ExceptionRule` (`ExceptionId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ExceptionRuleDay`
--

LOCK TABLES `ExceptionRuleDay` WRITE;
/*!40000 ALTER TABLE `ExceptionRuleDay` DISABLE KEYS */;
INSERT INTO `ExceptionRuleDay` VALUES (1,1,1),(2,1,2),(3,1,3),(4,1,4),(5,1,5);
/*!40000 ALTER TABLE `ExceptionRuleDay` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `GuestHour`
--

DROP TABLE IF EXISTS `GuestHour`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `GuestHour` (
  `GuestHourId` int NOT NULL AUTO_INCREMENT,
  `VenueId` int NOT NULL,
  `DayOfWeek` int NOT NULL,
  `StartTime` time(6) DEFAULT NULL,
  `EndTime` time(6) DEFAULT NULL,
  `IsOpen24Hours` tinyint(1) NOT NULL,
  `IsClosed` tinyint(1) NOT NULL,
  PRIMARY KEY (`GuestHourId`),
  KEY `IX_GuestHour_VenueId` (`VenueId`),
  CONSTRAINT `FK_GuestHour_Venue_VenueId` FOREIGN KEY (`VenueId`) REFERENCES `Venue` (`VenueId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `GuestHour`
--

LOCK TABLES `GuestHour` WRITE;
/*!40000 ALTER TABLE `GuestHour` DISABLE KEYS */;
INSERT INTO `GuestHour` VALUES (15,3,0,'09:00:00.000000','18:00:00.000000',0,0),(16,3,1,'09:00:00.000000','18:00:00.000000',0,0),(17,3,2,'09:00:00.000000','18:00:00.000000',0,0),(18,3,3,'09:00:00.000000','18:00:00.000000',0,0),(19,3,4,'09:00:00.000000','18:00:00.000000',0,0),(20,3,5,'09:00:00.000000','18:00:00.000000',0,0),(21,3,6,'09:00:00.000000','18:00:00.000000',0,0);
/*!40000 ALTER TABLE `GuestHour` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Holiday`
--

DROP TABLE IF EXISTS `Holiday`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Holiday` (
  `HolidayId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`HolidayId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Holiday`
--

LOCK TABLES `Holiday` WRITE;
/*!40000 ALTER TABLE `Holiday` DISABLE KEYS */;
INSERT INTO `Holiday` VALUES (1,'Tết Dương Lịch'),(2,'Tết Nguyên Đán'),(3,'Giỗ Tổ Hùng Vương'),(4,'Ngày Thống Nhất và Quốc tế Lao động'),(5,'Ngày Quốc Khánh');
/*!40000 ALTER TABLE `Holiday` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HolidayDate`
--

DROP TABLE IF EXISTS `HolidayDate`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HolidayDate` (
  `HolidayDateId` int NOT NULL AUTO_INCREMENT,
  `HolidayId` int NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`HolidayDateId`),
  KEY `IX_HolidayDate_HolidayId` (`HolidayId`),
  CONSTRAINT `FK_HolidayDate_Holiday_HolidayId` FOREIGN KEY (`HolidayId`) REFERENCES `Holiday` (`HolidayId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HolidayDate`
--

LOCK TABLES `HolidayDate` WRITE;
/*!40000 ALTER TABLE `HolidayDate` DISABLE KEYS */;
/*!40000 ALTER TABLE `HolidayDate` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Payment`
--

DROP TABLE IF EXISTS `Payment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Payment` (
  `PaymentId` int NOT NULL AUTO_INCREMENT,
  `ReservationId` int NOT NULL,
  `CustomerId` int NOT NULL,
  `Amount` decimal(65,30) NOT NULL,
  `PaymentMethod` int NOT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`PaymentId`),
  UNIQUE KEY `IX_Payment_ReservationId` (`ReservationId`),
  KEY `IX_Payment_CustomerId` (`CustomerId`),
  CONSTRAINT `FK_Payment_AspNetUsers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Payment_Reservation_ReservationId` FOREIGN KEY (`ReservationId`) REFERENCES `Reservation` (`ReservationId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Payment`
--

LOCK TABLES `Payment` WRITE;
/*!40000 ALTER TABLE `Payment` DISABLE KEYS */;
/*!40000 ALTER TABLE `Payment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `PaymentInfo`
--

DROP TABLE IF EXISTS `PaymentInfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `PaymentInfo` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `FullName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `BankCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AccountNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IdentityNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_PaymentInfo_UserId` (`UserId`),
  CONSTRAINT `FK_PaymentInfo_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `PaymentInfo`
--

LOCK TABLES `PaymentInfo` WRITE;
/*!40000 ALTER TABLE `PaymentInfo` DISABLE KEYS */;
/*!40000 ALTER TABLE `PaymentInfo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Price`
--

DROP TABLE IF EXISTS `Price`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Price` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TimeUnit` int NOT NULL,
  `DiscountPercentage` decimal(10,2) DEFAULT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `SetupFee` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Price`
--

LOCK TABLES `Price` WRITE;
/*!40000 ALTER TABLE `Price` DISABLE KEYS */;
INSERT INTO `Price` VALUES (4,0,NULL,1000.00,NULL),(5,0,NULL,1000.00,NULL),(6,0,NULL,1000.00,NULL),(7,0,NULL,1000.00,NULL),(8,0,NULL,1000.00,NULL),(9,0,NULL,1000.00,NULL),(10,0,NULL,1000.00,NULL),(11,0,NULL,1000.00,NULL),(12,0,NULL,1000.00,NULL),(13,0,NULL,1000.00,NULL),(14,0,NULL,1000.00,NULL),(15,0,NULL,1000.00,NULL),(16,0,NULL,1000.00,NULL),(17,0,NULL,1000.00,NULL),(18,0,NULL,1000.00,NULL),(19,0,NULL,1000.00,NULL),(20,0,NULL,1000.00,NULL),(21,0,NULL,1000.00,NULL),(22,0,NULL,1000.00,NULL),(23,0,NULL,1000.00,NULL),(24,0,NULL,1000.00,NULL),(25,0,NULL,1000.00,NULL),(26,0,NULL,1000.00,NULL),(27,0,NULL,1000.00,NULL),(28,0,NULL,1000.00,NULL),(29,0,NULL,1000.00,NULL),(30,0,NULL,1000.00,NULL),(31,0,NULL,1000.00,NULL),(34,0,NULL,1000.00,NULL),(35,0,NULL,1000.00,NULL);
/*!40000 ALTER TABLE `Price` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Reservation`
--

DROP TABLE IF EXISTS `Reservation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Reservation` (
  `ReservationId` int NOT NULL AUTO_INCREMENT,
  `CustomerId` int NOT NULL,
  `SpaceId` int NOT NULL,
  `ReservationDate` datetime(6) NOT NULL,
  `StartTime` time(6) NOT NULL,
  `EndTime` time(6) NOT NULL,
  `NumPeople` int NOT NULL,
  `TotalPrice` decimal(10,2) NOT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`ReservationId`),
  KEY `IX_Reservation_CustomerId` (`CustomerId`),
  KEY `IX_Reservation_SpaceId` (`SpaceId`),
  CONSTRAINT `FK_Reservation_AspNetUsers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Reservation_Space_SpaceId` FOREIGN KEY (`SpaceId`) REFERENCES `Space` (`SpaceId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Reservation`
--

LOCK TABLES `Reservation` WRITE;
/*!40000 ALTER TABLE `Reservation` DISABLE KEYS */;
/*!40000 ALTER TABLE `Reservation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Review`
--

DROP TABLE IF EXISTS `Review`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Review` (
  `ReviewId` int NOT NULL AUTO_INCREMENT,
  `CustomerId` int NOT NULL,
  `SpaceId` int NOT NULL,
  `Rating` int NOT NULL,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`ReviewId`),
  KEY `IX_Review_CustomerId` (`CustomerId`),
  KEY `IX_Review_SpaceId` (`SpaceId`),
  CONSTRAINT `FK_Review_AspNetUsers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Review_Space_SpaceId` FOREIGN KEY (`SpaceId`) REFERENCES `Space` (`SpaceId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Review`
--

LOCK TABLES `Review` WRITE;
/*!40000 ALTER TABLE `Review` DISABLE KEYS */;
/*!40000 ALTER TABLE `Review` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Space`
--

DROP TABLE IF EXISTS `Space`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Space` (
  `SpaceId` int NOT NULL AUTO_INCREMENT,
  `SpaceTypeId` int NOT NULL,
  `VenueId` int NOT NULL,
  `BookingWindowId` int DEFAULT NULL,
  `ExceptionId` int DEFAULT NULL,
  `PriceId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ListingType` int NOT NULL,
  `Capacity` int DEFAULT NULL,
  `Size` int DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `Deposit` int DEFAULT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`SpaceId`),
  KEY `IX_Space_BookingWindowId` (`BookingWindowId`),
  KEY `IX_Space_ExceptionId` (`ExceptionId`),
  KEY `IX_Space_PriceId` (`PriceId`),
  KEY `IX_Space_SpaceTypeId` (`SpaceTypeId`),
  KEY `IX_Space_VenueId` (`VenueId`),
  CONSTRAINT `FK_Space_BookingWindow_BookingWindowId` FOREIGN KEY (`BookingWindowId`) REFERENCES `BookingWindow` (`BookingWindowId`),
  CONSTRAINT `FK_Space_ExceptionRule_ExceptionId` FOREIGN KEY (`ExceptionId`) REFERENCES `ExceptionRule` (`ExceptionId`),
  CONSTRAINT `FK_Space_Price_PriceId` FOREIGN KEY (`PriceId`) REFERENCES `Price` (`Id`),
  CONSTRAINT `FK_Space_SpaceType_SpaceTypeId` FOREIGN KEY (`SpaceTypeId`) REFERENCES `SpaceType` (`SpaceTypeId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Space_Venue_VenueId` FOREIGN KEY (`VenueId`) REFERENCES `Venue` (`VenueId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Space`
--

LOCK TABLES `Space` WRITE;
/*!40000 ALTER TABLE `Space` DISABLE KEYS */;
INSERT INTO `Space` VALUES (1,17,3,NULL,NULL,35,'Space01','123',1,NULL,NULL,1,NULL,0,'2025-05-15 02:13:49.598941','2025-05-15 02:13:49.598942');
/*!40000 ALTER TABLE `Space` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SpaceAmenity`
--

DROP TABLE IF EXISTS `SpaceAmenity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SpaceAmenity` (
  `SpaceAmenityId` int NOT NULL AUTO_INCREMENT,
  `SpaceId` int NOT NULL,
  `AmenityId` int NOT NULL,
  PRIMARY KEY (`SpaceAmenityId`),
  KEY `IX_SpaceAmenity_AmenityId` (`AmenityId`),
  KEY `IX_SpaceAmenity_SpaceId` (`SpaceId`),
  CONSTRAINT `FK_SpaceAmenity_Amenity_AmenityId` FOREIGN KEY (`AmenityId`) REFERENCES `Amenity` (`AmenityId`) ON DELETE CASCADE,
  CONSTRAINT `FK_SpaceAmenity_Space_SpaceId` FOREIGN KEY (`SpaceId`) REFERENCES `Space` (`SpaceId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SpaceAmenity`
--

LOCK TABLES `SpaceAmenity` WRITE;
/*!40000 ALTER TABLE `SpaceAmenity` DISABLE KEYS */;
INSERT INTO `SpaceAmenity` VALUES (3,33,1);
/*!40000 ALTER TABLE `SpaceAmenity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SpaceAsset`
--

DROP TABLE IF EXISTS `SpaceAsset`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SpaceAsset` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SpaceId` int NOT NULL,
  `Url` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Type` int DEFAULT NULL,
  `UploadedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SpaceAsset_SpaceId` (`SpaceId`),
  CONSTRAINT `FK_SpaceAsset_Space_SpaceId` FOREIGN KEY (`SpaceId`) REFERENCES `Space` (`SpaceId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SpaceAsset`
--

LOCK TABLES `SpaceAsset` WRITE;
/*!40000 ALTER TABLE `SpaceAsset` DISABLE KEYS */;
INSERT INTO `SpaceAsset` VALUES (2,1,'https://res.cloudinary.com/dpsqzogd2/image/upload/v1747276171/Coworking-Space/Images/hiynjj9au8jfufvd6zwy.png',1,'2025-05-15 02:29:30.659282');
/*!40000 ALTER TABLE `SpaceAsset` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SpaceCollection`
--

DROP TABLE IF EXISTS `SpaceCollection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SpaceCollection` (
  `SpaceCollectionId` int NOT NULL AUTO_INCREMENT,
  `SpaceId` int NOT NULL,
  `CollectionId` int NOT NULL,
  PRIMARY KEY (`SpaceCollectionId`),
  KEY `IX_SpaceCollection_CollectionId` (`CollectionId`),
  KEY `IX_SpaceCollection_SpaceId` (`SpaceId`),
  CONSTRAINT `FK_SpaceCollection_Collection_CollectionId` FOREIGN KEY (`CollectionId`) REFERENCES `Collection` (`CollectionId`) ON DELETE CASCADE,
  CONSTRAINT `FK_SpaceCollection_Space_SpaceId` FOREIGN KEY (`SpaceId`) REFERENCES `Space` (`SpaceId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SpaceCollection`
--

LOCK TABLES `SpaceCollection` WRITE;
/*!40000 ALTER TABLE `SpaceCollection` DISABLE KEYS */;
/*!40000 ALTER TABLE `SpaceCollection` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SpaceType`
--

DROP TABLE IF EXISTS `SpaceType`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SpaceType` (
  `SpaceTypeId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsDaiLySpaceType` tinyint(1) NOT NULL,
  PRIMARY KEY (`SpaceTypeId`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SpaceType`
--

LOCK TABLES `SpaceType` WRITE;
/*!40000 ALTER TABLE `SpaceType` DISABLE KEYS */;
INSERT INTO `SpaceType` VALUES (12,'Open Desk (Bàn làm việc mở)','Đây là một chỗ làm việc không cố định trong một không gian chung. Bạn sẽ chia sẻ không gian với những người khác và có thể chọn bất kỳ bàn trống nào để làm việc.',0),(13,'Dedicated Desk (Bàn làm việc riêng)','Đây là một bàn làm việc được chỉ định riêng cho bạn trong một không gian chung. Dù vẫn chia sẻ không gian chung với người khác, bạn sẽ có một chiếc bàn cố định của riêng mình.',0),(14,'Private Office (Văn phòng riêng)','Đây là một phòng làm việc riêng tư dành cho một cá nhân. Bạn sẽ có không gian riêng hoàn toàn, tách biệt với những người khác.',0),(15,'Team Office (Văn phòng cho đội nhóm)','Đây là một phòng làm việc riêng tư được thiết kế cho một nhóm người làm việc cùng nhau.',0),(16,'Office Suite (Tổ hợp văn phòng)','Đây là một không gian văn phòng riêng tư, đa chức năng và hoàn chỉnh dành cho một đội nhóm. Nó có thể bao gồm nhiều phòng hoặc khu vực khác nhau cho các mục đích sử dụng khác nhau.',0),(17,'Coworking (Không gian làm việc chung)','Đây là một không gian nơi nhiều cá nhân hoặc công ty cùng chia sẻ cơ sở vật chất và tiện ích làm việc.',1),(18,'Event Space (Không gian sự kiện)','Đây là khu vực được cho thuê để tổ chức các sự kiện như hội thảo, workshop, buổi ra mắt sản phẩm, hoặc các buổi gặp mặt. Quy mô và tiện nghi của không gian sự kiện có thể rất đa dạng.',1),(19,'Hotel Lobby (Sảnh khách sạn)','Một số khách sạn cho phép hoặc có khu vực riêng tại sảnh để mọi người có thể ngồi làm việc hoặc gặp gỡ ngắn. Đây có thể là một lựa chọn linh hoạt và tiện lợi.',1),(20,'Private Meeting Room (Phòng họp riêng)','Đây là một phòng kín, được trang bị các thiết bị cần thiết cho các cuộc họp (ví dụ: bàn ghế, máy chiếu, bảng trắng). Nó đảm bảo sự riêng tư và không gian tập trung cho các buổi thảo luận.',1),(21,'Semi-private Meeting Space (Không gian họp bán riêng tư)',' Đây có thể là một khu vực được ngăn cách một phần (ví dụ bằng vách ngăn thấp, cây cối, hoặc cách bài trí nội thất) để tạo không gian tương đối riêng tư cho các cuộc họp nhóm nhỏ hoặc thảo luận không yêu cầu bảo mật tuyệt đối. Nó không hoàn toàn kín như \"Private Meeting Room\".',1),(22,'Training Room (Phòng đào tạo)','Tương tự như phòng họp nhưng thường được thiết kế và trang bị đặc thù cho các buổi đào tạo, huấn luyện, có thể có cách sắp xếp bàn ghế linh hoạt (kiểu lớp học, chữ U,...) và các công cụ hỗ trợ giảng dạy.',1);
/*!40000 ALTER TABLE `SpaceType` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Venue`
--

DROP TABLE IF EXISTS `Venue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Venue` (
  `VenueId` int NOT NULL AUTO_INCREMENT,
  `HostId` int NOT NULL,
  `VenueTypeId` int NOT NULL,
  `VenueAddressId` int NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `LogoUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Floor` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`VenueId`),
  KEY `IX_Venue_HostId` (`HostId`),
  KEY `IX_Venue_VenueAddressId` (`VenueAddressId`),
  KEY `IX_Venue_VenueTypeId` (`VenueTypeId`),
  CONSTRAINT `FK_Venue_AspNetUsers_HostId` FOREIGN KEY (`HostId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Venue_VenueAddress_VenueAddressId` FOREIGN KEY (`VenueAddressId`) REFERENCES `VenueAddress` (`VenueAddressId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Venue_VenueType_VenueTypeId` FOREIGN KEY (`VenueTypeId`) REFERENCES `VenueType` (`VenueTypeId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Venue`
--

LOCK TABLES `Venue` WRITE;
/*!40000 ALTER TABLE `Venue` DISABLE KEYS */;
INSERT INTO `Venue` VALUES (3,3,1,10,'Trung','123',NULL,'8');
/*!40000 ALTER TABLE `Venue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `VenueAddress`
--

DROP TABLE IF EXISTS `VenueAddress`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `VenueAddress` (
  `VenueAddressId` int NOT NULL AUTO_INCREMENT,
  `District` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Street` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `City` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Latitude` decimal(65,30) DEFAULT NULL,
  `Longitude` decimal(65,30) DEFAULT NULL,
  `FullAddress` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`VenueAddressId`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `VenueAddress`
--

LOCK TABLES `VenueAddress` WRITE;
/*!40000 ALTER TABLE `VenueAddress` DISABLE KEYS */;
INSERT INTO `VenueAddress` VALUES (10,'Hoàng Mai','Nguyễn An Ninh','Hà Nội',NULL,NULL,'Nguyễn An Ninh, Hoàng Mai, Hà Nội');
/*!40000 ALTER TABLE `VenueAddress` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `VenueHoliday`
--

DROP TABLE IF EXISTS `VenueHoliday`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `VenueHoliday` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `VenueId` int NOT NULL,
  `HolidayId` int NOT NULL,
  `IsObserved` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_VenueHoliday_HolidayId` (`HolidayId`),
  KEY `IX_VenueHoliday_VenueId` (`VenueId`),
  CONSTRAINT `FK_VenueHoliday_Holiday_HolidayId` FOREIGN KEY (`HolidayId`) REFERENCES `Holiday` (`HolidayId`) ON DELETE CASCADE,
  CONSTRAINT `FK_VenueHoliday_Venue_VenueId` FOREIGN KEY (`VenueId`) REFERENCES `Venue` (`VenueId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `VenueHoliday`
--

LOCK TABLES `VenueHoliday` WRITE;
/*!40000 ALTER TABLE `VenueHoliday` DISABLE KEYS */;
INSERT INTO `VenueHoliday` VALUES (8,3,1,0),(9,3,2,0),(10,3,3,0),(11,3,4,0),(12,3,5,0);
/*!40000 ALTER TABLE `VenueHoliday` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `VenueImage`
--

DROP TABLE IF EXISTS `VenueImage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `VenueImage` (
  `VenueImageId` int NOT NULL AUTO_INCREMENT,
  `VenueId` int NOT NULL,
  `ImageUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UploadedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`VenueImageId`),
  KEY `IX_VenueImage_VenueId` (`VenueId`),
  CONSTRAINT `FK_VenueImage_Venue_VenueId` FOREIGN KEY (`VenueId`) REFERENCES `Venue` (`VenueId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `VenueImage`
--

LOCK TABLES `VenueImage` WRITE;
/*!40000 ALTER TABLE `VenueImage` DISABLE KEYS */;
/*!40000 ALTER TABLE `VenueImage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `VenueType`
--

DROP TABLE IF EXISTS `VenueType`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `VenueType` (
  `VenueTypeId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `VenueTypePictureUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`VenueTypeId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `VenueType`
--

LOCK TABLES `VenueType` WRITE;
/*!40000 ALTER TABLE `VenueType` DISABLE KEYS */;
INSERT INTO `VenueType` VALUES (1,'Văn phòng dịch vụ','Văn phòng dịch vụ với các văn phòng riêng, phòng họp và tiện ích dùng chung.','https://res.cloudinary.com/dpsqzogd2/image/upload/w_1000,ar_16:9,c_fill,g_auto,e_sharpen/v1743759085/servicedOffice_bujyqf.jpg'),(2,'Không gian làm việc chung','Không gian làm việc chung với chỗ ngồi mở, văn phòng, phòng họp và các tiện ích dùng chung','https://res.cloudinary.com/dpsqzogd2/image/upload/w_1000,ar_16:9,c_fill,g_auto,e_sharpen/v1743759085/CoworkingSpace_kiyqmd.jpg'),(3,'Doanh nghiệp tư nhân','Một doanh nghiệp tư nhân có không gian thừa trong văn phòng để cho các doanh nghiệp khác thuê.','https://res.cloudinary.com/dpsqzogd2/image/upload/w_1000,ar_16:9,c_fill,g_auto,e_sharpen/v1743759085/PrivateBussiness_rll8bw.jpg'),(4,'Tòa nhà văn phòng','Một tòa nhà văn phòng mà chủ sở hữu có các văn phòng cho thuê trực tiếp trên LiquidSpace.','https://res.cloudinary.com/dpsqzogd2/image/upload/w_1000,ar_16:9,c_fill,g_auto,e_sharpen/v1743759085/OfficeBuilding_wlmibh.jpg'),(5,'Khách sạn','Một khách sạn có phòng họp và không gian làm việc cho thuê theo giờ hoặc theo ngày.','https://res.cloudinary.com/dpsqzogd2/image/upload/w_1000,ar_16:9,c_fill,g_auto,e_sharpen/v1743759086/Hotel_pcuguo.jpg'),(6,'Độc nhất','Một loại hình kinh doanh thuộc danh mục riêng biệt, ví dụ: bảo tàng, nhà máy rượu vang, thư viện, nhà thờ và nhiều loại khác.','https://res.cloudinary.com/dpsqzogd2/image/upload/w_1000,ar_16:9,c_fill,g_auto,e_sharpen/v1743759085/OneOfAkind_ucgojy.jpg');
/*!40000 ALTER TABLE `VenueType` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20250421101110_InitDB','9.0.3'),('20250423082845_updateHoliday','9.0.3'),('20250423083408_updateHoliday2','9.0.3'),('20250425081813_InitDb','9.0.3'),('20250428042852_AddExceptionRule','9.0.3'),('20250428080712_fixExceptionRule','9.0.3'),('20250429092532_new','9.0.3'),('20250506084107_Init','9.0.3'),('20250506092358_Init','9.0.3'),('20250509023255_Init','9.0.3'),('20250512080409_Init','9.0.3'),('20250512091642_config','9.0.3'),('20250513081814_Init','9.0.3'),('20250514094535_Init','9.0.3');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-15 16:07:51
