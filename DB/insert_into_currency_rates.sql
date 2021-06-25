INSERT INTO `example`.`currency_rates` (`date`, `currencyId`, `value`) VALUES (20210625, (
	SELECT `example`.`currencies`.`id` 
    FROM `example`.`currencies`
    WHERE `example`.`currencies`.`extId` = 'R01270'
), 89.9999);