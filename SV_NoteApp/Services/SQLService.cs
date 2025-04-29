using SV_NoteApp.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SV_NoteApp.Services
{
    public class SQLService
    {
        static String TestPath = "E:\\_DATABASE\\SV_NoteApp\\";
        static String AppPath = AppDomain.CurrentDomain.BaseDirectory + "\\res\\db\\";
        static SQLiteConnection sqlCon = new SQLiteConnection($"Data Source={TestPath}dbNote.db; Version=3;");

        public Object Query(String command)
        {
           
            Object theResult = new Object();

            if (hasTheTables()) {
                SQLiteCommand sqlCommand = new SQLiteCommand(command, sqlCon);
    
                sqlCon.Open();

                SQLiteDataReader sqlReader = sqlCommand.ExecuteReader();
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
                else if (sqlReader.FieldCount > 2)
                {
                    List<Note> NoteList = new List<Note>();
                    while (sqlReader.Read())
                    {
                        NoteList.Add((new Note { Id = sqlReader.GetInt32(0), Title = sqlReader.GetString(1), Text = sqlReader.GetString(2), CategoryId = sqlReader.GetInt32(3), CreateDate = sqlReader.GetString(4), ModifyDate = sqlReader.GetString(5), IsPrio = sqlReader.GetBoolean(6) }));
                    }
                    theResult = NoteList;
                }

            }
            else //Amennyiben üres valamelyik tábla
            {
                    if (sqlReader.FieldCount == 2)
                    {
                        theResult = new List<NoteCategory>();
                    }
                    else if (sqlReader.FieldCount > 2)
                    {
                        
                        theResult = new List<Note>();
                    }
                }
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

 private Boolean hasTheTables()
        {
            bool hasTables = false;

            bool hasNotes = false;
            bool hasCategories = false;

            List<string> sqlReaderTables = getTables();

            if (sqlReaderTables.Contains("Notes")) { hasNotes = true; } else { generateTable("Notes"); }
            if (sqlReaderTables.Contains("Categories")) { hasCategories = true; } else { generateTable("Categories"); }
            if (hasNotes & hasCategories) { hasTables = true; }


            return hasTables;
        }

        private List<String> getTables()
        {
            List<String> theTables = new List<String>();
            sqlCon.Open();
            string sqlgetTblCommandString = "SELECT tbl_name FROM sqlite_master WHERE type='table'"; //Táblázatok nevének lekérése
            SQLiteCommand sqlCommandTbl = new SQLiteCommand(sqlgetTblCommandString, sqlCon);
            SQLiteDataReader sqlReaderTbl = sqlCommandTbl.ExecuteReader();

            if (sqlReaderTbl.HasRows)
            {
                while (sqlReaderTbl.Read())
                {
                    theTables.Add(sqlReaderTbl.GetString(0));
                }
            }
            sqlCon.Close();

            return theTables;
        }

        private void generateTable(String tbl)
        {
            String tblGenerateCommand = null;

            switch (tbl)
            {
                case "Notes": tblGenerateCommand= "CREATE TABLE \"Notes\" (\r\n\t\"Id\"\tINTEGER NOT NULL,\r\n\t\"Title\"\tTEXT NOT NULL,\r\n\t\"Text\"\tTEXT NOT NULL,\r\n\t\"CatId\"\tINTEGER NOT NULL DEFAULT -1,\r\n\t\"CreateDate\"\tDATE NOT NULL,\r\n\t\"ModifyDate\"\tDATE NOT NULL,\r\n\t\"isPrio\"\tBOOL NOT NULL DEFAULT 0,\r\n\tPRIMARY KEY(\"Id\" AUTOINCREMENT)\r\n);";
                    break;
                case "Categories": tblGenerateCommand= "CREATE TABLE \"Categories\" (\r\n\t\"Id\"\tINTEGER NOT NULL,\r\n\t\"Name\"\tVARCHAR(15) NOT NULL,\r\n\tPRIMARY KEY(\"Id\")\r\n);";
                    break;
                default: tblGenerateCommand=null;
                    break;
            }
                
            if (tblGenerateCommand!=null)
            {
                Execute(tblGenerateCommand);
            }
        }

    }
}
