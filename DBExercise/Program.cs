using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Xml.Serialization;

namespace gestioneOrdini
{
    internal class Program
    {
        static void CheckAdmin(SqlConnection con)
        {

            //con.Open();
            string q = "select count(*) from login";
            var cmd = new SqlCommand(q, con);
            if ((int)cmd.ExecuteScalar() == 0)
                new SqlCommand("insert into login (username,password) VALUES ('admin','admin')").ExecuteNonQuery();
        }

        

        static bool login(SqlConnection con, string user, string pw)
        {
            SqlCommand cmd = new SqlCommand("select count(*) from utenti where login=@username and password=@password", con);
            cmd.Parameters.Add(new SqlParameter("@username", user));
            cmd.Parameters.Add(new SqlParameter("@password", pw));
            return (int)cmd.ExecuteScalar() > 0;
        }

        static void Register(SqlConnection con, string user, string pw)
        {
            SqlCommand cmd = new SqlCommand("insert into login (username,password) VALUES (@user,@pw)", con);
            cmd.Parameters.Add(new SqlParameter("@user", user));
            cmd.Parameters.Add(new SqlParameter("@pw", pw));
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Utente creato con successo!");
        }


        static void Query1(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("select o.orderid, customer, orderdate, sum(price*qty) as tot from orders as o inner join orderitems as i on o.orderid=i. orderid group by o.orderid, customer, orderdate", con);
            var orders = cmd.ExecuteReader();
            while (orders.Read())
            {
                Console.WriteLine("{0} {1}", orders["orderid"], orders["customer"], orders["orderdate"]);
            }


        }


        static void Query2(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("select o.item, o.qty, o.price from o.orderitems ", con);
            var orders = cmd.ExecuteReader();
            while (orders.Read())
            {
                Console.WriteLine("{0} {1} ", orders["item"], orders["qty"], orders["price"]);
            }


        }

        static void Query3(SqlConnection con, string item, int qty, double price )
        {
            SqlCommand cmd = new SqlCommand("insert into orderitems (item,qty,price) VALUES (@item,@qty,@price)", con);
            cmd.Parameters.Add(new SqlParameter("@item", item));
            cmd.Parameters.Add(new SqlParameter("@qty", qty));
            cmd.Parameters.Add(new SqlParameter("@price", price));
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Ordine creato con successo!");
        }




        static void Main(string[] args)
        {
            string connStr = "data source=.\\SQLEXPRESS; initial catalog=orders; integrated security=SSPI;";
            SqlConnection con = new SqlConnection(connStr);
            con.Open();
            CheckAdmin(con);

            Console.WriteLine("Benvenuto! Effettua il login per Procedere");
            Console.Write("Username: ");
            string user = Console.ReadLine();
            Console.Write("Password: ");
            string pass = Console.ReadLine();
            if (login(con, user, pass))
            {
                Console.WriteLine("Accesso riuscito");
                string scelta = Console.ReadLine(); 
                switch(scelta)
                {

                    case "1":
                        Register(con, user, pass);
                        break;
                    case "2":
                        Console.WriteLine("Lista degli ordini");
                        Query1(con);
                        break;
                    case "3":
                        Console.WriteLine("Dettaglio degli ordini");
                        Query2(con);
                        break;
                    case "4":
                        Console.WriteLine("Insert di un ordine");
                        Query3(con, item, qty, price);
                        Console.Write("Prodotto: ");
                        string item = Console.ReadLine();
                        Console.Write("Quantià: ");
                        int qty = Console.ReadLine();
                        Console.Write("Prezzo: ");
                        double price = Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Inserimento errato");
                        break;


                }
                
                

                
            }


            



            
            
            




        }


    }
}