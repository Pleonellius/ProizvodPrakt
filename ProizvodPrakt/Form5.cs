using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProizvodPrakt
{
    public partial class Form5 : Form
    {
        //Простой метод принимающий в качества параметра любой ListBox и выводящий в него список преподавателей
        public void GetListPrepods(ListBox lb)
        {
            //Чистим ListBox
            lb.Items.Clear();
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT * FROM Meropriyatie";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                
                // элементы массива [] - это значения столбцов из запроса SELECT
                lb.Items.Add($"Название: {reader[0].ToString()} Дата: {reader[1].ToString()}Время: {reader[2].ToString()}Место: {reader[3].ToString()}");

            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
        }
        //Простой метод добавляющий в таблицу записи, в качестве параметров принимает ФИО и Предмет
        public bool InsertPrepods(string i_nazvanie, string i_Data, string i_Vrema, string i_Mesto)
        {
            //определяем переменную, хранящую количество вставленных строк
            int InsertCount = 0;
            //Объявляем переменную храняющую результат операции
            bool result = false;
            // открываем соединение
            conn.Open();
            // запросы
            // запрос вставки данных
            string query = $"INSERT INTO Meropriyatie (Nazvanie, Data, Vrema, Mesto) VALUES ('{i_nazvanie}', '{i_Data}','{i_Vrema}', '{i_Mesto}')";
            try
            {
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(query, conn);
                // выполняем запрос
                InsertCount = command.ExecuteNonQuery();
                // закрываем подключение к БД
            }
            catch
            {
                //Если возникла ошибка, то запрос не вставит ни одной строки
                InsertCount = 0;
            }
            finally
            {
                //Но в любом случае, нужно закрыть соединение
                conn.Close();
                //Ессли количество вставленных строк было не 0, то есть вставлена хотя бы 1 строка
                if (InsertCount != 0)
                {
                    //то результат операции - истина
                    result = true;
                }
            }
            //Вернём результат операции, где его обработает алгоритм
            return result;
        }
        public bool DeletePrepods(string i_nazvanie)
        {
            //определяем переменную, хранящую количество вставленных строк
            int InsertCount = 0;
            //Объявляем переменную храняющую результат операции
            bool result = false;
            // открываем соединение
            conn.Open();
            // запрос удаления данных
            string query = $"DELETE FROM Meropriyatie WHERE (Nazvanie='{i_nazvanie}')";
            try
            {
                // объект для выполнения SQL-запроса
                MySqlCommand command = new MySqlCommand(query, conn);
                // выполняем запрос
                InsertCount = command.ExecuteNonQuery();
                // закрываем подключение к БД
            }
            catch
            {
                //Если возникла ошибка, то запрос не вставит ни одной строки
                InsertCount = 0;
            }
            finally
            {
                //Но в любом случае, нужно закрыть соединение
                conn.Close();
                //Ессли количество вставленных строк было не 0, то есть вставлена хотя бы 1 строка
                if (InsertCount != 0)
                {
                    //то результат операции - истина
                    result = true;
                }
            }
            //Вернём результат операции, где его обработает алгоритм
            return result;
        }
        //Объявляем соединения с БД
        MySqlConnection conn;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // строка подключения к БД
            string connStr = "server=caseum.ru;port=33333;user=st_2_1_19;database=st_2_1_19;password=68201560;";
            // создаём объект для подключения к БД
            conn = new MySqlConnection(connStr);
            //Вызов метода обновления списка преподавателей с передачей в качестве параметра ListBox
            GetListPrepods(listBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text== "" ||textBox3.Text== "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                //Объявляем переменные для вставки в БД
                string ins_nazv = textBox1.Text;
                string ins_Vrema = textBox3.Text;
                string ins_Mesto = textBox4.Text;
                string ins_Data = textBox2.Text;
                //Если метод вставки записи в БД вернёт истину, то просто обновим список и увидим вставленное значение
                if (InsertPrepods(ins_nazv, ins_Data, ins_Vrema, ins_Mesto))
                {
                    GetListPrepods(listBox1);
                }
                //Иначе произошла какая то ошибка и покажем пользователю уведомление
                else
                {
                    MessageBox.Show("Произошла ошибка.", "Ошибка");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Помещаем в переменную введёный ИД для удаления
            string id_del = textBox5.Text;
            //Если функция удалила строку, то
            if (DeletePrepods(id_del))
            {
                //Вызываем метод обновления листбокса
                GetListPrepods(listBox1);
            }
            //Иначе произошла какая то ошибка и покажем пользователю уведомление
            else
            {
                MessageBox.Show("Произошла ошибка.", "Ошибка");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
