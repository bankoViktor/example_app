CREATE FUNCTION `GET_CURRENCY_VALUE` (cursNumCode INT, targetDate DATE) RETURNS DECIMAL(16,4)
 READS SQL DATA
    DETERMINISTIC
BEGIN
	DECLARE result DECIMAL(16, 4);
    SET result =  (
		SELECT `currency_rates`.`value`
		FROM `currency_rates`, `currencies`
		WHERE `currencies`.`numCode` = cursNumCode AND `currencies`.`id` = `currency_rates`.`currencyId` AND `currency_rates`.`date` = targetDate
    );
	RETURN result;
END