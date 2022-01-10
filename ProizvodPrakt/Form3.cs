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
    public partial class Form3 : Form
    {
        string Familia;
        string FIO;
        string id_selected_naz;
        string id_selected_dat;
        string id_selected_vrem;
        string id_selected_mest;

        public void GetSelectedFIOString()
        {
            //Переменная для индекс выбранной строки в гриде
            string index_selected_nazv;
            string index_selected_vrema;
            string index_selected_data;
            string index_selected_mesto;
            //Индекс выбранной строки
            index_selected_nazv = dataGridView1.SelectedCells[0].RowIndex.ToString();
            index_selected_vrema = dataGridView1.SelectedCells[1].RowIndex.ToString();
            index_selected_data = dataGridView1.SelectedCells[2].RowIndex.ToString();
            index_selected_mesto = dataGridView1.SelectedCells[3].RowIndex.ToString();
            //ID конкретной записи в Базе данных, на основании индекса строки
            id_selected_nazv = dataGridView1.Rows[Convert.ToInt32(index_selected_nazv)].Cells[0].Value.ToString();
            id_selected_vrema = dataGridView1.Rows[Convert.ToInt32(index_selected_vrema)].Cells[1].Value.ToString();
            id_selected_data = dataGridView1.Rows[Convert.ToInt32(index_selected_data)].Cells[2].Value.ToString();
            id_selected_mesto = dataGridView1.Rows[Convert.ToInt32(index_selected_mesto)].Cells[3].Value.ToString();
            //Указываем ID выделенной строки в метке
            id_selected_naz = id_selected_nazv;
            id_selected_dat = id_selected_data;
            id_selected_vrem = id_selected_vrema;
            id_selected_mest = id_selected_mesto;
        }
        public bool InsertPrepods ()
        {
            //определяем переменную, хранящую количество вставленных строк
            int InsertCount = 0;
            //Объявляем переменную храняющую результат операции
            bool result = false;
            // открываем соединение
            conn.Open();
            // запросы
            // запрос вставки данных
            string query = $"INSERT INTO Zapisi (FiO, Familia, Meropriyatie, Data, Vrema, Mesto) VALUES ('{FIO}','{Familia}','{id_selected_naz}', '{id_selected_dat}','{id_selected_vrem}', '{id_selected_mest}')";
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
        public Form3()
        {
            InitializeComponent();
        }
        string id_selected_mesto = "0";
        string id_selected_data = "0";
        string id_selected_nazv = "0";
        string id_selected_vrema = "0";
        //Переменная соединения
        MySqlConnection conn;
        //DataAdapter представляет собой объект Command , получающий данные из источника данных.
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        //Объявление BindingSource, основная его задача, это обеспечить унифицированный доступ к источнику данных.
        private BindingSource bSource = new BindingSource();
        //DataSet - расположенное в оперативной памяти представление данных, обеспечивающее согласованную реляционную программную 
        //модель независимо от источника данных.DataSet представляет полный набор данных, включая таблицы, содержащие, упорядочивающие 
        //и ограничивающие данные, а также связи между таблицами.
        private DataSet ds = new DataSet();
        //Представляет одну таблицу данных в памяти.
        private DataTable table = new DataTable();
        public void GetListUsers()
        {
            //Запрос для вывода строк в БД
            string commandStr = "SELECT Nazvanie AS 'Название', Data AS 'Дата', Vrema AS 'Время', Mesto AS 'Место' FROM Meropriyatie";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;
            //Закрываем соединение
            conn.Close();
        }
        public void ManagerRole(int role)
        {
            switch (role)
            {
                //И в зависимости от того, какая роль (цифра) хранится в поле класса и передана в метод, показываются те или иные кнопки.
                //Вы можете скрыть их и не отображать вообще, здесь они просто выключены
                case 1:
                    button3.Enabled = true;
                    break;
                //Если по какой то причине в классе ничего не содержится, то всё отключается вообще
                default:
                    button3.Enabled = false;
                    break;
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            //Сокрытие текущей формы
            this.Hide();
            //Инициализируем и вызываем форму диалога авторизации
            Form2 form2 = new Form2();
            //Вызов формы в режиме диалога
            form2.ShowDialog();
            //Если авторизации была успешна и в поле класса хранится истина, то делаем движуху:
            if (Auth.auth)
            {
                //Отображаем рабочую форму
                this.Show();
                textBox1.Text = Auth.auth_id;
                textBox2.Text = Auth.auth_fio;
                //Вызываем метод управления ролями
                ManagerRole(Auth.auth_role);
            }
            //иначе
            else
            {
                //Закрываем форму
                this.Close();
            }
            //Объявляем переменную для передачи значения в другую форму
            string variable = textBox1.Text;
            //Класс SomeClass объявлен в файле Program.cs, в нём объявлено простое поле. Наша задача, присвоить этому полю значение, 
            //а в другой форме его вытащить.
            SomeClass.variable_class = variable;
            FIO = textBox1.Text;
            Familia = textBox2.Text;
            // строка подключения к БД
            string connStr = "server=caseum.ru;port=33333;user=st_2_1_19;database=st_2_1_19;password=68201560;";
            // создаём объект для подключения к БД
            conn = new MySqlConnection(connStr);
            //Вызываем метод для заполнение дата Грида
            GetListUsers();
            //Видимость полей в гриде
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;
            dataGridView1.Columns[3].FillWeight = 15;
            //Режим для полей "Только для чтения"
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;
        }
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            bSource.Filter = "[Название] LIKE'" + toolStripTextBox1.Text + "%'";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                InsertPrepods();
            }
        }
        private void dataGridView1_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Left))
            {
                //Отвечает за то куда нажали
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];

                dataGridView1.CurrentRow.Selected = true;
                GetSelectedFIOString();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }
    }
}
