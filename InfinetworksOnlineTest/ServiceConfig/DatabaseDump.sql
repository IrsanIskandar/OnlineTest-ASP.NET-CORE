/*
SQLyog Ultimate v12.5.1 (32 bit)
MySQL - 10.1.36-MariaDB : Database - opsonlinetest
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE IF NOT EXISTS`opsonlinetest` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `opsonlinetest`;

/*Table structure for table `adm_infinet` */

DROP TABLE IF EXISTS `adm_infinet`;

CREATE TABLE `adm_infinet` (
  `ID` INT(11) NOT NULL AUTO_INCREMENT,
  `Username` VARCHAR(50) NOT NULL,
  `FullName` VARCHAR(255) DEFAULT NULL,
  `AccessPermission` VARCHAR(100) DEFAULT NULL,
  `Password_adm` VARCHAR(20) NOT NULL,
  `CreateDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsActive` TINYINT(2) NOT NULL DEFAULT '1',
  PRIMARY KEY (`ID`)
) ENGINE=INNODB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Data for the table `adm_infinet` */

INSERT  INTO `adm_infinet`(`ID`,`Username`,`FullName`,`AccessPermission`,`Password_adm`,`CreateDate`,`IsActive`) VALUES 
(1,'Admin','Admin Infinetworks','Admin','p4c0mn3t','2018-11-17 02:07:25',1),
(2,'User','User Infinetworks','User','p4c0mn3t','2018-11-17 02:09:44',1);

/*Table structure for table `answers_quest` */

DROP TABLE IF EXISTS `answers_quest`;

CREATE TABLE `answers_quest` (
  `ID` BIGINT(10) NOT NULL AUTO_INCREMENT,
  `AnswersOne` VARCHAR(2000) DEFAULT NULL,
  `AnswersTwo` VARCHAR(2000) DEFAULT NULL,
  `AnswersThree` VARCHAR(2000) DEFAULT NULL,
  `AnswersFourth` VARCHAR(2000) DEFAULT NULL,
  `AnswersFive` VARCHAR(2000) DEFAULT NULL,
  `AnswersSix` VARCHAR(2000) DEFAULT NULL,
  `AnswersSeven` VARCHAR(2000) DEFAULT NULL,
  `AnswersEight` VARCHAR(2000) DEFAULT NULL,
  `AnswersNine` VARCHAR(2000) DEFAULT NULL,
  `AnswersTen` VARCHAR(2000) DEFAULT NULL,
  `AnswersEleven` VARCHAR(2000) DEFAULT NULL,
  `AnswersTwelve` VARCHAR(2000) DEFAULT NULL,
  `DateAnswers` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Usr_InterviewID` BIGINT(10) NOT NULL,
  `QuestionID` BIGINT(10) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `Usr_InterviewID` (`Usr_InterviewID`),
  KEY `QuestionID` (`QuestionID`),
  CONSTRAINT `answers_quest_ibfk_1` FOREIGN KEY (`Usr_InterviewID`) REFERENCES `user_interview` (`ID`),
  CONSTRAINT `answers_quest_ibfk_2` FOREIGN KEY (`QuestionID`) REFERENCES `question` (`ID`)
) ENGINE=INNODB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Table structure for table `question` */

DROP TABLE IF EXISTS `question`;

CREATE TABLE `question` (
  `ID` BIGINT(10) NOT NULL AUTO_INCREMENT,
  `Question` VARCHAR(2000) NOT NULL,
  `SubQuestionID` BIGINT(10) DEFAULT NULL,
  `IsActive` BIT(1) NOT NULL DEFAULT b'1',
  `Usr_InterviewID` BIGINT(10) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `Usr_Interview` (`Usr_InterviewID`),
  KEY `SubQuestionID` (`SubQuestionID`),
  CONSTRAINT `question_ibfk_1` FOREIGN KEY (`Usr_InterviewID`) REFERENCES `user_interview` (`ID`),
  CONSTRAINT `question_ibfk_2` FOREIGN KEY (`SubQuestionID`) REFERENCES `subquestion` (`ID`)
) ENGINE=INNODB DEFAULT CHARSET=latin1;

/*Data for the table `question` */

/*Table structure for table `subquestion` */

DROP TABLE IF EXISTS `subquestion`;

CREATE TABLE `subquestion` (
  `ID` BIGINT(10) NOT NULL AUTO_INCREMENT,
  `SubQuestion` VARCHAR(2000) NOT NULL,
  `CreateDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsActive` BIT(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`ID`)
) ENGINE=INNODB DEFAULT CHARSET=latin1;

/*Data for the table `subquestion` */

/*Table structure for table `user_interview` */

DROP TABLE IF EXISTS `user_interview`;

CREATE TABLE `user_interview` (
  `ID` BIGINT(10) NOT NULL AUTO_INCREMENT,
  `NamaLengkap` VARCHAR(255) NOT NULL,
  `NomorTelepon` VARCHAR(18) NOT NULL,
  `Divisi` VARCHAR(255) NOT NULL,
  `TglInterview` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsActive` BIT(1) NOT NULL DEFAULT b'1',
  PRIMARY KEY (`ID`)
) ENGINE=INNODB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

/* Procedure structure for procedure `spINFINET_AddUserInterview` */

DELIMITER $$

USE `opsonlinetest`$$

DROP PROCEDURE IF EXISTS  `spINFINET_AddUserInterview`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `spINFINET_AddUserInterview`(
	OUT p_UserID BIGINT,
	IN p_FullName VARCHAR(255),
	IN p_PhoneNumber VARCHAR(18),
	IN p_Divisi VARCHAR(255)
)
BEGIN
	DECLARE v_GetDate DATETIME;
	
	SET v_GetDate = CURRENT_TIMESTAMP();
	
	INSERT INTO user_interview 
	(NamaLengkap, NomorTelepon, Divisi, TglInterview)
	VALUES
	(p_FullName, p_PhoneNumber, p_Divisi, v_GetDate);
	
	SET p_UserID = LAST_INSERT_ID();
	
	CALL `spINFINET_GetUserInterview` (p_UserID);
END$$
DELIMITER ;

/* Procedure structure for procedure `spINFINET_GetAllAnswerUser` */

DELIMITER $$

USE `opsonlinetest`$$

DROP PROCEDURE IF EXISTS  `spINFINET_GetAllAnswerUser`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `spINFINET_GetAllAnswerUser`()
BEGIN
	SELECT Answers.ID AS 'NomorUrut',
		UserInter.NamaLengkap AS 'NamaLengkap',
		UserInter.NomorTelepon AS 'NomorTelepon',
		UserInter.Divisi AS 'Divisi',
		Answers.AnswersOne AS 'AnswerOne',
		Answers.AnswersTwo AS 'AnswerTwo',
		Answers.AnswersThree AS 'AnswerThree',
		Answers.AnswersFourth AS 'AnswerFourth',
		Answers.AnswersFive AS 'AnswerFive',
		Answers.AnswersSix  AS 'AnswerSix',
		Answers.AnswersSeven AS 'AnswerSeven',
		Answers.AnswersEight AS 'AnswerEight',
		Answers.AnswersNine AS 'AnswerNine'
	FROM answers_quest AS Answers
	LEFT JOIN user_interview AS UserInter ON Answers.Usr_InterviewID = UserInter.ID AND UserInter.IsActive = 1
    ORDER BY Answers.ID ASC;
END$$
DELIMITER ;

/* Procedure structure for procedure `spINFINET_GetAnswersUser` */

DELIMITER $$

USE `opsonlinetest`$$

DROP PROCEDURE IF EXISTS `spINFINET_GetAnswersUser`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `spINFINET_GetAnswersUser`(
	IN p_AnswersID BIGINT
)
BEGIN
	SELECT Answers.ID AS 'NomorUrut',
		UserInter.NamaLengkap AS 'NamaLengkap',
		UserInter.NomorTelepon AS 'NomorTelepon',
		UserInter.Divisi AS 'Divisi',
		Answers.AnswersOne AS 'AnswerOne',
		Answers.AnswersTwo AS 'AnswerTwo',
		Answers.AnswersThree AS 'AnswerThree',
		Answers.AnswersFourth AS 'AnswerFourth',
		Answers.AnswersFive AS 'AnswerFive',
		Answers.AnswersSix  AS 'AnswerSix',
		Answers.AnswersSeven AS 'AnswerSeven',
		Answers.AnswersEight AS 'AnswerEight',
		Answers.AnswersNine AS 'AnswerNine'
	FROM answers_quest AS Answers
	LEFT JOIN user_interview AS UserInter ON Answers.Usr_InterviewID = UserInter.ID AND UserInter.IsActive = 1
	WHERE Answers.ID = p_AnswersID
    ORDER BY Answers.ID DESC;
END$$
DELIMITER ;

/* Procedure structure for procedure `spINFINET_GetLoginAdminInfinet` */

DELIMITER $$

USE `opsonlinetest`$$

DROP PROCEDURE IF EXISTS  `spINFINET_GetLoginAdminInfinet`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `spINFINET_GetLoginAdminInfinet`(
	IN p_Username VARCHAR(50),
    IN p_Password VARCHAR(20),
    IN p_AccessUser VARCHAR(100)
)
BEGIN
	SELECT Username, FullName, AccessPermission
	FROM adm_infinet
    WHERE UserName = p_Username AND Password_adm = p_Password AND AccessPermission = p_AccessUser AND IsActive = 1;
END$$
DELIMITER ;

/* Procedure structure for procedure `spINFINET_GetSearchAnswer` */

DELIMITER $$

USE `opsonlinetest`$$

DROP PROCEDURE IF EXISTS  `spINFINET_GetSearchAnswer`$$


CREATE DEFINER=`root`@`localhost` PROCEDURE `spINFINET_GetSearchAnswer`(
	IN p_AnswerID BIGINT(10),
    IN p_NamaLengkap VARCHAR(255)
)
BEGIN
	SELECT Answers.ID AS 'NomorUrut',
		UserInter.NamaLengkap AS 'NamaLengkap',
		UserInter.NomorTelepon AS 'NomorTelepon',
		UserInter.Divisi AS 'Divisi',
		Answers.AnswersOne AS 'AnswerOne',
		Answers.AnswersTwo AS 'AnswerTwo',
		Answers.AnswersThree AS 'AnswerThree',
		Answers.AnswersFourth AS 'AnswerFourth',
		Answers.AnswersFive AS 'AnswerFive',
		Answers.AnswersSix  AS 'AnswerSix',
		Answers.AnswersSeven AS 'AnswerSeven',
		Answers.AnswersEight AS 'AnswerEight',
		Answers.AnswersNine AS 'AnswerNine'
	FROM answers_quest AS Answers
	LEFT JOIN user_interview AS UserInter ON Answers.Usr_InterviewID = UserInter.ID AND UserInter.IsActive = 1
    WHERE Answers.ID = p_AnswerID AND UserInter.NamaLengkap = p_NamaLengkap
    ORDER BY Answers.ID ASC;
END$$
DELIMITER ;

/* Procedure structure for procedure `spINFINET_GetUserInterview` */

DELIMITER $$

USE `opsonlinetest`$$

DROP PROCEDURE IF EXISTS  `spINFINET_GetUserInterview`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `spINFINET_GetUserInterview`(
	IN p_UserID BIGINT
)
BEGIN
	SELECT ID AS UserID,
		NamaLengkap AS NamaLengkap,
		NomorTelepon AS NomorTelepon,
		Divisi AS Divisi
	FROM user_interview
	WHERE ID = p_UserID AND IsActive = 1;
END$$
DELIMITER ;

/* Procedure structure for procedure `spINFINET_InsertUserAnsware` */

DELIMITER $$

USE `opsonlinetest`$$

DROP PROCEDURE IF EXISTS  `spINFINET_InsertUserAnsware`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `spINFINET_InsertUserAnsware`(
	IN p_AnswersOne VARCHAR(2000),
    IN p_AnswersTwo VARCHAR(2000),
    IN p_AnswersThree VARCHAR(2000),
    IN p_AnswersFourth VARCHAR(2000),
    IN p_AnswersFive VARCHAR(2000),
    IN p_AnswersSix VARCHAR(2000),
    IN p_AnswersSeven VARCHAR(2000),
    IN p_AnswersEight VARCHAR(2000),
    IN p_AnswersNine VARCHAR(2000),
    IN p_UserInterviewID BIGINT
)
BEGIN
	DECLARE v_GetDate DATETIME;
    SET v_GetDate = CURRENT_TIMESTAMP();
    
    INSERT INTO answers_quest
    (AnswersOne, Answerstwo, AnswersThree, AnswersFourth, AnswersFive, AnswersSix, AnswersSeven, AnswersEight, AnswersNine, DateAnswers, Usr_InterviewID)
    VALUES
    (p_AnswersOne, p_AnswersTwo, p_AnswersThree, p_AnswersFourth, p_AnswersFive, p_AnswersSix, p_AnswersSeven, p_AnswersEight, p_AnswersNine, v_GetDate, p_UserInterviewID);
END$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

/*CREATE USER PRIVILLAGE*/

/*[8-May 01.16.48][1 ms]*/ 
USE `mysql`; 
/*[8-May 01.18.01][1 ms]*/ 
CREATE USER 'infinet'@'127.0.0.1' IDENTIFIED BY 'p4c0mn3t'; 
/*[8-May 01.18.01][1 ms]*/ 
FLUSH PRIVILEGES; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT * FROM `mysql`.`user` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT * FROM `mysql`.`db` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT Db, Table_name, Table_priv FROM `mysql`.`tables_priv` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT Db, Table_name, Column_name, Column_priv FROM `mysql`.`columns_priv` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT Db, Routine_name, Routine_type, Proc_priv FROM `mysql`.`procs_priv` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT * FROM `mysql`.`user` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT * FROM `mysql`.`db` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT Db, Table_name, Table_priv FROM `mysql`.`tables_priv` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT Db, Table_name, Column_name, Column_priv FROM `mysql`.`columns_priv` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.07][0 ms]*/ 
SELECT Db, Routine_name, Routine_type, Proc_priv FROM `mysql`.`procs_priv` WHERE USER = 'infinet' AND HOST = '127.0.0.1'; 
/*[8-May 01.18.31][1 ms]*/ 
GRANT ALTER, ALTER ROUTINE, 
CREATE, CREATE ROUTINE, 
CREATE TEMPORARY TABLES, 
CREATE VIEW, DELETE, DROP, 
EVENT, EXECUTE, INDEX, INSERT, 
LOCK TABLES, REFERENCES, SELECT, 
SHOW VIEW, TRIGGER, UPDATE ON `opsonlinetest`.* TO 'infinet'@'127.0.0.1' WITH GRANT OPTION; 
/*[8-May 01.18.34][0 ms]*/ 
USE `opsonlinetest`;  
