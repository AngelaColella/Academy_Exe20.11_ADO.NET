using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Esercitazione20._11
{
    public class ConnectedMode
    {
        // dichiarazione della stringa di connessione
        const string connectionString = @"Persist Security Info = False; Integrated Security = True; Initial Catalog = Polizia; Server = WINAPIUZIYVIF6L\SQLEXPRESS";

        public static void GetDatiAgenteByArea()
        {
            using (SqlConnection connection = new SqlConnection(connectionString)) // si inserisce la connessione all'interno dello using in modo tale che ne venga fatta la Dispose una volta chiusa la connessione
            {
                // inserimento del parametro da command line
                Console.WriteLine("Inserire l'ID dell'Area: ");
                int ID_area = Convert.ToInt32(System.Console.ReadLine()); // si salva l'input e lo si converte ad interno 

                // apertura connessione
                connection.Open();

                // creazione comando
                SqlCommand command = new SqlCommand(); 
                command.Connection = connection; 
                // il comando sarà di tipo Text perchè si tratta di una SELECT
                command.CommandType = System.Data.CommandType.Text;
                // è necessario fare una join tra le tabelle Agente e Agente_Area (tabella di ponte) per poter ottenere i dati dell'agente assegnato ad un'area identificata dall'ID dell'area inserito dall'utente
                command.CommandText = "SELECT * FROM Agente INNER JOIN Agente_Area ON IDAgente = ID WHERE IDArea = @IDarea";
                
   
                // creazione comando 
                command.Parameters.AddWithValue("@IDarea", ID_area);

                SqlDataReader reader = command.ExecuteReader();

                // visualizzazione dati
                while (reader.Read())
                {
                    Console.WriteLine("ID Agente: {0} \n Nome: {1} \n Cognome: {2} \n CF: {3} \n Data di nascita: {4} \n Anni di Servizio: {5}",
                        reader["ID"],
                        reader["Nome"],
                        reader["Cognome"],
                        reader["CF"],
                        reader["Birthdate"],
                        reader["AnniServizio"]);
                }

                // Chiusura del reader
                reader.Close();

                // Chiusura della connessione
                connection.Close();

            }
        }
        public static void AddAgente()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // inserimento di tutti i parametri da command line
                Console.WriteLine("Inserire il nome dell'agente: ");
                string nome = System.Console.ReadLine();
                Console.WriteLine("Inserisci il cognome dell'agente: ");
                string cognome = System.Console.ReadLine();
                Console.WriteLine("Inserisci il codice fiscale dell'agente: ");
                string cf = System.Console.ReadLine();
                Console.WriteLine("Inserisci la data di nascita dell'agente: ");
                string bday = System.Console.ReadLine();
                Console.WriteLine("Inserisci gli anni di servizio dell'agente: ");
                int anniServizio = Convert.ToInt32(System.Console.ReadLine());

                // apertura connessione 
                connection.Open();

                // creazione comandi 
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "stpInsertAgente";

                // creazione parametri della stored procedure, ai quali si attribuisce il valore inserito da tastiera
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@cognome", cognome);
                command.Parameters.AddWithValue("@cf", cf);
                command.Parameters.AddWithValue("@bd", bday);
                command.Parameters.AddWithValue("@anniServizio", anniServizio);

                // esecuzione comando: non voglio visualizzare i dati, quindi invece di ExecuteReader, utilizzo:
                command.ExecuteNonQuery();

                // chiusura della connessione 
                connection.Close();
            }
        }

    }
}
