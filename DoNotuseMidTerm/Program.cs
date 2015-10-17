using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace DoNotuseMidTerm
{
   class Program
    {
        static void Main(string[] args)
        {
            //add a reader and spin this up from the file. if the file is blank use the hard coded values.
            Dictionary<string, int> movie = new Dictionary<string, int>();
            StreamReader inReader = new StreamReader("..\\..\\Inventory.txt"); //reading the inventory text file 
            using (inReader)
            {
                string inline = inReader.ReadLine(); // reading just the first inventory line 
                if (inline != null) // if inventory line does not = empty then go on to while statement 
                {
                    while (inline != null) //while the next line does not = empty then go to if statement 
                    {
                        if (inline != "") // if line does not equal blank"" then ...
                        {
                            string[] m = inline.Split(','); // split the line at the comma (movie "," 1)
                            //int m1 = int.Parse(m[m.Length]);
                            movie.Add(m[0], 1); // adding the movie and its quanity/cost to the text file 
                        }
                        
                       inline = inReader.ReadLine(); // reading the next text file line in inventory 
                    }
                }
                else
                {
                    movie.Add("maze runner", 1); // (name of movie, cost of movie )
                    movie.Add("the hunger games", 1);
                    movie.Add("black mass", 1);
                    movie.Add("the walk", 1);
                    movie.Add("legend", 1);
                    movie.Add("sicario", 1);
                    movie.Add("the intern", 1);
                    movie.Add("the visit", 1);
                    movie.Add("the perfect guy", 1);
                    movie.Add("everest", 1);
                }

            }
            
            
            StreamWriter writer = new StreamWriter("..\\..\\CustomerInfo.txt");  // creates text file for customer info
            StreamWriter writerInventory = new StreamWriter("..\\..\\Inventory.txt");// creates text file for inventory 
            StringBuilder buliderCustomer = new StringBuilder(); // builder for customer info
            StringBuilder builderInentory = new StringBuilder(); // builder for inventory 
            //StreamReader 
            try
            {           
            
            Console.WriteLine("CURRENT MOVIE SELECTION :");
            foreach (KeyValuePair<string,int> item in movie) // printing the full list of movies to console. 
            {
                Console.WriteLine(item.Key + "," + item.Value); //( movie , price )
            }
            using (writer) 
            {
                for (int i = 0; i < 3; i++) //will ask to enter three customer info and movie selection
                {
                    Console.WriteLine("Please enter customer FULL NAME, EMAIL, TELEPHONE NUMBER to begin check out.");
                   // string ownerinput = Console.ReadLine(); // taking in user input 
                   // ownerinput = ownerinput.ToLower(); // 
                    buliderCustomer.Append(Console.ReadLine().ToLower());  // adding customer info to stringbuilder 
                    Console.WriteLine("Please enter movie selection.");
                    String ownerinput = Console.ReadLine().ToLower(); // taking in movie selection, using a string because we have to 
                    //check in memory if the movie exists in the dictionary(memory). **not in text file yet**
                    if (movie.ContainsKey(ownerinput)) // if statement is checking to see if the movie is in the dictionary(memory).  
                    {
                        buliderCustomer.Append(","+ ownerinput + ", Due on: , " + DateTime.Today.AddDays(7).ToShortDateString() + "\n"); // adding the movie to customer string and adding duedate with stringbuilder, **in memory**
                        movie.Remove(ownerinput); // removes movie from Dictionary memory. 
                       
                    }
                    else  // IF Mmovie is not in Inventory 
                    {
                        Console.WriteLine(" MOVIE NOT FOUND ");
                    }
                }
                writer.WriteLine(buliderCustomer);// we have our finished customer builder, and are now writting that to Customer info text file 
            }

            foreach (KeyValuePair<string,int> pair in movie) // using builder to build movie list **in memory**
            {
                builderInentory.AppendLine(pair.Key +","+ pair.Value); // building the string for our dictionary **still in memory**               
            }
            Console.WriteLine("Movies left in Inventory" + "\n"+ builderInentory); // will print to console the remaining movies left after checkout. 
            using (writerInventory) // using writter to write 
            {
                writerInventory.WriteLine(builderInentory); // now we are writting our builder inventory **which was in memory** to our text file. 
            }
            //Console.ReadLine();

            StreamWriter writeFee = new StreamWriter("..\\..\\LateFee.txt"); // will list when MOVIE was returned, and IF there is a late fee. 
            StringBuilder buildFee = new StringBuilder(); // builds the late fee string in **memory**
            StreamReader readCustomer = new StreamReader("..\\..\\CustomerInfo.txt"); // reader will read through customer info 

            Console.WriteLine("\n" + "The following Customer are Past Due :" +"\n");
            string line = readCustomer.ReadLine(); // setting up reader to read customer info text document 
            int lineNumber = 0; // telling the reader to start reading at line number 0
            while (line != null) // looping through the customer info text doc while string line is not empty 
            {
                lineNumber++; // tells the reader to advance to the next line by 1 starting at 0
                string[] splitLine = line.Split(','); // split is looking for a character so you have to use single quotes. Taking customer infor readline and 
                // spiting it on the comma. 

                //int l = split.Length;
                //Console.WriteLine(l);
                DateTime DueDate = new DateTime();
                DueDate = DateTime.Parse(splitLine[splitLine.Length - 1]);// when you go from a length to an index always do -1. telling it to go to the last element in split. 
                    // then we're parsing string[] split in to a datetime 
                Console.WriteLine("DueDate: " + DueDate);
                Random ran = new Random();
                DateTime end = new DateTime();
                end = DateTime.Today.AddDays(14); // ending 14 days from today, will include 7 days(rental time) plus seven addtional
                // days to simulate late days. 
                DateTime start = new DateTime();
                start = DateTime.Today.AddDays(5); // today plus 5 rental days to simulate movies returned on time 
                int range = (end - start).Days;  // gives you the number of days for the random generator to use 
                DateTime returnDate = start.AddDays(ran.Next(range) + lineNumber); // return date random generator 

                if (DueDate < returnDate)// compare due date to a return date 
                {

                    buildFee.AppendLine(line);
                    Console.WriteLine("{0} Return Date: {1}", line, returnDate); // printing customer info file to console & return date
                }
                line = readCustomer.ReadLine(); // telling reader to read the next line. 

            }

            using (writeFee)
            {
                writeFee.WriteLine(buildFee);// writting late customers to tex file 
            }

            Console.ReadLine();
            }
            catch (FileNotFoundException) //text file could not be found 
            {

                Console.WriteLine("File not Found");
            }
            catch (IOException e) // unable to write to a file 
            {
                Console.WriteLine(
                    "{0}: The write operation could not " +
                    "be performed because the specified " +
                    "part of the file is locked.",
                    e.GetType().Name);
            }
            catch (FormatException) // catching and releasing duedate exception
            {

            }

            catch (Exception e)  // general catch all exception 
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
            // read the Customer info file, put the customer name and movie in a list**in memory**, then ask what movie was returned on what date. If movie was returned on time 
            // write to console "Customers account is clean", If movie returned late, then implement a late fee method, and print that out to late fee text file. 


        }
        public class Person
        {
            public string fullName;
            public string email;
            public string phone;
            

            public string FullName
            {
                get { return this.fullName; }
                set { this.fullName = value; }
            }

            public string Email
            {
                get { return this.email; }
                set { this.email = value; }
            }

            public string Phone
            {
                get { return this.phone; }
                set { this.phone = value; }
            }
           


            public Person()
            {
                fullName = "Unknown";
                email = "unknown@gmial.com";
                phone = "1-216-333-1111";
                // checkouts = "";
            }
           

            public void SetProfile(string newName, string newemail, string newphone)
            {
                FullName = newName;
                Email = newemail;
                Phone = newphone;

            }


        }

    }
}
