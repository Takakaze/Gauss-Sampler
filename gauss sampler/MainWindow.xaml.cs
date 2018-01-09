using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace gauss_sampler
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool save = false;
        
        /// <summary>
        /// 主窗体程序的维持
        /// MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 服从高斯分布的采样处理
        /// Knuth Gauss Sampler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sample(object sender, RoutedEventArgs e)
        {
            double[] a = new double[12];
            double count;
            count = 0;
            Random rd = new Random();
            for (int i = 0; i < 12; i++)
            {
                a[i] = rd.NextDouble();
                rannum.Text += a[i].ToString();
                rannum.Text += "\n";
                count = count + a[i];
            }
            count = count - 6;
            result.Text += count.ToString();
            result.Text += "\n";
        }

        /// <summary>
        /// 多重采样
        /// multiple Sampler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void multiSample(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < Convert.ToInt32(num.Text); i++)
                {
                    Sample(sender, e);
                    System.Threading.Thread.Sleep(20);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR!", MessageBoxButton.OK);
            }
            
        }

        /// <summary>
        /// 保证多次采样的次数选择框中不存在任何其他输入
        /// make sure no other input into samplertimes textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KEYDOWN(object sender, KeyEventArgs e)
        {
            //屏蔽非法按键
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                e.Handled = false;
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 退出操作
        /// exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_File_Exit(object sender, RoutedEventArgs e)
        {
            object obj = this;
            if (save == false && rannum.Text != null)
            {
                MessageBoxResult mbr = MessageBox.Show("you have not saved the file, are you sure to exit?", "WARNING", MessageBoxButton.YesNoCancel);
                if (mbr == MessageBoxResult.Yes)
                {
                    MainWindow MW = (MainWindow)obj;
                    this.Close();
                }
                else if (mbr == MessageBoxResult.No)
                {
                    MenuItem_File_save(sender, e);
                    this.Close();
                }
                else if (mbr == MessageBoxResult.Cancel)
                {
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 关于
        /// about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_File_info(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Version 0.17.12.30.2022\n By Takakaze-Ai 2017 present \nAll right reserved", "ABOUT", MessageBoxButton.OK);
        }

        /// <summary>
        /// 保存高斯采样结果
        /// save gauss sample result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_File_save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = @"文本文档/text file|*.txt";
            if (sfd.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter wt = new StreamWriter(fs, new UTF8Encoding(false)))
                    {
                        wt.Write(result.Text);
                        wt.Close();
                        save = true;
                    }
                }
            }
        }

        /// <summary>
        /// 保存随机数
        /// save random number result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_File_saverad(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = @"文本文档/text file|*.txt";
            if (sfd.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter wt = new StreamWriter(fs, new UTF8Encoding(false)))
                    {
                        wt.Write(rannum.Text);
                        wt.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 随机数生成器
        /// random number creater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Random(object sender, RoutedEventArgs e)
        {
            Random rd = new Random();
            int i;
            double d;
            if (type.SelectedIndex == 0)          //INT 情况下  /  INT type
            {
                i = rd.Next(-65536, 65535);
                rannum.Text += i.ToString();
                rannum.Text += "\n";
            }
            else if (type.SelectedIndex == 1)       //DOUBLE情况下  /  DOUBLE type
            {
                i = rd.Next(-65536, 65535);
                d = rd.NextDouble();
                double R = i * d;
                rannum.Text += R.ToString();
                rannum.Text += "\n";
            }

        }
        
        /// <summary>
        /// 清除所有临时生成结果
        /// clear all temporary data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear(object sender, RoutedEventArgs e)
        {
            result.Text = null;
            rannum.Text = null;
            num.Text = null;
            save = false;
        }

        /// <summary>
        /// 更换背景图片
        /// change background image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BGI(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"图片文件|*.jpg;*.png;*.bmp";
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)

            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));
                imageBrush.Stretch = Stretch.Fill;                                                          //设置图像的显示格式  
                this.Background = imageBrush;
            }
        }
    }

    public class TypeComboBox
    {
        public string TYPE { set; get; }
        public int ID { set; get; }
    }

    public class Typeload : ObservableCollection<TypeComboBox>
    {
        public Typeload()
        {
            TypeComboBox Tcb1 = new TypeComboBox { ID = 3 ,TYPE = "INT"};
            TypeComboBox Tcb2 = new TypeComboBox { ID = 2 ,TYPE = "DOUBLE" };
            this.Add(Tcb1);
            this.Add(Tcb2);
        }
    }
}
