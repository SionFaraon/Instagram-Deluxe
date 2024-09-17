using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Instagram_Deluxe
{
    internal class appManager
    {
        public static void init(User user, Button HprofilePic, Button PprofilePic, Label name, Label followers, Label following)
        {
            try
            {
                Image profileImage = Image.FromFile(user.Image);
                PprofilePic.BackgroundImage = profileImage;
                HprofilePic.BackgroundImage = profileImage;
                name.Text = user.UserName;
                followers.Text = user.Followers.Count.ToString();
                following.Text = user.Following.Count.ToString();

            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static void profilePicSwitch(User user)
        {
            try
            {
                OpenFileDialog newPicture = new OpenFileDialog();
                newPicture.InitialDirectory = "C:\\Users\\alins\\OneDrive\\Desktop";
                if(newPicture.ShowDialog() == DialogResult.OK)
                {
                    string path = newPicture.FileName;
                    user.Image = path;
                }
                else
                {
                    throw new ArgumentException("Imaginea nu s-a putut incarca!");
                }

            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static void allUser(List<User> users) {
            String[] files = Directory.GetFiles("C:\\Users\\alins\\OneDrive\\Desktop\\Programe\\Programe C#\\Forms\\Instagram Deluxe\\bin\\Debug\\Users", "*.txt");
            foreach(String file in files)
            {
                User tempuser = new User(file);
                users.Add(tempuser);
            }
        }

        public static void follow(List<User> users)
        {
            string fileDirectory = "C:\\Users\\alins\\OneDrive\\Desktop\\Programe\\Programe C#\\Forms\\Instagram Deluxe\\bin\\Debug\\Users\\";

            foreach (User user in users)
            {
                string filePath = $"{fileDirectory}{user.UserName}.txt";
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                
                string var;
                bool read = true;

                user.Followers = new List<User>();
                user.Following = new List<User>();

                sr.ReadLine();
                sr.ReadLine();

                while ((var = sr.ReadLine()) != ":Post:")
                {
                    if(var == ":Followers:")
                    {
                        read = true;
                        continue;
                    }
                    if (var == ":Following:")
                    {
                        read = false;
                        continue;
                    }

                    User existingUser = users.Find(u => u.UserName == var);
                    if (existingUser != null)
                    {
                        if (read)
                        {
                            user.Followers.Add(existingUser);
                        }
                        else
                        {
                            user.Following.Add(existingUser);
                        }
                    }
                }

                sr.Close();
                fs.Close();
            }
        }

        public static void listPeople(List<User> users, ListView list, Button profPic, Label name, Label followers, Label following, Panel messages, Panel page, User currentUsr, Button FlUfl)
        {
            try
            {
                list.Items.Clear();
                list.View = View.LargeIcon;

                ImageList imageList = new ImageList();
                imageList.ImageSize = new Size(50, 50);
                list.LargeImageList = imageList;
                list.HideSelection = true;

                foreach (User user in users)
                {
                    Image image = Image.FromFile(user.Image);
                    imageList.Images.Add(image);

                    ListViewItem item = new ListViewItem
                    {
                        Text = user.UserName,
                        ForeColor = Color.White,
                        ImageIndex = imageList.Images.Count - 1,
                        Tag = user
                    };

                    list.Items.Add(item);
                }

                list.ItemActivate += (sender, e) =>
                {
                    ListView listView = sender as ListView;
                    if(listView.SelectedItems.Count > 0) 
                    {
                        ListViewItem selectedItem = listView.SelectedItems[0];
                        User selectedUser = selectedItem.Tag as User;
                        messages.Visible = false;
                        page.Visible = true;
                        appManager.VisitPage(selectedUser, profPic, name, followers, following, currentUsr, FlUfl);
                    }
                };

            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static void VisitPage(User user, Button profilePic, Label name, Label followers, Label following, User currentUsr, Button FlUfl)
        {
            Image image = Image.FromFile(user.Image);
            profilePic.BackgroundImage = image;
            name.Text = user.UserName;
            followers.Text = user.Followers.Count.ToString();
            following.Text = user.Following.Count.ToString();
            if(user.Followers.Find(u => u.UserName == currentUsr.UserName) != null)
            {
                FlUfl.Text = "Unfollow";
            }
            else
            {
                FlUfl.Text = "Follow";
            }
        }

        public static void Follow(List<User> users, int user, String nameOf, Label following, Label follower, Button FlUfl)
        {
            int pageUserIndex = users.FindIndex(u => u.UserName == nameOf);
            if (FlUfl.Text == "Follow")
            {
                users[user].Following.Add(users[pageUserIndex]);
                users[pageUserIndex].Followers.Add(users[user]);
                FlUfl.Text = "Unfollow";
            }else if(FlUfl.Text == "Unfollow")
            {
                users[user].Following.Remove(users[pageUserIndex]);
                users[pageUserIndex].Followers.Remove(users[user]);
                FlUfl.Text = "Follow";
            }
            following.Text = users[user].Following.Count.ToString();
            follower.Text = users[pageUserIndex].Followers.Count.ToString();
            users[user].Update();
            users[pageUserIndex].Update();
        }

        public static List<User> Friends(User cUser)
        {
            List<User> friends = new List<User>();
            foreach (User user in cUser.Following)
            {
                if (user.Following.Contains(cUser))
                {
                    friends.Add(user);
                }
            }
            return friends;
        }

        public static String Path()
        {
            OpenFileDialog newImage = new OpenFileDialog();
            newImage.InitialDirectory = "C:\\Users\\alins\\OneDrive\\Desktop";
            if (newImage.ShowDialog() == DialogResult.OK)
            {
                return newImage.FileName;
            }
            return null;
        }

        public static void postsList(User user, ListView list)
        {
            try
            {
                list.Items.Clear();
                list.View = View.LargeIcon;

                ImageList imageList = new ImageList();
                imageList.ImageSize = new Size(175, 175);
                list.LargeImageList = imageList;
                list.HideSelection = true;

                foreach(Post post in user.Posts) 
                {
                    Image image = Image.FromFile(post.Image);
                    imageList.Images.Add(image);

                    ListViewItem item = new ListViewItem
                    {
                        Text = post.Description,
                        ForeColor = Color.White,
                        ImageIndex = imageList.Images.Count - 1,
                        Tag = post
                    };

                    list.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
