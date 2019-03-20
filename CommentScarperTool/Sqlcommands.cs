using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaKhoa
{
    class Sqlcommands
    {
        SqlConnection dataConnection = new SqlConnection();
        /// <summary>
        /// Procedure CMScommon()
        /// Hàm thiết lập
        /// Date 30/03/2014
        public Sqlcommands()
        {
            dataConnection.ConnectionString = @"Data Source=DESKTOP-PMIF4BT\SQLEXPRESS;Initial Catalog=QLNHAKHOA;Integrated Security=True";
       
        }
        /// <summary>
        /// Procedure Open()
        /// Thực hiện kết nối đến SQLSERVER 2012
        /// Date 30/03/2014
        /// </summary>
        /// <returns>True-False(Thông báo lỗi nếu có)</returns>
        public Boolean open()
        {
            try
            {
                if (dataConnection.State == ConnectionState.Closed)
                {
                    dataConnection.Open();
                }
                return true;
            }
            catch (SqlException ex)
            {
                this.errorMessage(ex.Number);
            }
            return false;
        }

        /// <summary>
        /// Procedure Close()
        /// Đóng kết nối đến SQLSERVER 2012
        /// Date 30/03/2014
        /// </summary>
        /// <returns>True-False(Thông báo lỗi nếu có)</returns>
        public void close()
        {
            try
            {
                if (dataConnection.State == ConnectionState.Open)
                {
                    dataConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                this.errorMessage(ex.Number);
            }
        }

        /// Hàm lấy giá trị kết nối database
        /// </summary>
        /// <returns></returns>
        public SqlConnection getConnection()
        {
            return this.dataConnection;
        }
        public void executeNonQueryWithTran(string sqlCommand, object[] param
           , object[] value, ref SqlConnection con, ref SqlTransaction tran)
        {

            SqlCommand command = new SqlCommand(sqlCommand);

            if (param != null)
            {
                for (int i = 0; i < param.Length; i++)
                {
                    if (value[i] != null)
                    {
                        command.Parameters.AddWithValue("@" + param[i], value[i]);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@" + param[i], System.DBNull.Value);
                    }
                }
            }
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = this.dataConnection;
            command.Transaction = tran;
            command.ExecuteNonQuery();

        }
        
        /// <summary>
        /// Function getDataTable()
        /// Đọc dữ liệu từ SQL trả về DataTable
        /// Date 30/03/2014
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        /// <returns>DataTable</returns>
        public DataTable getDataTable(string sqlCommand)
        {
            DataSet ds = this.getDataSet(sqlCommand);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            return null;
        }
       
        /// <summary>
        /// Hàm đọc dữ liệu từ thủ tục SQL trả về DataTable có tham số
        /// </summary>
        /// <param name="StoredProcedureName">Tên thủ tục SQL</param>        
        /// <returns>DataTable</returns>
        public DataTable getDataTableStoredProcedure(object[] param, object[] value, string storedProcedureName)
        {
            DataSet ds = this.getDataSet(param, value, storedProcedureName);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Function getDataReader()
        /// Hàm đọc dữ liệu từ SQL trả về SqlDataReader
        /// Date 30/03/2014
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>   
        /// <returns>SQLDataReader</returns>
        public SqlDataReader getDataReader(string sqlCommand)
        {
            if (this.open())
            {
                try
                {
                    SqlDataReader myReader;
                    SqlCommand dataCommand = new SqlCommand();
                    dataCommand.Connection = this.dataConnection;
                    dataCommand.CommandText = sqlCommand;
                    myReader = dataCommand.ExecuteReader();
                    //this.close();
                    return myReader;
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                }
            }
            return null;
        }

        /// <summary>
        /// Function getDataset()
        /// Hàm đọc dữ liệu từ SQL trả về DataSet 
        /// Date 30/03/2014
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        /// /// <returns>DataSet</returns>
        public DataSet getDataSet(string sqlCommand)
        {
            if (this.open())
            {
                try
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter adp;
                    SqlCommandBuilder sqlBuilder = new SqlCommandBuilder();
                    adp = new SqlDataAdapter(sqlCommand, this.dataConnection);
                    adp.Fill(ds);
                    sqlBuilder.DataAdapter = adp;
                    this.close();
                    return ds;
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                }
            }
            return null;
        }

        /// <summary>
        /// Hàm đọc dữ liệu từ SQL trả về DataSet có tham số truyền vào
        /// </summary>
        /// <param name="param">Mảng tham số(@,@,...) </param>
        /// <param name="value">Mảng giá trị</param>
        /// <param name="storedProcedureName">Tên thủ tục</param>
        /// <returns></returns>
        public DataSet getDataSet(object[] param, object[] value, string storedProcedureName)
        {
            if (this.open())
            {
                try
                {
                    DataSet ds = new DataSet();
                    SqlCommand command = new SqlCommand();
                    command.Connection = this.dataConnection;
                    if (param != null)
                    {
                        for (int i = 0; i < param.Length; i++)
                        {
                            if (value[i] != null)
                                command.Parameters.Add(new SqlParameter("@" + param[i], value[i]));
                            else
                                command.Parameters.Add(new SqlParameter("@" + param[i], System.DBNull.Value));
                        }
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;
                    SqlDataAdapter adp;
                    SqlCommandBuilder sqlBuilder = new SqlCommandBuilder();
                    adp = new SqlDataAdapter(command);
                    adp.Fill(ds);
                    sqlBuilder.DataAdapter = adp;
                    this.close();
                    return ds;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    //this.errorMessage(ex.Number);
                }
            }
            return null;
        }

        /// <summary>
        /// Function executeScalar()
        /// Hàm truy vấn có giá trị trả về
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        /// <returns>object</returns>
        public object getObject(string sqlCommand)
        {
            if (this.open())
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlCommand);
                    command.Connection = this.dataConnection;
                    command.CommandType = System.Data.CommandType.Text;
                    return command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                    return null;
                }
                finally
                {
                    this.close();
                }
            }
            else return null;
        }

        /// <summary>
        /// Function executeScalar()
        /// Hàm truy vấn có giá trị trả về từ thủ tục
        /// </summary>
        /// <param name="storedProcedureName">Thủ tục SQL</param>
        /// <returns>object</returns>
        public object getObjectStoredProcedure(object[] param, object[] values, string storedProcedureName)
        {
            if (this.open())
            {
                try
                {
                    SqlCommand command = new SqlCommand(storedProcedureName);
                    command.Connection = this.dataConnection;
                    if (param != null)
                    {
                        for (int i = 0; i < param.Length; i++)
                        {
                            if (values[i] != null)
                                command.Parameters.Add(new SqlParameter("@" + param[i], values[i]));
                            else
                                command.Parameters.Add(new SqlParameter("@" + param[i], System.DBNull.Value));
                        }
                    }
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;

                    return command.ExecuteScalar();
                    
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                    return null;
                }finally{
                    this.close();
                }
            }
            return null;
        }

        /// <summary>
        /// Hàm lấy giá trị trả về sau khi cập nhật dữ liệu vào SQL
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        /// <returns>object</returns>
        public object executeUpdateScalar(string sqlCommand)
        {
            if (this.open())
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlCommand);
                    command.Connection = this.dataConnection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                    command = new SqlCommand("SELECT @@IDENTITY");
                    command.Connection = this.dataConnection;
                    command.CommandType = System.Data.CommandType.Text;

                    return command.ExecuteScalar();
                    
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                    return null;
                }
                finally
                {
                    this.close();
                }
            }
            else return null;
        }

        
        /// <summary>
        /// Thủ tục cập nhật dữ liệu (thêm, sửa, xoá) vào SQL
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        public Boolean executeNonQuery(string sqlCommand, [Optional,DefaultParameterValue(false)] Boolean allowOpen)
        {
            if (this.open())
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlCommand);
                    command.Connection = this.dataConnection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                    if(!allowOpen)
                        this.close();
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Thủ tục cập nhật dữ liệu (thêm, sửa, xoá) vào SQL từ thủ tục
        /// </summary>
        /// <param name="storedProcedureName">Thủ tục SQL</param>
        public Boolean executeNonQueryStoredProcedure(object[] param
            , object[] value
            , string storedProcedureName
            , [Optional, DefaultParameterValue(false)] Boolean allowOpen)
        {
            if (this.open())
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = this.dataConnection;
                    if (param != null)
                    {
                        for (int i = 0; i < param.Length; i++)
                        {
                            if (value[i] != null)
                                command.Parameters.Add(new SqlParameter("@" + param[i], value[i]));
                            else
                                //command.Parameters.Add(null);
                                command.Parameters.Add(new SqlParameter("@" + param[i], System.DBNull.Value));
                        }
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;
                    command.ExecuteNonQuery();                     
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    this.errorMessage(ex.Number);
                    return false;
                }
                finally
                {if (!allowOpen)
                        this.close();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Thủ tục cập nhật dữ liệu (thêm, sửa, xoá) vào SQL từ thủ tục
        /// </summary>
        /// <param name="storedProcedureName">Thủ tục SQL</param>
        public Boolean executeNonQueryStoredProcedure(object[] param
            , object[] value
            , SqlDbType[] type
            , string storedProcedureName
            , [Optional, DefaultParameterValue(false)] Boolean allowOpen)
        {
            if (this.open())
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = this.dataConnection;
                    if (param != null)
                    {
                        for (int i = 0; i < param.Length; i++)
                        {
                            if (value[i] != null)
                            {
                                SqlParameter para = new SqlParameter("@" + param[i], value[i]);
                                para.SqlDbType = type[i];
                                command.Parameters.Add(para);
                            }
                            else
                            {
                                SqlParameter para = new SqlParameter("@" + param[i], System.DBNull.Value);
                                para.SqlDbType = type[i];
                                command.Parameters.Add(para);
                            }
                        }
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;
                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    this.errorMessage(ex.Number);
                    return false;
                }
                finally
                {
                    if (!allowOpen)
                        this.close();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Thủ tục cập nhật trường ảnh vào SQL
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        /// <param name="arrImage">Ảnh</param>
        public void executeUpdateImage(string sqlCommand, byte[] arrImage)
        {
            if (this.open())
            {
                try
                {
                    SqlCommand command = new SqlCommand(sqlCommand);
                    command.Connection = this.dataConnection;
                    command.Parameters.Add(new SqlParameter("@COMPUTER_FILE_CONTENT", SqlDbType.Image)).Value = arrImage;
                    command.ExecuteNonQuery();
                    this.close();
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                }
            }
        }

        /// <summary>
        /// Cập nhật DataSet
        /// </summary>
        /// <param name="tbname">DataTable</param>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        public bool UpdateDataset(ref DataTable tbname, string sqlCommand)
        {
            if (this.open())
            {
                try
                {
                    SqlDataAdapter adp;
                    SqlCommandBuilder sqlBuilder = new SqlCommandBuilder();
                    adp = new SqlDataAdapter(sqlCommand, this.dataConnection);
                    sqlBuilder.DataAdapter = adp;
                    adp.Update(tbname);
                    this.close();
                    return true;
                }
                catch (SqlException ex)
                {
                    this.errorMessage(ex.Number);
                }
            }
            return false;
        }

        /// <summary>
        /// Hàm hiển thị thông báo lỗi khi thực thi SQL
        /// </summary>
        /// <param name="eNumber">Mã lỗi</param>
        public void errorMessage(int eNumber)
        {
            string err = "";
            Boolean ask = true;
            switch (eNumber)
            {
                case -1:
                    err = "Lỗi - Chưa lưu thông tin máy chủ CSDL!";
                    break;
                case 208:
                    err = "Lỗi - Table hoặc View không tồn tại!";
                    break;
                case 102:
                    err = "Lỗi - Sai cú pháp!";
                    break;
                case 64:
                    err = "Lỗi - Xem lại cáp đường truyền mạng!";
                    break;
                case 547:
                    err = "Lỗi - Dữ liệu đã được sử dụng, không thể xoá!";
                    ask = false;
                    break;
                case 2812:
                    err = "Lỗi - không tồn tại stored procedure!";
                    break;
                case 10054:
                    err = "Lỗi - Disable mạng!";
                    break;
                
                case 4060:
                    err = "Lỗi - Kết nối dữ liệu!";
                    break;

                case 53:
                    err = "Lỗi - Kết nối máy chủ CSDL!";
                    break;

                case 2627:
                    err = "Lỗi - Trùng khoá";
                    break;

                default:
                    err = "Lỗi thực thi, xem lại stored procedure hoặc độ dài và cho phép null của các trường theo quy định!";
                    break;
            }
            if (!ask){
                MessageBox.Show(err);
            }else if(MessageBox.Show(err + "\n Có muốn đóng chương trình không?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Hàm kết nối dữ liệu cho LookUpEdit control
        /// </summary>
        /// <param name="sqlCommand">Câu lệnh SQL</param>
        /// <param name="lk">LookUpEdit control</param>
        /// <param name="display">Tên trường hiển thị</param>
        /// <param name="value">Tên trường giá trị</param>
        /*
        public void setLookup(string sqlCommand
            , ref DevExpress.XtraEditors.LookUpEdit lk
            , string displayMember, string valueMember)
        {
            DataTable dt = this.getDataTable(sqlCommand);
            if (dt != null)
            {
                lk.Properties.DataSource = dt;
                lk.Properties.DisplayMember = displayMember;
                lk.Properties.ValueMember = valueMember;
            }
            else lk.Properties.DataSource = null;

        }*/

        //public bool CheckValidRole(int MANV,string MENU_FORM_URL)
        //{
        //    string sql="SELECT * "+
        //    " FROM         dbo.MENU_FORM INNER JOIN "+
        //              " dbo.GROUP_RELATIONSHIP_MENU_FORM ON  "+
        //              " dbo.MENU_FORM.ID_MENU_FORM = dbo.GROUP_RELATIONSHIP_MENU_FORM.ID_MENU_FORM INNER JOIN "+
        //              " dbo.GROUP_USER ON dbo.GROUP_RELATIONSHIP_MENU_FORM.ID_GROUP = dbo.GROUP_USER.ID_GROUP INNER JOIN "+
        //              " dbo.GROUP_USER_RELATIONSHIP_USER ON dbo.GROUP_USER.ID_GROUP = dbo.GROUP_USER_RELATIONSHIP_USER.ID_GROUP "+
        //    " WHERE     (dbo.GROUP_USER_RELATIONSHIP_USER.MANV = " + MANV + ") AND (dbo.MENU_FORM.MENU_FORM_URL = '" + MENU_FORM_URL  + "')";
        //    DataTable dt = getDataTable(sql);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    MessageBox.Show("Bạn không có quyền truy cập chức năng này!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        //    return false;
        //}
        public void load_dgv(string str_sqlSelect, DataSet dts, DataGridView dgv)
        {
            dts = new DataSet();
            dts.Clear();
            dts = getDataSet(str_sqlSelect);
            SqlDataAdapter sql_adap = new SqlDataAdapter(str_sqlSelect, getConnection());
            dgv.DataSource = dts;
            dgv.DataMember = dts.Tables[0].ToString();
            dgv.AutoResizeColumns();
            dgv.AutoResizeRows();
            dgv.Columns[dgv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void load_comboBox(string str_sqlSelect, ComboBox cbb)
        {
            DataTable dt = new DataTable();
            dt = getDataTable(str_sqlSelect);
            cbb.DataSource = dt;
            cbb.DisplayMember = dt.Columns[1].ToString();
            cbb.ValueMember = dt.Columns[0].ToString();
        }
    }
}
