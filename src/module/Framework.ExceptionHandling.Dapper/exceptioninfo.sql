/*
 Navicat Premium Data Transfer

 Source Server         : 衢州纪委
 Source Server Type    : MySQL
 Source Server Version : 80011
 Source Host           : 172.18.100.231:3306
 Source Schema         : qzban_logistics

 Target Server Type    : MySQL
 Target Server Version : 80011
 File Encoding         : 65001

 Date: 26/09/2021 13:39:59
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for exceptioninfo
-- ----------------------------
DROP TABLE IF EXISTS `exceptioninfo`;
CREATE TABLE `exceptioninfo`  (
  `Id` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Message` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '异常详情',
  `LogLevel` smallint(6) NULL DEFAULT NULL COMMENT '记录的日志级别',
  `Handled` bit(1) NULL DEFAULT NULL COMMENT '是否处理',
  `CreateTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `Source` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '来源模块',
  `StackTrace` varchar(10000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '堆栈信息',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
