using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Data;
using static HospitalAdonet.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace HospitalAdonet
{
    public partial class Form1 : Form
    {
        string connectionString = null;
        SqlConnection connection = null;
        SqlDataAdapter adapter = null;
        DataSet set = new DataSet();

        public class Examination
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Doctor
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public float Salary { get; set; }
            public float Premium { get; set; }
        }


        public Form1()
        {            
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["HospitalDb"].ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = NameTextBox.Text;
            SqlCommand cmdText = connection.CreateCommand();
            cmdText.CommandText = $"select w.Places " +
                                  $"from Wards as w JOIN Departments as d on w.DepartmentId = d.Id " +
                                  $"where d.Name = '{name}'";

            int number = (int)cmdText.ExecuteScalar();
            MessageBox.Show($"Count Of Places : {number}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmdText = connection.CreateCommand();
            cmdText.CommandText = $"select * " +
                                  $"from Examinations " +
                                  $"order by Id";

            SqlDataReader reader = cmdText.ExecuteReader();


            List<Examination> examinations = new List<Examination>();

            while (reader.Read())
            {
                examinations.Add(new Examination()
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"]
                });
            }
            MessageBox.Show($"Examination: {examinations.Count}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string time = DateTimePicker.Text;
            SqlCommand cmdText = connection.CreateCommand();
            cmdText.CommandText = $"delete from DoctorsExaminations where StartTime < '{time}'";

            cmdText.ExecuteNonQuery();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int salary = (int)SalaryUpDown.Value;
            SqlCommand cmdText = connection.CreateCommand();
            cmdText.CommandText = $"select * " +
                                  $"from Doctors " +
                                  $"where Salary > {salary}";

            SqlDataReader reader = cmdText.ExecuteReader();

            List<Doctor> list = new List<Doctor>();

            while (reader.Read())
            {
                list.Add(new Doctor()
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Surname = (string)reader["Surname"],
                    Salary = (float)reader["Salary"],
                    Premium = (float)reader["Premium"]
                });
            }

            MessageBox.Show($"Examination: {list.Count}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlCommand cmdText = connection.CreateCommand();
            cmdText.CommandText = $"select top 1 Amount " +
                                  $"from Donations " +
                                  $"order by Amount desc";

            MessageBox.Show($"{Convert.ToDecimal(cmdText.ExecuteScalar())}");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string name = ExTextBox.Text;
            SqlCommand cmdText = connection.CreateCommand();
            cmdText.CommandText = $"insert into Examinations (Name) values ('{name}')";

            MessageBox.Show($"{cmdText.ExecuteNonQuery()}");
        }
    }
}