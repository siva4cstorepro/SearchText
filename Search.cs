using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchText
{

    static class SearchResult
    {
        //string strSearchText;
        //string strFoundFilePath;
        //string strFoundLineNumber;
        //string strFoundLineText;


        //public SearchResult(string inSearchText, string inFoundFilePath, string inFoundLineNumber, string inFoundLineText)
        //{
        //    strSearchText = inSearchText;
        //    strFoundFilePath = inFoundFilePath;
        //    strFoundLineNumber = inFoundLineNumber;
        //    strFoundLineText = inFoundLineText;
        //}


        //public bool writeSearchResult()
        //{
        //    bool isOpSuccessful = false;


        //    return isOpSuccessful;
        //}


    }

    class Search
    {
        private List<string> lstSearchText;
        private List<string> lstFolders;
        private List<string> lstIncludedExtension;
        private List<string> lstExcludedExtension;
        private List<string> lstAllSearchTextOccurences = new List<string>();
        private List<string> files;
        private string strStatusMessage;
        private DataTable dataTable;
        public delegate void ProgressUpdate(string strShowStatus);
        public event ProgressUpdate OnProgressUpdate;

        //TODO: Do file upload search
        public Search(List<string> inSearchText, string inSearchFolder, string inIncludedExtension = ".cs", string inExcludedExtension = "*.mdf")
        {
            lstSearchText = inSearchText;
            lstFolders = inSearchFolder.Split(',').ToList();
            lstIncludedExtension = inIncludedExtension.Split(',').ToList();
            lstExcludedExtension = inExcludedExtension.Split(',').ToList();
            files = new List<string>();
            foreach (string extension in lstIncludedExtension)
            {
                Directory.GetFiles(inSearchFolder, "*" + extension, SearchOption.AllDirectories).ToList().ForEach(item => files.Add(item));
            }
            for (int i = 0; i < files.Count; i++)
            {
                foreach (string ext in new string[] { ".mdf", ".git", ".ldf" })
                {
                    if (files[i].Contains(ext))
                    {
                        files.RemoveAt(i);
                    }

                }
            }

            dataTable = new DataTable("SearchResults");
            //dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("StoredProcedure", typeof(string));
            dataTable.Columns.Add("Path", typeof(string));
            dataTable.Columns.Add("LineNumber", typeof(string));
            dataTable.Columns.Add("Line", typeof(string));
        }

        public bool performSearch(out string outShowStatus)
        {
            bool isOpSuccessful = false;


            foreach (string strFolderPath in lstFolders) // Foreach folder
            {
                string strAllIncludedExtensions = String.Join(",", lstIncludedExtension);
                
                int intTotalFilesCount = files.Count;
                int intFoundInFilesCount = 0;
                int intSearchedInFilesCount = 0;

                if (files.Count > 0)
                {
                    foreach (string file in files) // Foreach file
                    {
                        intSearchedInFilesCount++;
                        string[] fileLines = File.ReadAllLines(file);

                        int i = 0;
                        foreach (string currentLine in fileLines) // Foreach line
                        {
                            i++;
                            void helper(string strSearchText)
                            {
                                if (currentLine.Contains(strSearchText))
                                {
                                    StringBuilder sbFoundTextEntry = new StringBuilder("");
                                    sbFoundTextEntry.Append(strSearchText);
                                    sbFoundTextEntry.Append("|");
                                    sbFoundTextEntry.Append(file);
                                    sbFoundTextEntry.Append("|");
                                    sbFoundTextEntry.Append(i.ToString());
                                    sbFoundTextEntry.Append("|");
                                    sbFoundTextEntry.Append(currentLine.TrimStart(' ', '\t'));
                                    sbFoundTextEntry.Append("\n");

                                    lstAllSearchTextOccurences.Add(sbFoundTextEntry.ToString());
                                    dataTable.Rows.Add(sbFoundTextEntry.ToString().Split('|'));
                                    intFoundInFilesCount++;
                                    isOpSuccessful = true;
                                }
                            }
                            Parallel.ForEach(lstSearchText, strSearchText => helper(strSearchText));
                            strStatusMessage = "In the Folder " + strFolderPath + " there are " + intTotalFilesCount.ToString() + " files with extension " + strAllIncludedExtensions.ToString() + " searched in " + intSearchedInFilesCount.ToString() + " found matching files in " + intFoundInFilesCount.ToString();
                            if (OnProgressUpdate != null)
                            {
                                OnProgressUpdate(strStatusMessage);
                            }
                        }

                    }
                }
                else
                {
                    strStatusMessage = "No files find with these extenstions: " + strAllIncludedExtensions + " in the folder " + strFolderPath;
                }
            }






            if (isOpSuccessful)
            {
                if (lstAllSearchTextOccurences.Count > 0)
                {
                    writeAndDownloadFile();
                    isOpSuccessful = true;
                }
                else
                {

                }

            }

            outShowStatus = strStatusMessage;
            return isOpSuccessful;
        }


        private bool writeAndDownloadFile()
        {
            bool isOpSuccessful = false;
            string path = @"C:\CSPData\Search Tool\Result\results.txt";

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (TextWriter tw = new StreamWriter(fs))
                {
                    try
                    {
                        lstAllSearchTextOccurences.Insert(0, "StoredProcedure|Path|LineNumber|Line");
                        lstAllSearchTextOccurences.ForEach(tw.WriteLine);
                        isOpSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        isOpSuccessful = false;
                    }

                }
            }

            string connectionString = @"Data Source=(local)\SQL2017;Initial Catalog=SearchTextDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "IF EXISTS( SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SearchResults') " + @"DROP TABLE [dbo].[SearchResults]";
                        command.ExecuteNonQuery();
                        command.CommandText = "IF NOT EXISTS( SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SearchResults') " + @"CREATE TABLE [dbo].[SearchResults](
                                                                                                                                                                [StoredProcedure][NVARCHAR](MAX) NULL, 
                                                                                                                                                                [Path] [NVARCHAR](MAX) NULL,
                                                                                                                                                                [LineNumber] [NVARCHAR] (50) NULL,
	                                                                                                                                                            [Line][NVARCHAR](MAX) NULL
                                                                                                                                                           ) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
                                                                                                                                                        ";
                        command.ExecuteNonQuery();
                    }
                    foreach (DataColumn c in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    }

                    bulkCopy.DestinationTableName = dataTable.TableName;
                    try
                    {
                        bulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return isOpSuccessful;
        }
    }
}
