using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace GameServer_FightTheLandlord
{
	public class UserDo
	{
		public string Name { get; private set; }
		public string Path { get; private set; }

		//读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
		static ReaderWriterLockSlim ReadWriteLock;

		public UserDo()
		{
			Name = "UserData";
			Path = "C:///fallen//" + Name + ".txt";
			ReadWriteLock = new ReaderWriterLockSlim();
		}

		public string Read()//读取用户数据UserData
		{
			try {
				FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
				fs.Close();
				FileInfo file = new FileInfo(Path);
				var reader = file.OpenText();
				string listJson = reader.ReadToEnd();
				reader.Close();
				return listJson;

			}
			catch (Exception e) { Console.WriteLine(e.ToString());return e.ToString(); }
		}
	
        public void Save(string listJson)//保存用户数据
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
                //注意：长时间持有读线程锁或写线程锁会使其他线程发生饥饿 (starve)。 为了得到最好的性能，需要考虑重新构造应用程序以将写访问的持续时间减少到最小。
                //从性能方面考虑，请求进入写入模式应该紧跟文件操作之前，在此处进入写入模式仅是为了降低代码复杂度
                //因进入与退出写入模式应在同一个try finally语句块内，所以在请求进入写入模式之前不能触发异常，否则释放次数大于请求次数将会触发异常
                //ReadWriteLock.EnterReadLock();
                FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Close();
                FileInfo file = new FileInfo(Path);
                var writer = file.CreateText();
                writer.WriteLine(listJson);
                writer.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            //finally
            //{
            //    //退出写入模式，释放资源占用
            //    //注意：一次请求对应一次释放
            //    //若释放次数大于请求次数将会触发异常[写入锁定未经保持即被释放]
            //    //若请求处理完成后未释放将会触发异常[此模式不下允许以递归方式获取写入锁定]
            //    //ReadWriteLock.ExitWriteLock();
            //}
        }
    

    public void Insert(string[] names, string[] values)
		{
			string listJson = Read();
			if (listJson == "")
			{
				listJson += "UserData";
				Save(listJson);
				/*
				listJson += "Id=1,";
				listJson += "IsDeleted=" + false + ",";
				listJson += names [0] + "=" + values [0] + ",";
				listJson += names [1] + "=" + values [1] + ",";
				listJson += names [2] + "=" + values [2] + ",";
				listJson += names [3] + "=" + values [3] + ",";
				listJson += names [4] + "=" + values [4] + ",";
				listJson += names [5] + "=" + values [5] + ",";
				listJson += names [6] + "=" + values [6] + ",";
				listJson += names [7] + "=" + values [7];
				DataTool.Save (listJson);
				*/
			}
			else //添加一个用户
			{
				listJson += "&Id=" + (GetList().Count + 1) + ",";
				listJson += "IsDeleted=" + false + ",";
				listJson += names[0] + "=" + values[0] + ",";
				listJson += names[1] + "=" + values[1] + ",";
				listJson += names[2] + "=" + values[2] + ",";
				listJson += names[3] + "=" + values[3] + ",";
				listJson += names[4] + "=" + values[4] + ",";
				listJson += names[5] + "=" + values[5] + ",";
				listJson += names[6] + "=" + values[6] + ",";
				listJson += names[7] + "=" + values[7];
				Save(listJson);
			}
		}

		public void Delete(int id)  //按ID删除一个用户
		{
			var list = GetList();
			if (list == null || list.Count <= 0)
			{
				return;
			}
			var user = list.Where(u => u.Id == id).FirstOrDefault();
			if (user != null)
			{
				list.Remove(user);
				string listJson = ToListJson(list);
				Save(listJson);
			}
		}

		public void Update(int id, string name, string value)
		{
			var list = GetList();
			if (list == null || list.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				var item = list[i];
				if (item.Id == id)
				{
					if (name == "IsDeleted")
					{
						item.IsDeleted = Convert.ToBoolean(value);
					}
					else if (name == "Name")
					{
						item.Name = value;
					}
					else if (name == "Pwd")
					{
						item.Pwd = value;
					}
					else if (name == "QQEmail")
					{
						item.QQEmail = value;
					}
					else if (name == "Type")
					{
						item.Type = int.Parse(value);
					}
					else if (name == "Gold")
					{
						item.Gold = int.Parse(value);
					}
					else if (name == "RegisterTime")
					{
						item.RegisterTime = value;
					}
					else if (name == "EndLoginTime")
					{
						item.EndLoginTime = value;
					}
					else if (name == "UpdateTime")
					{
						item.UpdateTime = value;
					}
				}
			}
			string listJson = ToListJson(list);
			Save(listJson);
		}

		public string ToListJson(List<UserData> list)
		{
			string listJson = "UserData";
			if (list == null || list.Count <= 0)
			{
				return listJson;
			}
			for (int i = 0; i < list.Count; i++)
			{
				listJson += "&Id=" + list[i].Id + ",";
				listJson += "IsDeleted=" + list[i].IsDeleted + ",";
				listJson += "Name=" + list[i].Name + ",";
				listJson += "Pwd=" + list[i].Pwd + ",";
				listJson += "QQEmail=" + list[i].QQEmail + ",";
				listJson += "Type=" + list[i].Type + ",";
				listJson += "Gold=" + list[i].Gold + ",";
				listJson += "RegisterTime=" + list[i].RegisterTime + ",";
				listJson += "EndLoginTime=" + list[i].EndLoginTime + ",";
				listJson += "UpdateTime=" + list[i].UpdateTime;
			}
			return listJson;
		}

		public List<UserData> GetList()
		{
			List<UserData> list = new List<UserData>();
			string listJson = Read();
			if (listJson == "")
			{
				return list;
			}
			if (!listJson.Contains("&"))
			{
				Console.WriteLine(listJson);
				/*
				var attrs = listJson.Split (',');
				UserData user = new UserData ();
				user.Id = int.Parse (attrs [0].Split ('=') [1]);
				user.IsDeleted = Convert.ToBoolean (attrs [1].Split ('=') [1]);
				user.Name = attrs [2].Split ('=') [1];
				user.Pwd = attrs [3].Split ('=') [1];
				user.QQEmail = attrs [4].Split ('=') [1];
				user.Type = int.Parse (attrs [5].Split ('=') [1]);
				user.Gold = int.Parse (attrs [6].Split ('=') [1]);
				user.RegisterTime = attrs [7].Split ('=') [1];
				user.EndLoginTime = attrs [8].Split ('=') [1];
				user.UpdateTime = attrs [9].Split ('=') [1];
				list.Add (user);
				*/
			}
			else
			{
				var jsons = listJson.Split('&');
				for (int i = 1; i < jsons.Length; i++)
				{
					var attrs = jsons[i].Split(',');
					UserData user = new UserData();
					user.Id = int.Parse(attrs[0].Split('=')[1]);
					user.IsDeleted = Convert.ToBoolean(attrs[1].Split('=')[1]);
					user.Name = attrs[2].Split('=')[1];
					user.Pwd = attrs[3].Split('=')[1];
					user.QQEmail = attrs[4].Split('=')[1];
					user.Type = int.Parse(attrs[5].Split('=')[1]);
					user.Gold = int.Parse(attrs[6].Split('=')[1]);
					user.RegisterTime = attrs[7].Split('=')[1];
					user.EndLoginTime = attrs[8].Split('=')[1];
					user.UpdateTime = attrs[9].Split('=')[1];
					list.Add(user);
				}
			}
			return list;
		}


		public int Login(string name)
		{
			var user = GetList().Where(u => u.Name == name).FirstOrDefault();
			if (user != null)
			{
				return user.Id;
			}
			else
			{
				return 0;
			}
		}

		public int Register(string name, string pwd)
		{
			var user = GetList().Where(u => u.Name == name).FirstOrDefault();//查询是否已有同名用户 为空则无
			if (user == null)
			{
				try
				{
					Insert(
						new string[] { "Name", "Pwd", "QQEmail", "Type", "Gold", "RegisterTime", "EndLoginTime", "UpdateTime" },
						new string[] { name, pwd, "", "0", "0", DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString() });
					return 1;
				}
				catch (Exception)
				{
					return -1;
				}
			}
			return -2;
		}

		public int Register(string name, string pwd,int Score)
		{
			var user = GetList().Where(u => u.Name == name).FirstOrDefault();//查询是否已有同名用户 为空则无
			if (user == null)
			{
				try
				{
					Insert(
						new string[] { "Name", "Pwd", "QQEmail", "Type", "Gold", "RegisterTime", "EndLoginTime", "UpdateTime" },
						new string[] { name, pwd, "", "0", Score.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString() });
					return 1;
				}
				catch (Exception)
				{
					return -1;
				}
			}
			return -2;
		}

		public string GetUserInfo(int id, string name)//获取用户信息
		{
			string value = "";
			var list = GetList();
			if (list == null || list.Count <= 0)
			{
				return value;
			}
			for (int i = 0; i < list.Count; i++)
			{
				var item = list[i];
				if (item.Id == id)
				{
					if (name == "IsDeleted")
					{
						value = item.IsDeleted.ToString();
					}
					else if (name == "Name")
					{
						value = item.Name;
					}
					else if (name == "Pwd")
					{
						value = item.Pwd;
					}
					else if (name == "QQEmail")
					{
						value = item.QQEmail;
					}
					else if (name == "Type")
					{
						value = item.Type.ToString();
					}
					else if (name == "Gold")
					{
						value = item.Gold.ToString();
					}
					else if (name == "RegisterTime")
					{
						value = item.RegisterTime;
					}
					else if (name == "EndLoginTime")
					{
						value = item.EndLoginTime;
					}
					else if (name == "UpdateTime")
					{
						value = item.UpdateTime;
					}
				}
			}
			return value;
		}

		public string GetUserInfo(string username, string itemname)
		{
			int userId = Login(username);
			return GetUserInfo(userId,  itemname);
		}
		public void SetUserInfo(int id, string name, string value)
		{
			Update(id, name, value);
		}


		public void SetUserInfo(string username, string itemname, string itemvalue)
		{
			int userId = Login(username);
			Update(userId, itemname, itemvalue);
		}

		public void SendQQEmail(string receiveQQEmail)
		{
			try
			{
				//SmtpClient client = new SmtpClient ("smtp.qq.com",3306);
				//client.Send ("757721728@qq.com", receiveQQEmail, "用户登录的请求结果", new Random ().Next (1000, 10000).ToString());
				//client.Credentials = (ICredentialsByHost)new NetworkCredential ("757721728@qq.com", "");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}

