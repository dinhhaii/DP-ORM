SELECT      c.name  AS 'ColumnName'
            ,t.name AS 'TableName'
FROM        sys.columns c
JOIN        sys.tables  t   ON c.object_id = t.object_id
ORDER BY    TableName
            ,ColumnName;

SELECT OBJECT_NAME(f.parent_object_id) TableName, COL_NAME(fc.parent_object_id,fc.parent_column_id) ColName
FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id INNER JOIN sys.tables t ON t.OBJECT_ID = fc.referenced_object_id
WHERE OBJECT_NAME (f.referenced_object_id) = 'Team'
   
SELECT object_name(f.parent_object_id) ParentTableName, object_name(f.referenced_object_id) RefTableName, col_name(fc.parent_object_id,fc.parent_column_id) ColName
FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
WHERE f.parent_object_id = object_id('Client')