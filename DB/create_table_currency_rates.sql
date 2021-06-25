CREATE TABLE `example`.`currency_rates` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `date` DATE NOT NULL,
  `currencyId` INT NOT NULL,
  `value` DECIMAL(12,4) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `currencyId_FK_idx` (`currencyId` ASC) VISIBLE,
  CONSTRAINT `currencyId_FK`
    FOREIGN KEY (`currencyId`)
    REFERENCES `example`.`currencies` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
