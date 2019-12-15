using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public interface IDatabase
    {
        List<object> GenerateListFromTable(string tableName);
        object FindByPrimaryKey(Dictionary<string, object> primaryKeys, string tableName);
        List<ForeignKey> FindForeignKeyOfTable(string tableName);

        string GetRefTableName(List<ForeignKey> foreignKeys, string propertyName);
        List<string> FindPrimaryKeyName(string tableName);
        int UpdateObjectToDB(object obj);
        int AddObjectToDB(object obj);
        DataTable GroupWithCondition(string tableName, string selectClause, string whereClause, string groupByClause, string havingClause);
        int DeleteObjectInDB(object obj);

    }
}
