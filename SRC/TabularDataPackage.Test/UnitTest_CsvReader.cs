/* CSVReader - a simple open source C# class library to read CSV data
 * by Andrew Stellman - http://www.stellman-greene.com/CSVReader
 * 
 * Unit tests/TestCSVReader.cs - NUnit tests to verify the CSVReader class
 * 
 * download the latest version: http://svn.stellman-greene.com/CSVReader
 * 
 * (c) 2008, Stellman & Greene Consulting
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of Stellman & Greene Consulting nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY STELLMAN & GREENE CONSULTING ''AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL STELLMAN & GREENE CONSULTING BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 */

using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using System.Data;

namespace Com.StellmanGreene.CSVReader.Unit_tests
{
    [TestFixture]
    public class TestCSVReader
    {
        string fileContents = @"text field,123.456,""quoted """" field"",10446744073709551616,x,1,true,false,FALSE,99999999999,.000000001,""10""
more text with ÙÚÞāΨΤΉeĉ special characters,99.33,xyz,.05, ,0,false,TRUE,true,1000,1.46,xyz";
        List<List<object>> expectedRows = null;

        string filename = "testfile.csv";

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ulong u = ulong.Parse("10446744073709551616");
            byte b1 = 1;
            byte b2 = 0;
            short s = 1000;
            expectedRows = new List<List<object>>() {
                new List<object>() { "text field", 123.456F, "quoted \" field", u, 'x', b1, true, false, false, 99999999999, .000000001F, "10" },
                new List<object> () {"more text with ÙÚÞāΨΤΉeĉ special characters", 99.33F, "xyz", .05F, ' ', b2, false, true, true, s, 1.46F, "xyz" },
            };
        }

        [SetUp]
        public void SetUp()
        {
            if (File.Exists(filename))
                File.Delete(filename);
        }



        // Methods to test reading individual rows

        [Test]
        public void TestStringReadRows()
        {
            using (CSVReader reader = new CSVReader(fileContents))
                CheckExpectedRows(expectedRows, reader);
        }

        [Test]
        public void TestStringReaderReadRows()
        {
            using (CSVReader reader = new CSVReader(new StringReader(fileContents)))
                CheckExpectedRows(expectedRows, reader);
        }

        [Test]
        public void TestFileInfoReadRows()
        {
            File.WriteAllText(filename, fileContents);
            using (CSVReader reader = new CSVReader(new FileInfo(filename)))
                CheckExpectedRows(expectedRows, reader);
        }

        [Test]
        public void TestExtensionMethodReadLine()
        {
            string firstRow = fileContents.Substring(0, fileContents.LastIndexOf('\n') - 1);
            string secondRow = fileContents.Substring(fileContents.LastIndexOf('\n') + 1);
            CheckNextRow(firstRow.ReadCSVLine(), expectedRows[0]);
            CheckNextRow(secondRow.ReadCSVLine(), expectedRows[1]);
        }

        [Test]
        public void TestEmptyFile()
        {
            File.WriteAllText(filename, "");
            using (CSVReader reader = new CSVReader(new FileInfo(filename)))
                Assert.IsNull(reader.ReadRow(), "ReadRow() should return null for an empty file");
        }

        [Test]
        public void TestEmptyString()
        {
            using (CSVReader reader = new CSVReader(""))
                Assert.IsNull(reader.ReadRow(), "ReadRow() should return null for an empty string");
        }

        [Test]
        public void TestNullString()
        {
            try
            {
                using (CSVReader reader = new CSVReader((string)null))
                    Assert.IsNull(reader.ReadRow(), "ReadRow() should return null for an empty string");

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }


        /// <summary>
        /// Check the returned rows against an expected list of rows
        /// </summary>
        /// <param name="expectedRows">List of object lists that contains the expected row data</param>
        /// <param name="reader">Reader to read from</param>
        private static void CheckExpectedRows(List<List<object>> expectedRows, CSVReader reader)
        {
            foreach (List<object> expected in expectedRows)
            {
                List<object> actual = reader.ReadRow();
                CheckNextRow(actual, expected);
            }
            Assert.IsNull(reader.ReadRow(), "ReadRow() should return null if reading past end of file");
        }

        /// <summary>
        /// Check the next row in the reader against an expeced list of objects
        /// </summary>
        /// <param name="reader">Reader to read from</param>
        /// <param name="expected">Expected list of objects</param>
        private static void CheckNextRow(List<object> actual, List<object> expected)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i].GetType(), actual[i].GetType(), "Type mismatch for column " + i);
                Assert.AreEqual(expected[i], actual[i], "Values for column " + i + " don't match");
            }
        }




        // Methods to test reading entire data tables

        [Test]
        public void TestStringDataTable()
        {
            DataTable table;
            using (CSVReader reader = new CSVReader(fileContents))
                table = reader.CreateDataTable(false);
            CheckTable(table);
        }

        [Test]
        public void TestStringReaderDataTable()
        {
            DataTable table;
            using (CSVReader reader = new CSVReader(new StringReader(fileContents)))
                table = reader.CreateDataTable(false);
            CheckTable(table);
        }

        [Test]
        public void TestFileInfoDataTable()
        {
            DataTable table;
            File.WriteAllText(filename, fileContents);
            using (CSVReader reader = new CSVReader(new FileInfo(filename)))
                table = reader.CreateDataTable(false);
            CheckTable(table);
        }

        [Test]
        public void TestExtensionMethodDataTable()
        {
            DataTable table = fileContents.ReadCSVTable(false);
            CheckTable(table);
        }


        /// <summary>
        /// Check the expected data table against the actual values
        /// </summary>
        /// <param name="actual">Actual data table created by the test</param>
        private void CheckTable(DataTable actual)
        {
            Assert.AreEqual(2, actual.Rows.Count);

            //expectedRows = new List<List<object>>() {
            //    new List<object>() { "text field", 123.456F, "quoted \" field", u, 'x', b1, true, false, false, 99999999999, .000000001F, "10" },
            //    new List<object> () {"more text", 99.33F, "xyz", .05F, ' ', b2, false, true, true, s, 1.46F, "xyz" },
            //};

            List<Type> expectedTypes = new List<Type>() { typeof(string), typeof(float), typeof(string), typeof(float), typeof(char), 
                typeof(byte), typeof(bool), typeof(bool), typeof(bool), typeof(long), typeof(float), typeof(string) };

            for (int rowNum = 0; rowNum < 2; rowNum++)
            {
                DataRow row = actual.Rows[rowNum];
                Assert.AreEqual(expectedRows[rowNum].Count, row.ItemArray.Length);
                for (int colNum = 0; colNum < expectedTypes.Count; colNum++)
                {
                    Assert.AreEqual(expectedTypes[colNum], row[colNum].GetType(), "Row " + (rowNum + 1) + " column " + (colNum + 1) + " - type mismatch");
                    if (expectedTypes[colNum] == typeof(float))
                    {
                        float a = float.Parse(expectedRows[rowNum][colNum].ToString());
                        float b = (float)row[colNum];
                        Assert.AreEqual(a, b, .0000001, "Row " + (rowNum + 1) + " column " + (colNum + 1) + " - value mismatch");
                    }
                    else
                        Assert.AreEqual(expectedRows[rowNum][colNum], row[colNum], "Row " + (rowNum + 1) + " column " + (colNum + 1) + " - value mismatch");
                }
            }
        }

        [Test]
        public void TestUTF8SpecialChar()
        {
            DataTable table = CSVReader.ReadCSVFile(AppDomain.CurrentDomain.BaseDirectory + @"\Unit tests\UTF8-specialchar.csv", true);

            List<string> columns = new List<string>() { "ééééésetnb", "first", "middle", "last", "name1", "name2", "name3", "name4", "medline_search1" };

            int i = 0;
            foreach (DataColumn column in table.Columns)
            {
                Assert.AreEqual(columns[i++], column.ColumnName);
            }

            expectedRows = new List<List<object>>() {
                new List<object>() { "A6212654", "Joe", "K.", "Margolis", "margolis rk", "", "", "", "(\"margolis rk\"[au] AND 1968:1998[dp])" },
                new List<object>() { "A6212655", "Renée", "K.", "Margolis", "margolis rk", "", "", "", "(\"margolis rk\"[au] AND 1968:1998[dp])" },
            };

            i = 0;
            foreach (List<object> list in expectedRows)
            {
                DataRow row = table.Rows[i++];
                for (int c = 0; c < list.Count; c++)
                    Assert.AreEqual(list[c], row[c]);
            }
        }
    }
}

