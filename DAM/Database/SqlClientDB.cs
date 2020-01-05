using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DAM.AttributeEntity;
namespace DAM
{
    public class SqlClientDB : IDatabase
    {
        private static string connectionString { get; set; }
        private static SqlConnection connection { get; set; }
        public SqlClientDB(string _connectionString)
        {
            connectionString = _connectionString;
        }

        public static SqlConnection GetConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed || connection == null)
            {
                connection = new SqlConnection(connectionString);
            }
            return connection;
        }

        public static void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public List<object> GenerateListFromTable(string tableName)
        {
            string typeName = string.Format("{0}.Entity.{1}", typeof(SqlClientDB).Namespace, tableName);
            Type entityType = Type.GetType(typeName);
            PropertyInfo[] properties = entityType.GetProperties();
            List<object> tables = new List<object>();

            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery().Select("*").From(tableName);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    List<ForeignKey> FK = FindForeignKeyOfTable(tableName);
                    List<string> PK = FindPrimaryKeyName(tableName);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        if (FK.Count == 0)
                        {
                            foreach (PropertyInfo property in properties)
                            {
                                object propertyReader = reader[property.Name];
                                PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                if (null != propEntity && propEntity.CanWrite)
                                {
                                    if (propertyReader.GetType() == typeof(string))
                                    {
                                        propEntity.SetValue(entity, (propertyReader as string).Trim(), null);
                                    }
                                    else
                                    {
                                        propEntity.SetValue(entity, propertyReader, null);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Dictionary<string, object> pKey = new Dictionary<string, object>();
                            foreach (string PKName in PK)
                            {
                                pKey.Add(PKName, reader[PKName]);
                            }
                            entity = FindByPrimaryKey(pKey, tableName);
                        }
                        tables.Add(entity);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return tables;
        }


        public object FindByPrimaryKey(Dictionary<string, object> primaryKeys, string tableName)
        {
            string typeName = string.Format("{0}.Entity.{1}", typeof(SqlClientDB).Namespace, tableName);
            Type entityType = Type.GetType(typeName);
            PropertyInfo[] properties = entityType.GetProperties();

            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery();

                if (primaryKeys != null)
                {
                    string condition = "";
                    foreach (KeyValuePair<string, object> item in primaryKeys)
                    {
                        if (item.Key != null && item.Value != DBNull.Value)
                        {
                            condition += item.Value.GetType() == typeof(string) ? string.Format("{0} = '{1}'", item.Key, item.Value) : string.Format("{0} = {1}", item.Key, item.Value);
                            if (!item.Equals(primaryKeys.Last()))
                            {
                                condition += " and ";
                            }
                        }
                    }
                    if (condition != "")
                        query = SqlClientQuery.InitQuery().Select("*").From(tableName).Where(condition);
                }
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    List<ForeignKey> FK = FindForeignKeyOfTable(tableName);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        object id = null;
                        //Get each record and save to 'entity'
                        foreach (PropertyInfo property in properties)
                        {
                            if (Attribute.IsDefined(property, typeof(Column)))
                            {
                                object propertyReader = reader[property.Name];
                                if (Attribute.IsDefined(property, typeof(PrimaryKey)))
                                    id = propertyReader;
                                PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                if (null != propEntity && propEntity.CanWrite)
                                {
                                    if (propertyReader.GetType() == typeof(string))
                                    {
                                        propEntity.SetValue(entity, (propertyReader as string).Trim(), null);
                                    }
                                    else
                                    {
                                        propEntity.SetValue(entity, propertyReader, null);
                                    }
                                }

                            }
                            else if ((Attribute.IsDefined(property, typeof(ManyToOne))
                                || Attribute.IsDefined(property, typeof(OneToOne)) &&
                                Attribute.IsDefined(property, typeof(JoinColumn))))
                            {
                                Dictionary<string, object> pKey = new Dictionary<string, object>();
                                foreach (ForeignKey fk in FK)
                                {
                                    if (fk.refTableName == property.PropertyType.Name)
                                    {
                                        for (int i = 0; i < fk.foreignKeys.Count; i++)
                                        {
                                            string pkRefTable = fk.primaryKeysOfRefTable[i];
                                            string fkTable = fk.foreignKeys[i];
                                            if (reader[fkTable] == null)
                                            {
                                                throw new Exception("Foreign key must be set a value!");
                                            }
                                            pKey.Add(pkRefTable, reader[fkTable]);
                                        }

                                        object obj = FindByPrimaryKey(pKey, fk.refTableName);
                                        PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (null != propEntity && propEntity.CanWrite)
                                        {
                                            propEntity.SetValue(entity, obj, null);
                                        }
                                    }
                                }
                            }
                            else if (Attribute.IsDefined(property, typeof(OneToOne)) &&
                                !Attribute.IsDefined(property, typeof(JoinColumn)))
                            {
                                var attr = (OneToOne)property.GetCustomAttribute(typeof(OneToOne));
                                if (attr == null) continue;
                                var type = attr.refClassType;
                                string refTableName = "";
                                var info = type.GetTypeInfo().GetCustomAttribute(typeof(Table)) as Table;
                                if (info != null)
                                {
                                    refTableName = info.ToString();
                                    List<ForeignKey> refFK = FindForeignKeyOfTable(refTableName);
                                    refFK = refFK.Where(e => e.refTableName.Equals(tableName)).ToList<ForeignKey>();
                                    if (refFK.Count > 0 && refFK[0].foreignKeys.Count > 0)
                                    {
                                        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                                        keyValuePairs.Add(refFK[0].foreignKeys[0], id);
                                        List<object> list = findByKeyValues(keyValuePairs, refTableName, new List<string>());
                                        PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (list.Count > 0) propEntity.SetValue(entity, list[0], null);
                                        propEntity = type.GetProperty(attr.mappedBy, BindingFlags.Public | BindingFlags.Instance);
                                        foreach (var item in list)
                                        {
                                            propEntity.SetValue(item, entity);
                                        }
                                    }
                                }
                            }
                            else if (Attribute.IsDefined(property, typeof(OneToMany)))
                            {
                                var attr = (OneToMany)property.GetCustomAttribute(typeof(OneToMany));
                                if (attr == null) continue;
                                var type = attr.refClassType;
                                string refTableName = "";
                                var info = type.GetTypeInfo().GetCustomAttribute(typeof(Table)) as Table;
                                if (info != null)
                                {
                                    refTableName = info.ToString();
                                    List<ForeignKey> refFK = FindForeignKeyOfTable(refTableName);
                                    refFK = refFK.Where(e => e.refTableName.Equals(tableName)).ToList<ForeignKey>();
                                    if (refFK.Count > 0 && refFK[0].foreignKeys.Count > 0)
                                    {
                                        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                                        keyValuePairs.Add(refFK[0].foreignKeys[0], id);
                                        List<object> list = findByKeyValues(keyValuePairs, refTableName, new List<string>());
                                        PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (list.Count > 0) propEntity.SetValue(entity, list, null);
                                        propEntity = type.GetProperty(attr.mappedBy, BindingFlags.Public | BindingFlags.Instance);
                                        foreach (var item in list)
                                        {
                                            propEntity.SetValue(item, entity);
                                        }
                                    }
                                }
                            }
                            else if (Attribute.IsDefined(property, typeof(ManyToMany)) 
                                && Attribute.IsDefined(property, typeof(JoinTable)))
                            {
                                var manyToManyAttr = (ManyToMany)property.GetCustomAttribute(typeof(ManyToMany));
                                var joinTableAttr = (JoinTable)property.GetCustomAttribute(typeof(JoinTable));

                                if (manyToManyAttr == null || joinTableAttr==null) continue;
                                var type = manyToManyAttr.refClassType;
                                string refTableName = "";
                                string primaryKey = FindPrimaryKeyName(tableName)[0];

                                var info = type.GetTypeInfo().GetCustomAttribute(typeof(Table)) as Table;
                                if (info != null)
                                {
                                    refTableName = info.ToString();
                                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                                    keyValuePairs.Add("_Join Table", joinTableAttr.Name);
                                    keyValuePairs.Add("_Join On_Column1", primaryKey);
                                    keyValuePairs.Add("_Join On_Column2", joinTableAttr.RefJoinColumn);

                                    keyValuePairs.Add(joinTableAttr.JoinColumn, id);
                                    List<object> list = findByKeyValues(keyValuePairs, refTableName, new List<string>());
                                    
                                    PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (list.Count > 0) propEntity.SetValue(entity, list, null);
                                        propEntity = type.GetProperty(manyToManyAttr.mappedBy, BindingFlags.Public | BindingFlags.Instance);
                                        foreach (var item in list)
                                        {
                                        Dictionary<string, object> keyValuePairs2 = new Dictionary<string, object>();
                                        string primaryKeyField = findPrimaryField(item);
                                        object primaryKeyValue = item.GetType().GetProperty(primaryKeyField).GetValue(item, null);
                                        keyValuePairs2.Add("_Join Table", joinTableAttr.Name);
                                        keyValuePairs2.Add("_Join On_Column1", primaryKeyField);
                                        keyValuePairs2.Add("_Join On_Column2", joinTableAttr.JoinColumn);
                                        keyValuePairs2.Add(joinTableAttr.RefJoinColumn, primaryKeyValue);
                                        List<object> list2 = findByKeyValues(keyValuePairs2, tableName, new List<string>());
                      
                                        propEntity.SetValue(item, list2);
                                        }

                                }

                            }
                            
                        }
                        return entity;
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return null;
        }

        public List<object> findByKeyValues(Dictionary<string, object> primaryKeys, string tableName, List<string> checkedTables)
        {
            string typeName = string.Format("{0}.Entity.{1}", typeof(SqlClientDB).Namespace, tableName);
            Type entityType = Type.GetType(typeName);
            PropertyInfo[] properties = entityType.GetProperties();
            List<object> results = new List<object>();

            if (checkedTables.Contains(tableName)) return results;
            checkedTables.Add(tableName);
            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery();

                if (primaryKeys != null)
                {
                    string condition = "";

                    string joinTable = "";
                    string joinOnColumn1 = "";
                    string joinOnColumn2 = "";
                    foreach (KeyValuePair<string, object> item in primaryKeys)
                    {
                        if (item.Key != null && item.Value != DBNull.Value)
                        {
                            if (!item.Key.Contains("_Join"))
                            {
                                condition += item.Value.GetType() == typeof(string) ? string.Format("{0} = '{1}'", item.Key, item.Value) : string.Format("{0} = {1}", item.Key, item.Value);
                                if (!item.Equals(primaryKeys.Last()))
                                {
                                    condition += " and ";
                                }
                            }
                            else
                            {
                                if (item.Key.Equals("_Join Table")) joinTable = item.Value.ToString();
                                else if(item.Key.Equals("_Join On_Column1")) joinOnColumn1 = item.Value.ToString();
                                else if (item.Key.Equals("_Join On_Column2")) joinOnColumn2 = item.Value.ToString();

                            }
                        }
                    }
                    query = SqlClientQuery.InitQuery().Select("*").From(tableName);
                    if (joinTable.Length > 0 && joinOnColumn1.Length > 0 && joinOnColumn2.Length>0)
                        query.InnerJoin(joinTable).On(joinOnColumn1,joinOnColumn2);
                    if (condition != "")
                        query.Where(condition);
                }
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    List<ForeignKey> FK = FindForeignKeyOfTable(tableName);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        object id = null;
                        //Get each record and save to 'entity'
                        foreach (PropertyInfo property in properties)
                        {
                            if (Attribute.IsDefined(property, typeof(Column)))
                            {
                                object propertyReader = reader[property.Name];
                                if (Attribute.IsDefined(property, typeof(PrimaryKey)))
                                    id = propertyReader;
                                PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                if (null != propEntity && propEntity.CanWrite)
                                {
                                    if (propertyReader.GetType() == typeof(string))
                                    {
                                        propEntity.SetValue(entity, (propertyReader as string).Trim(), null);
                                    }
                                    else
                                    {
                                        propEntity.SetValue(entity, propertyReader, null);
                                    }
                                }

                            }
                            else if ((Attribute.IsDefined(property, typeof(ManyToOne))
                               || Attribute.IsDefined(property, typeof(OneToOne)) &&
                               Attribute.IsDefined(property, typeof(JoinColumn))))
                            {
                                Dictionary<string, object> pKey = new Dictionary<string, object>();
                                foreach (ForeignKey fk in FK)
                                {
                                    if (fk.refTableName == property.PropertyType.Name)
                                    {
                                        for (int i = 0; i < fk.foreignKeys.Count; i++)
                                        {
                                            string pkRefTable = fk.primaryKeysOfRefTable[i];
                                            string fkTable = fk.foreignKeys[i];
                                            if (reader[fkTable] == null)
                                            {
                                                throw new Exception("Foreign key must be set a value!");
                                            }
                                            pKey.Add(pkRefTable, reader[fkTable]);
                                        }
                                        List<object> list = findByKeyValues(pKey, fk.refTableName, checkedTables);
                                        if (list.Count > 0)
                                        {
                                            PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                            if (null != propEntity && propEntity.CanWrite)
                                            {
                                                propEntity.SetValue(entity, list[0], null);
                                            }
                                        }

                                    }
                                }
                            }
                            else if (Attribute.IsDefined(property, typeof(OneToOne)) &&
                                !Attribute.IsDefined(property, typeof(JoinColumn)))
                            {
                                var attr = (OneToOne)property.GetCustomAttribute(typeof(OneToOne));
                                if (attr == null) continue;
                                var type = attr.refClassType;
                                string refTableName = "";
                                var info = type.GetTypeInfo().GetCustomAttribute(typeof(Table)) as Table;
                                if (info != null)
                                {
                                    refTableName = info.ToString();
                                    List<ForeignKey> refFK = FindForeignKeyOfTable(refTableName);
                                    refFK = refFK.Where(e => e.refTableName.Equals(tableName)).ToList<ForeignKey>();
                                    if (refFK.Count > 0 && refFK[0].foreignKeys.Count > 0)
                                    {
                                        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                                        keyValuePairs.Add(refFK[0].foreignKeys[0], id);

                                        List<object> list = findByKeyValues(keyValuePairs, refTableName, checkedTables);
                                        PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (list.Count > 0) propEntity.SetValue(entity, list[0], null);
                                        propEntity = type.GetProperty(attr.mappedBy, BindingFlags.Public | BindingFlags.Instance);
                                        foreach (var item in list)
                                        {
                                            propEntity.SetValue(item, entity);
                                        }
                                    }
                                }
                            }
                            else if (Attribute.IsDefined(property, typeof(OneToMany)))
                            {
                                var attr = (OneToMany)property.GetCustomAttribute(typeof(OneToMany));
                                if (attr == null) continue;
                                var type = attr.refClassType;
                                string refTableName = "";
                                var info = type.GetTypeInfo().GetCustomAttribute(typeof(Table)) as Table;
                                if (info != null)
                                {
                                    refTableName = info.ToString();
                                    List<ForeignKey> refFK = FindForeignKeyOfTable(refTableName);
                                    refFK = refFK.Where(e => e.refTableName.Equals(tableName)).ToList<ForeignKey>();
                                    if (refFK.Count > 0 && refFK[0].foreignKeys.Count > 0)
                                    {
                                        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                                        keyValuePairs.Add(refFK[0].foreignKeys[0], id);
                                        List<object> list = findByKeyValues(keyValuePairs, refTableName, checkedTables);
                                        PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (list.Count > 0) propEntity.SetValue(entity, list, null);
                                        propEntity = type.GetProperty(attr.mappedBy, BindingFlags.Public | BindingFlags.Instance);
                                        foreach (var item in list)
                                        {
                                            propEntity.SetValue(item, entity);
                                        }
                                    }
                                }
                            }

                        }
                        results.Add(entity);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return results;
        }

        public List<object> findByObject(object obj)
        {
            string tableName = "";
            var info = obj.GetType().GetTypeInfo().GetCustomAttribute(typeof(Table)) as Table;
            if (info != null) tableName = info.ToString();
            List<object> results = new List<object>();
            string typeName = string.Format("{0}.Entity.{1}", typeof(SqlClientDB).Namespace, tableName);
            Type entityType = Type.GetType(typeName);
            PropertyInfo[] properties = entityType.GetProperties();
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            foreach (PropertyInfo property in properties)
            {
                object objValue = property.GetValue(obj);
                if (Attribute.IsDefined(property, typeof(Column)) && objValue != null)
                {
                    keyValuePairs.Add(property.Name, objValue);
                }
                else if (Attribute.IsDefined(property, typeof(JoinColumn)) && objValue != null)
                {
                    var attr = (JoinColumn)property.GetCustomAttribute(typeof(JoinColumn));
                    if (attr != null)
                        keyValuePairs.Add(attr.ToString(), objValue);
                }
            }


            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery();

                if (keyValuePairs.Count > 0)
                {
                    string condition = "";
                    foreach (KeyValuePair<string, object> item in keyValuePairs)
                    {
                        if (item.Key != null && item.Value != DBNull.Value)
                        {
                            condition += item.Value.GetType() == typeof(string) ? string.Format("{0} = '{1}'", item.Key, item.Value) : string.Format("{0} = {1}", item.Key, item.Value);
                            if (!item.Equals(keyValuePairs.Last()))
                            {
                                condition += " and ";
                            }
                        }
                    }
                    if (condition != "")
                        query = SqlClientQuery.InitQuery().Select("*").From(tableName).Where(condition);
                }
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    List<ForeignKey> FK = FindForeignKeyOfTable(tableName);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        object entity = Activator.CreateInstance(entityType);
                        //Get each record and save to 'entity'
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.PropertyType != typeof(string) && property.PropertyType != typeof(int) && property.PropertyType != typeof(long) &&
                            property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(float) && property.PropertyType != typeof(double))
                            {
                                Dictionary<string, object> pKey = new Dictionary<string, object>();
                                foreach (ForeignKey fk in FK)
                                {
                                    if (fk.refTableName == property.PropertyType.Name)
                                    {
                                        for (int i = 0; i < fk.foreignKeys.Count; i++)
                                        {
                                            string pkRefTable = fk.primaryKeysOfRefTable[i];
                                            string fkTable = fk.foreignKeys[i];
                                            if (reader[fkTable] == null)
                                            {
                                                throw new Exception("Foreign key must be set a value!");
                                            }
                                            pKey.Add(pkRefTable, reader[fkTable]);
                                        }
                                        object obj2 = FindByPrimaryKey(pKey, fk.refTableName);
                                        PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                        if (null != propEntity && propEntity.CanWrite)
                                        {
                                            propEntity.SetValue(entity, obj2, null);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                object propertyReader = reader[property.Name];
                                PropertyInfo propEntity = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                                if (null != propEntity && propEntity.CanWrite)
                                {
                                    if (propertyReader.GetType() == typeof(string))
                                    {
                                        propEntity.SetValue(entity, (propertyReader as string).Trim(), null);
                                    }
                                    else
                                    {
                                        propEntity.SetValue(entity, propertyReader, null);
                                    }
                                }
                            }
                        }
                        results.Add(entity);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return results;
        }

        //Update
        public int UpdateObjectToDB(object obj)
        {

            string tableName = "";
            int result = 0;
            Dictionary<string, object> valueUpdate = new Dictionary<string, object>();
            Attribute[] attributes = Attribute.GetCustomAttributes(obj.GetType());
            Type entityType = Type.GetType(obj.GetType().ToString());
            PropertyInfo[] properties = entityType.GetProperties();

            foreach (var attr in attributes)
            {
                tableName = attr.ToString();
            }
            List<string> columnName = getColumnnameTable(tableName);
            var foreignKey = FindForeignKeyOfTable(tableName);

            foreach (PropertyInfo property in properties)
            {
                object propertyValue = obj.GetType().GetProperty(property.Name).GetValue(obj, null);
                bool isHave = false;
                foreach (var item in columnName)
                {
                    if (item == property.Name)
                    {
                        valueUpdate.Add(property.Name, propertyValue);
                        isHave = true;
                    }
                }
                if (isHave == false)
                {

                    foreach (var foreign in foreignKey)
                    {
                        if (obj.GetType().GetProperty(property.Name).GetValue(obj, null) != null)
                        {
                            var foreignKeyValue = obj.GetType().GetProperty(property.Name).GetValue(obj, null);
                            var h = foreignKeyValue.GetType().Name;
                            if (foreignKeyValue.GetType().Name == foreign.refTableName)
                            {
                                valueUpdate.Add(foreign.foreignKeys[0], foreignKeyValue.GetType().GetProperty(foreign.primaryKeysOfRefTable[0]).GetValue(foreignKeyValue, null));
                            }
                        }
                    }
                }
            }
            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery();
                var conditionValue = FindPrimaryKeyName(tableName);
                string condition = "";
                foreach (var item in conditionValue)
                {
                    condition += valueUpdate[item].GetType() == typeof(string) ? string.Format("{0} = '{1}'", item, valueUpdate[item]) : string.Format("{0} = {1}", item, valueUpdate[item]);
                    if (!item.Equals(conditionValue.Last()))
                    {
                        condition += " and ";
                    }
                }

                // Remove Primary in SET

                foreach (var item in conditionValue)
                {
                    if (valueUpdate.ContainsKey(item))
                    {
                        valueUpdate.Remove(item);
                        continue;
                    }
                }

                if (condition != "")
                    query.Update(tableName).Set(valueUpdate).Where(condition);
                var connection1 = new SqlConnection(connectionString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection1);
                connection1.Open();
                result = sqlCommand.ExecuteNonQuery();
                connection1.Close();
            }

            return result;
        }

        //Add 
        public int AddObjectToDB(object obj)
        {

            string tableName = "";
            int result = 0;
            Dictionary<string, object> valueAdd = new Dictionary<string, object>();
            Attribute[] attributes = Attribute.GetCustomAttributes(obj.GetType());
            Type entityType = Type.GetType(obj.GetType().ToString());
            PropertyInfo[] properties = entityType.GetProperties();

            foreach (var attr in attributes)
            {
                tableName = attr.ToString();
            }
            List<string> columnName = getColumnnameTable(tableName);
            var foreignKey = FindForeignKeyOfTable(tableName);

            foreach (PropertyInfo property in properties)
            {
                object propertyValue = obj.GetType().GetProperty(property.Name).GetValue(obj, null);
                bool isHave = false;
                foreach (var item in columnName)
                {
                    if (item == property.Name)
                    {
                        valueAdd.Add(property.Name, propertyValue);
                        isHave = true;
                    } 
                }
                if(isHave==false)
                {

                        foreach (var foreign in foreignKey)
                        {
                            if (obj.GetType().GetProperty(property.Name).GetValue(obj, null) != null)
                            {
                                var foreignKeyValue = obj.GetType().GetProperty(property.Name).GetValue(obj, null);
                                var h = foreignKeyValue.GetType().Name;
                                if (foreignKeyValue.GetType().Name == foreign.refTableName)
                                {
                                valueAdd.Add(foreign.foreignKeys[0], foreignKeyValue.GetType().GetProperty(foreign.primaryKeysOfRefTable[0]).GetValue(foreignKeyValue, null));
                                }
                            }
                        }
                }

            }
            using (var connection1 = new SqlConnection(connectionString))
            {
         
                Query query = SqlClientQuery.InitQuery();
                var conditionValue = FindPrimaryKeyName(tableName);

                // Remove Primary in Add and ForeignKeyid if ==0

                foreach (var item in conditionValue)
                {
                    if (valueAdd.ContainsKey(item))
                    {
                        valueAdd.Remove(item);
                        continue;
                    }
                }
                try
                {
                    List<string> foreignKeyName = getColumnForeignKeyNameOfTable(tableName);
                    foreach (var item in foreignKeyName)
                    {
                        if (valueAdd.ContainsKey(item) && (int)valueAdd[item] == 0)
                        {
                            valueAdd.Remove(item);
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message );
                }

                query.Insert(tableName).Values(valueAdd);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection1);
                connection1.Open();
                result = sqlCommand.ExecuteNonQuery();
                connection1.Close();
            }

            return result;
        }

        public int DeleteObjectInDB(object obj)
        {
            string tableName = "";
            int result = 0;
            Dictionary<string, object> valueCondition = new Dictionary<string, object>();
            Attribute[] attributes = Attribute.GetCustomAttributes(obj.GetType());
            Type entityType = Type.GetType(obj.GetType().ToString());
            PropertyInfo[] properties = entityType.GetProperties();


            foreach (var attr in attributes)
            {
                tableName = attr.ToString();
            }
            var primaryName = FindPrimaryKeyName(tableName);
            foreach (PropertyInfo property in properties)
            {
                object propertyValue = obj.GetType().GetProperty(property.Name).GetValue(obj, null);
                foreach (var item in primaryName)
                {
                    if (item.ToLower() == property.Name.ToLower())
                    {
                        valueCondition.Add(item, propertyValue);
                        break;
                    }
                }

            }

            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery();

                string condition = "";

                foreach (var item in primaryName)
                {
                    condition += valueCondition[item].GetType() == typeof(string) ? string.Format("{0} = '{1}'", item, valueCondition[item]) : string.Format("{0} = {1}", item, valueCondition[item]);
                    if (!item.Equals(primaryName.Last()))
                    {
                        condition += " and ";
                    }
                }
                query.Delete(tableName).Where(condition);
                var connection1 = new SqlConnection(connectionString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection1);
                connection1.Open();
                result = sqlCommand.ExecuteNonQuery();
                connection1.Close();
            }
            return result;
        }

        private string findPrimaryField(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach(var property in properties)
            {
                if(Attribute.IsDefined(property, typeof(PrimaryKey))) {
                    return property.Name;
                }
            }
            return "";
        }

        private List<string> getColumnnameTable(string tableName)
        {
            List<string> result = new List<string>();
            using (connection = new SqlConnection(connectionString))
            {
                string queryString = @"SELECT Column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableName + "'";
                Query query = SqlClientQuery.InitQuery(queryString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            result.Add((string)reader.GetValue(i));
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            }
            return result;
        }
        public List<ForeignKey> FindForeignKeyOfTable(string tableName)
        {
            List<ForeignKey> result = new List<ForeignKey>();

            using (connection = new SqlConnection(connectionString))
            {
                string queryString = string.Format(@"SELECT object_name(f.referenced_object_id) RefTableName, col_name(fc.parent_object_id,fc.parent_column_id) ForeignKey FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id WHERE f.parent_object_id = object_id('{0}')", tableName);
                Query query = SqlClientQuery.InitQuery(queryString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    ForeignKey foreignKey = new ForeignKey(tableName);

                    while (reader.Read())
                    {
                        string reftableName = (reader["RefTableName"] as string).Trim();
                        string fk = (reader["ForeignKey"] as string).Trim();

                        if (reftableName != null && fk != null)
                        {
                            if (reftableName != foreignKey.refTableName)
                            {
                                foreignKey = new ForeignKey(tableName);
                                List<string> pks = FindPrimaryKeyName(reftableName);
                                foreignKey.refTableName = reftableName;
                                foreignKey.primaryKeysOfRefTable = pks;
                                result.Add(foreignKey);
                            }

                            result[result.Count - 1].AddForeignKey(fk);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }

        public string GetRefTableName(List<ForeignKey> foreignKeys, string propertyName)
        {
            foreach (ForeignKey item in foreignKeys)
            {
                if (item.foreignKeys.Contains(propertyName))
                {
                    return item.refTableName;
                }
            }
            return null;
        }

        private List<string> getColumnForeignKeyNameOfTable(string tableName)
        {
            List<string> result = new List<string>();

            using (connection = new SqlConnection(connectionString))
            {
                string queryString = string.Format(@"SELECT COLUMN_NAME as Foreignkey FROM dam.INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME LIKE '{0}' AND CONSTRAINT_NAME LIKE 'FK%'", tableName);
                Query query = SqlClientQuery.InitQuery(queryString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                using (connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result.Add((string)reader.GetValue(i));
                            }
                        }
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                }
            }

            return result;
        }

        public List<string> FindPrimaryKeyName(string tableName)
        {
            List<string> result = new List<string>();

            using (connection = new SqlConnection(connectionString))
            {
                string queryString = string.Format(@"SELECT COLUMN_NAME as PrimaryKey FROM dam.INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME LIKE '{0}' AND CONSTRAINT_NAME LIKE 'PK%'", tableName);
                Query query = SqlClientQuery.InitQuery(queryString);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        string pk = (reader["PrimaryKey"] as string).Trim();
                        if (pk != null)
                            result.Add(pk);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }

        // Test (not use)
        public GroupTable GroupRows(string tableName, string groupedColumns)
        {
            GroupTable result = new GroupTable();

            using (connection = new SqlConnection(connectionString))
            {
                Query query = SqlClientQuery.InitQuery().Select(groupedColumns).From(tableName).GroupBy(groupedColumns);
                SqlCommand sqlCommand = (SqlCommand)query.GenerateCommand(connection);
                string[] colNamesArray = groupedColumns.Split(',');
                List<string> colNamesList = new List<string>();
                string groupTableName = "";
                foreach (var columnName in colNamesArray)
                {
                    colNamesList.Add(columnName.Trim());
                    if (columnName != colNamesArray.Last())
                    {
                        groupTableName += columnName.Trim() + "_";
                    }
                }

                result.columnsName = colNamesList;
                result._tableName = groupTableName;

                try
                {
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        List<object> valuesOfRow = new List<object>();
                        foreach(var colName in result.columnsName)
                        {
                            valuesOfRow.Add(reader[colName]);
                        }
                        result.AddRow(valuesOfRow);
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return result;
        }
    }
}
