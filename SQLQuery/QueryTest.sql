SELECT      c.name  AS 'ColumnName'
            ,t.name AS 'TableName'
FROM        sys.columns c
JOIN        sys.tables  t   ON c.object_id = t.object_id
ORDER BY    TableName
            ,ColumnName;

SELECT OBJECT_NAME(f.parent_object_id) TableName, COL_NAME(fc.parent_object_id,fc.parent_column_id) ColName
FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id INNER JOIN sys.tables t ON t.OBJECT_ID = fc.referenced_object_id
WHERE OBJECT_NAME (f.referenced_object_id) = 'Team'
   
SELECT object_name(f.referenced_object_id) RefTableName, col_name(fc.parent_object_id,fc.parent_column_id) ForeignKey, col_name() RefPrimaryKey
FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
WHERE f.parent_object_id = object_id('Client')

select 
    col.[name] as column_name, 
    tab.[name] as table_name
from sys.tables tab
    inner join sys.indexes pk
        on tab.object_id = pk.object_id 
        and pk.is_primary_key = 1
    inner join sys.index_columns ic
        on ic.object_id = pk.object_id
        and ic.index_id = pk.index_id
    inner join sys.columns col
        on pk.object_id = col.object_id
        and col.column_id = ic.column_id
where tab.[name] = 'Team'
order by 
    pk.[name],
    ic.index_column_id

SELECT COLUMN_NAME FROM dam.INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME LIKE 'Team' AND CONSTRAINT_NAME LIKE 'PK%'

SELECT object_name(f.referenced_object_id) RefTableName, col_name(fc.parent_object_id,fc.parent_column_id) ForeignKey, col.[name] RefPrimaryKey, col.object_id, fc.constraint_object_id
FROM sys.foreign_keys f INNER JOIN sys.foreign_key_columns fc on f.object_id = fc.constraint_object_id inner join sys.tables tab on tab.[name] = object_name(f.referenced_object_id)
    inner join sys.indexes pk on tab.object_id = pk.object_id and pk.is_primary_key = 1
    inner join sys.index_columns ic on ic.object_id = pk.object_id and ic.index_id = pk.index_id
    inner join sys.columns col on pk.object_id = col.object_id and col.column_id = ic.column_id 
where f.parent_object_id = object_id('Client') 