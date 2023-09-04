using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace _01BASIC
{
    public partial class winform : Form
    {
        private SerialPort serialPort = new SerialPort();
        public winform()
        {
            InitializeComponent();
        }

        private void PortNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("helloworld");
            Console.WriteLine("sender : " + sender);
            Console.WriteLine("EventArgs : " + e);
            ComboBox cd = (ComboBox)sender;
            Console.WriteLine("Selectedindex : " + cd.SelectedIndex);
            Console.WriteLine("Selected Value : " + cd.Items[cd.SelectedIndex]);


        }
        private void SerialPort_DataRacv(object sender, SerialDataReceivedEventArgs e)
        {
            String recvData = this.serialPort.ReadLine();

            //Invoke()
            //스레드 생성 코드
            //Invoke(new Action(() => { 처리로직 }));

            // LED 점등 스레드
            if (recvData.StartsWith("LED:")) { 
            Invoke(new Action(() => { Console.WriteLine(recvData); this.textArea.AppendText(recvData + "\r\n"); }));
            }
            // 온도 확인 스레드
            if (recvData.StartsWith("TEMP :")) {
                Invoke(new Action(() => { this.TEMP_BOX.Text = ""; TEMP_BOX.Text = recvData.Replace("TEMP : ", ""); })) ;
            }
            if (recvData.StartsWith("PH :"))
            {
                Invoke(new Action(() => { this.JODO_BOX.Text = ""; JODO_BOX.Text = recvData.Replace("PH : ", ""); }));
            }
            if (recvData.StartsWith("UW :"))
            {
                Invoke(new Action(() => { this.UW_BOX.Text = ""; UW_BOX.Text = recvData.Replace("UW : ", ""); }));
            }
        }

        private void conn_btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Conn_btn Click! : "+ this.PortNumber.Items[this.PortNumber.SelectedIndex].ToString());
            try { 
            this.serialPort.PortName = this.PortNumber.Items[this.PortNumber.SelectedIndex].ToString();
            this.serialPort.BaudRate = 9600;
            this.serialPort.DataBits = 8;
            this.serialPort.StopBits = System.IO.Ports.StopBits.One;
            this.serialPort.Parity = System.IO.Ports.Parity.None;
            this.serialPort.Open();
            Console.WriteLine("CONNECTION SUCCESS");
            this.textArea.AppendText("Connected...\r\n");
            this.serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataRacv);

            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                this.serialPort.Close();
                this.textArea.AppendText("Conn Fail..."+ex+"\r\n");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort.Write("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort.Write("0");
        }
    }
}
