using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Inaugura.Stocks;
using Inaugura.Stocks.Data;

namespace StocksImporter
{
    public partial class Form1 : Form
    {
        List<Stock> mAllStocks;
        private Inaugura.Stocks.Data.MSSQL.StockStore mStockStore;
        private Random rand = new Random((int)DateTime.Now.Ticks);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mStockStore = new Inaugura.Stocks.Data.MSSQL.StockStore(System.Configuration.ConfigurationManager.ConnectionStrings["StockData"].ConnectionString);
            
            string[] directories = System.IO.Directory.GetDirectories(@"C:\Users\Chris Powers\Documents\Stock Data\");                        
            foreach (string dir in directories)
                ProcessDirectory(mStockStore, dir);
            MessageBox.Show("Done");
        }

        private void ProcessDirectory(IStockStore store, string directory)
        {
            // read the info file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.IO.Path.Combine(directory, "info.xml"));

            string name = xmlDoc.SelectSingleNode("market/name").InnerText;
            string code = xmlDoc.SelectSingleNode("market/code").InnerText;

            // Get the market
            Market[] markets = store.GetMarkets();
            Market market = null;
            foreach(Market m in markets)
                if(m.Code == code)
                    market = m;
                       
            DateTime lastEODDate = DateTime.MinValue;
            // create the market if it does not exist
            if(market == null)
            {
                market = new Market(name, code);
                store.AddMarket(market);
            }
            else
                lastEODDate = market.GetLastEODDate(store);

            mAllStocks = new List<Stock>(mStockStore.GetStocks(market.ID));

            string[] files = System.IO.Directory.GetFiles(directory, "*.txt");
            foreach(string file in files)
            {
                if (ImportFile(file, market, lastEODDate))
                {
                    // move the file to the processed directory
                    string dest = System.IO.Path.Combine(directory, "Processed");
                    if (!System.IO.Directory.Exists(dest))
                        System.IO.Directory.CreateDirectory(dest);
                    dest = System.IO.Path.Combine(dest, System.IO.Path.GetFileName(file));
                    if (System.IO.File.Exists(dest))
                        System.IO.File.Delete(dest);
                    System.IO.File.Move(file, dest);
                }
            }

            // see if we have a symbols file
            files = System.IO.Directory.GetFiles(directory, "*.symbols");
            foreach (string file in files)
            {
                UpdateSymbols(market, file);
                string dest = System.IO.Path.Combine(directory, "Processed");
                if (!System.IO.Directory.Exists(dest))
                    System.IO.Directory.CreateDirectory(dest);
                dest = System.IO.Path.Combine(dest, System.IO.Path.GetFileName(file));
                if (System.IO.File.Exists(dest))
                    System.IO.File.Delete(dest);
                System.IO.File.Move(file, dest);
            }
        }

        private void UpdateSymbols(Market market, string file)
        {
            string[] lines = System.IO.File.ReadAllLines(file);
            List<Stock> stocks = new List<Stock>(market.GetStocks(mStockStore));

            foreach (string line in lines)
            {
                string[] data = line.Split('\t');
                foreach (Stock stock in stocks)
                {
                    if (stock.Symbol == data[0])
                    {
                        stock.Name = data[1];
                        stock.Update(mStockStore);
                        break;
                    }
                }
            }
            
        }

        private bool ImportFile(string path, Market market, DateTime lastEODDate)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    // make sure its not a header
                    if (!line.StartsWith("<"))
                    {
                        string[] data = line.Split(',');
                        Inaugura.Stocks.Stock stock = null;
                        foreach (Inaugura.Stocks.Stock s in mAllStocks)
                            if (s.Symbol == data[0])
                                stock = s;

                        if (stock == null)
                        {
                            stock = new Inaugura.Stocks.Stock(market.ID, "N/A", data[0]);
                            mStockStore.AddStock(stock);
                            mLstStocks.Items.Add(string.Format("Adding {0}", stock));
                            mAllStocks.Add(stock);
                        }

                        DateTime date = new DateTime(Convert.ToInt32(data[1].Substring(0, 4)), Convert.ToInt32(data[1].Substring(4, 2)), Convert.ToInt32(data[1].Substring(6, 2)));
                        // add the EOD data
                        Inaugura.Stocks.EODData eod = new Inaugura.Stocks.EODData(stock.ID, date, Convert.ToSingle(data[2]), Convert.ToSingle(data[5]), Convert.ToSingle(data[3]), Convert.ToSingle(data[4]), Convert.ToInt32(data[6]));

                        //if (eod.Date <= lastEODDate)
                        //   return;


                        stock.AddEODData(mStockStore, eod);
                        //mLstEODData.Items.Add(string.Format("Adding {0}", eod));
                    }
                }
            }
            catch (Exception ex)
            {
                this.mListErrors.Items.Add(string.Format("Adding {0}", ex.ToString()));
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AForge.Neuro.ActivationNetwork network = new AForge.Neuro.ActivationNetwork(new AForge.Neuro.SigmoidFunction(), 3, 4, 1);

            AForge.Neuro.Learning.BackPropagationLearning teacher = new AForge.Neuro.Learning.BackPropagationLearning(network);

            using (System.IO.TextWriter writer = System.IO.File.CreateText("Output.csv"))
            {

                for (int i = 0; i < 2000; i++)
                {
                    double[] input = this.GetInput();
                    double[] output = new double[] { CalculateOutput(input) };
                    double error = teacher.Run(input, output);
                    writer.WriteLine(string.Format("{0},{1},{2},{3},{4}", input[0], input[1], input[2], output[0], error));
                }

                for (int i = 0; i < 20; i++)
                {
                    int i1 = rand.Next(10) - 5;
                    double[] input = this.GetInput();
                    double[] output = network.Compute(input);
                    writer.WriteLine(string.Format("{0},{1},{2},{3},{4}", input[0], input[1], input[2], output[0], CalculateOutput(input)));
                }
            }
        }

        private double[] GetInput()
        {
            int i1 = rand.Next(2);
            int i2 = rand.Next(2);
            int i3 = rand.Next(2);

            return new double[] { i1, i2, i3 };
        }

        private double CalculateOutput(double[] input)
        {
            return Convert.ToDouble(Convert.ToBoolean(input[0]) & Convert.ToBoolean(input[1]));
            
            int sum = 0;
            return input[0];
            foreach (int i in input)
                sum += i;
            return sum;
        }
    }
}

