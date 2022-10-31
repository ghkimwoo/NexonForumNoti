using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using UI_Design.Class;
namespace UI_Design
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        private List<Label> menws;
        private List<Color> menw_colors;
        
        private List<GameInfo> gameInfos = new List<GameInfo>();
        private SQLiteConnection conn = null;
        private static Timer aTimer = new Timer();
        SQLiteControl sqlControl = new SQLiteControl();
        ApiControl apiControl = new ApiControl();
        private void Form1_Load(object sender, EventArgs e)
        {
            LogBox_Input("INFO", "프로그램이 시작되었습니다.");
            menws = new List<Label>();
            menws.Add(btn_Menu1);
            menws.Add(btn_Menu2);
            menws.Add(btn_Menu3);

            menw_colors = new List<Color>();
            menw_colors.Add(Color.FromArgb(53, 124, 225));
            menw_colors.Add(Color.DarkOrange);
            menw_colors.Add(Color.FromArgb(177, 70, 194));
            
            Tab_Main.SelectedIndex = 0;
            gameInfos = sqlControl.SQLiteSetup();
            foreach(var gameinfo in gameInfos)
            {
                delete_BoardId_Combobox.Items.Add(gameinfo.GameType);
                GameSelect_ComboBox.Items.Add(gameinfo.GameType);
            }
        }
       
        private void setMenuChange(int index)
        {
            if (Tab_Main.SelectedIndex != index)
            {
                menws[Tab_Main.SelectedIndex].ForeColor = Color.FromArgb(111, 111, 111);
                menws[index].ForeColor = menw_colors[index];
                Tab_Menu_Select_Bar.BackColor = menw_colors[index];
                Tab_Menu_Select_Bar.Location = new Point(menws[index].Location.X, 0);
                Tab_Main.SelectedIndex = index;
            }
        }
        private void btn_Menu1_Click(Object sender, EventArgs e)
        {
            setMenuChange(0);
        }
        private void btn_Menu2_Click(Object sender, EventArgs e)
        {
            setMenuChange(1);
        }
        private void btn_Menu3_Click(Object sender, EventArgs e)
        {
            setMenuChange(2);
        }
        private void btn_Start_Click(Object sender, EventArgs e)
        {
            SetTimer((int)Min_UpDown.Value);
            aTimer.Start();
            LogBox_Input("INFO", "정보 검색이 시작되었습니다.");
        }
        private void btn_Stop_Click(Object sender, EventArgs e)
        {
            aTimer.Stop();
            LogBox_Input("INFO", "정보 검색이 종료되었습니다.");
        }
        private static void SetTimer(int min)
        {
            aTimer.Interval = 300000;
            Form1 form1 = new Form1();
            aTimer.Tick += new EventHandler(form1.btn_Test_Click);
        }
        private void set_BoardId_Click(Object sender, EventArgs e)
        {
            if(add_BoardId != null)
            {
                int boardid = apiControl.getBoardId(add_BoardId.Text);
                var gameinfo = apiControl.getBoardList(new GameInfo() { BoardId = boardid, GameType = add_BoardId.Text });
                gameInfos.Add(gameinfo);
                LogBox_Input("INFO", "게시판이 추가되었습니다.");
            }
            if (delete_BoardId_Combobox.SelectedText != null) 
            {
                delete_BoardId_Combobox.Items.Remove(delete_BoardId_Combobox.SelectedText);
                gameInfos.Remove(gameInfos.Find(x => x.GameType == delete_BoardId_Combobox.SelectedText));
                sqlControl.SQLiteDelete(delete_BoardId_Combobox.SelectedText);
                LogBox_Input("INFO", "게시판이 삭제되었습니다.");
            }
                
        }
        
        void btn_Test_Click(Object sender, EventArgs e)
        {
            LogBox_Input("INFO", "내용을 불러옵니다.");
            string title = GameSelect_ComboBox.SelectedItem.ToString();
            foreach (var gameinfo in gameInfos)
            {
                if (gameinfo.GameType == title)
                {
                    if (gameinfo.BoardId == 0)
                        gameinfo.BoardId = apiControl.getBoardId(gameinfo.GameType);
                    GameInfo newGameInfo = apiControl.getBoardList(gameinfo);
                    if(newGameInfo.ThreadId > gameinfo.ThreadId)
                    {
                        new ToastContentBuilder()
                        .AddArgument("Action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText(WebUtility.HtmlDecode(newGameInfo.title))
                        .AddText(newGameInfo.GameType + " 게시판에 새로운 글이 올라왔습니다.")
                        .Show();
                        gameinfo.ThreadId = newGameInfo.ThreadId;
                        gameinfo.title = newGameInfo.title;
                        gameinfo.readCount = newGameInfo.readCount;
                        gameinfo.likeCount = newGameInfo.likeCount;
                        gameinfo.createTime = newGameInfo.createTime;
                        gameinfo.modifyTime = newGameInfo.modifyTime;
                        gameinfo.userName = newGameInfo.userName;
                    }
                }
            }
        }
        private void Form_Closed(Object sender, FormClosedEventArgs e)
        {
            MessageBox.Show("프로그램을 종료합니다.");
            sqlControl.SQLiteClose(gameInfos);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void LogBox_Input(string Status, string Message)
        {
            LogBox.AppendText(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : " + Status + " " + Message + "\n");
            LogBox.ScrollToCaret();
        }

        #region Windows Form 디자이너에서 생성한 코드

            /// <summary>
            /// 디자이너 지원에 필요한 메서드입니다. 
            /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
            /// </summary>
        private void InitializeComponent()
        {
            this.Tab_Back = new System.Windows.Forms.Panel();
            this.Tab_Menu_Back = new System.Windows.Forms.Panel();
            this.Tab_Menu_Select_Back = new System.Windows.Forms.Panel();
            this.Tab_Menu_Select_Bar = new System.Windows.Forms.Panel();
            this.btn_Menu1 = new System.Windows.Forms.Label();
            this.btn_Menu2 = new System.Windows.Forms.Label();
            this.btn_Menu3 = new System.Windows.Forms.Label();
            this.Tab_Main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.Min_UpDown = new System.Windows.Forms.NumericUpDown();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.Work_Start_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.GameSelect_ComboBox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.delete_BoardId_Combobox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.add_BoardId = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.Tab_Back.SuspendLayout();
            this.Tab_Menu_Back.SuspendLayout();
            this.Tab_Menu_Select_Back.SuspendLayout();
            this.Tab_Main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Min_UpDown)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tab_Back
            // 
            this.Tab_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.Tab_Back.Controls.Add(this.Tab_Menu_Back);
            this.Tab_Back.Controls.Add(this.Tab_Main);
            this.Tab_Back.Location = new System.Drawing.Point(0, 0);
            this.Tab_Back.Name = "Tab_Back";
            this.Tab_Back.Size = new System.Drawing.Size(798, 523);
            this.Tab_Back.TabIndex = 0;
            // 
            // Tab_Menu_Back
            // 
            this.Tab_Menu_Back.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab_Menu_Back.Controls.Add(this.Tab_Menu_Select_Back);
            this.Tab_Menu_Back.Controls.Add(this.btn_Menu1);
            this.Tab_Menu_Back.Controls.Add(this.btn_Menu2);
            this.Tab_Menu_Back.Controls.Add(this.btn_Menu3);
            this.Tab_Menu_Back.Location = new System.Drawing.Point(0, 0);
            this.Tab_Menu_Back.Name = "Tab_Menu_Back";
            this.Tab_Menu_Back.Size = new System.Drawing.Size(798, 40);
            this.Tab_Menu_Back.TabIndex = 0;
            // 
            // Tab_Menu_Select_Back
            // 
            this.Tab_Menu_Select_Back.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab_Menu_Select_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.Tab_Menu_Select_Back.Controls.Add(this.Tab_Menu_Select_Bar);
            this.Tab_Menu_Select_Back.Location = new System.Drawing.Point(0, 37);
            this.Tab_Menu_Select_Back.Name = "Tab_Menu_Select_Back";
            this.Tab_Menu_Select_Back.Size = new System.Drawing.Size(798, 3);
            this.Tab_Menu_Select_Back.TabIndex = 0;
            // 
            // Tab_Menu_Select_Bar
            // 
            this.Tab_Menu_Select_Bar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(124)))), ((int)(((byte)(225)))));
            this.Tab_Menu_Select_Bar.Location = new System.Drawing.Point(0, 0);
            this.Tab_Menu_Select_Bar.Name = "Tab_Menu_Select_Bar";
            this.Tab_Menu_Select_Bar.Size = new System.Drawing.Size(150, 3);
            this.Tab_Menu_Select_Bar.TabIndex = 0;
            // 
            // btn_Menu1
            // 
            this.btn_Menu1.Font = new System.Drawing.Font("Noto Sans KR", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Menu1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(124)))), ((int)(((byte)(225)))));
            this.btn_Menu1.Location = new System.Drawing.Point(0, 0);
            this.btn_Menu1.Name = "btn_Menu1";
            this.btn_Menu1.Size = new System.Drawing.Size(150, 40);
            this.btn_Menu1.TabIndex = 1;
            this.btn_Menu1.Text = "메인";
            this.btn_Menu1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_Menu1.Click += new System.EventHandler(this.btn_Menu1_Click);
            // 
            // btn_Menu2
            // 
            this.btn_Menu2.Font = new System.Drawing.Font("Noto Sans KR", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Menu2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.btn_Menu2.Location = new System.Drawing.Point(150, 0);
            this.btn_Menu2.Name = "btn_Menu2";
            this.btn_Menu2.Size = new System.Drawing.Size(150, 40);
            this.btn_Menu2.TabIndex = 2;
            this.btn_Menu2.Text = "설정";
            this.btn_Menu2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_Menu2.Click += new System.EventHandler(this.btn_Menu2_Click);
            // 
            // btn_Menu3
            // 
            this.btn_Menu3.Font = new System.Drawing.Font("Noto Sans KR", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Menu3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.btn_Menu3.Location = new System.Drawing.Point(300, 0);
            this.btn_Menu3.Name = "btn_Menu3";
            this.btn_Menu3.Size = new System.Drawing.Size(150, 40);
            this.btn_Menu3.TabIndex = 3;
            this.btn_Menu3.Text = "Log";
            this.btn_Menu3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_Menu3.Click += new System.EventHandler(this.btn_Menu3_Click);
            // 
            // Tab_Main
            // 
            this.Tab_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab_Main.Controls.Add(this.tabPage1);
            this.Tab_Main.Controls.Add(this.tabPage2);
            this.Tab_Main.Controls.Add(this.tabPage3);
            this.Tab_Main.Location = new System.Drawing.Point(0, 0);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.SelectedIndex = 0;
            this.Tab_Main.Size = new System.Drawing.Size(806, 509);
            this.Tab_Main.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.Min_UpDown);
            this.tabPage1.Controls.Add(this.btn_Stop);
            this.tabPage1.Controls.Add(this.Work_Start_Button);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.GameSelect_ComboBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(798, 483);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("넥슨Lv1고딕 Low OTF", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(392, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "2. 시간을 선택하세요(단위 : 분)";
            // 
            // Min_UpDown
            // 
            this.Min_UpDown.Location = new System.Drawing.Point(395, 72);
            this.Min_UpDown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.Min_UpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.Min_UpDown.Name = "Min_UpDown";
            this.Min_UpDown.Size = new System.Drawing.Size(44, 21);
            this.Min_UpDown.TabIndex = 4;
            this.Min_UpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(402, 187);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(226, 81);
            this.btn_Stop.TabIndex = 3;
            this.btn_Stop.Text = "중지";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // Work_Start_Button
            // 
            this.Work_Start_Button.Location = new System.Drawing.Point(109, 187);
            this.Work_Start_Button.Name = "Work_Start_Button";
            this.Work_Start_Button.Size = new System.Drawing.Size(226, 81);
            this.Work_Start_Button.TabIndex = 2;
            this.Work_Start_Button.Text = "시작";
            this.Work_Start_Button.UseVisualStyleBackColor = true;
            this.Work_Start_Button.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("넥슨Lv1고딕 Low OTF", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(126, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "1. 게임을 선택하세요";
            // 
            // GameSelect_ComboBox
            // 
            this.GameSelect_ComboBox.FormattingEnabled = true;
            this.GameSelect_ComboBox.Location = new System.Drawing.Point(129, 73);
            this.GameSelect_ComboBox.Name = "GameSelect_ComboBox";
            this.GameSelect_ComboBox.Size = new System.Drawing.Size(121, 20);
            this.GameSelect_ComboBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.delete_BoardId_Combobox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.add_BoardId);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(798, 483);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // delete_BoardId_Combobox
            // 
            this.delete_BoardId_Combobox.FormattingEnabled = true;
            this.delete_BoardId_Combobox.Location = new System.Drawing.Point(387, 93);
            this.delete_BoardId_Combobox.Name = "delete_BoardId_Combobox";
            this.delete_BoardId_Combobox.Size = new System.Drawing.Size(121, 20);
            this.delete_BoardId_Combobox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("넥슨Lv1고딕 Low OTF", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(384, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(236, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "삭제할 게시판 이름을 선택하세요.";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(255, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(226, 75);
            this.button1.TabIndex = 2;
            this.button1.Text = "저장하기";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("넥슨Lv1고딕 Low OTF", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(64, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(236, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "등록할 게시판 이름을 입력하세요.";
            // 
            // add_BoardId
            // 
            this.add_BoardId.Location = new System.Drawing.Point(67, 93);
            this.add_BoardId.Name = "add_BoardId";
            this.add_BoardId.Size = new System.Drawing.Size(100, 21);
            this.add_BoardId.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.LogBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(798, 483);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(41, 45);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(676, 302);
            this.LogBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.Tab_Back);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Tab_Back.ResumeLayout(false);
            this.Tab_Menu_Back.ResumeLayout(false);
            this.Tab_Menu_Select_Back.ResumeLayout(false);
            this.Tab_Main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Min_UpDown)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Tab_Back;
        private System.Windows.Forms.Panel Tab_Menu_Back;
        private System.Windows.Forms.Panel Tab_Menu_Select_Back;
        private System.Windows.Forms.Panel Tab_Menu_Select_Bar;
        private System.Windows.Forms.Label btn_Menu1;
        private System.Windows.Forms.Label btn_Menu2;
        private System.Windows.Forms.Label btn_Menu3;
        private System.Windows.Forms.TabControl Tab_Main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private ComboBox GameSelect_ComboBox;
        private Button Work_Start_Button;
        private Label label1;
        private Button btn_Stop;
        private Label label2;
        private NumericUpDown Min_UpDown;
        private Button button1;
        private Label label3;
        private TextBox add_BoardId;
        private ComboBox delete_BoardId_Combobox;
        private Label label4;
        private TextBox LogBox;
    }
}

