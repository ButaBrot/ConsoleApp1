using Microsoft.Data.Sqlite;

namespace ConsoleApp1;

public static class Program
{
    public static void Main()
    {
        //EmploeeyDatabase.InitializeDatabase();
        EmploeeyDatabase.AddData("Монитор 4 на 5 НЕ СУЩЕСТВУЕТ!!!!!!НЕ!!!!!!!!!");
        var tempList=  EmploeeyDatabase.GetData();
        tempList.ForEach(x=> Console.WriteLine($"Вот твой бд {x}"));
    }
}

public static class EmploeeyDatabase
{
    private static string myTable = "GusTable";
    private static string TestText = "GusText";
    const string emploeeydbDbPath = "../GusDB.db";
    public async static void InitializeDatabase()
    {

        if (!File.Exists(emploeeydbDbPath))
        {
            await File.Create(emploeeydbDbPath).DisposeAsync();
        }
        using (SqliteConnection db = new SqliteConnection($"Filename={emploeeydbDbPath}"))
        {
            try
            {
                db.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            String tableCommand = $"CREATE TABLE IF NOT EXISTS {myTable} (Primary_Key INTEGER PRIMARY KEY, {TestText} NVARCHAR(2048) NULL )";

            SqliteCommand createTable = new SqliteCommand(tableCommand, db);

            createTable.ExecuteReader();
        }

    }

    public static void AddData(string inputText)
    {

        using (SqliteConnection db = new SqliteConnection($"Filename={emploeeydbDbPath}"))
        {
            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = $"INSERT INTO {myTable} VALUES (NULL, @{TestText});";
            insertCommand.Parameters.AddWithValue($"@{TestText}", inputText);

            insertCommand.ExecuteReader();

            db.Close();
        }

    }
    public static List<String> GetData()
    {
        List<String> entries = new List<string>();


        using (SqliteConnection db = new SqliteConnection($"Filename={emploeeydbDbPath}"))
        {
            db.Open();

            //SqliteCommand selectCommand = new SqliteCommand ($"SELECT Text_Entry from {myTable}", db);
            SqliteCommand selectCommand = new SqliteCommand($"SELECT  {TestText} from {myTable}", db);


            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                entries.Add(query.GetString(0));
            }

            db.Close();
        }

        return entries;
    }

}