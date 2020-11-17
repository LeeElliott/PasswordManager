using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SQLiteTest : MonoBehaviour
{
    string connection;
    IDbConnection dbcon;

    // Use this for initialization
    void Start()
    {
        connection = "URI=file:" + Application.persistentDataPath + "/" + "StorageDatabase";
        dbcon = new SqliteConnection(connection);
        dbcon.Open();

        OpenDatabase();
        AddNewEntry("Google", "NA", "Username", "NA", "Password", "NA");
        ReadEntry("Google");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        // Close connection
        dbcon.Close();
    }

    private void OpenDatabase()
    {
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
    }

    private void AddNewEntry(string s, string sK, string u, string uK, string p, string pK)
    {
        // Check if site AND username currently exist in table

        // Insert values in table
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO StorageTable" + " ( " +
                        "ColumnOne, " +
                        "ColumnTwo, " +
                        "ColumnThree, " +
                        "ColumnFour, " +
                        "ColumnFive, " +
                        "ColumnSix ) "

                        + "VALUES ( '"
                        + s + "', '"
                        + sK + "', '"
                        + u + "', '"
                        + uK + "', '"
                        + p + "', '"
                        + pK + "' )";
        cmnd.ExecuteNonQuery();
    }

    private void ReadEntry(string s)
    {
        // Read selected data
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT ColumnOne FROM StorageTable WHERE ColumnOne = '" + s + " '";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("id: " + reader[0].ToString());
            Debug.Log("val: " + reader[1].ToString());
            Debug.Log("id: " + reader[2].ToString());
            Debug.Log("val: " + reader[3].ToString());
            Debug.Log("id: " + reader[4].ToString());
            Debug.Log("val: " + reader[5].ToString());
        }
    }

    private void LoadList(int i)
    {
        // Read selected data
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT ColumnOne FROM StorageTable";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        // reader[0].ToString();
    }

    private void LastResort()
    {
        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = "DROP TABLE IF EXISTS StorageTable";
        dbcmd.ExecuteNonQuery();
    }

    private IDataReader GetRowCount()
    {
        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText =
            "SELECT COALESCE(MAX(id)+1, 0) FROM StorageTable";
        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }
}