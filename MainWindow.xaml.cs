using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace quiz
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		private Root rt;
		int LeftCorrect, RightCorrect; // 当前问题的正确答案的选项，从1开始
		int LeftQuestionIndex, RightQuestionIndex; // 当前问题在json文件里的索引
		int LeftCount, RightCount; // 对的题目计数

		public MainWindow()
		{
			InitializeComponent();
			rt = JsonConvert.DeserializeObject<Root>(File.ReadAllText("data.json"));
			Refresh(1);
			Refresh(2);
		}

		/// <summary>
		/// 刷新题目
		/// </summary>
		/// <param name="Blank">左边为1，右边为2</param>
		void Refresh(int Blank)
		{
			InitQuestionIndex:
			int QuestionIndex = Ran(rt.Question.Count + 1);
            if (QuestionIndex < 0)
            {
				goto InitQuestionIndex;
            }

			string Question = rt.Question[QuestionIndex];
			string Correct = rt.Correct[QuestionIndex];
			string Wrong = rt.Wrong[QuestionIndex];

			try
			{
				string Wrong1 = Wrong.Substring(0, Wrong.IndexOf(";"));
				Wrong = Wrong.Substring(Wrong.IndexOf(";") + 1);
				string Wrong2 = Wrong.Substring(0, Wrong.IndexOf(";"));
				Wrong = Wrong.Substring(Wrong.IndexOf(";") + 1);
				string Wrong3 = Wrong.Substring(0, Wrong.IndexOf(";"));
				Wrong = Wrong.Substring(Wrong.IndexOf(";") + 1);

				int CorrectPosition, Wrong1Position, Wrong2Position, Wrong3Position;
				CorrectPosition = Ran(1, 5);
				for (Wrong1Position = Ran(1, 5); (Wrong1Position == CorrectPosition);)
				{
					Wrong1Position++;
					if (Wrong1Position > 4) Wrong1Position = 1;

				}
				for (Wrong2Position = Ran(1, 5); (Wrong2Position == CorrectPosition || Wrong2Position == Wrong1Position);)
				{
					Wrong2Position++;
					if (Wrong2Position > 4) Wrong2Position = 1;

				}
				for (Wrong3Position = Ran(1, 5); (Wrong3Position == CorrectPosition || Wrong3Position == Wrong1Position || Wrong3Position == Wrong2Position);)
				{
					Wrong3Position++;
					if (Wrong3Position > 4) Wrong3Position = 1;

				}

				/*switch (CorrectPosition)
				{
					case 1:
						Wrong1Position = 2;
						Wrong2Position = 3;
						Wrong3Position = 4;
						break;
					case 2:
						break;
					default: break;
				}*/

				if (Blank == 1) //左边的
				{
					Q1.Text = Question;
					LeftCorrect = CorrectPosition;
					LeftQuestionIndex = QuestionIndex;
					switch (CorrectPosition)
					{
						case 1: A1.Content = Correct; break;
						case 2: B1.Content = Correct; break;
						case 3: C1.Content = Correct; break;
						case 4: D1.Content = Correct; break;
						default: goto InitQuestionIndex;
					}
					switch (Wrong1Position)
					{
						case 1: A1.Content = Wrong1; break;
						case 2: B1.Content = Wrong1; break;
						case 3: C1.Content = Wrong1; break;
						case 4: D1.Content = Wrong1; break;
						default: goto InitQuestionIndex;
					}
					switch (Wrong2Position)
					{
						case 1: A1.Content = Wrong2; break;
						case 2: B1.Content = Wrong2; break;
						case 3: C1.Content = Wrong2; break;
						case 4: D1.Content = Wrong2; break;
						default: goto InitQuestionIndex;
					}
					switch (Wrong3Position)
					{
						case 1: A1.Content = Wrong3; break;
						case 2: B1.Content = Wrong3; break;
						case 3: C1.Content = Wrong3; break;
						case 4: D1.Content = Wrong3; break;
						default: goto InitQuestionIndex;
					}
				}
				else //右边的
				{
					Q2.Text = Question;
					RightCorrect = CorrectPosition;
					RightQuestionIndex = QuestionIndex;
					switch (CorrectPosition)
					{
						case 1: A2.Content = Correct; break;
						case 2: B2.Content = Correct; break;
						case 3: C2.Content = Correct; break;
						case 4: D2.Content = Correct; break;
						default: goto InitQuestionIndex;
					}
					switch (Wrong1Position)
					{
						case 1: A2.Content = Wrong1; break;
						case 2: B2.Content = Wrong1; break;
						case 3: C2.Content = Wrong1; break;
						case 4: D2.Content = Wrong1; break;
						default: goto InitQuestionIndex;
					}
					switch (Wrong2Position)
					{
						case 1: A2.Content = Wrong2; break;
						case 2: B2.Content = Wrong2; break;
						case 3: C2.Content = Wrong2; break;
						case 4: D2.Content = Wrong2; break;
						default: goto InitQuestionIndex;
					}
					switch (Wrong3Position)
					{
						case 1: A2.Content = Wrong3; break;
						case 2: B2.Content = Wrong3; break;
						case 3: C2.Content = Wrong3; break;
						case 4: D2.Content = Wrong3; break;
						default: goto InitQuestionIndex;
					}

				}

			}
			catch
			{
				MessageBox.Show("json语法错误！", null, MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		#region 随机数
		/// <summary>
		/// 生成并返回随机数
		/// </summary>
		/// <param name="Stop">从0到多少，不含</param>
		/// <returns>返回生成的随机数，从0开始</returns>
		int Ran(int Stop)
		{
			Random rd = new Random();
			return RealRan() % (Stop - 1);
		}

		/// <summary>
		/// 生成并返回随机数
		/// </summary>
		/// <param name="Start">从多少开始，含</param>
		/// <param name="Stop">到多少结束，不含</param>
		/// <returns></returns>
		int Ran(int Start, int Stop)
		{
			Random rd = new Random();
			return RealRan() % (Stop - Start - 1) + Start;
		}

		int RealRan()
		{
			byte[] randomBytes = new byte[4];
			RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
			rngServiceProvider.GetBytes(randomBytes);
			Int32 result = BitConverter.ToInt32(randomBytes, 0);
			return result;
		}
		#endregion

		#region  判断左边
		private void A1_Click(object sender, RoutedEventArgs e)
		{
			if (A1.Content == rt.Correct[LeftQuestionIndex])
			{
				LeftCount++;
				LeftBox.Text = LeftCount.ToString();
			}
			Refresh(1);
		}
		private void B1_Click(object sender, RoutedEventArgs e)
		{
			if (B1.Content == rt.Correct[LeftQuestionIndex])
			{
				LeftCount++;
				LeftBox.Text = LeftCount.ToString();
			}
			Refresh(1);
		}
		private void C1_Click(object sender, RoutedEventArgs e)
		{
			if (C1.Content == rt.Correct[LeftQuestionIndex])
			{
				LeftCount++;
				LeftBox.Text = LeftCount.ToString();
			}
			Refresh(1);
		}
		private void D1_Click(object sender, RoutedEventArgs e)
		{
			if (D1.Content == rt.Correct[LeftQuestionIndex])
			{
				LeftCount++;
				LeftBox.Text = LeftCount.ToString();
			}
			Refresh(1);
		}
		#endregion

		#region  判断右边
		private void A2_Click(object sender, RoutedEventArgs e)
		{
			if (A2.Content == rt.Correct[RightQuestionIndex])
			{
				RightCount++;
				RightBox.Text = RightCount.ToString();
			}
			Refresh(2);
		}
		private void B2_Click(object sender, RoutedEventArgs e)
		{
			if (B2.Content == rt.Correct[RightQuestionIndex])
			{
				RightCount++;
				RightBox.Text = RightCount.ToString();
			}
			Refresh(2);
		}
		private void C2_Click(object sender, RoutedEventArgs e)
		{
			if (C2.Content == rt.Correct[RightQuestionIndex])
			{
				RightCount++;
				RightBox.Text = RightCount.ToString();
			}
			Refresh(2);
		}
		private void D2_Click(object sender, RoutedEventArgs e)
		{
			if (D2.Content == rt.Correct[RightQuestionIndex])
			{
				RightCount++;
				RightBox.Text = RightCount.ToString();
			}
			Refresh(2);
		}
		#endregion


	}

	public class Root
	{
		/// <summary>
		/// 问题
		/// </summary>
		public List<string> Question { get; set; }
		/// <summary>
		/// 正确答案
		/// </summary>
		public List<string> Correct { get; set; }
		/// <summary>
		/// 错误答案
		/// </summary>
		public List<string> Wrong { get; set; }
	}

}
