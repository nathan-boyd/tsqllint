﻿CREATE TABLE MyTable 
	(ID INT, 
	Name nvarchar(50))
WITH (DATA_COMPRESSION = ROW);

CREATE TABLE MyTableTwo
	(ID INT, 
	Name nvarchar(50))
WITH (SYSTEM_VERSIONING = ON);