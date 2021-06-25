CREATE TABLE `example`.`currencies` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `extId` VARCHAR(8) NOT NULL,
  `nominal` INT NOT NULL,
  `numCode` INT NOT NULL,
  `charCode` VARCHAR(3) NOT NULL,
  `name` VARCHAR(80) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `currencyId_UNIQUE` (`extId` ASC) VISIBLE,
  UNIQUE INDEX `charCode_UNIQUE` (`charCode` ASC) VISIBLE,
  UNIQUE INDEX `numCode_UNIQUE` (`numCode` ASC) VISIBLE);
