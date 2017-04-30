using System;

public delegate void DownloadStartHandler(object sender, DownloadStartEventArgs e);	//声明委托
public delegate void DownloadEndHandler(object sender, DownloadEndEventArgs e);
public delegate void DownloadingHandler(object sender, DownloadingEventArgs e);

public class DownloadStartEventArgs
{
	public string Url{get{return _url;} set{ _url = value;}}
	private string _url;
	public DownloadStartEventArgs(string url){this._url = url;}
}

public class DownloadEndEventArgs
{
	public string Url{get{return _url;} set{ _url = value;}}
	private string _url;

	public long ByteCount{ get { return _byteCount; } set { _byteCount = value;} }
	private long _byteCount;

	public DownloadEndEventArgs(string url, long size){ this._url = url; this._byteCount = size;}
}

public class DownloadingEventArgs
{
	public string Url{ get{ return _url; } set{ _url = value; } }
	private string _url;

	public double Percent { get { return _percent; } set { _percent = value; } }
	private double _percent;

	public DownloadingEventArgs(string url, double percent){ this._url = url; this._percent = percent; }
}

public class Crawler
{
	public event DownloadStartHandler DownloadStart;	//声明事件
	//以上声明定义了DownloadStart事件，而该事件只接受DownloadStartHandler这个delegate所定义的delegate（函数原型）类型的注册
	public event DownloadEndHandler DownloadEnd;	//声明事件
	public event DownloadingHandler Downloading;	//声明事件

	public string Name {get { return name; } set { name = value;}}
	private string name;
	private string site;

	public Crawler(string name, string site)
	{
		this.name = name;
		this.site = site;
	}

	public void Craw()
	{
		while (true)
		{
			string url = GetNextUrl();
			if( url == null ) break;
			long size = GetSizeOfUrl(url);

			if( DownloadStart != null)	//下载开始的事件发生
			{
				DownloadStart(this, new DownloadStartEventArgs(url));
			}

			for (long i = 0; i < size + 1024; i += 1024 )
			{
				//下载数据
				System.Threading.Thread.Sleep(100);
				double percent = (int)(i * 100.0 / size);

				if( Downloading != null )
				{
					Downloading( this, new DownloadingEventArgs(url, percent));
				}
			}

			if( DownloadEnd != null )
			{
				DownloadEnd( this, new DownloadEndEventArgs(url, size));
			}
		}
	}

	private string GetNextUrl()
	{
		int a = rnd.Next(10);
		if( a == 0) return null;
		return site + "/Page" + a + "html";
	}

	private long GetSizeOfUrl( string url)
	{
		return rnd.Next(3000 * url.Length);
	}

	private Random rnd = new Random();
}

class Test
{
	static void Main()
	{
		Crawler crawler = new Crawler("Crawler101", "http://www.pku.edu.cn");

		crawler.DownloadStart += new DownloadStartHandler(ShowStart);   //注册事件
		crawler.DownloadEnd += new DownloadEndHandler( ShowEnd);
		crawler.Downloading += new DownloadingHandler( ShowPercent);

		crawler.Craw();
	}


	static void ShowStart(object sender, DownloadStartEventArgs e)
	{
		Console.WriteLine((sender as Crawler).Name + " 开始下载 : " + e.Url);
	}

	static void ShowEnd(object sender, DownloadEndEventArgs e)
	{
		Console.WriteLine("\nr下载 " + e.Url + "结束， 下载了 " + e.ByteCount + "字节");
	}

	static void ShowPercent(object sender, DownloadingEventArgs e)
	{
		Console.WriteLine("r下载 " + e.Url + "..................." + e.Percent + " %");
	}
}



