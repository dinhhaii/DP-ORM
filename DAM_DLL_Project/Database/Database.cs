using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAM
{
    public interface IDatabase
    {
        Type GetTypeByClassName(string className);
        List<object> GenerateListFromTable(string tableName);
        object FindByPrimaryKey(Dictionary<string, object> primaryKeys, string tableName);
        List<ForeignKey> FindForeignKeyOfTable(string tableName);

        string GetRefTableName(List<ForeignKey> foreignKeys, string propertyName);
        List<string> FindPrimaryKeyName(string tableName);
        int UpdateObjectToDB(object obj);
        int AddObjectToDB(object obj);
        int DeleteObjectInDB(object obj);
        List<object> findByObject(object obj);


    }
}
