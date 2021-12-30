CREATE DATABASE `voting_system` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE voting_system;
CREATE TABLE `voters` (
  `voter_id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(45) DEFAULT NULL,
  `last_name` varchar(45) DEFAULT NULL,
  `address` varchar(100) DEFAULT NULL,
  `username` varchar(45) DEFAULT NULL,
  `password` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`voter_id`),
  UNIQUE KEY `voter_id_UNIQUE` (`voter_id`),
  UNIQUE KEY `username_UNIQUE` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
CREATE TABLE `elections` (
  `election_id` int NOT NULL AUTO_INCREMENT,
  `election_year` int DEFAULT NULL,
  `active` tinyint DEFAULT '0',
  PRIMARY KEY (`election_id`),
  UNIQUE KEY `election_id_UNIQUE` (`election_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
CREATE TABLE `issues` (
  `issue_id` int NOT NULL AUTO_INCREMENT,
  `issue_description` varchar(100) DEFAULT NULL,
  `issue_title` varchar(45) DEFAULT NULL,
  `election_id` int DEFAULT '-1',
  PRIMARY KEY (`issue_id`),
  UNIQUE KEY `issue_id_UNIQUE` (`issue_id`),
  KEY `election_id_idx` (`election_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
CREATE TABLE `candidates` (
  `candidate_id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(45) DEFAULT NULL,
  `last_name` varchar(45) DEFAULT NULL,
  `affiliation` varchar(45) DEFAULT NULL,
  `assigned_election` int DEFAULT NULL,
  PRIMARY KEY (`candidate_id`),
  UNIQUE KEY `canidate_id_UNIQUE` (`candidate_id`),
  KEY `assigned_election_idx` (`assigned_election`),
  CONSTRAINT `assigned_election` FOREIGN KEY (`assigned_election`) REFERENCES `elections` (`election_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
CREATE TABLE `results` (
  `voter_id` int DEFAULT '0',
  `candidate_id` int DEFAULT '0',
  `election_id` int NOT NULL,
  `issue_id` int DEFAULT '0',
  `candidate_result` tinyint DEFAULT '0',
  `issue_result` tinyint DEFAULT '0',
  KEY `voter_id_idx` (`voter_id`),
  KEY `canidate_id_idx` (`candidate_id`),
  KEY `election_id_idx` (`election_id`),
  KEY `candidate_resulte_idx` (`issue_result`),
  CONSTRAINT `election_id` FOREIGN KEY (`election_id`) REFERENCES `elections` (`election_id`),
  CONSTRAINT `voter_id` FOREIGN KEY (`voter_id`) REFERENCES `voters` (`voter_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
CREATE TABLE `voted_elections` (
  `voter_id` int DEFAULT NULL,
  `election_id` int DEFAULT NULL,
  KEY `voterId_idx` (`voter_id`),
  KEY `electionId_idx` (`election_id`),
  CONSTRAINT `electionId` FOREIGN KEY (`election_id`) REFERENCES `elections` (`election_id`),
  CONSTRAINT `voterId` FOREIGN KEY (`voter_id`) REFERENCES `voters` (`voter_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
( 1, "Alexis","AguileraOrtiz","1234 street Lincoln Ne 68521","alexisaguilera03","alexis1");
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
(2,"Abby","Seibel","1234 street Lincoln Ne 68521", "abbyseibel03","abby1");
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
(3, "Bao", "Bui", "1234 street Lincoln Ne 68521", "baobui03", "bao1");
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
(4,"Emmanuel", "Mateo", "1234 street Lincoln Ne 68521", "emmanuelmateo03", "emmanuel1");
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
(5, "Kevin","Pham","1234 street Lincoln Ne 68521", "kevinpham03", "kevin1");
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
(6, "Test","Test","1234 street Lincoln Ne 68521", "test", "test");
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
(7, "admin","admin","1234 street Lincoln Ne 68521", "admin", "admin");
INSERT INTO `voting_system`.`voters`
(`voter_id`,
`first_name`,
`last_name`,
`address`,
`username`,
`password`)
VALUES
(8, "company","company","1234 street Lincoln Ne 68521", "company", "company");
INSERT INTO `voting_system`.`elections`
(`election_id`,
`election_year`,
`active`)
VALUES
(1,2020,true);
INSERT INTO `voting_system`.`elections`
(`election_id`,
`election_year`,
`active`)
VALUES
(2,2021,false);
INSERT INTO `voting_system`.`candidates`
(`candidate_id`,
`first_name`,
`last_name`,
`affiliation`,
`assigned_election`)
VALUES
(1,"Dawn","KeyKong", "DK CREW",1);
INSERT INTO `voting_system`.`candidates`
(`candidate_id`,
`first_name`,
`last_name`,
`affiliation`,
`assigned_election`)
VALUES
(2,"Pat","Man", "Cabinets",1);
INSERT INTO `voting_system`.`candidates`
(`candidate_id`,
`first_name`,
`last_name`,
`affiliation`,
`assigned_election`)
VALUES
(3,"Fawn","KeyKong", "DK CREW",2);
INSERT INTO `voting_system`.`candidates`
(`candidate_id`,
`first_name`,
`last_name`,
`affiliation`,
`assigned_election`)
VALUES
(4,"Blinky","Gost", "Cabinets",2);
INSERT INTO `voting_system`.`issues`
(`issue_id`,
`issue_description`,
`issue_title`,
`election_id`)
VALUES
(1,"Legalize the usage of barrel throwing when to defend yourself against trespassers", "Legalize Barrel Throwing", 1 );
INSERT INTO `voting_system`.`issues`
(`issue_id`,
`issue_description`,
`issue_title`,
`election_id`)
VALUES
(2,"Decriminalize the usage of large white dots for recreational purposes", "Legalize Large White Dots", 1 );
INSERT INTO `voting_system`.`issues`
(`issue_id`,
`issue_description`,
`issue_title`,
`election_id`)
VALUES
(3,"Increase the rate of minting of quarters", "Increase Quarter Production", 1 );
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(1,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(2,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(3,1,1,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(4,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(5,1,1,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(1,2,1,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(2,2,1,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(3,2,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(4,2,1,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`candidate_id`,
`election_id`,
`candidate_result`)
VALUES
(5,2,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(1,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(2,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(3,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(4,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(5,1,1,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(1,1,2,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(2,1,2,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(3,1,2,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(4,1,2,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(5,1,2,true);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(1,1,3,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(2,1,3,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(3,1,3,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(4,1,3,false);
INSERT INTO `voting_system`.`results`
(`voter_id`,
`election_id`,
`issue_id`,
`issue_result`)
VALUES
(5,1,3,false);
INSERT INTO `voting_system`.`voted_elections`
(`voter_id`,
`election_id`)
VALUES
(1,1);
INSERT INTO `voting_system`.`voted_elections`
(`voter_id`,
`election_id`)
VALUES
(2,1);
INSERT INTO `voting_system`.`voted_elections`
(`voter_id`,
`election_id`)
VALUES
(3,1);
INSERT INTO `voting_system`.`voted_elections`
(`voter_id`,
`election_id`)
VALUES
(4,1);
INSERT INTO `voting_system`.`voted_elections`
(`voter_id`,
`election_id`)
VALUES
(5,1);
