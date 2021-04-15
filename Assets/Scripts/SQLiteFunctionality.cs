using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SQLiteFunctionality : MonoBehaviour
{
    string connection;
    IDbConnection dbcon;

    // Use this for initialization
    public void OnStart()
    {
        connection = "URI=file:" + Application.persistentDataPath + "/" + "StorageDatabase";
        dbcon = new SqliteConnection(connection);

        OpenDatabase();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        
    }

    public void OpenDatabase()
    {
        dbcon.Open();
        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();

        dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS StorageTable" + " ( " +
                        "ColumnOne TEXT, " +
                        "ColumnTwo TEXT, " +
                        "ColumnThree TEXT, " +
                        "ColumnFour TEXT, " +
                        "ColumnFive TEXT, " +
                        "ColumnSix TEXT )";
        dbcmd.ExecuteNonQuery();
        
        // Close connection
        dbcon.Close();
    }

    public void AddNewEntry(string s, string sK, string u, string uK, string p, string pK)
    {
        dbcon.Open();
        // Check if site AND username currently exist in table

        // Insert values in table
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = string.Format("INSERT INTO StorageTable ( " +
                        "ColumnOne, " +
                        "ColumnTwo, " +
                        "ColumnThree, " +
                        "ColumnFour, " +
                        "ColumnFive, " +
                        "ColumnSix ) "

                        + "VALUES (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")",
                        s, sK, u, uK, p, pK);
        cmnd.ExecuteNonQuery();

        // Close connection
        dbcon.Close();
    }

    public void ReadEntry(string s)
    {
        dbcon.Open();
        
        // Read selected data
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM StorageTable WHERE ColumnOne = '" + s + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("Column 1: " + reader[0].ToString());
            Debug.Log("Column 2: " + reader[1].ToString());
            Debug.Log("Column 3: " + reader[2].ToString());
            Debug.Log("Column 4: " + reader[3].ToString());
            Debug.Log("Column 5: " + reader[4].ToString());
            Debug.Log("Column 6: " + reader[5].ToString());
        }

        reader.Close();

        // Close connection
        dbcon.Close();
    }

    public string LookupSingle(int row, int col)
    {
        dbcon.Open();

        string keyString = "";

        // Read selected data
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM StorageTable WHERE rowid == " + (row + 1);
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        keyString = reader[col].ToString();

        reader.Close();

        // Close connection
        dbcon.Close();

        return keyString;
    }

    public List<string> LoadList(int i)
    {
        dbcon.Open();
        
        // Read selected data
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM StorageTable WHERE rowid == " + (i + 1);
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        List<string> item = new List<string>();
        item.Add(reader[0].ToString());
        item.Add(reader[1].ToString());
        item.Add(reader[2].ToString());
        item.Add(reader[3].ToString());

        reader.Close();

        // Close connection
        dbcon.Close();

        return item;
    }

    public void UpdateEntry(string columnOne, string columnThree, string columnFive)
    {
        dbcon.Open();

        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = "UPDATE StorageTable SET ColumnFive = '" + columnFive + "' WHERE ColumnOne = '" + 
            columnOne + "' AND ColumnThree = '" + columnThree + "'";
        dbcmd.ExecuteNonQuery();

        // Close connection
        dbcon.Close();
    }

    public void RemoveEntry(string columnOne, string columnThree)
    {
        dbcon.Open();

        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = "DELETE FROM StorageTable WHERE ColumnOne = '" + columnOne + "' AND ColumnThree = '" + columnThree + "'";
        dbcmd.ExecuteNonQuery();

        // Close connection        
        dbcon.Close();
    }

    public void LastResort()
    {
        dbcon.Open();

        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = "DROP TABLE IF EXISTS StorageTable";
        dbcmd.ExecuteNonQuery();

        // Close connection
        dbcon.Close();
    }

    public int GetRowCount()
    {
        dbcon.Open();

        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText =
            "SELECT COUNT(*) FROM StorageTable";
        IDataReader reader = dbcmd.ExecuteReader();
        int count = 0;

        while (reader.Read())
        {            
            int.TryParse(reader[0].ToString(), out count);

            //Debug.Log(reader[0]);
            //Debug.Log(reader[0].GetType());
            //Debug.Log(count);
        }

        reader.Close();

        // Close connection
        dbcon.Close();

        return count;
    }
}