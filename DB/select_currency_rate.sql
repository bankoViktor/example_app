SELECT `currency_rates`.`value`
FROM `currency_rates`, `currencies`
WHERE `currencies`.`numCode` = 946 AND `currencies`.`id` = `currency_rates`.`currencyId` AND `currency_rates`.`date` = 20201225;