using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Instagram_Deluxe
{
    internal class User
    {
        public String Image {  get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following {  get; set; }
        public List<Post>  Posts { get; set; }

        public User() 
        {
            Image = null;
            UserName = null;
            Password = null;
            Followers = new List<User>();
            Following = new List<User>();
            Posts = new List<Post>();
        }

        public User(String file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                String var = sr.ReadLine();
                this.Image = var.Split('|')[0];
                this.UserName = var.Split('|')[1];
                this.Password = var.Split('|')[2];
                sr.Close();
                fs.Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Error: " + ex.Message);
            } 
        }

        public User(String username, String password ,String confPassword)
        {
            try
            {
                if(username == "" || password == "" || confPassword == "") { 
                    throw new ArgumentException("Campuri necompletate!");
                }
                else
                {
                    String[] files = Directory.GetFiles("C:\\Users\\alins\\OneDrive\\Desktop\\Programe\\Programe C#\\Forms\\Instagram Deluxe\\bin\\Debug\\Users", "*.txt");
                    foreach(String file in files)
                    {
                        if (file.Split('\\')[12].Split('.')[1] == username)
                        {
                            throw new ArgumentException("Usernameul " + username + " exista deja!");
                        }
                    }

                    if(password != confPassword)
                    {
                        throw new ArgumentException("Confirmare Esuata");
                    }
                    else
                    {
                        this.Image = "C:\\Users\\alins\\OneDrive\\Desktop\\Programe\\Programe C#\\Forms\\Instagram Deluxe\\bin\\Debug\\Users\\profilePics\\enericProfilePic.jpg";
                        this.UserName = username;
                        this.Password = password;
                        this.Followers = new List<User>();
                        this.Following = new List<User>();

                        FileStream fs = new FileStream($"C:\\Users\\alins\\OneDrive\\Desktop\\Programe\\Programe C#\\Forms\\Instagram Deluxe\\bin\\Debug\\Users\\{username}.txt", FileMode.Create, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(fs);
                        writer.WriteLine($"{this.Image}|{this.UserName}|{this.Password}");
                        writer.WriteLine(":Followers:");
                        writer.WriteLine(":Following:");
                        writer.WriteLine(":Post:");
                        writer.Close();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static int LogIn(String username, String password, List<User> allUsers)
        {
            try
            {
                for(int i = 0; i < allUsers.Count; i++)
                {
                    if (allUsers[i].UserName == username)
                    {
                        if (allUsers[i].Password == password)
                        {
                            return i;
                        }
                        else
                        {
                            throw new ArgumentException("Parola incorecta!");
                        }
                    }
                }
                throw new ArgumentException($"Usernameul {username} NU exista!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return -1;
            }
        }


        public void Update()
        {
            try
            {
                String[] files = Directory.GetFiles("C:\\Users\\alins\\OneDrive\\Desktop\\Programe\\Programe C#\\Forms\\Instagram Deluxe\\bin\\Debug\\Users", "*.txt");
                foreach( String file in files)
                {
                    if (file.Split('\\')[12].Split('.')[0] == this.UserName)
                    {
                        FileStream sw = new FileStream(file, FileMode.Truncate, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(sw);
                        writer.WriteLine($"{this.Image}|{this.UserName}|{this.Password}");
                        writer.WriteLine(":Followers:");
                        foreach ( User user in this.Followers)
                        {
                            writer.WriteLine(user.UserName);
                        }
                        writer.WriteLine(":Following:");
                        foreach ( User user in this.Following)
                        {
                            writer.WriteLine(user.UserName);
                        }
                        writer.WriteLine(":Post:");
                        foreach(Post post in this.Posts)
                        {
                            writer.WriteLine($"{post.Image}|{post.Description}|{post.Date}|{post.Likes}");
                        }
                        writer.Close();
                        sw.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
