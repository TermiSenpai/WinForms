using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsContacts
{
    public partial class Main : Form
    {
        private BussinessLogicLayer _bussinessLogicLayer;
        public Main()
        {
            InitializeComponent();
            _bussinessLogicLayer = new BussinessLogicLayer();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            PopulateContactos();
        }

        #region EVENTS
        private void buttonAnadir_Click(object sender, EventArgs e)
        {
            openDetallesDialog();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewLinkCell cell = (DataGridViewLinkCell)gridContacts.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell.Value.ToString() == "Editar")
            {
                EditContact(e);
            }
            else if (cell.Value.ToString() == "Borrar")
            {
                DeleteContact(e);
            }
        }
        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            PopulateContactos(textBuscar.Text);
        }

        #endregion

        #region PRIVATE METHODS
        private void openDetallesDialog()
        {
            Detalles detalles = new Detalles();
            detalles.ShowDialog(this);
        }

        public void PopulateContactos(string searchString = null)
        {
            List<Contacto> contactos = _bussinessLogicLayer.GetContactos(searchString);
            gridContacts.DataSource = contactos;
        }

        private Contacto ContactoFromRow(DataGridViewCellEventArgs e)
        {
            return new Contacto
            {
                Id = int.Parse((gridContacts.Rows[e.RowIndex].Cells[0]).Value.ToString()),
                Nombre = (gridContacts.Rows[e.RowIndex].Cells[1]).Value.ToString(),
                Apellidos = (gridContacts.Rows[e.RowIndex].Cells[2]).Value.ToString(),
                Direccion = (gridContacts.Rows[e.RowIndex].Cells[4]).Value.ToString(),
                Telefono = (gridContacts.Rows[e.RowIndex].Cells[3]).Value.ToString()
            };
        }

        private void EditContact(DataGridViewCellEventArgs e)
        {
            Detalles detalles = new Detalles();
            detalles.LoadContacto(ContactoFromRow(e));

            detalles.ShowDialog(this);
        }

        private void DeleteContact(DataGridViewCellEventArgs e)
        {
            Contacto contacto = ContactoFromRow(e);
            _bussinessLogicLayer.DeleteContacto(contacto);

            PopulateContactos();
        }
        
        #endregion
    }
}
