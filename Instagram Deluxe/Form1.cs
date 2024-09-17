using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Instagram_Deluxe
{
    public partial class Instagram : Form
    {
        List<User> users = new List<User>();
        int currentUser = -1;

        public Instagram()
        {
            InitializeComponent();
            appManager.allUser(users);
            appManager.follow(users);
        }

        //Autentification
        private void Lregister_button_Click(object sender, EventArgs e)
        {
            register_panel.Visible = true;
            logIn_panel.Visible = false;
        }

        private void RlogIn_button_Click(object sender, EventArgs e)
        {
            logIn_panel.Visible = true;
            register_panel.Visible = false;
        }

        private void Rregister_button_Click(object sender, EventArgs e)
        {
            User mainUser = new User(Rusername_textBox.Text, Rpassword_textBox.Text, RconfPassword_textBox.Text);
            users.Add(mainUser);
            Rusername_textBox.Text = "";
            Rpassword_textBox.Text = "";
            RconfPassword_textBox.Text = "";
            logIn_panel.Visible = true;
            register_panel.Visible = false;
        }

        private void LlogIn_button_Click(object sender, EventArgs e)
        {
            currentUser = User.LogIn(Lusername_textBox.Text, Lpassword_textBox.Text, users);
            logIn_panel.Visible = false;
            if (currentUser == -1)
            {
                logIn_panel.Visible = true;
                
            }
            else
            {
                autentification_panel.Visible = false;
                home_panel.Visible = true;
            }
            Lusername_textBox.Text = "";
            Lpassword_textBox.Text = "";
            appManager.init(users[currentUser], Ppicture_button, profPicture_button, profName_label, profFollowers_label, profFollowing_label);
            
            //users that follow you and you follow them
            List<User> friends = appManager.Friends(users[currentUser]);
            appManager.listPeople(friends, people_listView, userPicture_button, userName_label, userFollowers_label, userFollowing_label, home_panel, userPage_panel, users[currentUser], follow_button);
        }

        //Home
        private void messages_button_Click(object sender, EventArgs e)
        {
            messages_panel.Visible = true;
            home_panel.Visible = false;
            appManager.listPeople(users, search_listView, userPicture_button, userName_label, userFollowers_label, userFollowing_label, messages_panel, userPage_panel, users[currentUser], follow_button);
        }
        
        private void Ppicture_button_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            profPage_panel.Visible = true;
        }
        
        //Messages
        private void back_button_Click(object sender, EventArgs e)
        {
            messages_panel.Visible = false;
            home_panel.Visible = true;
        }

        //Your profile page
        private void addPost_button_Click(object sender, EventArgs e)
        {
            addPost_panel.Visible = true;
        }

        private void profBack_button_Click(object sender, EventArgs e)
        {
            profPage_panel.Visible = false;
            home_panel.Visible = true;
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            profPage_panel.Visible = false;
            autentification_panel.Visible = true;
            logIn_panel.Visible = true;
        }

        private void profPicture_button_Click(object sender, EventArgs e)
        {
            appManager.profilePicSwitch(users[currentUser]);
            appManager.init(users[currentUser], Ppicture_button, profPicture_button, profName_label, profFollowers_label, profFollowing_label);
            users[currentUser].Update();
        }


        //User profile page
        private void userBack_button_Click(object sender, EventArgs e)
        {
            userPage_panel.Visible = false;
            home_panel.Visible= true;

            //users that follow you and you follow them
            List<User> friends = appManager.Friends(users[currentUser]);
            appManager.listPeople(friends, people_listView, userPicture_button, userName_label, userFollowers_label, userFollowing_label, home_panel, userPage_panel, users[currentUser], follow_button);
        }

        private void follow_button_Click(object sender, EventArgs e)
        {
            appManager.Follow(users, currentUser, userName_label.Text, profFollowing_label, userFollowers_label, follow_button);
        }

        //Add Post
        private void addPostExit_button_Click(object sender, EventArgs e)
        {
            addPost_panel.Visible = false;
        }

        String path;

        private void submitPost_button_Click(object sender, EventArgs e)
        {
            if (path != null)
            {
                Post post = new Post(path, description_richTextBox.Text);

                if (users[currentUser].Posts == null)
                {
                    users[currentUser].Posts = new List<Post>();
                }

                users[currentUser].Posts.Add(post);
            }

            appManager.postsList(users[currentUser], urPosts_listView);

            addPost_panel.Visible = false;
            users[currentUser].Update();
        }

        private void image_button_Click(object sender, EventArgs e)
        {
            path = appManager.Path();
        }
    }
}
