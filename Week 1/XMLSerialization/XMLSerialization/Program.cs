﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<Person>();
            //FillList(list);
            //@-string for disabling escape sequences like \t
            //SerializeToFile(@"C:\revature\VisualStudio\data.xml", list);
            //var desList = await DeserializeFromFileAsync(@"C:\revature\data.xml");

            Task<IEnumerable<Person>> desListTask = DeserializeFromFileAsync(@"C:\revature\VisualStudio\data.xml");

            //IEnumerable<Person> result = desListTask.Result; // synchronously sits around until 
            IEnumerable<Person> result = new List<Person>();

            try
            {
                result = desListTask.Result; // synchronously sits around until the result is ready
            }
            catch(AggregateException ex)
            {
                Console.WriteLine("file wasn't found");
            }

            list.AddRange(result);
            FillList(list);

            // @-string for disabling escape sequences like \t
            SerializeToFile(@"C:\revature\VisualStudio\XMLSerialization\data.xml", list);




            List<int> largeNumbers = new List<int>();
            foreach (var item in largeNumbers)
            {
                ExpensiveCalculation(item);
            }

            // run as many calculations at the same time as we can, using threads
            // multiple cpu cores
            Parallel.ForEach(largeNumbers, item => ExpensiveCalculation(item));
        }

        private static void ExpensiveCalculation(int item)
        {
            
        }

        private static void SerializeToFile(string fileName, List<Person> people)
        {
            var serializer = new XmlSerializer(typeof(List<Person>));

            //var fileStream = new FileStream(fileName, FileMode.Create);
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(fileName, FileMode.Create);
                serializer.Serialize(fileStream, people);
            }
            catch (PathTooLongException ex)
            {
                Console.WriteLine($"Path {fileName} was too long! {ex.Message}");
            }
            catch(IOException ex)
            {
                Console.WriteLine($"Some other error with file I/O: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;      // re-throws the same exception
            }
            finally
            {
                if(fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
           // serializer.Serialize(fileStream, people);
        }


        //private async static Task<IEnumerable<Person>> DeserializeFromFileAsync(string fileName)
        //{
        //    var serializer = new XmlSerializer(typeof(List<Person>));

        //    //FileStream fileStream = null;

        //    //var fileStream = new FileStream(fileName, FileMode.Open);

        //    //object result = (List<Person>)serializer.Deserialize(fileStream);
        //    //return result;

        //    //asynchronous
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var fileStream = new FileStream(fileName, FileMode.Open))
        //        {
        //            Task task = fileStream.CopyToAsync(memoryStream);
        //            await task;
        //        }

        //        return (List<Person>)serializer.Deserialize(fileStream);

        //    }

        //    using (var fileStream = new FileStream(fileName, FileMode.Open))
        //    {
        //        return (List<Person>)serializer.Deserialize(fileStream);
        //    }
        //}


        private async static Task<IEnumerable<Person>> DeserializeFromFileAsync(string fileName)
        {
            var serializer = new XmlSerializer(typeof(List<Person>));


            // we CAN do try/finally like this, but the using statement is easier
            //FileStream fileStream = null;
            //try
            //{
            //    fileStream = new FileStream(fileName, FileMode.Open);

            //    var result = (List<Person>)serializer.Deserialize(fileStream);
            //    return result;
            //}
            //finally
            //{
            //    fileStream.Dispose();
            //}

            using (var memoryStream = new MemoryStream())
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }
                memoryStream.Position = 0; // reset "cursor" of stream to beginning
                return (List<Person>)serializer.Deserialize(memoryStream);
            }
        }


        private static void FillList(List<Person> list)
        {
            list.Add(new Person
            {
                Id = 1,
                Name = new Name
                {
                    First = "Fred",
                    Last = "Belotte"
                },
                Address = new Address
                {
                    Line1 = "123 Drury Ln",
                    Line2 = "",
                    City = "Tampa",
                    State = "Florida",
                    ZipCode = "12345"
                },

                Age = 30,

                //Nicknames = new List<string> {  "Freddie", "Freddo"}
            });

            list.Add(new Person
            {
                Id = 1,
                Name = new Name
                {
                    First = "Nick",
                    Last = "Escalona"
                },
                Address = new Address
                {
                    Line1 = "123 Drury Ln",
                    Line2 = "",
                    City = "Tampa",
                    State = "Florida",
                    ZipCode = "12345"
                }
            });


        }
    }
}
