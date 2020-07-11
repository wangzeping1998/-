/*
Navicat MySQL Data Transfer

Source Server         : MinMMO
Source Server Version : 80019
Source Host           : localhost:3306
Source Database       : darkgod

Target Server Type    : MYSQL
Target Server Version : 80019
File Encoding         : 65001

Date: 2020-07-11 10:11:57
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `id` int NOT NULL AUTO_INCREMENT,
  `acct` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `pass` varchar(255) NOT NULL,
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `level` int NOT NULL,
  `exp` int NOT NULL,
  `power` int NOT NULL,
  `coin` int NOT NULL,
  `diamond` int NOT NULL,
  `crystal` int NOT NULL,
  `hp` int NOT NULL,
  `ad` int NOT NULL,
  `ap` int NOT NULL,
  `addef` int NOT NULL,
  `apdef` int NOT NULL,
  `dodge` int NOT NULL,
  `pierce` int NOT NULL,
  `critical` int NOT NULL,
  `guideId` int NOT NULL,
  `strong` varchar(255) NOT NULL,
  `time` bigint NOT NULL,
  `task` varchar(255) NOT NULL,
  `fuben` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;
