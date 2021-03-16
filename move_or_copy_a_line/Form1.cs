using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
/*
English Languege
The purpose of the program
Minimum spanning tree Kruskal algorithm and Minimum spanning tree Premium algorithm
It is to be able to comprehend better as their application.
+ - + - ++ - + - + - + - + - GRAPHICATION LOGIC + - + - + - + - + - + - + - + - +
Please Read The Instructions That Are Written Here Before Running The Program ...
• When the program starts, you can add a node by clicking picturebox1 at the bottom left.
• How many nodes you will create, you must first create the nodes and then click the create matrix button.
• Please click the create matrix button after adding as many nodes as the number of nodes you will add.
• When the create matrix button is clicked, a matrix is created on the tabpage1 page of the tabcontrol object on the right.
• If you press the left click of the mouse on the buttons in the matrix, the connection between the nodes is established and the cost between the nodes increases one by one with each click, and you will see that it increases faster.
• If you press the mouse's right click, the length-cost between nodes decreases. it decreases faster if you hold it down.
• By pressing the buttons on the matrix or pressing a node and pressing another node, you establish a connection between nodes.
• The number of nodes is limited to 12 as the matrix can fit as much as the size of the tabepage1 object.
• If you created the graf, you can select one of the Kruskal or Prim's algorithms and press the run button.
• When you click the Run button, you can follow through which nodes the algorithm has passed and its total loss from the cost list section.
• If the graph structure you created is wrong or you want to create a new graph
  You can click the create a new graph button in the lower left corner.
• If you click on the checkbox1 object whose text name is 'grid' in the lower left corner, you will change the background of the picturebox1.
• When you run any of the kruskal or premium algorithms, color changes occur in the graph structure, and the refresh button is used to restore the color changes.
• Bonus algorithm starts from 0 node.
• For the Kruskal algorithm to work properly, please make a connection between all nodes. Establish the connections of unconnected nodes.

 * */

/*
 * Orgin Turkish Languegue  
 +-+-++-+-+-+-+-GRAF OLUSTURMA MANTIĞI +-+-+-+-+-+-+-+-+
 Lutfen Programı Çalıştırmadan Önce Burada Yazan Talimatları Okuyunuz...
 -program başladığında Sol alttaki picturebox1'e tıklayarak node ekleyebilirsiniz.
 -kaç adet node ekleyecekseniz ilk önce nodeları ekleyip ondan sonra matrix olustur buttonuna basmalısınız.
 -lütfen matrix olustur buttonuna ekleyeceğiniz node sayısı kadar node olusturduktan sonra basınız.
 -matrix olustur butonuna basıldığında sağ taraftaki tabcontrol nesnesinin tabpage1 sayfasına matrix olusturulur.
 -matrixdeki buttonlara mouse'un left click(sol tık)'a basarsanız nodelar arasında bağlantı kurulur ve herbir tıklamada 
  nodelar arasındaki maliyet birer birer artar basılı tutarsanız daha hızlı arttığını göreceksiniz.
 -mouse'un right click(sağ tık)'a basarsanız nodelar arası uzunluk-maliyet  azalır basılı tutarsanız daha hızlı azalır.
 -matrixdeki buttonlara basarak veya bir noda basıp başka bir noda bastığınızda node'lar arası bağlantı kurmuş olursunuz.
 -tabepage1 nesnesinin boyutu kadar matrix sığabildiği için node sayısı 12 ile sınırlandırılmıştır.
 -graf'ı oluşturduysanız radiobuttonlardan birini seçip Çalıştır Buttonuna basabilirsiniz 
 -Çalıştıra bastığınızda tabpage2'de algoritma hangi nodelardan geçtiğini ve minimum toplam mağliyeti yazdırır.
 -Eğer Oluşturduğunuz graf yapısı yanlış veya yeni bir graf oluşturmak istiyorsanız
  sol alt köşedeki yeni graf oluştur butonuna tıklayabilirsiniz.
 -sol alt köşedeki text ismi 'ızgara' olan checkbox1 nesnesine tıklarsanız picturebox1 in arkaplanını değiştirirsiniz.
 -kruskal veya prim algoritmalarından birini çalıştırdığınızda graf yapısında renk değişimleri olur renk değişimlerini
  eski haline getirmek için yenile buttonu kullanılır. 
*/

namespace move_or_copy_a_line
{
    public partial class Form1 : Form
    {

        int[,] matrix = new int[100,100];
        int[,] spanning=new int[100,100];
        int k, infinity=9999;
        static int INF = int.MaxValue;
        private List<LineList> MyLines = new List<LineList>();
        Point StartDownLocation = new Point();
        public Point MouseDownLocation;
        private MakeMovable _move;
        private bool IsMouseDown = false;
        private int m_StartX=0;
        private int m_StartY=0;
        private int m_CurX=0;
        private int m_CurY=0;
        string tut;
        bool controls=false,renk_cntrls=false;
        bool cntrol_mouse_right;
        ovalbutton btn,ovl_btn;
        Button m_btn;
        Label lbl;
        int x = -1,a=0,b=0,lx=40,hms=0,simdilik=-1;
        bool nodecntrl = false;
        string buttn;

        class ovalbutton:Button
        {
            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                GraphicsPath gp = new GraphicsPath();
                Rectangle rec = new Rectangle(Point.Empty,this.Size);
                gp.AddEllipse(rec);
                Region = new Region(gp);
            }
        }
        public Form1()
        {
            InitializeComponent();
            _move = new MakeMovable(this);
            _move.SetMovable(panel1);
            _move.SetMovable(label1);
            timer1.Interval = 100;
        }
        static int find(int i,int  []parent)
        {
            try
            {
                while (parent[i] != i)
                    i = parent[i];
            }
            catch(Exception e)
            {
               // MessageBox.Show(e.ToString());
            }
            return i;
        }
        static void union1(int i, int j, int[] parent)
        {
            int a = find(i,parent);
            int b = find(j,parent);
            try
            {
                parent[a] = b;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }     
        }
        private void Kruskal_minimum_spanning_tree()
        {
            tabControl1.Enabled = false;
            button2.Enabled = false;
            textBox2.Text = "";
            int V = (simdilik + 1);
            int[] parent = new int[V];
            int[,] cost=new int[100,100];
            for (int i = 0; i < (simdilik + 1); i++)
            {
                for (int j = 0; j < (simdilik + 1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        cost[i, j] = INF;
                    }
                        
                    else
                    {
                        cost[i, j] = matrix[i, j];

                    }
                }
            }
            int mincost = 0; 

            for (int i = 0; i < V; i++)
                parent[i] = i;

            int edge_count = 0;
            while (edge_count < V - 1)
            {
                int min = INF, a = -1, b = -1;
                for (int i = 0; i < V; i++)
                {
                    for (int j = 0; j < V; j++)
                    {
                        if (find(i,parent) != find(j,parent) && cost[i, j] < min)
                        {
                            min = cost[i, j];
                            a = i;
                            b = j;
                        }
                    }
                }

                union1(a, b,parent);
                if(a!=-1||b!=-1)
                textBox2.Text += " Edge " + edge_count + " : (" + a + ", " + b + ") Cost : " + min + " " + Environment.NewLine;
                Console.Write("Edge {0}:({1}, {2}) cost:{3} \n",
                    edge_count, a, b, min);
                edge_count++;
                mincost += min;

                try
                {
                    Control butn_Renk = this.Controls.Find("graf" + a.ToString(), true)[0] as Control;
                    Control butn_Renk2 = this.Controls.Find("graf" + b.ToString(), true)[0] as Control;
                    butn_Renk.BackColor = Color.FromArgb(125, 38, 205);//125 38 205//110, 123, 139
                    Application.DoEvents();
                    Thread.Sleep(500);
                    butn_Renk2.BackColor = Color.FromArgb(125, 38, 205);
                    renk_cntrls = true;
                    LineList DrawLine = new LineList();
                    DrawLine.X1 = butn_Renk.Location.X + 20;
                    DrawLine.Y1 = butn_Renk.Location.Y + 20;
                    DrawLine.X2 = butn_Renk2.Location.X + 20;
                    DrawLine.Y2 = butn_Renk2.Location.Y + 20;
                    DrawLine.Control = 1;
                    MyLines.Add(DrawLine);
                    pictureBox1.Invalidate();
                }
                catch (Exception e)
                {
                   // MessageBox.Show(e.ToString());
                }                                             
            }
            if(mincost<2147000000&&mincost>0)
            textBox2.Text += " Minimum Cost = "+mincost;
        }
        private void Prim_minimum_spanning_tree()
        {
            button2.Enabled = false;
            tabControl1.Enabled = false;
            textBox2.Text = "";
            bool knkg = false;
            int edge_count = 0;
            for (int k = 0; k < (simdilik + 1); k++)
            {
                if(matrix[0,k]!=0)
                {
                    knkg = true;
                }
                Console.WriteLine();
                for (int l = 0; l < (simdilik + 1); l++)
                    Console.Write("\t" + matrix[k, l]); ;
            }
            if(knkg==false)
            {
                MessageBox.Show("THE INITIAL NODE IS RANDOM 'ATTACHED TO 0' NODE SHOULD HAVE A CONNECTION WITH OTHER NODES");
            }
            int Max = 100, n = simdilik + 1;
            Console.WriteLine("simdilik" + (simdilik + 1));
            int[,] cost = new int[Max, Max];
            int[] distance = new int[Max];
            int[] from = new int[Max];
            int[] visited = new int[Max];
            int u, v = 0, min_distance;
            int no_of_edges, i, min_cost, j;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (matrix[i, j] == 0)
                        cost[i, j] = infinity;
                    else
                    {
                        cost[i, j] = matrix[i, j];

                    }

                    spanning[i, j] = 0;
                }
            }

            distance[0] = 0;
            visited[0] = 1;

            for (i = 1; i < n; i++)
            {
                distance[i] = cost[0, i];
                from[i] = 0;
                visited[i] = 0;
            }
            min_cost = 0;       // spanning tree nin maliyeti
            no_of_edges = n - 1;        

            while (no_of_edges > 0)
            {
                min_distance = infinity;
                for (i = 1; i < n; i++)
                    if (visited[i] == 0 && distance[i] < min_distance)
                    {
                        v = i;
                        min_distance = distance[i];
                    }

                u = from[v];
                Console.WriteLine("s" + u + "" + v);


                
                Control butn_Renk = this.Controls.Find("graf" + u.ToString(), true)[0] as Control;
                Control butn_Renk2 = this.Controls.Find("graf" + v.ToString(), true)[0] as Control;
                butn_Renk.BackColor = Color.FromArgb(125, 38, 205); //110 123 139////125 38 205
                Application.DoEvents();
                Thread.Sleep(500);
                butn_Renk2.BackColor = Color.FromArgb(125, 38, 205);
                renk_cntrls = true;
                LineList DrawLine = new LineList();
                DrawLine.X1 = butn_Renk.Location.X + 20;
                DrawLine.Y1 = butn_Renk.Location.Y + 20;
                DrawLine.X2 = butn_Renk2.Location.X + 20;
                DrawLine.Y2 = butn_Renk2.Location.Y + 20;
                DrawLine.Control = 1;
                MyLines.Add(DrawLine);
                pictureBox1.Invalidate();                
                
                spanning[u, v] = distance[v];
                spanning[v, u] = distance[v];
                no_of_edges--;
                visited[v] = 1;     
                textBox2.Text += " Edge " + edge_count + " : (" + u + ", " + v + ") Cost : " + spanning[u, v]+""+ Environment.NewLine ;
                edge_count++;
                
                for (i = 1; i < n; i++)
                    if (visited[i] == 0 && cost[i, v] < distance[i])
                    {
                        distance[i] = cost[i, v];
                        from[i] = v;
                    }

                min_cost = min_cost + cost[u, v];
            }
            textBox2.Text += " Minimum Cost= " + min_cost + "";
        }
        private void Matrix_olustur_Click(object sender, EventArgs e)
        {
            controls = true;
                    for (int i = 40; i < lx; i += 40)
                    {
                        for (int j = 40; j < lx; j += 40)
                        {
                    hms++;
                    m_btn = new Button();
                    if (i!=j)
                    {
                        m_btn.Text = Convert.ToChar(0x221E).ToString();//Unicode ifadeyi karaktere dönüştürmek.  
                       // m_btn.Text = hms.ToString();
                    }
                     else
                     {    
                        m_btn.Text = "-".ToString();
                       // m_btn.Text = hms.ToString();
                    }
                    m_btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    m_btn.Name = "my"+hms.ToString();
                    m_btn.Location = new Point(i,j);
                    m_btn.FlatStyle = FlatStyle.Flat;
                    m_btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(252, 186, 63);
                    m_btn.FlatAppearance.MouseOverBackColor = Color.DeepSkyBlue;
                    m_btn.BackColor = Color.FromArgb(252, 253, 205);//Color.FromArgb(226, 182, 157);
                    m_btn.Width = 40;
                    m_btn.Height = 40;
                    m_btn.AutoSize = true;
                    this.m_btn.MouseDown+= new System.Windows.Forms.MouseEventHandler(this.m_btn_mousedown);
                    this.m_btn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_btn_mouseup);
                    this.m_btn.MouseEnter += new System.EventHandler(this.m_btn_MouseEnter);
                    this.m_btn.MouseLeave += new System.EventHandler(this.m_btn_MouseLeave);
                    // 
                    tabPage1.Controls.Add(m_btn);
                }
            }
            Matrix_olustur.Enabled = false;
        }
        private void m_btn_MouseLeave(object sender, EventArgs e)
        {
            m_btn = sender as Button;
            int dvm = 0;
            for (int i = 0; i < x + 1; i++)
            {
                for (int j = 0; j < x + 1; j++)
                {
                    dvm++;
                    if (m_btn.Name == "my" + dvm.ToString())
                    {
                        Control butni = this.Controls.Find("graf" + i.ToString(), true)[0] as Control;
                        Control butnj = this.Controls.Find("graf" + j.ToString(), true)[0] as Control;

                        if(renk_cntrls==false)
                        {
                            butni.BackColor = Color.FromArgb(238, 201, 0);
                            butnj.BackColor = Color.FromArgb(238, 201, 0);
                        }  
                        else
                        {
                            butni.BackColor = Color.FromArgb(125, 38, 205);//125 38 205
                            butnj.BackColor = Color.FromArgb(125, 38, 205);
                        }
                        break;
                    }
                }
            }
        }
        private void m_btn_MouseEnter(object sender, EventArgs e)
        {
            m_btn = sender as Button;
            int dvm = 0;
            for (int i = 0; i < x + 1; i++)
            {
                for (int j = 0; j < x + 1; j++)
                {
                    dvm++;
                    if (m_btn.Name == "my" + dvm.ToString())
                    {
                        Control butni = this.Controls.Find("graf" + i.ToString(), true)[0] as Control;
                        Control butnj = this.Controls.Find("graf" + j.ToString(), true)[0] as Control;
                       
                        butni.BackColor = Color.DeepSkyBlue;
                        butnj.BackColor = Color.DeepSkyBlue;
                        break;
                    }
                }
            }


         }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                Prim_minimum_spanning_tree();
            else if (radioButton2.Checked == true)
                Kruskal_minimum_spanning_tree();
            button2.Enabled = true;
            tabControl1.Enabled = true;
        }

        private void m_btn_mousedown(object sender, MouseEventArgs e)
        {
            m_btn = sender as Button;
            cntrol_mouse_right = false;
                int iter = 0;
                if (m_btn.Text == "-")
                {
                    return;
                }
                else if (m_btn.Text != Convert.ToChar(0x221E).ToString())
                {
                if (e.Button == MouseButtons.Left)
                {
                    m_btn.Text = (Convert.ToInt32(m_btn.Text) + 1).ToString();
                    iter = Convert.ToInt32(m_btn.Text);
                }
                else if(e.Button == MouseButtons.Right)
                {
                    cntrol_mouse_right = true;
                    if(Convert.ToInt32(m_btn.Text)!=0)
                    m_btn.Text = (Convert.ToInt32(m_btn.Text) - 1).ToString();
                    iter = Convert.ToInt32(m_btn.Text);
                }

                }
                else
                {
                    m_btn.Text = 0 + "";
                    m_btn.Text = (Convert.ToInt32(m_btn.Text) + 1).ToString();
                }
                int dvm = 0;
                for (int i = 0; i < x + 1; i++)
                {
                    for (int j = 0; j < x + 1; j++)
                    {
                        dvm++;
                        if (m_btn.Name == "my" + dvm.ToString())
                        {
                            int t = (i + 1) + j * (x + 1);
                            Control m_btnjf = this.Controls.Find("my" + t.ToString(), true)[0] as Control;
                            m_btnjf.Text = m_btn.Text;
                            Control butni = this.Controls.Find("graf" + i.ToString(), true)[0] as Control;
                            Control butnj = this.Controls.Find("graf" + j.ToString(), true)[0] as Control;
                            matrix[i, j] = Convert.ToInt32(m_btn.Text);
                            matrix[j, i] = Convert.ToInt32(m_btn.Text);
                            LineList DrawLine = new LineList();
                            DrawLine.X1 = butni.Location.X + 20;
                            DrawLine.Y1 = butni.Location.Y + 20;
                            DrawLine.X2 = butnj.Location.X + 20;
                            DrawLine.Y2 = butnj.Location.Y + 20;
                            MyLines.Add(DrawLine);
                            pictureBox1.Invalidate();
                            try
                            {
                                Control label_ij = this.Controls.Find("g_lbl" + i + "," + j, true)[0] as Control;
                                label_ij.Text = m_btn.Text;
                            }
                            catch
                            {
                                try
                                {
                                    Control label_ij = this.Controls.Find("g_lbl" + j + "," + i, true)[0] as Control;
                                    label_ij.Text = m_btn.Text;
                                }
                                catch
                                {
                                    int lbl_ort_X = (butni.Location.X + butnj.Location.X + 40) / 2;
                                    int lbl_ort_Y = (butni.Location.Y + butnj.Location.Y + 40) / 2;
                                    lbl = new Label();
                                    lbl.Text = 1.ToString();
                                    lbl.Name = "g_lbl" + i + "," + j;
                                    lbl.Location = new Point(lbl_ort_X, lbl_ort_Y);
                                    lbl.Font = new Font("Tahoma", 11F, FontStyle.Bold);
                                    lbl.AutoSize = true;
                                    lbl.Parent = pictureBox1;
                                    lbl.BackColor = Color.Transparent;
                                }
                            }
                        }
                    }
                }          
            buttn =m_btn.Name;
            timer1.Interval = 100;
            timer1.Start();
        }
        private void m_btn_mouseup(object sender, MouseEventArgs e)
        {
            m_btn = sender as Button;
            timer1.Stop();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Control m_butn = this.Controls.Find(buttn, true)[0] as Control;
            int iter = 0;
           
            if(m_btn.Text == "-")
            {
                timer1.Stop();
                return;
            }
            else if (m_btn.Text != Convert.ToChar(0x221E).ToString())
            {
                if (cntrol_mouse_right==false)
                {
                    m_btn.Text = (Convert.ToInt32(m_btn.Text) + 1).ToString();
                    iter = Convert.ToInt32(m_btn.Text);
                }
                else if (cntrol_mouse_right==true)
                {
                    if (Convert.ToInt32(m_btn.Text) != 0)
                        m_btn.Text = (Convert.ToInt32(m_btn.Text) - 1).ToString();
                    iter = Convert.ToInt32(m_btn.Text);
                }
            }
            else
            {
                m_btn.Text = 0 + "";
                m_btn.Text = (Convert.ToInt32(m_btn.Text) + 1).ToString();
            }
            if(iter>10)
            {
                timer1.Interval = 10;
            }
            else if (iter > 100)
            {
                timer1.Interval = 5;
            }
            int dvm = 0;
            for (int i = 0; i < x + 1; i++)
            {
                for (int j = 0; j < x + 1; j++)
                {
                    dvm++;
                    if (m_btn.Name == "my" + dvm.ToString())
                    {
                     //   Console.WriteLine("i=" + i + "j=" + j);
                        int t = (i + 1) + j * (x + 1);
                        Control m_btnjf = this.Controls.Find("my" + t.ToString(), true)[0] as Control;
                        m_btnjf.Text = m_btn.Text;

                        matrix[i, j] = Convert.ToInt32(m_btn.Text);
                        matrix[j, i] = Convert.ToInt32(m_btn.Text);

                        Control butni = this.Controls.Find("graf" + i.ToString(), true)[0] as Control;
                        Control butnj = this.Controls.Find("graf" + j.ToString(), true)[0] as Control;
                        try
                        {
                            Control label_ij = this.Controls.Find("g_lbl" + i + "," + j, true)[0] as Control;
                            label_ij.Text = matrix[i, j].ToString();
                        }
                        catch
                        {
                            try
                            {
                                Control label_ij = this.Controls.Find("g_lbl" + j + "," + i, true)[0] as Control;
                                label_ij.Text = matrix[i, j].ToString();
                            }
                            catch
                            {

                            }
                        }

                        break;
                    }
                }
            }

        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            a = e.X;
            b = e.Y;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= MyLines.Count - 1; i++)
            {
                MyLines[i].Control=0;
            }
            for (int i = 0; i <= simdilik; i++)
            {
                Control butn_Renk = this.Controls.Find("graf" + i.ToString(), true)[0] as Control;
                butn_Renk.BackColor = Color.FromArgb(238, 201, 0);
            }
            if (radioButton1.Checked==true)
            {
                textBox1.Enabled = false;
            }
            else if (radioButton1.Checked == false)
            {
                textBox1.Enabled = true;
            }
            pictureBox2.Image = Properties.Resources.Edsger_W_Dijkstra1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= MyLines.Count - 1; i++)
            {
                MyLines[i].Control = 0;
            }
            for (int i = 0; i <= simdilik; i++)
            {
                Control butn_Renk = this.Controls.Find("graf" + i.ToString(), true)[0] as Control;
                butn_Renk.BackColor = Color.FromArgb(238, 201, 0);
            }
            if (radioButton2.Checked == true)
            {
                textBox1.Enabled = false;
            }
            else if (radioButton2.Checked == false)
            {
                textBox1.Enabled = true;
            }
            pictureBox2.Image = Properties.Resources.Joseph_Bernard_Kruskal1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= MyLines.Count - 1; i++)
            {
                MyLines[i].Control = 0;
            }
            for (int i = 0; i <= simdilik; i++)
            {
                Control butn_Renk = this.Controls.Find("graf" + i.ToString(), true)[0] as Control;
                butn_Renk.BackColor = Color.FromArgb(238, 201, 0);
            }
            renk_cntrls = false;
        }

        void button_click(object sender, EventArgs e)
        {
            if (controls == true)
            {
            int t = 0, t2 = 0;
            string kelime;
            btn = sender as ovalbutton;
            if (tut != btn.Name && IsMouseDown == true)
            {
                kelime = btn.Name;
                kelime = kelime.Remove(0, 4);//graf
                int k2 = Convert.ToInt16(kelime);
                if (matrix[k, k2] == 0)
                {
                    t = (k + 1) + k2 * (x + 1);
                    t2 = (k2 + 1) + k * (x + 1);
                    try
                    {
                        Control m_butn = this.Controls.Find("my" + t.ToString(), true)[0] as Control;
                        Control m_butn2 = this.Controls.Find("my" + t2.ToString(), true)[0] as Control;
                        m_butn.Text = 1.ToString();
                        m_butn2.Text = 1.ToString();

                        matrix[k, k2] = 1;
                        matrix[k2, k] = 1;
                        IsMouseDown = false;
                        LineList DrawLine = new LineList();
                        DrawLine.X1 = m_StartX;
                        DrawLine.Y1 = m_StartY;
                        DrawLine.X2 = btn.Location.X + 20;
                        DrawLine.Y2 = btn.Location.Y + 20;
                        MyLines.Add(DrawLine);
                        pictureBox1.Invalidate();

                        int lbl_ort_X = (m_StartX + btn.Location.X + 20) / 2;
                        int lbl_ort_Y = (m_StartY + btn.Location.Y + 20) / 2;


                        lbl = new Label();
                        lbl.Text = 1.ToString();
                        lbl.Name = "g_lbl" + k + "," + k2;
                        lbl.Location = new Point(lbl_ort_X, lbl_ort_Y);
                        lbl.Font = new Font("Tahoma", 11F, FontStyle.Bold);
                        lbl.AutoSize = true;
                        lbl.Parent = pictureBox1;
                        lbl.BackColor = Color.Transparent;
                    }
                    catch (Exception es)
                    {
                        MessageBox.Show("PLEASE DO NOT LINK TO GRAPH WITHOUT CREATING MATRIX" + es.ToString());
                    }

                }
                else
                {
                    matrix[k, k2] += 1;
                    matrix[k2, k] += 1;
                    t = (k + 1) + k2 * (x + 1);
                    t2 = (k2 + 1) + k * (x + 1);
                    try
                    {
                        Control m_butn = this.Controls.Find("my" + t.ToString(), true)[0] as Control;
                        Control m_butn2 = this.Controls.Find("my" + t2.ToString(), true)[0] as Control;
                        m_butn.Text = matrix[k, k2].ToString();
                        m_butn2.Text = matrix[k, k2].ToString();

                        IsMouseDown = false;
                        LineList DrawLine = new LineList();
                        DrawLine.X1 = m_StartX;
                        DrawLine.Y1 = m_StartY;
                        DrawLine.X2 = btn.Location.X + 20;
                        DrawLine.Y2 = btn.Location.Y + 20;
                        MyLines.Add(DrawLine);
                        pictureBox1.Invalidate();

                        try
                        {
                            Control labelss = this.Controls.Find("g_lbl" + k + "," + k2, true)[0] as Control;
                            labelss.Text = matrix[k, k2].ToString();
                        }
                        catch (Exception eses)
                        {
                            Control labelss = this.Controls.Find("g_lbl" + k2 + "," + k, true)[0] as Control;
                            labelss.Text = matrix[k, k2].ToString();
                            //MessageBox.Show(eses.ToString());
                        }
                    }
                    catch (Exception esss)
                    {
                        MessageBox.Show(esss.ToString());
                    }


                }
            }
            else
            {
                IsMouseDown = true;
                tut = btn.Name;
                m_StartX = btn.Location.X + 20;
                m_StartY = btn.Location.Y + 20;
                m_CurX = btn.Location.X;
                m_CurY = btn.Location.Y;
                StartDownLocation = btn.Location;
                kelime = btn.Name;
                kelime = kelime.Remove(0, 4);//graf
                k = Convert.ToInt16(kelime);
            }
        }   
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
                Pen dashed_pen = new Pen(Color.Green, 1);
                dashed_pen.DashStyle = DashStyle.Dash;
                m_CurX = e.X;
                m_CurY = e.Y;
                pictureBox1.Invalidate();    
        }
        private void ovl_btn_Click(object sender, EventArgs e)
        {
            ovl_btn = sender as ovalbutton;
            if (ovl_btn.Name == "kapat" + 1)
            {
                this.Close();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Parent = panel1;
            label1.BackColor = Color.Transparent;
            label1.Text = "Minimum Spanning Tree Algorithm";
            label1.ForeColor = Color.FromArgb(252, 253, 205);//.White;///Color.FromArgb(252, 253, 205);
            tabPage1.BackColor = Color.FromArgb(255, 149, 1);//Color.FromArgb(19, 130, 175);
            textBox2.BackColor = Color.FromArgb(255, 149, 1);//Color.FromArgb(245, 146, 81);
            radioButton1.Checked = true;
            checkBox1.Checked = true;
            radioButton1.ForeColor = Color.FromArgb(252, 253, 205);
            radioButton2.ForeColor = Color.FromArgb(252, 253, 205);//Color.FromArgb(255, 149, 1);
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(255, 149, 1); // Color.FromArgb(123, 193, 68);
            panel1.BackColor = Color.FromArgb(42, 61, 81);
            panel2.BackColor = Color.FromArgb(42, 61, 81);
            panel3.BackColor = Color.FromArgb(42, 61, 81);
            panel4.BackColor = Color.FromArgb(42, 61, 81);

            ovl_btn = new ovalbutton();
            ovl_btn.Text = "X";
            ovl_btn.Name = "kapat" + 1;
            ovl_btn.Location = new Point(727, 0);
            ovl_btn.Width = 20;
            //ovl_btn.Height = 20;
            ovl_btn.AutoSize = true;
            ovl_btn.FlatStyle = FlatStyle.Flat;
            ovl_btn.BackColor = Color.Transparent;
            ovl_btn.Parent = panel1;
            ovl_btn.FlatAppearance.BorderSize = 0;
            ovl_btn.Click += new EventHandler(this.ovl_btn_Click);
            ovl_btn.ForeColor = Color.White;//Color.FromArgb(123, 193, 68);
            ovl_btn.FlatAppearance.MouseDownBackColor = Color.OrangeRed;
            ovl_btn.FlatAppearance.MouseOverBackColor = Color.Red;
            ovl_btn.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            ovl_btn.Dock = DockStyle.Right;
            panel1.Controls.Add(ovl_btn);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                pictureBox1.Image = Properties.Resources.Kare;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.beyaz;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int i, x1, y1, x2, y2,control;
            for (i = 0; i <= MyLines.Count - 1; i++)
            {
                control = MyLines[i].Control;
                if(control==1)
                {
                    Pen LinePen = new Pen(Color.Aqua, 4);//192 192 192//Color.Aqua
                    x1 = MyLines[i].X1;
                    x2 = MyLines[i].X2;
                    y1 = MyLines[i].Y1;
                    y2 = MyLines[i].Y2;
                    e.Graphics.DrawLine(LinePen, x1, y1, x2, y2);
                }
                else
                {
                    Pen LinePen = new Pen(Color.FromArgb(238, 0, 0), 2);
                    x1 = MyLines[i].X1;
                    x2 = MyLines[i].X2;
                    y1 = MyLines[i].Y1;
                    y2 = MyLines[i].Y2;
                    e.Graphics.DrawLine(LinePen, x1, y1, x2, y2);
                }
            }
            if (IsMouseDown == true)
            {
                Pen dashed_pen = new Pen(Color.Blue, 1);
                e.Graphics.DrawLine(dashed_pen, m_StartX, m_StartY, m_CurX, m_CurY);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (controls == false&&simdilik<12)
            {
                x++;
                simdilik++;
                btn = new ovalbutton();
                btn.Text = Convert.ToString(simdilik);//x+1
                btn.Name = "graf" + x.ToString();
                btn.Location = new Point(a, b);
                btn.Width = 40;
                btn.Height = 40;
                btn.AutoSize = true;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.MouseDownBackColor = Color.Pink;
                btn.FlatAppearance.MouseOverBackColor = Color.DeepSkyBlue;
                btn.BackColor = Color.FromArgb(238, 201, 0);//Color.Orange;//238 201 0
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += new EventHandler(this.button_click);
                btn.Font = new Font("Tahoma", 14F, FontStyle.Bold);
                ForeColor = Color.Black;
                pictureBox1.Controls.Add(btn);

                lbl = new Label();
                lbl.Text = (simdilik).ToString();//x+1
                lbl.Name = (x + 1).ToString();
                lbl.Location = new Point(lx + 5, 5);
                lbl.Font = new Font("Tahoma", 14F, FontStyle.Bold);
                lbl.AutoSize = true;
                lbl.Parent = tabPage1;
                lbl.BackColor = Color.Transparent;

                lbl = new Label();
                lbl.Text = (simdilik).ToString();//x+1
                lbl.Name = (x + 1).ToString();
                lbl.Location = new Point(5, lx + 5);
                lbl.Font = new Font("Tahoma", 14F, FontStyle.Bold);
                lbl.AutoSize = true;
                lbl.Parent = tabPage1;
                lbl.BackColor = Color.Transparent;
                lx += 40;
            }
            else if(simdilik>=12&&nodecntrl==false)
            {
                MessageBox.Show("In this program, the maximum number of nodes is 12 !!!");
                nodecntrl = true;

            }
        }
    }
           
}
