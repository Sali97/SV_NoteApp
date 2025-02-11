using SV_NoteApp.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SV_NoteApp.Services
{
    public class SQLService
    {
        //static String TestPath = "E:\\_DATABASE\\SV_NoteApp\\";
        static String AppPath = AppDomain.CurrentDomain.BaseDirectory + "\\res\\db\\";
        static SQLiteConnection sqlCon = new SQLiteConnection(@"Data Source=" + AppPath + "dbNote.db; Version=3;");

        public Object Query(String command)
        {
            SQLiteCommand sqlCommand = new SQLiteCommand(command, sqlCon);

            sqlCon.Open();

            SQLiteDataReader sqlReader = sqlCommand.ExecuteReader();

            Object theResult = new Object();

            if (sqlReader.HasRows)
            {
                if (sqlReader.FieldCount==2)
                {
                    List<NoteCategory> CategoryList = new List<NoteCategory>();
                    while (sqlReader.Read())
                    {
                        CategoryList.Add(new NoteCategory { Id = sqlReader.GetInt32(0), Name = sqlReader.GetString(1) });
                    }
                    theResult = CategoryList;
                }
                else if (sqlReader.FieldCount == 4)
                {
                    List<Note> NoteList = new List<Note>();
                    while (sqlReader.Read())
                    {
                        NoteList.Add((new Note {Id= sqlReader.GetInt32(0),  Title = sqlReader.GetString(1), Text = sqlReader.GetString(2), CategoryId = sqlReader.GetInt32(3) }));
                    }
                    theResult = NoteList;
                }

            }
            else
            {
                Console.WriteLine("Az adatbázis üres.");
            }

            sqlCon.Close();

            return theResult;
        }

        public void Execute(String command)
        {
            sqlCon.Open();

            SQLiteCommand sqlCommand = new SQLiteCommand(command, sqlCon);

            sqlCommand.ExecuteNonQuery();

            sqlCon.Close();
        }

    }
}
