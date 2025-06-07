using MongoDB.Driver;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital
{
    public partial class FormMainMenu : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        private readonly string externalFile = ConfigurationManager.AppSettings["ExternalFileForBulkActivites"];

        //Fields
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;

        public int test;
        

        //Constructor
        public FormMainMenu()
        {
            InitializeComponent();
            random = new Random();
            btn_GoToHomePage.Visible = false;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void FormMainMenu_Load(object sender, EventArgs e)
        {
          
            
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg , int wParam , int lParam);

        //Methods

        private Color SelectThemeColor()
            //Function Select One Random Color From The List Of Colors For The Theme
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while(tempIndex==index)
            {
               index = random.Next(ThemeColor.ColorList.Count);
            }

            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }


        private void ActivateButton(object btnSender)
            //1- Select random color for the theme
            //2- Changes the button backgroung color
            //3- changes the font color of the button 
            //4- changes the font size of the button
        {
            if(btnSender != null)
            {
                if(currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = Color.FromArgb(0, 150, 138);
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Imprint MT Shadow", 10.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelTitleBar.BackColor = color;
                    btn_GoToHomePage.Visible = true;
                }
            }
        }


        private void DisableButton()
        {
            foreach(Control prevoiusBtn in panelMenu.Controls)
            {
                if(prevoiusBtn.GetType() == typeof(Button))
                {
                    prevoiusBtn.BackColor = Color.FromArgb(51, 51, 76);
                    prevoiusBtn.ForeColor = Color.Ivory;
                    prevoiusBtn.Font = new System.Drawing.Font("Imprint MT Shadow", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                }
            }
        }

        

        private void OpenChildForm(Form childform, object btnSender)
        //Method to open forms after click on buttons
        {
            if(activeForm != null)
            {
                activeForm.Close();
            }

            ActivateButton(btnSender);
            activeForm = childform;
            childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock = DockStyle.Fill;
            this.panelDeskTopPanel.Controls.Add(childform);
            this.panelDeskTopPanel.Tag = childform;
            childform.BringToFront();
            childform.Show();
            lbl_Title.Text = childform.Text;

        }

        private void btn_PatientPage_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormPatient(), sender);

        }

        private void btn_MedicinePage_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormMedicine(), sender);
        }

        private void btn_PatientTreatments_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormTreatment(), sender);
        }

        private void btn_FilterPatients_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormFilterPatients(), sender);
        }
        private void btn_FilterMedicines_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormFilterMedicines(), sender);
        }

        private void btn_GoToHomePage_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
               activeForm.Close();
            Reset();
        }

        private void Reset()
        {
            DisableButton();
            lbl_Title.Text = "HOME";
            panelTitleBar.BackColor = Color.FromArgb(0, 150, 138);
            currentButton = null;
            btn_GoToHomePage.Visible = false;
        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btn_CloseWindow_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_MaximizeWindow_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btn_MinimizeWindow_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
