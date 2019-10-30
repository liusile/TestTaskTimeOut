using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTaskTimeOut
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 模拟上传(超时)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
                //设置超时1秒，上传耗时3秒
                var result = TaskTimeout.WaitAsync(ct => UploadAsync(ct),
                   TimeSpan.FromMilliseconds(1000), CancellationToken.None).Result;
                MessageBox.Show($"上传返回结果：{result}");
            }
            catch(AggregateException ex)
            {
                if (ex.InnerException is TimeoutException)
                    MessageBox.Show($"上传超时啦....");
                else
                    MessageBox.Show(ex.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private Task<string> UploadAsync(CancellationToken ct)
        {

            return Task.Run(() => {
                Thread.Sleep(3000);
                return "我是上传返回的结果呀....";
            }, ct).ContinueWith(t => t.Result, ct);
        }
        /// <summary>
        /// 模拟上传(不超时)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //设置超时4秒，上传耗时3秒
                var result = TaskTimeout.WaitAsync(ct => UploadAsync(ct),
                   TimeSpan.FromMilliseconds(4000), CancellationToken.None).Result;
                MessageBox.Show($"上传返回结果：{result}");
            }
            catch
            {
                MessageBox.Show($"上传超时啦....");
            }
        }
    }
}
