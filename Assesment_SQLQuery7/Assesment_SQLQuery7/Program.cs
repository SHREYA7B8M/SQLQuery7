using System;
using System.Data;
using System.Data.SqlClient;

namespace Assesment_SQLQuery7
{
    internal class Program
    {
        static SqlDataReader reader;
        static SqlCommand cmd;
        static SqlConnection con;
        static string conStr = "server=SCARLETSPELLCAS;database=LibraryDb;trusted_connection=true;";

        static void Main(string[] args)
        {
            con = new SqlConnection(conStr);
            con.Open();

            DataSet booksDataSet = new DataSet();
            RetrieveData(booksDataSet);

            DisplayBookInventory(booksDataSet);

            AddNewBook(booksDataSet);

            UpdateBookQuantity(booksDataSet);

            ApplyChangesToDatabase(booksDataSet);

            con.Close();
        }

        static void RetrieveData(DataSet dataSet)
        {
            string query = "SELECT * FROM Books";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            adapter.Fill(dataSet, "Books");
        }

        static void DisplayBookInventory(DataSet dataSet)
        {
            DataTable booksTable = dataSet.Tables["Books"];
            Console.WriteLine("Book Inventory:\n");

            foreach (DataRow row in booksTable.Rows)
            {
                Console.WriteLine($"Book ID: {row["BookId"]}");
                Console.WriteLine($"Title: {row["Title"]}");
                Console.WriteLine($"Author: {row["Author"]}");
                Console.WriteLine($"Genre: {row["Genre"]}");
                Console.WriteLine($"Quantity: {row["Quantity"]}\n");
            }
        }

        static void AddNewBook(DataSet dataSet)
        {
            DataTable booksTable = dataSet.Tables["Books"];
            DataRow newRow = booksTable.NewRow();

            Console.WriteLine("Step 3: Add New Book");
            Console.Write("Enter Book Title: ");
            newRow["Title"] = Console.ReadLine();

            Console.Write("Enter Author: ");
            newRow["Author"] = Console.ReadLine();

            Console.Write("Enter Genre: ");
            newRow["Genre"] = Console.ReadLine();

            Console.Write("Enter Quantity: ");
            newRow["Quantity"] = int.Parse(Console.ReadLine());

            booksTable.Rows.Add(newRow);
        }

        static void UpdateBookQuantity(DataSet dataSet)
        {
            DataTable booksTable = dataSet.Tables["Books"];

            Console.WriteLine("Step 4: Update Book Quantity");
            Console.Write("Enter Book Title to update quantity: ");
            string titleToUpdate = Console.ReadLine();

            DataRow[] foundRows = booksTable.Select($"Title = '{titleToUpdate}'");

            if (foundRows.Length == 0)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.Write("Enter New Quantity: ");
            int newQuantity = int.Parse(Console.ReadLine());

            foreach (DataRow row in foundRows)
            {
                row["Quantity"] = newQuantity;
            }
        }

        static void ApplyChangesToDatabase(DataSet dataSet)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();

            SqlCommand insertCommand = new SqlCommand("INSERT INTO Books (Title, Author, Genre, Quantity) VALUES (@Title, @Author, @Genre, @Quantity)", con);
            SqlCommand updateCommand = new SqlCommand("UPDATE Books SET Quantity = @Quantity WHERE BookId = @BookId", con);
            SqlCommand deleteCommand = new SqlCommand("DELETE FROM Books WHERE BookId = @BookId", con);

            adapter.InsertCommand = insertCommand;
            adapter.UpdateCommand = updateCommand;
            adapter.DeleteCommand = deleteCommand;

            adapter.InsertCommand.Parameters.Add("@Title", SqlDbType.NVarChar, 50, "Title");
            adapter.InsertCommand.Parameters.Add("@Author", SqlDbType.NVarChar, 50, "Author");
            adapter.InsertCommand.Parameters.Add("@Genre", SqlDbType.NVarChar, 50, "Genre");
            adapter.InsertCommand.Parameters.Add("@Quantity", SqlDbType.Int, 0, "Quantity");

            adapter.UpdateCommand.Parameters.Add("@Quantity", SqlDbType.Int, 0, "Quantity");
            adapter.UpdateCommand.Parameters.Add("@BookId", SqlDbType.Int, 0, "BookId");

            adapter.DeleteCommand.Parameters.Add("@BookId", SqlDbType.Int, 0, "BookId");

            adapter.Update(dataSet, "Books");

        }
    }
}
