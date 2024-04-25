using StockAdviser.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;


namespace StockAdviser
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Обьявление переменных и классов
        Call_DB Call_DB = new Call_DB();
        public int[] btnColor = new[] { 101, 124, 144 };
        public int[] btnBackColor = new[] { 174, 188, 202 };
        public int[] btnSearhColor = new[] { 174, 210, 209 };
        public bool isCorp = false;

        //Ключ подключения хранится в App.config
        static string api_key = ConfigurationSettings.AppSettings["Ap_key"];
        public string time_method = "Time Series (30min)";
        public string url_string = "https://www.alphavantage.co/query?" +
            "function=TIME_SERIES_INTRADAY" +
            "&symbol=SYMBOL" +
            "&interval=30min" +
            $"&apikey={api_key}";
        private void Form1_Load(object sender, EventArgs e)
        {
            groupBoxWinLouse.Hide();
            groupBoxCorp.Hide();
            searh_load();

            chartStock.Legends.Clear();
            chartStock.Series["StockSeries"].Color = Color.FromArgb(101, 124, 144);
            chartStock.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chartStock.ChartAreas["ChartArea1"].AxisX.Maximum = 21;
            chartStock.ChartAreas[0].AxisY.LabelStyle.Format = "0.00";

            Random random = new Random();
            for (int i = 0; i < 21; i++)
            {
                chartStock.Series["StockSeries"].Points.AddXY(i, random.Next(1,30));
            }
            chartStock.Series["StockSeries"].BorderWidth = 3;
            chartStock.Series["StockSeries"].IsValueShownAsLabel = true;
            chartStock.Series["StockSeries"].ChartType = SeriesChartType.Column;
        }

        //Поисковая система
        public void searh_load()
        {
            DataSet stockTable = Call_DB.Request("SELECT names_s FROM US_symbol_stock");
            string[] stockString = new string[stockTable.Tables[0].Rows.Count];
            for (int i = 0; i < stockTable.Tables[0].Rows.Count; i++)
            {
                stockString[i] = stockTable.Tables[0].Rows[i][0].ToString();
            }
            var values = new AutoCompleteStringCollection();
            values.AddRange(stockString);
            comboBoxSearch.AutoCompleteCustomSource = values;
            comboBoxSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBoxSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        //Кнопки
        private void buttonDayValue_Click(object sender, EventArgs e)//Почасовые показатели за последний день
        {
            buttonColorOff();
            buttonStock.BackColor = Color.FromArgb(
                btnColor[0], btnColor[1], btnColor[2]);
            panelDayValue.Show();
            labelHead.Text = "Показатели ценных бумаг";
            open_searh();
            isCorp = false;
            groupBoxDuration.Show();
            groupBoxSettings.Show();
            textBoxInfo.Show();
            labelInfo.Show();
            groupBoxWinLouse.Hide();
            groupBoxCorp.Hide();
            chartStock.Show();
            url_string = "https://www.alphavantage.co/query?" +
            "function=TIME_SERIES_INTRADAY" +
            "&symbol=SYMBOL" +
            "&interval=30min" +
            $"&apikey={api_key}";
            time_method = "Time Series (30min)";

        }
        private void buttonWinsLosers_Click(object sender, EventArgs e)//Лучшие и худшие игроки рынка
        {
            buttonColorOff();
            buttonWinsLosers.BackColor = Color.FromArgb(
               btnColor[0], btnColor[1], btnColor[2]);
            labelHead.Text = "Лучшие и худшие игроки рынка";
            close_searh();
            chartStock.Show();
            groupBoxWinLouse.Show();
            groupBoxSettings.Hide();
            groupBoxCorp.Hide();
        }
        private void buttonCorpAnalys_Click(object sender, EventArgs e)//Плановый анализ компании и ее акций
        {
            buttonColorOff();
            buttonCorpAnalys.BackColor = Color.FromArgb(
                btnColor[0], btnColor[1], btnColor[2]);
            labelHead.Text = "Плановый анализ компании и ее акций";
            groupBoxDuration.Hide();
            textBoxInfo.Hide();
            labelInfo.Hide();
            chartStock.Hide();
            groupBoxWinLouse.Hide();
            groupBoxSettings.Hide();
            groupBoxCorp.Show();
            open_searh();
            isCorp = true;
            url_string = "https://www.alphavantage.co/query?" +
                "function=OVERVIEW" +
                "&symbol=SYMBOL" +
                $"&apikey={api_key}";
        }
        private void buttonFovourites_Click(object sender, EventArgs e)//Акции в списке избранных
        {
            buttonColorOff();
            buttonFovourites.BackColor = Color.FromArgb(
                btnColor[0], btnColor[1], btnColor[2]);
            labelHead.Text = "Акции в списке избранных";
            close_searh();
            groupBoxWinLouse.Hide();
            groupBoxSettings.Hide();
            groupBoxCorp.Hide();
        }
        private void buttonInfo_Click(object sender, EventArgs e)//Как это работает?
        {
            buttonColorOff();
            buttonInfo.BackColor = Color.FromArgb(
                btnColor[0], btnColor[1], btnColor[2]);
            labelHead.Text = "Как это работает?";
            close_searh();
            groupBoxWinLouse.Hide();
            groupBoxCorp.Hide();
            Info_form info_Form = new Info_form();
            info_Form.ShowDialog();

        }
        private void buttonCast_Click(object sender, EventArgs e)//Настройки кастомизации
        {
            buttonColorOff();
            buttonCast.BackColor = Color.FromArgb(
                btnColor[0], btnColor[1], btnColor[2]);
            labelHead.Text = "Настройки кастомизации";
            close_searh();
            groupBoxWinLouse.Hide();
            groupBoxCorp.Hide();
        }
        private void buttonExit_Click(object sender, EventArgs e)//Выход
        {
            this.Close();
        }
        //Покраска кнопок
        public void buttonColorOff()
        {
            buttonStock.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonWinsLosers.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonCorpAnalys.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonFovourites.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonInfo.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonCast.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
        }
        //Скрыть или показать поиск
        public void open_searh()
        {
            pictureBoxSearch.Show();
            labelSearch.Show();
            comboBoxSearch.Show();
            buttonSearch.Show();

        }
        public void close_searh() 
        {
            pictureBoxSearch.Hide();
            labelSearch.Hide();
            comboBoxSearch.Hide();
            buttonSearch.Hide();
            groupBoxDuration.Hide();
            textBoxInfo.Hide();
            labelInfo.Hide();
            groupBoxSettings.Hide();
            chartStock.Hide();
        }
        //Запуск поиска
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet corp;
                DataSet stock_symbol = Call_DB.Request($"SELECT * FROM US_symbol_stock WHERE names_s = '{comboBoxSearch.Text}'");
                string url = url_string.Replace("SYMBOL", $"{stock_symbol.Tables[0].Rows[0][1]}");
                JObject stocks_value;
                if (isCorp)
                {
                    Call_DB.Open();
                    if (Call_DB.Select_corp(stock_symbol.Tables[0].Rows[0][1].ToString()))
                    {
                        corp = Call_DB.Request("SELECT * FROM Corp_orders");

                        textBoxCorpInfo.Text = "";
                        textBoxCorpInfo.Text +=
                        "Символ: " + corp.Tables[0].Rows[0][1] +
                        Environment.NewLine + "Тип акций: " + corp.Tables[0].Rows[0][2] +
                        Environment.NewLine + "Название: " + corp.Tables[0].Rows[0][3] +
                        Environment.NewLine + "CIK: " + corp.Tables[0].Rows[0][4] +
                        Environment.NewLine + "Обмен: " + corp.Tables[0].Rows[0][5] +
                        Environment.NewLine + "Валюта: " + corp.Tables[0].Rows[0][6] +
                        Environment.NewLine + "Страна: " + corp.Tables[0].Rows[0][7] +
                        Environment.NewLine + "Сектор: " + corp.Tables[0].Rows[0][8] +
                        Environment.NewLine + "Направление: " + corp.Tables[0].Rows[0][9] +
                        Environment.NewLine + "Адрес: " + corp.Tables[0].Rows[0][10] +
                        Environment.NewLine + "Окончание года: " + corp.Tables[0].Rows[0][11] +
                        Environment.NewLine + "Последний квартал: " + corp.Tables[0].Rows[0][12] +
                        Environment.NewLine + "Капитализация: " + corp.Tables[0].Rows[0][13] +
                        Environment.NewLine + "EBITDA: " + corp.Tables[0].Rows[0][14] +
                        Environment.NewLine + "EPS: " + corp.Tables[0].Rows[0][15] +
                        Environment.NewLine + "Дивиденды на акцию: " + corp.Tables[0].Rows[0][16] +
                        Environment.NewLine + "Предполагаемая цена: " + corp.Tables[0].Rows[0][17] +
                        Environment.NewLine + "Минимум за 52 н.: " + corp.Tables[0].Rows[0][18] +
                        Environment.NewLine + "Максимум за 52 н.: " + corp.Tables[0].Rows[0][19] +
                        Environment.NewLine + "Скользящая за 50 д.: " + corp.Tables[0].Rows[0][20] +
                        Environment.NewLine + "Скользящая за 200 д.: " + corp.Tables[0].Rows[0][21] +
                        Environment.NewLine + "Выплата дивидендов: " + corp.Tables[0].Rows[0][22] +
                        Environment.NewLine + "Последняя выплата: " + corp.Tables[0].Rows[0][23];
                }
                    else
                    {
                        stocks_value = api_request(url);
                        corpMeth(stocks_value);
                    }
                }
                else
                {
                    stocks_value = api_request(url);
                    durationMeth(stocks_value);
                }
            }
            catch { MessageBox.Show("Поисковая строка пуста"); }

        }
        //Обработка временного интервала
        public void durationMeth(JObject stock)
        {
            List<string> time_list = new List<string>();
            List<double> value_list = new List<double>();

            var timeSeries = stock[time_method];
            if(timeSeries != null )
            {
                var metaData = stock["Meta Data"];

                textBoxInfo.Text = "";
                textBoxInfo.Text += "Символ: "
                    + metaData["2. Symbol"].ToString();
                textBoxInfo.Text += Environment.NewLine + "Обновлено: "
                    + metaData["3. Last Refreshed"].ToString();
                if(metaData["6. Time Zone"] != null)
                {
                    textBoxInfo.Text += Environment.NewLine + "Временная зона: "
                    + metaData["6. Time Zone"].ToString();
                }
                int a = 0;
                foreach (var item in timeSeries)
                {
                    string[] sale = item.ToString().Split('"');
                    if(time_list.Count > 21 )
                    {
                        break;
                    }
                    time_list.Add(sale[1]/*.Substring(11)*/);
                    value_list.Add(Convert.ToDouble(sale[5],
                        System.Globalization.CultureInfo.InvariantCulture));
                    textBoxInfo.Text += Environment.NewLine + $"{a} - {time_list[a]} - {value_list[a]}";
                   
                    a++;
                }
                chartCreate(value_list, time_list);
            }
            else
            {
                MessageBox.Show("Приносим свои извенения, но вы превысили лимит запросов к API");
            }

        }
        public void corpMeth(JObject stock)
        {
            string corp_add;
            textBoxCorpInfo.Text = "";
            textBoxCorpInfo.Text +=
                "Символ: " + stock["Symbol"] +
                Environment.NewLine + "Тип акций: " + stock["AssetType"] +
                Environment.NewLine + "Название: " + stock["Name"] +
                Environment.NewLine + "CIK: " + stock["CIK"] +
                Environment.NewLine + "Обмен: " + stock["Exchange"] +
                Environment.NewLine + "Валюта: " + stock["Currency"] +
                Environment.NewLine + "Страна: " + stock["Country"] +
                Environment.NewLine + "Сектор: " + stock["Sector"] +
                Environment.NewLine + "Направление: " + stock["Industry"] +
                Environment.NewLine + "Адрес: " + stock["Address"] +
                Environment.NewLine + "Окончание года: " + stock["FiscalYearEnd"] +
                Environment.NewLine + "Последний квартал: " + stock["LatestQuarter"] +
                Environment.NewLine + "Капитализация: " + stock["MarketCapitalization"] +
                Environment.NewLine + "EBITDA: " + stock["EBITDA"] +
                Environment.NewLine + "EPS: " + stock["EPS"] +
                Environment.NewLine + "Дивиденды на акцию: " + stock["DividendYield"] +
                Environment.NewLine + "Предполагаемая цена: " + stock["AnalystTargetPrice"] +
                Environment.NewLine + "Минимум за 52 н.: " + stock["52WeekHigh"] +
                Environment.NewLine + "Максимум за 52 н.: " + stock["52WeekLow"] +
                Environment.NewLine + "Скользящая за 50 д.: " + stock["50DayMovingAverage"] +
                Environment.NewLine + "Скользящая за 200 д.: " + stock["200DayMovingAverage"] +
                Environment.NewLine + "Выплата дивидендов: " + stock["DividendDate"] +
                Environment.NewLine + "Последняя выплата: " + stock["ExDividendDate"];

            corp_add = 
                "INSERT INTO Corp_orders " +
                "(`symbol`,`AssetType`,`Namess`,`CIK`,`Exchange`,`Currency`,`Country`,`Sector`," +
                "`Industry`,`Address`,`FiscalYearEnd`,`LatestQuarter`,`MarketCapitalization`," +
                "`EBITDA`,`EPS`,`DividendYield`,`AnalystTargetPrice`,`52WeekHigh`,`52WeekLow`," +
                "`50DayMovingAverage`,`200DayMovingAverage`,`DividendDate`,`ExDividendDate`) " +
                "VALUES " +
                $"('{stock["Symbol"]}','{stock["AssetType"]}','{stock["Name"]}','{stock["CIK"]}'," +
                $"'{stock["Exchange"]}','{stock["Currency"]}','{stock["Country"]}','{stock["Sector"]}'," +
                $"'{stock["Industry"]}','{stock["Address"]}','{stock["FiscalYearEnd"]}'," +
                $"'{stock["LatestQuarter"]}','{stock["MarketCapitalization"]}','{stock["EBITDA"]}'," +
                $"'{stock["EPS"]}','{stock["DividendYield"]}','{stock["AnalystTargetPrice"]}'," +
                $"'{stock["52WeekHigh"]}','{stock["52WeekLow"]}','{stock["50DayMovingAverage"]}'," +
                $"'{stock["200DayMovingAverage"]}','{stock["DividendDate"]}','{stock["ExDividendDate"]}')";
            Call_DB.Request(corp_add);

        }
        //Создаем и заполняем график
        public void chartCreate(List<double>value_list, List<string>time_list)
        {
            chartStock.ChartAreas["ChartArea1"].AxisY.IsStartedFromZero = false;
            chartStock.Series["StockSeries"].Points.Clear();
            

            for (int x = value_list.Count() - 1; x >= 0; x--)
            {
                string val;
                if (time_list[x].Length > 12)
                {
                    val = time_list[x].Substring(11);
                    val = val.Substring(0, val.Length - 3);
                }
                else { val = time_list[x]; }

                chartStock.Series["StockSeries"].Points.AddXY(val, value_list[x]);
            }
            
        }
        //Запросы к API
        public JObject api_request(string _URL)
        {
            var request = new Api_request(_URL);
            request.Run();
            var response = request.Response;
            var json = JObject.Parse(response);

            return json;
        }
        private void buttonMethDay_Click(object sender, EventArgs e)
        {
            button2colorOff();
            buttonMethDay.BackColor = Color.FromArgb(
                btnSearhColor[0], btnSearhColor[1], btnSearhColor[2]);
            url_string = "https://www.alphavantage.co/query?" +
            "function=TIME_SERIES_INTRADAY" +
            "&symbol=SYMBOL" +
            "&interval=30min" +
            $"&apikey={api_key}";
            time_method = "Time Series (30min)";

        }
        private void buttonMethEvrDay_Click(object sender, EventArgs e)
        {
            button2colorOff();
            buttonMethEvrDay.BackColor = Color.FromArgb(
                btnSearhColor[0], btnSearhColor[1], btnSearhColor[2]);
            url_string = "https://www.alphavantage.co/query?" +
            "function=TIME_SERIES_DAILY" +
            "&symbol=SYMBOL" +
            $"&apikey={api_key}";
            time_method = "Time Series (Daily)";

        }
        private void buttonMethWeek_Click(object sender, EventArgs e)
        {
            button2colorOff();
            buttonMethWeek.BackColor = Color.FromArgb(
                btnSearhColor[0], btnSearhColor[1], btnSearhColor[2]);
            url_string = "https://www.alphavantage.co/query?" +
            "function=TIME_SERIES_WEEKLY" +
            "&symbol=SYMBOL" +
            $"&apikey={api_key}";
            time_method = "Weekly Time Series";

        }
        private void buttonMethMon_Click(object sender, EventArgs e)
        {
            button2colorOff();
            buttonMethMon.BackColor = Color.FromArgb(
                btnSearhColor[0], btnSearhColor[1], btnSearhColor[2]);
            url_string = "https://www.alphavantage.co/query?" +
            "function=TIME_SERIES_MONTHLY" +
            "&symbol=SYMBOL" +
            $"&apikey={api_key}";
            time_method = "Monthly Time Series";

        }
        private void buttonMethYear_Click(object sender, EventArgs e)
        {
            button2colorOff();
            buttonMethYear.BackColor = Color.FromArgb(
                btnSearhColor[0], btnSearhColor[1], btnSearhColor[2]);
            
        }
        public void button2colorOff()
        {
            buttonMethDay.BackColor = Color.FromArgb(
                  btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonMethEvrDay.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonMethWeek.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonMethMon.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
            buttonMethYear.BackColor = Color.FromArgb(
                btnBackColor[0], btnBackColor[1], btnBackColor[2]);
        }
        //Настройка типа графига
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex) 
            { 
                case 0:
                    chartStock.Series["StockSeries"].ChartType = SeriesChartType.Column;
                    break;
                case 1:
                    chartStock.Series["StockSeries"].ChartType = SeriesChartType.Line;
                    break;
                case 2:
                    chartStock.Series["StockSeries"].ChartType = SeriesChartType.Area;
                    break;
                case 3:
                    chartStock.Series["StockSeries"].ChartType = SeriesChartType.Spline;
                    break;
            }
        }
        //Настройка цвета графика
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    chartStock.Series["StockSeries"].Color = Color.FromArgb(174, 188, 202);
                    break;
                case 1:
                    chartStock.Series["StockSeries"].Color = Color.FromArgb(101, 124, 144);
                    break;
                case 2:
                    chartStock.Series["StockSeries"].Color = Color.FromArgb(174, 210, 209);
                    break;
                case 3:
                    chartStock.Series["StockSeries"].Color = Color.Red;
                    break;
                case 4:
                    chartStock.Series["StockSeries"].Color = Color.Blue;
                    break;
                case 5:
                    chartStock.Series["StockSeries"].Color = Color.Yellow;
                    break;
            }
        }
        //Вывод лучших и худших игроков рынка
        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://www.alphavantage.co/query?" +
            "function=TOP_GAINERS_LOSERS" +
            $"&apikey={api_key}";

            JObject stocks_value = api_request(url);
            textBox1.Text = stocks_value["last_updated"].ToString();
            var top_gainers = stocks_value["top_gainers"];
            var top_losers = stocks_value["top_losers"];

            foreach (var stock in top_gainers)
            {
                textBox2.Text +=
                    stock["ticker"] + " - " +
                    stock["price"] + " + " +
                    stock["change_percentage"] +
                    Environment.NewLine;
            }
            foreach (var stock in top_losers)
            {
                textBox3.Text +=
                    stock["ticker"] + " - " +
                    stock["price"] + "  " +
                    stock["change_percentage"] +
                    Environment.NewLine;
            }
        }

		private void button_like_Click(object sender, EventArgs e)
		{
			if (comboBoxSearch.Text != "")
			{
                try
                {
                    Call_DB.Open();
					DataSet stock_symbol = Call_DB.Request($"SELECT * FROM US_symbol_stock WHERE names_s = '{comboBoxSearch.Text}'");
					string symboyl = stock_symbol.Tables[0].Rows[0][1].ToString();
					string stock = comboBoxSearch.Text;
					if (Call_DB.Select_fov(stock_symbol.Tables[0].Rows[0][1].ToString()))
					{
						DialogResult result = MessageBox.Show(
		                "Данная акция уже находится в избранном, удалить ее?",
		                "Обратите внимание",
		                MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);

						if (result == DialogResult.Yes)
							button1.BackColor = Color.Red;

						this.TopMost = true;
					}
					else
					{
						Call_DB.Request($"INSERT INTO Favourites (`symbol_s`,`names_s`) VALUES ('{symboyl}','{stock}')");
                        
					}
                    Call_DB.Close();
					button_like.BackgroundImage = new Bitmap(Properties.Resources.love_like_heart_icon_196980);
					System.Threading.Thread.Sleep(500);
					button_like.BackgroundImage = new Bitmap(Properties.Resources.heart_likes_like_love_icon_251441);
				}
				catch { 
                    MessageBox.Show("Невозможно добавить в избранное");
					Call_DB.Close();
				}
            }
			else
			{
				MessageBox.Show("Поисковая строка пуста");
			}
		}
	}
}
