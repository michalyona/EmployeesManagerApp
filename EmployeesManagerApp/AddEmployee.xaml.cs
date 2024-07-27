using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL;


namespace UI
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public delegate bool AddEmployeeDelegate(Employee employee);
    public partial class AddEmployee : Window
    {
        public event AddEmployeeDelegate EventAddEmployee;

        static string connectionString = "Data Source=DESKTOP-OQND5M9\\SQLEXPRESS;Initial Catalog=InterviewsManager;Integrated Security=True";
        Class1 b = new Class1(connectionString);
        public AddEmployee()
        {
            InitializeComponent();
            
            JobTitleComboBox.ItemsSource = b.GetRole();
        }
        public bool EmployeeAdd(Employee employee)
        {
            return EventAddEmployee?.Invoke(employee) ?? false;
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            if (ValidateInputs())
            {
                // Add employee to the database
                var employee = new Employee
                {
                    Id = int.Parse(IdTextBox.Text),
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Age = int.Parse(AgeTextBox.Text),
                    StartOfWorkYear = int.Parse(StartOfWorkingYearTextBox.Text),
                    RoleInCompany = JobTitleComboBox.SelectedItem.ToString(),
                    PhoneNumber = PhoneNumberTextBox.Text,
                    Email = MailAddressTextBox.Text
                };
                try
                {
                    b.Add(employee);
                    if (EmployeeAdd(employee))
                        MessageBox.Show("Employee added Successfully");
                    else
                        MessageBox.Show("Failed to add employee");

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                

                 
            }
        }
        private bool ValidateInputs()
        {
            if (!Regex.IsMatch(IdTextBox.Text, @"^\d{9}$"))
            {
                MessageBox.Show("ID must be 9 digits.");
                return false;
            }
            if (!Regex.IsMatch(FirstNameTextBox.Text, @"^[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("First Name must contain at least 2 letters without numbers.");
                return false;
            }
            if (!Regex.IsMatch(LastNameTextBox.Text, @"^[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Last Name must contain at least 2 letters without numbers.");
                return false;
            }
            if (!int.TryParse(AgeTextBox.Text, out int age) || age < 18 || age > 67)
            {
                MessageBox.Show("Age must be a number between 18 and 67.");
                return false;
            }
            if (!int.TryParse(StartOfWorkingYearTextBox.Text, out int startYear) || startYear < 1900 || startYear > DateTime.Now.Year)
            {
                MessageBox.Show("Start Year must be a valid year.");
                return false;
            }
            if (JobTitleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Role must be selected.");
                return false;
            }
            if (!Regex.IsMatch(PhoneNumberTextBox.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Phone number must be 10 digits.");
                return false;
            }
            if (!Regex.IsMatch(MailAddressTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email is not valid.");
                return false;
            }

            return true;
        }


        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
        

        

