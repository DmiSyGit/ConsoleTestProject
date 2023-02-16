using System.Data.SqlClient;
using System.Diagnostics;

namespace ConsoleTestProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // К сожалению я не понял как именно нужно выполнить тестовое задание, поэтому оставлю коментарии
            // 6 незнаю как нужно сделать, не сталкивался с таким не разу, слышал лишь что можно делать что-то вроде "кэширования" результатов и потом б.д уже будет брать из "кэша"
            string connStr = "Data Source=DESKTOP-GK894KI\\SQLEXPRESS;Initial Catalog=ConsoleTestProjectBase;Integrated Security=True";
            Console.Write("Введите действие: ");
            int num = Convert.ToInt32(Console.ReadLine());//Можно так же получить аргументы из массива args[], если вызов через консоль или ярлык с аргументами
            switch (num)
            {
                //  К б.д так же можно подключиться используя фреймворк, который сгененрирует классы для работы с таблицами
                case 1:
                    using (SqlConnection connection = new SqlConnection(connStr))
                    {
                        connection.Open();
                        string commandText = "CREATE TABLE [dbo].[UsersTable]([idUser] [int] IDENTITY(1,1) NOT NULL,[UserFullName] [nvarchar](300) NOT NULL,[UserDateOfBirth] [date] NOT NULL,[UserGender] [bit] NOT NULL,CONSTRAINT [PK_UsersTable] PRIMARY KEY CLUSTERED ([idUser] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY]";
                        SqlCommand command = new SqlCommand(commandText, connection);
                        command.ExecuteNonQuery();
                    }
                    break;
                case 2:
                    Console.Write("Введите ФИО: ");
                    string fullName = Console.ReadLine();
                    Console.Write("Введите дату рождения в формате dd.mm.yyyy: ");
                    string dateOfBirth = Console.ReadLine();
                    Console.Write("Введите пол(0 - мужской/1 - женский): ");
                    int gender = Convert.ToInt32(Console.ReadLine());//Проверки корректности ввода не стал делать, что бы не тратить время и не перегружать читабельность
                    using (SqlConnection connection = new SqlConnection(connStr))
                    {
                        connection.Open();
                        string commandText = "INSERT INTO UsersTable(UserFullName, UserDateOfBirth, UserGender) VALUES (@fullName, @dateOfBirth, @gender)";
                        SqlCommand command = new SqlCommand(commandText, connection);
                        command.Parameters.Add(new SqlParameter("@fullName", fullName));
                        command.Parameters.Add(new SqlParameter("@dateOfBirth", dateOfBirth));
                        command.Parameters.Add(new SqlParameter("@gender", gender));
                        command.ExecuteNonQuery();
                    }
                    break;
                case 3:
                    using (SqlConnection connection = new SqlConnection(connStr))
                    {
                        connection.Open();
                        string commandText = "SELECT * FROM UsersTable";
                        SqlCommand command = new SqlCommand(commandText, connection);
                        SqlDataReader sqlDataReader = command.ExecuteReader();
                        while(sqlDataReader.Read())
                        {
                            string fname = sqlDataReader.GetString(1);
                            DateTime dBirth = sqlDataReader.GetDateTime(2);
                            string gen;
                            if (sqlDataReader.GetBoolean(3))
                            {
                                gen = "Ж";
                            }
                            else
                            {
                                gen = "М";
                            }
                            Console.WriteLine($"ФИО: {fname}; Дата рождения: {dBirth.ToString("D")}; Пол: {gen}");
                        }
                    }
                    break;
                    case 4:
                    // Несовсем понял как требуется заполнить случайными(из имеющихся списков имен/фамилий/отчеств брать случайные или полностью случайные слова)
                    // В любом случае это не сложно, цикл и random
                    break;
                case 5:
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    using (SqlConnection connection = new SqlConnection(connStr))
                    {
                        connection.Open();
                        string commandText = "SELECT * FROM UsersTable WHERE UserGender = 'false' AND UserFullName LIKE 'f%'";
                        SqlCommand command = new SqlCommand(commandText, connection);
                        SqlDataReader sqlDataReader = command.ExecuteReader();
                        while (sqlDataReader.Read())
                        {
                            string fname = sqlDataReader.GetString(1);
                            DateTime dBirth = sqlDataReader.GetDateTime(2);
                            string gen;
                            if (sqlDataReader.GetBoolean(3))
                            {
                                gen = "Ж";
                            }
                            else
                            {
                                gen = "М";
                            }
                            Console.WriteLine($"ФИО: {fname}; Дата рождения: {dBirth.ToString("D")}; Пол: {gen}");
                        }
                    }
                    stopwatch.Stop();
                    Console.WriteLine("Время выполнения: " + stopwatch.ElapsedMilliseconds + "ms");
                    break;
                default: 
                    Console.WriteLine("Введен неверный номер действия!");
                    break;
            }
        }
    }
}