//using System.Data.SqlClient;
using UnityEngine;

public class AccessDatabase : MonoBehaviour
{
    /*
    SqlConnection myConnection = new SqlConnection("user id=;" + 
                                       "password=;server=;" + 
                                       "Trusted_Connection=yes;" + 
                                       "database=Duels; " + 
                                       "connection timeout=30");
                                       */

    // Start is called before the first frame update
    void Start()
    {
        /*
        try
        {
            myConnection.Open();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        
        SqlCommand myCommand= new SqlCommand("CREATE TABLE Profiles(PlayerID INTEGER PRIMARY KEY, Username STRING NOT NULL UNIQUE, HP INTEGER NOT NULL)", myConnection);
        myCommand.ExecuteNonQuery();
        */
    }
}
