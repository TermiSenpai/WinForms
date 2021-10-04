using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsContacts
{
    class BussinessLogicLayer
    {
        private DataAccessLayer _dataAccessLayer;

        public BussinessLogicLayer()
        {
            _dataAccessLayer = new DataAccessLayer();
        }

        public Contacto SaveContacto(Contacto contacto)
        {
            if (contacto.Id == 0) _dataAccessLayer.InsertContacto(contacto);
            else _dataAccessLayer.UpdateContacto(contacto);
            return contacto;
        }

        public List<Contacto> GetContactos(string searchString = null)
        {
            return _dataAccessLayer.GetContactos(searchString);
        }

        public void DeleteContacto(Contacto contacto)
        {
            _dataAccessLayer.DeleteContacto(contacto.Id);
        }
    }
}
