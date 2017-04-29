using System;
delegate double Fun( double x );
public class DelegateIntegral
{

	public static void Main()

	{

		Fun fun = new Fun(Math.Sin);
		double d = Integral( fun, 0, Math.PI / 2, 1e-4 );
		Console.WriteLine( d );

		Fun fun2 = new Fun( Linear );
		double d2 = Integral( fun2, 0, 2, 1e-3 );
		Console.WriteLine( d2 );

		Rnd rnd = new Rnd();
		double d3 = Integral( new Fun(rnd.Num), 0, 1, 0.01 );
		Console.WriteLine( d3 );
	}

	static double Linear( double a )
	{
		return a * 2 + 1;
	}

	class Rnd
	{
		Random r = new Random();
		public double Num( double x )
		{
			return r.NextDouble();
		}
	}

	static double Integral(Fun f, double a, double b, double eps)// 积分计算   eps 表示精度
	{
		int n, k;
		double fa, fb, h, t1, p, s, x, t = 0;

		fa = f(a);
		fb = f(b);

		// 迭代初值
		n = 1;
		h = b - a;   //积分区间长度
		t1 = h * (fa + fb) / 2.0;    //定积分的一个近似值，此值小于真实值
		p = double.MaxValue;     //最大的double值

		// 迭代计算
		while (p >= eps)
		{
			s = 0.0;   //此处事实上写死了要计算积分的函数的区间左端点函数值必为零。
			for (k = 0; k <= n - 1; k++)
			{
				x = a + (k + 0.5) * h;     //与相应步长对应的小区间中点x值
				s = s + f(x);                  //与相应步长对应的小区间两端，两端点函数值的和。
			}

			t = (t1 + h * s) / 2.0;
			p = Math.Abs(t1 - t);
			t1 = t;
			n = n + n;     //n变大为原值的2倍，1，2，4，8
			h = h / 2.0;   //h变小为原来的一半，1, 1/2, 1/4, 1/8
		}
		return t;
	}
}
