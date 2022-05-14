

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for roomcardinfo
-- ----------------------------
DROP TABLE IF EXISTS `roomcardinfo`;
CREATE TABLE `roomcardinfo`  (
  `Id` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'ID',
  `IsDeleted` bit(1) NULL DEFAULT NULL COMMENT '是否删除',
  `DeleterId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '删除人员Id',
  `DeletionTime` datetime(0) NULL DEFAULT NULL COMMENT '删除时间',
  `CreatorId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '创建人Id',
  `CreationTime` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `LastModificationTime` datetime(0) NULL DEFAULT NULL COMMENT '最后一次修改时间',
  `LastModifierId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '修改人员Id',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
