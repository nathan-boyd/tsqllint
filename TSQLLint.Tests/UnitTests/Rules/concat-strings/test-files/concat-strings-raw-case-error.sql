SELECT CASE WHEN 'a' + N'b' = N'ab' THEN 1 ELSE 0 END;

SELECT CASE WHEN N'a' + 'b' = N'ab' THEN 1 ELSE 0 END;

SELECT CASE WHEN N'a' + N'b' = 'ab' THEN 1 ELSE 0 END;

SELECT CASE WHEN 'a' + 'b' = N'ab' THEN 1 ELSE 0 END;