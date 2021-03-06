using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LocalERP.DataAccess.DataDAO;
using LocalERP.DataAccess.Data;

namespace LocalERP.WinForm
{
    public partial class ElementFlowDetailForm : Form
    {
        private int mode;

        public delegate void ModifiedComplete();
        public event ModifiedComplete modifiedComplete;

        public ElementFlowDetailForm(int mode)
        {
            InitializeComponent();
            this.mode = mode;
        }

        private void ElementFlowForm_Load(object sender, EventArgs e)
        {
            this.switchMode(mode);

            DataTable elementTable = ElementDao.getInstance().FindList();
            this.comboBox_element.DataSource = elementTable;
            this.comboBox_element.ValueMember = "ID";
            this.comboBox_element.DisplayMember = "name";
            this.comboBox_element.SelectedValue = (int)(elementTable.Rows[0]["ID"]);
        }

        private void switchMode(int mode) {
            if (mode == 1) {
                this.label_type.Text = "配件出库";
                this.Text = "配件出库记录";
            }
        }

        /// <summary>
        /// get value from controls
        /// </summary>
        private bool getNum(out int result) {
            int temp = 0;
            if ( !string.IsNullOrEmpty(this.textBox_num.Text) && int.TryParse(this.textBox_num.Text, out temp))
            {
                this.errorProvider1.SetError(this.textBox_num, string.Empty);
                result = temp;
                return true;
            }
            else
            {
                this.errorProvider1.SetError(this.textBox_num, "请输入整数!");
                result = temp;
                return false;
            }
        }

        /// <summary>
        /// event
        /// </summary>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            int flowType = 1;
            int type = 1;
            if (mode != 0)
            {
                flowType = -1;
                type = 2;
            }
            int num;
            if (!this.getNum(out num))
                return;

            int elementID = (int)this.comboBox_element.SelectedValue;

            int totalNum = ElementDao.getInstance().FindByID(elementID).Num + num * flowType;

            if (totalNum < 0)
            {
                MessageBox.Show("库存不足,操作失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ElementFlow flow = new ElementFlow(
                elementID, this.comboBox_element.Text, flowType, num, totalNum, this.textBox_comment.Text, this.dateTimePicker1.Value, type);

            ElementFlowDao.getInstance().Insert(flow);

            ElementDao.getInstance().UpdateNum(elementID, totalNum);

            MessageBox.Show("保存成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (modifiedComplete != null)
                modifiedComplete.Invoke();
            this.Close();
        }

        private void textBox_num_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox_num.Text))
                return;
            int num;
            this.getNum(out num);
        }
    }
}