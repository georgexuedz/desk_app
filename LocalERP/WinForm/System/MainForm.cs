using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using LocalERP.DataAccess.Utility;
using System.IO;
using LocalERP.DataAccess.Data;
using LocalERP.UiDataProxy;
using System.Reflection;

namespace LocalERP.WinForm
{
    public partial class MainForm : Form
    {

        LoadingForm loadingForm;
        public MainForm()
        {
            InitializeComponent();
            loadingForm = new LoadingForm();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            NavigationUC navigation = new NavigationUC(this);
            navigation.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(navigation);

            getWelcomeForm().Show(this.dockPanel1);
            this.toolStripStatusLabel1.Text = "当前日期: " + DateTime.Now.ToShortDateString();

            FormMgr.getInstance().MainForm = this;

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private WelcomeForm welcomeForm = null;
        private WelcomeForm getWelcomeForm() { if (welcomeForm == null || welcomeForm.IsDisposed) welcomeForm = new WelcomeForm(this); return welcomeForm; }

        public void setForm(String buttonText) {
            this.setForm(buttonText, 0, 0);
        }

        public void setForm(String buttonText, int openMode, int ID)
        {
            switch (buttonText) {
                case DataUtility.PURCHASE_LIST:
                    FormMgr.getInstance().getProductPurchaseListForm().Show(this.dockPanel1);
                    break;
                case DataUtility.PURCHASE:
                    FormMgr.getInstance().getProductPurchaseForm().Show(this.dockPanel1);
                    FormMgr.getInstance().getProductPurchaseForm().reload(openMode, ID);
                    break;
                case DataUtility.PURCHASE_BACK:
                    FormMgr.getInstance().getProductPurchaseBackForm().Show(this.dockPanel1);
                    FormMgr.getInstance().getProductPurchaseBackForm().reload(openMode, ID);
                    break;
                case DataUtility.SELL_LIST:
                    FormMgr.getInstance().getProductSellListForm().Show(this.dockPanel1);
                    break;
                case DataUtility.SELL:
                    FormMgr.getInstance().getProductSellForm().Show(this.dockPanel1);
                    FormMgr.getInstance().getProductSellForm().reload(openMode, ID);
                    break;
                case DataUtility.SELL_BACK:
                    FormMgr.getInstance().getProductSellBackForm().Show(this.dockPanel1);
                    FormMgr.getInstance().getProductSellBackForm().reload(openMode, ID);
                    break;
                case DataUtility.LIB_LIST:
                    FormMgr.getInstance().getProductLibListForm().Show(this.dockPanel1);
                    break;
                case DataUtility.LIB_OVERFLOW:
                    FormMgr.getInstance().getProductLibOverflowForm().Show(this.dockPanel1);
                    FormMgr.getInstance().getProductLibOverflowForm().reload(openMode, ID);
                    break;
                case DataUtility.LIB_LOSS:
                    FormMgr.getInstance().getProductLibLossForm().Show(this.dockPanel1);
                    FormMgr.getInstance().getProductLibLossForm().reload(openMode, ID);
                    break;
                /*case DataUtility.ELEMENT_LIB:
                    //getElementLibForm().Show(this.dockPanel1);
                    break;*/
                case DataUtility.QUERY_LIB:
                    FormMgr.getInstance().getQueryLibForm().Show(this.dockPanel1);
                    break;
                case DataUtility.QUERY_DETAIL:
                    FormMgr.getInstance().getQueryDetailForm().Show(this.dockPanel1);
                    break;
                case DataUtility.STATISTIC:
                    FormMgr.getInstance().getProductStatisticForm().Show(this.dockPanel1);
                    break;
                case DataUtility.DATA_CUSTOMER:
                    FormMgr.getInstance().getCustomerCIForm().Show(this.dockPanel1);
                    break;
                case DataUtility.DATA_PRODUCT:
                    FormMgr.getInstance().getProductCIForm().Show(this.dockPanel1);
                    break;
                default:
                    break;
            }
        }

        public void showLoading() {
            try
            {
                this.loadingForm.ShowDialog();
            }
            catch (TargetInvocationException ex) {
                ex.Message.ToString();
            }
            //this.Enabled = false;
        }

        public void hideLoading() {
            this.loadingForm.Close();
            //this.Enabled = true;
        }

        private void 设置登陆密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordForm form = new PasswordForm();
            form.ShowDialog();
        }

        private void 关于软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        private void 导出数据文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Access文件(*.mdb)|*.mdb";
            saveFile.FilterIndex = 1;
            saveFile.FileName = "ERP_back_" + DateTime.Now.ToString("yyyyMMdd");
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile.FileName.Length > 0){
                try{
                    File.Copy(Application.StartupPath+"\\ERP.mdb", saveFile.FileName.ToString(), true);
                    MessageBox.Show("保存成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex){
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void 导入数据文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Access文件(*.mdb)|*.mdb";
            openFile.FilterIndex = 1;
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFile.FileName.Length > 0)
            {
                try
                {
                    if (MessageBox.Show("导入数据后,现有数据将全部被覆盖,是否继续?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        return;

                    File.Copy(openFile.FileName.ToString(), Application.StartupPath + "\\ERP.mdb", true);
                    MessageBox.Show("导入数据成功,请重启软件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void 显示导航栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mySplitter1.SplitterHide = false;
        }

        private void 显示欢迎页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getWelcomeForm().Show(this.dockPanel1);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton_purchase_Click(object sender, EventArgs e)
        {
            this.setForm(DataUtility.PURCHASE);
        }

        private void toolStripButton_sell_Click(object sender, EventArgs e)
        {
            this.setForm(DataUtility.SELL);
        }

        private void toolStripButton_queryLib_Click(object sender, EventArgs e)
        {
            this.setForm(DataUtility.QUERY_LIB);
        }

        private void toolStripButton_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton_statistic_Click(object sender, EventArgs e)
        {
            this.setForm(DataUtility.STATISTIC);
        }
    }
}